using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using STR = TKCommon.Utility.StringUtility;
using ReversiCommon.Utility;
using ReversiCommon.Collections;
using TKCommon.Debugger;

namespace ReversiCommon.Evaluators.Logic
{
    /// <summary>
    /// <para>定石評価関数ロジッククラス</para>
    /// </summary>
    public class JosekiEvaluatorLogic
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>定石のスコアを取得する</para>
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public int GetJosekiScore(string pattern)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT SCORE");
            sb.AppendLine("  FROM ST_JOSEKI");
            sb.AppendLine(" WHERE PATTERN = " + STR.GetSqlString(pattern));

            StopWatchLogger.StartEventWatch("JosekiEvaluator-SQLExecute");
            DataTable dt = SQL.ReadDataAdapter(sb.ToString());
            StopWatchLogger.StopEventWatch("JosekiEvaluator-SQLExecute");
            if (dt == null || dt.Rows.Count < 1)
            {
                return 0;
            }
            else
            {
                return int.Parse(dt.Rows[0]["SCORE"].ToString());
            }
        }

        /// <summary>
        /// <para>定石ビューのデータを取得する</para>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static DataTable GetJosekiViewData(Color color)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            if (color == Color.Black)
            {
                sb.AppendLine("  FROM ST_JOSEKI_TOPS_BLACK_VIEW");
            }
            else
            {
                sb.AppendLine("  FROM ST_JOSEKI_TOPS_WHITE_VIEW");
            }

            return SQL.ReadDataAdapter(sb.ToString());
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
