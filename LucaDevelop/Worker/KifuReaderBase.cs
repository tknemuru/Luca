using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.TypedDataSet;
using System.IO;
using TKCommon.Utility;
using LucaDevelop.Collections;
using ReversiCommon.Collections;
using System.Diagnostics;
using ReversiCommon.Utility;
//using TKCommon.Debugger;
using TKCommon.Collections;
using System.Data;
using ReversiCommon.Collections.JsonCollections;
using TKCommon.Collections.JsonCollections;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class KifuReaderBase
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// ファイルパス
        /// </summary>
        protected string m_Path;

        /// <summary>
        /// ファイルパターン
        /// </summary>
        protected string m_Pattern;

        /// <summary>
        /// マスタインデックスの順序を格納したディクショナリ
        /// </summary>
        private Dictionary<string, int> m_MasterIndexDic;

        /// <summary>
        /// インデックスベクトルディクショナリ
        /// </summary>
        private Dictionary<string, List<SparseVector<int>>> m_IndexVectorDic;

        /// <summary>
        /// 結果スコアリスト
        /// </summary>
        private Dictionary<string, List<int>> m_ResultScoreList;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KifuReaderBase(string path, string pattern)
        {
            this.m_Path = path;
            this.m_Pattern = pattern;
            this.m_IndexVectorDic = new Dictionary<string, List<SparseVector<int>>>();
            this.m_ResultScoreList = new Dictionary<string, List<int>>();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 棋譜を読み込む
        /// </summary>
        /// <returns></returns>
        public void Read()
        {
            IEnumerable<string> files = Directory.EnumerateFiles(this.m_Path, this.m_Pattern, SearchOption.AllDirectories);

            foreach (string file in files)
            {
                // 棋譜データの取得
                List<string> strKifuList = this.Read(file);

                // 棋譜データを汎用フォーマットに変換
                List<RawKifu> kifuList = this.ConvertKifuData(strKifuList, file);

                // 棋譜リストを登録用フォーマットに変換する
                this.DummyExecute(kifuList, file);
            }
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 棋譜を読み込む
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<string> Read(string file)
        {
            return FileUtility.ReadListData(file);
        }

        /// <summary>
        /// 棋譜リストを登録用フォーマットに変換する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="kifuList"></param>
        /// <returns></returns>
        public abstract List<RawKifu> ConvertKifuData(List<string> kifuList, string fileName);

        /// <summary>
        /// <para>ダミーリバーシを実行する</para>
        /// </summary>
        private void DummyExecute(List<RawKifu> kifuList, string file)
        {
            // マスタインデックスディクショナリを作成
            this.MakeMasterIndexDic();

            SparseVector<double> b = new SparseVector<double>(0);
            List<SparseVector<double>> A = new List<SparseVector<double>>();

            // 対局していく
            int i = 1;
            foreach (RawKifu kifu in kifuList)
            {
                TurnKeeper.InitializeTurnKeeper();
                Board.InitializeBoard();

                List<string> list = StringUtility.ToListSplitCount(kifu.Kifu, 2);
                var cnvList = from a in list
                              select ReversiConvertUtility.ConvertToThorPoint(a);
                
                foreach (string point in cnvList)
                {
                    if (Board.IsReversible())
                    {
                        // 裏返す
                        Board.Reverse(int.Parse(point));

                        // ターンをまわす
                        TurnKeeper.ChangeTurn();

                        // インデックス情報を取得
                        this.SetIndexList(kifu);
                    }
                    else
                    {
                        // パス
                        TurnKeeper.ChangeOnlyColor();

                        // 自分自身は裏返せる？
                        if (Board.IsReversible())
                        {
                            // 裏返す
                            Board.Reverse(int.Parse(point));

                            // ターンをまわす
                            TurnKeeper.ChangeTurn();

                            // インデックス情報を取得
                            this.SetIndexList(kifu);
                        }
                        else
                        {
                            // インデックス情報を取得
                             this.SetIndexList(kifu);
 
                            // ゲーム終了
                            break;
                        }
                    }
                }
                i++;

                // デバッグ
                if ((i % 10) == 0)
                {
                    Console.WriteLine("完了行：" + i.ToString() + "/" + kifuList.Count().ToString());
                }

                // JSONファイルを出力
                if ((i % 2000) == 0)
                {
                    this.OutputCsvFile(file);
                }
            }

            // 最後は強制的にファイル出力
            this.OutputCsvFile(file);
        }

        /// <summary>
        /// インデックス情報をセットする
        /// </summary>
        /// <param name="row"></param>
        private void SetIndexList(RawKifu kifu)
        {
            // インデックスを取得
            List<CostIndexGenerator> indexList = CostIndexGenerator.GetIndexList();

            // インデックスベクトルに格納
            int masterCount = this.m_MasterIndexDic.Count;
            SparseVector<int> vector = new SparseVector<int>(masterCount + IndexExtraInformation.All.Count(), 0);
            foreach (CostIndexGenerator index in indexList)
            {
                int i = this.m_MasterIndexDic[index.Name + "$" + NormalizeIndex.Get(index.Name, index.Index).ToString()];
                vector[i] += (int)NormalizeIndex.GetNormalizeType(index.Name, index.Index);
            }

            // 追加情報
            foreach (IndexExtraInformation ext in IndexExtraInformation.All)
            {
                switch (ext.Index)
                {
                    case (int)IndexExtraInformation.EXTRA_INFO_INDEX.PARITY :
                        vector[masterCount + IndexExtraInformation.Parity.Index] = int.Parse(TurnKeeper.Parity);
                        break;
                    case (int)IndexExtraInformation.EXTRA_INFO_INDEX.MOBILITY :
                        int mobility = Board.GetAllPutablePointerCount(Color.Black) - Board.GetAllPutablePointerCount(Color.White);
                        vector[masterCount + IndexExtraInformation.Mobility.Index] = mobility;
                        break;
                    //case (int)IndexExtraInformation.EXTRA_INFO_INDEX.WHITE_MOBILITY :
                    //    vector[masterCount + IndexExtraInformation.WhiteMobility.Index] = Board.GetAllPutablePointerCount(Color.White);
                    //    break;
                }
            }
            
            // ディクショナリに格納
            string stage = TurnKeeper.NowStage.ToString();
            if (!this.m_IndexVectorDic.ContainsKey(stage))
            {
                this.m_IndexVectorDic.Add(stage, new List<SparseVector<int>>());
                Debug.Assert(!this.m_ResultScoreList.ContainsKey(stage), "インデックスと結果スコアの関係がおかしいです。");
                this.m_ResultScoreList.Add(stage, new List<int>());    
            }
            this.m_IndexVectorDic[stage].Add(vector);
            this.m_ResultScoreList[stage].Add(kifu.BlackScore);
        }

        /// <summary>
        /// <para>マスタインデックスデータを取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetMasterIndexData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT MSI.INDEX_NAME");
            sb.AppendLine("     , INN.NORMALIZE_INDEX_NO");
            sb.AppendLine("  FROM MS_INDEX_NORMALIZE AS INN");
            sb.AppendLine(" INNER JOIN MS_INDEX AS MSI");
            sb.AppendLine("    ON MSI.DIGIT = INN.DIGIT");
            sb.AppendLine(" GROUP BY MSI.INDEX_NAME");
            sb.AppendLine("     , INN.NORMALIZE_INDEX_NO");
            sb.AppendLine(" ORDER BY MSI.INDEX_NAME");
            sb.AppendLine("     , INN.NORMALIZE_INDEX_NO");

            return MySqlUtility.ReadDataAdapterNoTimeOut(sb.ToString());
        }

        /// <summary>
        /// マスタインデックスのJSONファイルを作成する
        /// </summary>
        /// <param name="dt"></param>
        public void MakeMasterIndexDic()
        {
            if (this.m_MasterIndexDic != null) { return; }
            this.m_MasterIndexDic = new Dictionary<string, int>();

            string path = System.Configuration.ConfigurationManager.AppSettings["Master Index File Path"];
            List<MasterIndexJson> indexList;
            if (File.Exists(path))
            {
                indexList = JsonUtility.Deserialize<List<MasterIndexJson>>(path);
            }
            else
            {
                DataTable masterDt = this.GetMasterIndexData();

                // JSONファイルを出力しておく
                indexList = new List<MasterIndexJson>();
                foreach (DataRow row in masterDt.Rows)
                {
                    indexList.Add(new MasterIndexJson(row["INDEX_NAME"].ToString(), row["NORMALIZE_INDEX_NO"].ToString()));
                }
                JsonUtility.Serialize(indexList, path);
            }

            for (int i = 0; i < indexList.Count(); i++)
            {
                this.m_MasterIndexDic.Add(indexList[i].IndexName + "$" + indexList[i].NormalizeIndexNo, i);
            }
        }

        /// <summary>
        /// CSVファイルを出力する
        /// </summary>
        /// <param name="file"></param>
        public void OutputCsvFile(string file)
        {
            foreach (KeyValuePair<string, List<SparseVector<int>>> matrix in this.m_IndexVectorDic)
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["Index Matrix File Path"];
                List<string> list = new List<string>();
                foreach (SparseVector<int> vector in matrix.Value)
                {
                    list.Add(vector.ToCsv());
                }
                FileUtility.WriteListData(list, string.Format(path, FileUtility.GetFileName(file), matrix.Key), true);
            }

            foreach (KeyValuePair<string, List<int>> vector in this.m_ResultScoreList)
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["Result Score File Path"];
                FileUtility.WriteLine(string.Join(",", vector.Value), string.Format(path, FileUtility.GetFileName(file), vector.Key));
            }

            this.m_IndexVectorDic = new Dictionary<string, List<SparseVector<int>>>();
            this.m_ResultScoreList = new Dictionary<string, List<int>>();
        }

        ///// <summary>
        ///// JSONファイルを出力する
        ///// </summary>
        //public void OutputJsonFile(string file)
        //{
        //    // JSONオブジェクトにコピー
        //    foreach (KeyValuePair<string, List<SparseVector<int>>> matrix in this.m_IndexVectorDic)
        //    {
        //        SparseMatrix<int> A = new SparseMatrix<int>(matrix.Value, 0);
        //        SparseMatrixJson<int> Ajson = new SparseMatrixJson<int>(A, 0);

        //        // JSONファイルを出力
        //        string path = System.Configuration.ConfigurationManager.AppSettings["Index Matrix File Path"];
        //        JsonUtility.Serialize(Ajson, string.Format(path, FileUtility.GetFileName(file), matrix.Key), true);
        //    }

        //    // JSONオブジェクトにコピー
        //    foreach (KeyValuePair<string, SparseVector<int>> vector in this.m_ResultScoreList)
        //    {
        //        SparseVectorJson<int> bjson = new SparseVectorJson<int>(vector.Value, 0);

        //        // JSONファイルを出力
        //        string path = System.Configuration.ConfigurationManager.AppSettings["Result Score File Path"];
        //        JsonUtility.Serialize(bjson, string.Format(path, FileUtility.GetFileName(file), vector.Key), true);
        //    }

        //    this.m_IndexVectorDic = new Dictionary<string, List<SparseVector<int>>>();
        //    this.m_ResultScoreList = new Dictionary<string,SparseVector<int>>();
        //}
        #endregion
        #endregion
    }
}
