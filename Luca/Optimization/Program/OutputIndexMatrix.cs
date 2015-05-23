using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Optimization.Logic;
using System.Data;
using System.Diagnostics;
using ReversiCommon.Collections;

namespace Luca.Optimization.Program
{
    /// <summary>
    /// インデックス行列出力クラス
    /// </summary>
    public class OutputIndexMatrix
    {
        #region "定数"
        /// <summary>
        /// 一回のステージのターン
        /// </summary>
        public const int ONE_STAGE_TURN = 4;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// ロジックインスタンス
        /// </summary>
        private OutputIndexMatrixLogic m_Logic;
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// インデックスをCSVに出力する
        /// </summary>
        public void Execute()
        {
            List<int> turnList = new List<int>();
            for (int i = 1; i <= TurnKeeper.MAX_TURN; i++)
            {
                if ((i % ONE_STAGE_TURN) == 0)
                {
                    turnList.Add(i);
                    this.m_Logic = new OutputIndexMatrixLogic((i / ONE_STAGE_TURN));
                    this.Execute(turnList);
                    turnList = new List<int>();
                }
                else
                {
                    turnList.Add(i);
                }
            }
        }

        /// <summary>
        /// インデックスをCSVに出力する
        /// </summary>
        public void Execute(List<int> turnList)
        {
            // マスタインデックスデータを取得
            DataTable masterDt = this.m_Logic.GetMasterIndexData(turnList);

            // CSVヘッダを出力
            this.m_Logic.WriteCsvHeader(masterDt);

            foreach(int turn in turnList)
            {

                // 生棋譜データのリストを取得
                DataTable rawKifuDt = this.m_Logic.GetRawKifuList(turn);

                // 棋譜インデックスデータから棋譜ID*インデックスNo+インデックス名をキーにしたディクショナリを作成する
                DataTable kifuIndexDt = this.m_Logic.GetKifuIndexData(turn);
                Dictionary<string, string> kifuDic = new Dictionary<string, string>();
                foreach (DataRow kifuIndexRow in kifuIndexDt.Rows)
                {
                    string key = kifuIndexRow["KIFU_ID"].ToString() + "*" + kifuIndexRow["NORMALIZE_INDEX_NO"].ToString() + kifuIndexRow["INDEX_NAME"].ToString();
                    kifuDic.Add(key, kifuIndexRow["SCORE_VALUE"].ToString());
                }

                foreach (DataRow rawKifuRow in rawKifuDt.Rows)
                {
                    List<string> line = new List<string>();
                    foreach (DataRow masterRow in masterDt.Rows)
                    {
                        // 棋譜ID*インデックスNo+インデックス名をキーにディクショナリを検索
                        string key = rawKifuRow["KIFU_ID"].ToString() + "*" + masterRow["NORMALIZE_INDEX_NO"].ToString() + masterRow["INDEX_NAME"].ToString();
                        if (kifuDic.ContainsKey(key))
                        {
                            // 存在したらvalueを設定
                            line.Add(kifuDic[key]);
                        }
                        else
                        {
                            // 存在しない場合は0を設定
                            line.Add("0");
                        }

                    }
                    string score = (int.Parse(rawKifuRow["THEORY_BLACK_SCORE"].ToString()) - int.Parse(rawKifuRow["THEORY_WHITE_SCORE"].ToString())).ToString();
                    line.Add(score);

                    Debug.Assert(((masterDt.Rows.Count + 1) == line.Count()), "ヘッダと行の列数が一致しません。");

                    this.m_Logic.WriteCsvLine(line);
                }
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
