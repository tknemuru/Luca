using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using ReversiCommon.Collections;
using STR = TKCommon.Utility.StringUtility;
using TCST = TKCommon.Constant.Const;

namespace Luca.Stat.Logic
{
    /// <summary>
    /// <para>ミラーインデックスセットロジック</para>
    /// </summary>
    public class STA002Logic
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
        /// <para>生棋譜データをDBから取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetRawKifuData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            sb.AppendLine("  FROM AR_RAW_KIFU");
            sb.AppendLine(" WHERE CALCD_FLG = " + STR.GetSqlString(TCST.SQL_FLG_NO));

            return SQL.ReadDataAdapter(sb.ToString());
        }

        /// <summary>
        /// <para>スコアインデックスを更新する</para>
        /// </summary>
        /// <param name="kifuId"></param>
        /// <param name="turnNo"></param>
        /// <param name="score"></param>
        public void UpdateScoreIndex(string kifuId, string turnNo, int score)
        {
            List<string> cmdList = new List<string>();
            List<CostIndexGenerator> costIndexList = CostIndexGenerator.GetIndexList();

            foreach (CostIndexGenerator index in costIndexList)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ST_SCORE_INDEX");
                sb.AppendLine("     ( INDEX_NAME");
                sb.AppendLine("     , INDEX_NO");
                sb.AppendLine("     , STAGE");
                sb.AppendLine("     , SCORE");
                sb.AppendLine("     , UPDATE_CNT");
                sb.AppendLine("     )");
                sb.AppendLine("VALUES");
                sb.AppendLine("     ( " + STR.GetSqlString(index.Name));
                sb.AppendLine("     , " + index.Index);
                sb.AppendLine("     , " + TurnKeeper.NowStage.ToString());
                sb.AppendLine("     , " + score);
                sb.AppendLine("     , 1");
                sb.AppendLine("     )");
                sb.AppendLine("    ON DUPLICATE KEY UPDATE SCORE = SCORE + " + score);
                sb.AppendLine("                          , UPDATE_CNT = UPDATE_CNT + 1");
                cmdList.Add(sb.ToString());
            }

            // 更新処理
            SQL.ExecuteCommand(cmdList);
        }

        /// <summary>
        /// <para>インデックスをDBに記録する</para>
        /// </summary>
        /// <param name="kifuId"></param>
        public void UpdateCalcdFlg(string kifuId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE AR_RAW_KIFU");
            sb.AppendLine("   SET CALCD_FLG = " + STR.GetSqlString(TCST.SQL_FLG_YES));
            sb.AppendLine(" WHERE KIFU_ID = " + kifuId);
            SQL.ExecuteCommand(sb.ToString());
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
