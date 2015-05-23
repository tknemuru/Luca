using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using STR = TKCommon.Utility.StringUtility;
using TCST = TKCommon.Constant.Const;

namespace Luca.Stat.Logic
{
    /// <summary>
    /// <para>評価値統計ロジッククラス</para>
    /// </summary>
    public class STA001Logic
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
        /// <para>スコアインデックスを挿入する</para>
        /// </summary>
        public void InsertScoreIndex(string indexNo, string indexName, string stage)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ST_SCORE_INDEX");
            sb.AppendLine("     ( INDEX_NO");
            sb.AppendLine("     , INDEX_NAME");
            sb.AppendLine("     , STAGE");
            sb.AppendLine("     , SCORE");
            sb.AppendLine("     )");
            sb.AppendLine("VALUES");
            sb.AppendLine("     ( " + indexNo);
            sb.AppendLine("     , " + STR.GetSqlString(indexName));
            sb.AppendLine("     , " + stage);
            sb.AppendLine("     , 0");
            sb.AppendLine("     )");

            SQL.ExecuteCommand(sb.ToString());
        }

        /// <summary>
        /// <para>ターンステージのリストを取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetTurnStageList()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT STAGE");
            sb.AppendLine("  FROM MS_TURN_STAGE");
            sb.AppendLine(" GROUP BY STAGE");

            return SQL.ReadDataAdapter(sb.ToString());
        }

        /// <summary>
        /// <para>ターンステージデータをTRUNCATEする</para>
        /// </summary>
        public void TruncateTurnStage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TRUNCATE TABLE MS_TURN_STAGE");
            SQL.ExecuteCommand(sb.ToString());
        }

        /// <summary>
        /// <para>ターンステージデータを登録する</para>
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="stage"></param>
        public void InsertTurnStage(int turn, int stage)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO MS_TURN_STAGE");
            sb.AppendLine("     ( TURN_NO");
            sb.AppendLine("     , STAGE");
            sb.AppendLine("     )");
            sb.AppendLine("VALUES");
            sb.AppendLine("     ( " + turn.ToString());
            sb.AppendLine("     , " + stage.ToString());
            sb.AppendLine("     )");

            SQL.ExecuteCommand(sb.ToString());
        }

        /// <summary>
        /// <para>ループ単位となる棋譜インデックスデータを取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetKifuIndexUnit()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT AKI.KIFU_ID");
            sb.AppendLine("     , AKI.TURN_NO");
            sb.AppendLine("     , ARK.THEORY_BLACK_SCORE AS SCORE");
            sb.AppendLine("  FROM AR_KIFU_INDEX AS AKI");
            sb.AppendLine(" INNER JOIN AR_RAW_KIFU AS ARK");
            sb.AppendLine("    ON AKI.KIFU_ID = ARK.KIFU_ID");
            sb.AppendLine(" WHERE AKI.CALCD_FLG = " + STR.GetSqlString(TCST.SQL_FLG_NO));
            sb.AppendLine(" GROUP BY KIFU_ID");
            sb.AppendLine("     , AKI.TURN_NO");
            sb.AppendLine("     , ARK.THEORY_BLACK_SCORE");

            return SQL.ReadDataAdapter(sb.ToString());

            /*
            sb.AppendLine("SELECT KIFU.INDEX_NAME");
            sb.AppendLine("     , KIFU.INDEX_NO");
            sb.AppendLine("     , RAW.THEORY_BLACK_SCORE AS SCORE");
            sb.AppendLine("     , TS.STAGE");
            sb.AppendLine("  FROM AR_KIFU_INDEX AS KIFU");
            sb.AppendLine(" INNER JOIN AR_RAW_KIFU AS RAW");
            sb.AppendLine("    ON KIFU.KIFU_ID = RAW.KIFU_ID");
            sb.AppendLine(" INNER JOIN MS_TURN_STAGE AS TS");
            sb.AppendLine("    ON KIFU.TURN_NO = TS.TURN_NO");
            sb.AppendLine(" WHERE KIFU.CALCD_FLG = " + STR.GetSqlString(TCST.SQL_FLG_NO));

            return SQL.ReadDataAdapter(sb.ToString());
            */
        }

        /// <summary>
        /// <para>存在しないスコアインデックスをインサートする</para>
        /// </summary>
        /// <param name="kifuId"></param>
        /// <param name="turnNo"></param>
        public void InsertNotExistsScoreIndex(string kifuId, string turnNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ST_SCORE_INDEX");
            sb.AppendLine("     ( INDEX_NO");
            sb.AppendLine("     , INDEX_NAME");
            sb.AppendLine("     , STAGE");
            sb.AppendLine("     , SCORE");
            sb.AppendLine("     )");
            sb.AppendLine("SELECT DISTINCT AKI.INDEX_NO");
            sb.AppendLine("     , AKI.INDEX_NAME");
            sb.AppendLine("     , MTS.STAGE");
            sb.AppendLine("     , 0");
            sb.AppendLine("  FROM AR_KIFU_INDEX AKI");
            sb.AppendLine(" INNER JOIN MS_TURN_STAGE MTS");
            sb.AppendLine("    ON AKI.TURN_NO = MTS.TURN_NO");
            sb.AppendLine(" WHERE AKI.KIFU_ID = " + kifuId);
            sb.AppendLine("   AND AKI.TURN_NO = " + turnNo);
            sb.AppendLine("   AND NOT EXISTS(SELECT 1");
            sb.AppendLine("                    FROM ST_SCORE_INDEX SUB_SSI");
            sb.AppendLine("                   WHERE AKI.INDEX_NO = SUB_SSI.INDEX_NO");
            sb.AppendLine("                     AND AKI.INDEX_NAME = SUB_SSI.INDEX_NAME");
            sb.AppendLine("                     AND MTS.STAGE = SUB_SSI.STAGE)");

            SQL.ExecuteCommand(sb.ToString());
        }

        /// <summary>
        /// <para>現在のインデックス評価値を取得する</para>
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        public int GetNowEvaluateScore(string kifuId, string turnNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT SUM(SSI.SCORE) AS SUM_SCORE");
            sb.AppendLine("  FROM ST_SCORE_INDEX AS SSI");
            sb.AppendLine(" INNER JOIN MS_TURN_STAGE AS MTS");
            sb.AppendLine("    ON MTS.STAGE = SSI.STAGE");
            sb.AppendLine(" INNER JOIN AR_KIFU_INDEX AS AKI");
            sb.AppendLine("    ON AKI.KIFU_ID = " + kifuId);
            sb.AppendLine("   AND AKI.TURN_NO = " + turnNo);
            sb.AppendLine("   AND AKI.INDEX_NO = SSI.INDEX_NO");
            sb.AppendLine("   AND AKI.INDEX_NAME = SSI.INDEX_NAME");
            sb.AppendLine("   AND MTS.TURN_NO = AKI.TURN_NO");

            return int.Parse(SQL.ReadDataAdapter(sb.ToString()).Rows[0]["SUM_SCORE"].ToString());
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

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE ST_SCORE_INDEX AS TGT_SSI");
            sb.AppendLine("   SET SCORE = SCORE + " + score.ToString());
            sb.AppendLine(" WHERE EXISTS(SELECT 1");
            sb.AppendLine("                FROM AR_KIFU_INDEX AS AKI");
            sb.AppendLine("               INNER JOIN MS_TURN_STAGE AS MTS");
            sb.AppendLine("                  ON MTS.TURN_NO = AKI.TURN_NO");
            sb.AppendLine("               WHERE AKI.KIFU_ID = " + kifuId);
            sb.AppendLine("                 AND AKI.TURN_NO = " + turnNo);
            sb.AppendLine("                 AND AKI.INDEX_NAME = TGT_SSI.INDEX_NAME");
            sb.AppendLine("                 AND AKI.INDEX_NO = TGT_SSI.INDEX_NO");
            sb.AppendLine("                 AND MTS.STAGE = TGT_SSI.STAGE)");
            cmdList.Add(sb.ToString());

            StringBuilder sbS = new StringBuilder();
            sbS.AppendLine("UPDATE AR_KIFU_INDEX");
            sbS.AppendLine("   SET CALCD_FLG = " + STR.GetSqlString(TCST.SQL_FLG_YES));
            sbS.AppendLine(" WHERE KIFU_ID = " + kifuId);
            sbS.AppendLine("   AND TURN_NO = " + turnNo);
            cmdList.Add(sbS.ToString());

            // 更新処理
            SQL.ExecuteCommand(cmdList);
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
