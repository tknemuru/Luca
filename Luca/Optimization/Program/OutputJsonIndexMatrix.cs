using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Optimization.Logic;
using System.Data;
using ReversiCommon.Collections;
using TKCommon.Collections;
using ReversiCommon.Utility;
using TKCommon.Debugger;
using TKCommon.Collections.JsonCollections;
using TKCommon.Utility;
using System.IO;
using ReversiCommon.Collections.JsonCollections;

namespace Luca.Optimization.Program
{
    /// <summary>
    /// JSONインデックス行列出力クラス
    /// </summary>
    public class OutputJsonIndexMatrix
    {
        #region "定数"
        /// <summary>
        /// JSONファイルパス
        /// </summary>
        public const string JSON_FILE_PATH = @"C:\work\visualstudio\Luca_TRUNK\Luca\Optimization\Json\";

        /// <summary>
        /// JSONファイル名
        /// </summary>
        public const string JSON_FILE_NAME = @"index_matrix_{0}.txt";

        /// <summary>
        /// JSONファイル名（結果ベクトル）
        /// </summary>
        public const string JSON_FILE_NAME_VECTOR_B = @"vector_b_{0}.txt";

        /// <summary>
        /// JSONファイル名（マスタインデックス）
        /// </summary>
        public const string JSON_FILE_NAME_MASTER_INDEX = @"master_index_{0}.txt";

        /// <summary>
        /// 一回のステージのターン
        /// </summary>
        public const int ONE_STAGE_TURN = 4;

        /// <summary>
        /// 開始ステージ
        /// </summary>
        public const int START_STAGE = 15;

        /// <summary>
        /// 終了ステージ
        /// </summary>
        public const int END_STAGE = 15;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// ロジックインスタンス
        /// </summary>
        private OutputJsonIndexMatrixLogic m_Logic;

