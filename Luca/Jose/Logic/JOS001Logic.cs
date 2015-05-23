using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using STR = TKCommon.Utility.StringUtility;

namespace Luca.Jose.Logic
{
    /// <summary>
    /// <para>定石評価ロジッククラス</para>
    /// </summary>
    public class JOS001Logic
    {
        #region "定数"
        /// <summary>
        /// <para>評価終了ターン数</para>
        /// </summary>
        public const int END_TURN_NO = 14;
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>生の棋譜データを取得する</para>
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public DataTable GetRawKifuData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            sb.AppendLine("  FROM AR_RAW_KIFU AS ARK");
            sb.AppendLine(" WHERE NOT EXISTS(SELECT 1");
            sb.AppendLine("                    FROM AR_JOSEKI_REGISTER_RIREKI AJR");
            sb.AppendLine("                   WHERE AJR.KIFU_ID = ARK.KIFU_ID");
            sb.AppendLine("                     AND AJR.END_TURN_NO >= " + END_TURN_NO.ToString() + ")");

            return SQL.ReadDataAdapter(sb.ToString());
        }

        /// <summary>
        /// <para>定石データを更新する</para>
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="score"></param>
        /// <param name="kifuId"></param>
        public void MergeIntoJoseki(string pattern, string score)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ST_JOSEKI");
            sb.AppendLine("     ( PATTERN");
            sb.AppendLine("     , SCORE");
            sb.AppendLine("     )");
            sb.AppendLine("VALUES");
            sb.AppendLine("     ( " + STR.GetSqlString(pattern));
            sb.AppendLine("     , " + score);
            sb.AppendLine("     )");
            sb.AppendLine("    ON DUPLICATE KEY UPDATE SCORE = SCORE + " + score);
            
            SQL.ExecuteCommand(sb.ToString());
        }

        /// <summary>
        /// <para>定石履歴を記録する</para>
        /// </summary>
        public void InsertJosekiRireki(string kifuId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AR_JOSEKI_REGISTER_RIREKI");
            sb.AppendLine("     ( KIFU_ID");
            sb.AppendLine("     , END_TURN_NO");
            sb.AppendLine("     )");
            sb.AppendLine("VALUES");
            sb.AppendLine("     ( " + kifuId);
            sb.AppendLine("     , " + END_TURN_NO.ToString());
            sb.AppendLine("     )");

            SQL.ExecuteCommand(sb.ToString());
        }

        /// <summary>
        /// <para>現在の定石のスコアを取得する</para>
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public int GetNowJosekiScore(string pattern)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT SCORE");
            sb.AppendLine("  FROM ST_JOSEKI");
            sb.AppendLine(" WHERE PATTERN = " + STR.GetSqlString(pattern));

            DataTable dt = SQL.ReadDataAdapter(sb.ToString());
            if (dt == null || dt.Rows.Count < 1)
            {
                return 0;
            }
            else
            {
                return int.Parse(dt.Rows[0]["SCORE"].ToString());
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
