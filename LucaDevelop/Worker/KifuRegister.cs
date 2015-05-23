using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.TypedDataSet;
using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using ReversiCommon.Collections;
using STR = TKCommon.Utility.StringUtility;
using TKCommon.Debugger;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// 棋譜登録クラス
    /// </summary>
    public class KifuRegister
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// コマンドリスト
        /// </summary>
        private List<string> m_CmdList;
        #endregion

        #region "コンストラクタ"
        public KifuRegister()
        {
            this.m_CmdList = new List<string>();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 棋譜を登録する
        /// </summary>
        public void Register(List<ReversiCommonDataSet.AR_DEVELOP_KIFU_INDEXRow> dt)
        {
            this.InsertArDevelopKifuIndex(dt);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// AR_DEVELOP_KIFU_INDEXにデータを登録する
        /// </summary>
        /// <param name="rows"></param>
        private void InsertArDevelopKifuIndex(List<ReversiCommonDataSet.AR_DEVELOP_KIFU_INDEXRow> dt)
        {
            foreach (ReversiCommonDataSet.AR_DEVELOP_KIFU_INDEXRow row in dt)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO AR_DEVELOP_KIFU_INDEX");
                sb.AppendLine("     ( INDEX_NAME");
                sb.AppendLine("     , INDEX_NO");
                sb.AppendLine("     , STAGE");
                sb.AppendLine("     , NORMALIZE_VALUE");
                sb.AppendLine("     , BLACK_SCORE");
                sb.AppendLine("     , PARITY");
                sb.AppendLine("     , BLACK_MOBILITY");
                sb.AppendLine("     , WHITE_MOBILITY");
                sb.AppendLine("     , DISCRIMINATION");
                sb.AppendLine("     )");
                sb.AppendLine("VALUES");
                sb.AppendLine("     ( " + STR.GetSqlString(row.INDEX_NAME));
                sb.AppendLine("     , " + row.INDEX_NO);
                sb.AppendLine("     , " + row.STAGE);
                sb.AppendLine("     , " + row.NORMALIZE_VALUE);
                sb.AppendLine("     , " + row.BLACK_SCORE);
                sb.AppendLine("     , " + row.PARITY);
                sb.AppendLine("     , " + row.BLACK_MOBILITY);
                sb.AppendLine("     , " + row.WHITE_MOBILITY);
                sb.AppendLine("     , " + STR.GetSqlString(row.DISCRIMINATION));
                sb.AppendLine("     )");

                this.m_CmdList.Add(sb.ToString());
            }

            if (this.m_CmdList.Count() < 60000) { return; }

            StopWatchLogger.StartEventWatch("ExecuteCommand");
            SQL.ExecuteCommand(this.m_CmdList);
            StopWatchLogger.StopEventWatch("ExecuteCommand");
            this.m_CmdList = new List<string>();
            StopWatchLogger.StopEventWatch("DummyExecute");
            StopWatchLogger.DisplayAllEventTimes();
        }
        #endregion
        #endregion
    }
}