        /// <summary>
        /// ステージ
        /// </summary>
        private int m_Stage;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutputJsonIndexMatrix()
        {
            this.m_Logic = new OutputJsonIndexMatrixLogic();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// インデックスの統計を実行する
        /// </summary>
        public void Execute()
        {
            List<int> turnList = new List<int>();
            for (int i = (((START_STAGE - 1) * ONE_STAGE_TURN) + 1); i <= (END_STAGE * ONE_STAGE_TURN); i++)
            {
                if ((i % ONE_STAGE_TURN) == 0)
                {
                    this.m_Stage = (i / ONE_STAGE_TURN);
                    turnList.Add(i);
                    this.m_Logic = new OutputJsonIndexMatrixLogic();
                    DebugUtility.OutputStringToConsole("[STAGE" + this.m_Stage.ToString() + " START]" + DateTime.Now.ToString());
                    StopWatchLogger.StartEventWatch("IndexOptimizer:Stage" + this.m_Stage.ToString());
                    this.Execute(turnList);
                    StopWatchLogger.StopEventWatch("IndexOptimizer:Stage" + this.m_Stage.ToString());
                    DebugUtility.OutputStringToConsole("[STAGE" + this.m_Stage.ToString() + " END]" + DateTime.Now.ToString());
                    StopWatchLogger.DisplayAllEventTimes();
                    StopWatchLogger.ClearAllEventTimes();
                    turnList = new List<int>();
                }
                else
                {
                    turnList.Add(i);
                }
            }
        }

        /// <summary>
        /// インデックスの統計を実行する
        /// </summary>
        public void Execute(List<int> turnList)
        {
            // マスタインデックスデータを取得
            List<MasterIndexJson> masterIndexList = this.MakeJsonMasterIndex(turnList);

            SparseMatrix<double> A = new SparseMatrix<double>(this.m_Logic.GetMatrixHeight(turnList), (masterIndexList.Count + IndexExtraInformation.All.Count), 0);
            SparseVector<double> b = new SparseVector<double>(0);

            int Ay = 0;

            foreach (int turn in turnList)
            {

                // 生棋譜データのリストを取得
                StopWatchLogger.StartEventWatch("GetRawKifuList");
                DataTable rawKifuDt = this.m_Logic.GetRawKifuList(turn);
                StopWatchLogger.StopEventWatch("GetRawKifuList");

                // 棋譜インデックスデータから棋譜ID*インデックスNo+インデックス名をキーにしたディクショナリを作成する
                StopWatchLogger.StartEventWatch("GetKifuIndexData");
                DataTable kifuIndexDt = this.m_Logic.GetKifuIndexData(turn);
                StopWatchLogger.StopEventWatch("GetKifuIndexData");

                StopWatchLogger.StartEventWatch("kifuDic.Add");
                Dictionary<string, double> kifuDic = new Dictionary<string, double>();
                foreach (DataRow kifuIndexRow in kifuIndexDt.Rows)
                {
                    string key = kifuIndexRow["KIFU_ID"].ToString() + "*" + kifuIndexRow["NORMALIZE_INDEX_NO"].ToString() + kifuIndexRow["INDEX_NAME"].ToString();
                    kifuDic.Add(key, double.Parse(kifuIndexRow["SCORE_VALUE"].ToString()));
                }
                StopWatchLogger.StopEventWatch("kifuDic.Add");

                StopWatchLogger.StartEventWatch("foreach rawKifuRow");
                foreach (DataRow rawKifuRow in rawKifuDt.Rows)
                {
                    int Ax = 0;
                    foreach (MasterIndexJson masterRow in masterIndexList)
                    {
                        // 棋譜ID*インデックスNo+インデックス名をキーにディクショナリを検索
                        string key = rawKifuRow["KIFU_ID"].ToString() + "*" + masterRow.NormalizeIndexNo + masterRow.IndexName;
                        if (kifuDic.ContainsKey(key))
                        {
                            // 存在したらvalueを設定
                            A[Ay, Ax] = kifuDic[key];
                        }
                        Ax++;
                    }

                    // 拡張情報を追加
                    foreach (IndexExtraInformation extInfo in IndexExtraInformation.All)
                    {
                        A[Ay, (Ax + extInfo.Index)] = double.Parse(rawKifuRow[extInfo.Name].ToString());
                    }

                    Ay++;
                    b.Add(double.Parse(rawKifuRow["THEORY_BLACK_SCORE"].ToString()) - double.Parse(rawKifuRow["THEORY_WHITE_SCORE"].ToString()));
                }
                StopWatchLogger.StopEventWatch("foreach rawKifuRow");
                StopWatchLogger.DisplayAllEventTimes();
            }

            // JSONオブジェクトにコピー
            SparseMatrixJson<double> Ajson = new SparseMatrixJson<double>(A, 0);

            // JSONファイルを出力
            JsonUtility.Serialize(Ajson, JSON_FILE_PATH + string.Format(JSON_FILE_NAME, this.m_Stage));

            // JSONオブジェクトにコピー
            SparseVectorJson<double> bjson = new SparseVectorJson<double>(b, 0);

            // JSONファイルを出力
            JsonUtility.Serialize(bjson, JSON_FILE_PATH + string.Format(JSON_FILE_NAME_VECTOR_B, this.m_Stage));
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// マスタインデックスのJSONファイルを作成する
        /// </summary>
        /// <param name="dt"></param>
        public List<MasterIndexJson> MakeJsonMasterIndex(List<int> turnList)
        {
            List<MasterIndexJson> indexList;
            if (File.Exists(JSON_FILE_PATH + string.Format(JSON_FILE_NAME_MASTER_INDEX, this.m_Stage)))
            {
                indexList = JsonUtility.Deserialize<List<MasterIndexJson>>(JSON_FILE_PATH + string.Format(JSON_FILE_NAME_MASTER_INDEX, this.m_Stage));
            }
            else
            {
                StopWatchLogger.StartEventWatch("GetMasterIndexData");
                DataTable masterDt = this.m_Logic.GetMasterIndexData(turnList);
                StopWatchLogger.StopEventWatch("GetMasterIndexData");

                // JSONファイルを出力しておく
                indexList = new List<MasterIndexJson>();
                foreach (DataRow row in masterDt.Rows)
                {
                    indexList.Add(new MasterIndexJson(row["INDEX_NAME"].ToString(), row["NORMALIZE_INDEX_NO"].ToString()));
                }
                JsonUtility.Serialize(indexList, JSON_FILE_PATH + string.Format(JSON_FILE_NAME_MASTER_INDEX, this.m_Stage));
            }

            return indexList;
        }
        #endregion
        #endregion
    }
}
