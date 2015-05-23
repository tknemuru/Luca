using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using ReversiCommon.Collections;
using STR = TKCommon.Utility.StringUtility;
using TKCommon.Collections;
using System.Diagnostics;
using ReversiCommon.Utility;

namespace Luca.Optimization.Logic
{
    /// <summary>
    /// JSONインデックス行列出力ロジッククラス
    /// </summary>
    public class OutputJsonIndexMatrixLogic
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
        /// <para>行を作成する元のデータを取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetMasterIndexData(List<int> turnList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT MSI.INDEX_NAME");
            sb.AppendLine("     , INN.NORMALIZE_INDEX_NO");
            sb.AppendLine("  FROM MS_INDEX_NORMALIZE AS INN");
            sb.AppendLine(" INNER JOIN MS_INDEX AS MSI");
            sb.AppendLine("    ON MSI.DIGIT = INN.DIGIT");
            sb.AppendLine(" WHERE EXISTS(SELECT 1");
            sb.AppendLine("                FROM AR_KIFU_INDEX AS AKI");
            sb.AppendLine("               WHERE AKI.TURN_NO IN (" + this.GetJoinTurnList(turnList) + ")");
            sb.AppendLine("                 AND AKI.INDEX_NAME = MSI.INDEX_NAME");
            sb.AppendLine("                 AND AKI.NORMALIZE_INDEX_NO = INN.NORMALIZE_INDEX_NO");
            sb.AppendLine("             )");
            //sb.AppendLine("   AND MSI.INDEX_NAME <> 'corner2X5'");
            sb.AppendLine(" GROUP BY MSI.INDEX_NAME");
            sb.AppendLine("     , INN.NORMALIZE_INDEX_NO");
            sb.AppendLine(" ORDER BY MSI.INDEX_NAME");
            sb.AppendLine("     , INN.NORMALIZE_INDEX_NO");

            return SQL.ReadDataAdapterNoTimeOut(sb.ToString());
        }

        /// <summary>
        /// <para>棋譜インデックスデータを取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetKifuIndexData(int turn)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT AKI.INDEX_NAME");
            sb.AppendLine("     , AKI.NORMALIZE_INDEX_NO");
            sb.AppendLine("     , AKI.KIFU_ID");
            sb.AppendLine("     , AKI.TURN_NO");
            sb.AppendLine("     , SUM(AKI.NORMALIZE_TYPE) AS SCORE_VALUE");
            //sb.AppendLine("     , RAW.THEORY_BLACK_SCORE");
            //sb.AppendLine("     , RAW.THEORY_WHITE_SCORE");
            sb.AppendLine("  FROM AR_KIFU_INDEX AS AKI");
            //sb.AppendLine(" INNER JOIN AR_RAW_KIFU AS RAW");
            //sb.AppendLine("    ON RAW.KIFU_ID = AKI.KIFU_ID");
            sb.AppendLine(" WHERE AKI.TURN_NO = " + turn.ToString());
            sb.AppendLine(" GROUP BY AKI.INDEX_NAME");
            sb.AppendLine("     , AKI.NORMALIZE_INDEX_NO");
            sb.AppendLine("     , AKI.KIFU_ID");
            sb.AppendLine("     , AKI.TURN_NO");
            //sb.AppendLine(" ORDER BY AKI.KIFU_ID");

            return SQL.ReadDataAdapterNoTimeOut(sb.ToString());
        }

        /// <summary>
        /// <para>差集合を求めるための棋譜インデックスデータを取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetForExceptKifuIndexData(int turn)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT AKI.INDEX_NAME");
            sb.AppendLine("     , AKI.NORMALIZE_INDEX_NO");
            sb.AppendLine("  FROM AR_KIFU_INDEX AS AKI");
            sb.AppendLine(" WHERE AKI.TURN_NO = " + turn.ToString());
            sb.AppendLine(" GROUP BY AKI.INDEX_NAME");
            sb.AppendLine("     , AKI.NORMALIZE_INDEX_NO");

            return SQL.ReadDataAdapter(sb.ToString());
        }

        /// <summary>
        /// <para>生棋譜データのリストを取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetRawKifuList(int turn)
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("SELECT DISTINCT RAW.KIFU_ID");
            //sb.AppendLine("     , RAW.THEORY_BLACK_SCORE");
            //sb.AppendLine("     , RAW.THEORY_WHITE_SCORE");
            //sb.AppendLine("     , EXT.PARITY AS " + IndexExtraInformation.Parity.Name);
            //sb.AppendLine("     , EXT.BLACK_MOBILITY AS " + IndexExtraInformation.BlackMobility.Name);
            //sb.AppendLine("     , EXT.WHITE_MOBILITY AS " + IndexExtraInformation.WhiteMobility.Name);
            //sb.AppendLine("  FROM AR_KIFU_INDEX AS AKI");
            //sb.AppendLine(" INNER JOIN AR_RAW_KIFU AS RAW");
            //sb.AppendLine("    ON AKI.KIFU_ID = RAW.KIFU_ID");
            //sb.AppendLine(" INNER JOIN AR_KIFU_TURN_EXTEND_INFO AS EXT");
            //sb.AppendLine("    ON AKI.KIFU_ID = EXT.KIFU_ID");
            //sb.AppendLine("   AND AKI.TURN_NO = EXT.TURN_NO");
            //sb.AppendLine(" WHERE AKI.TURN_NO = " + turn.ToString());
            //sb.AppendLine(" ORDER BY RAW.KIFU_ID");

            return SQL.ReadDataAdapter(sb.ToString());
        }

        /// <summary>
        /// <para>行列の高さを取得する</para>
        /// </summary>
        /// <returns></returns>
        public int GetMatrixHeight(List<int> turnList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT COUNT(*) AS CNT");
            sb.AppendLine("FROM (SELECT 1");
            sb.AppendLine("        FROM AR_KIFU_INDEX AS AKI");
            sb.AppendLine("       INNER JOIN AR_RAW_KIFU AS RAW");
            sb.AppendLine("          ON AKI.KIFU_ID = RAW.KIFU_ID");
            sb.AppendLine("       WHERE AKI.TURN_NO IN (" + this.GetJoinTurnList(turnList) + ")");
            sb.AppendLine("       GROUP BY RAW.KIFU_ID");
            sb.AppendLine("           , AKI.TURN_NO) AS TEMP");

            return int.Parse(SQL.ReadDataAdapter(sb.ToString()).Rows[0]["CNT"].ToString());
        }

        /// <summary>
        /// 統計用の行列を生成する
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<List<double>> CreateMatrix(DataTable dt)
        {
            List<List<double>> retMatrix = new List<List<double>>();

            List<double> line = new List<double>();
            int nowTurn = 1;
            foreach (DataRow row in dt.Rows)
            {
                line.Add(double.Parse(row["NORMALIZE_TYPE"].ToString()));
                if (int.Parse(row["TURN"].ToString()) != nowTurn)
                {
                    retMatrix.Add(line);
                    nowTurn = int.Parse(row["TURN"].ToString());
                }
            }

            return retMatrix;
        }

        /// <summary>
        /// スコアインデックスデータを登録する
        /// </summary>
        /// <param name="masterDt"></param>
        /// <param name="x"></param>
        public void InsertScoreIndex(DataTable masterDt, SparseVector<double> x, int stage)
        {
            List<string> cmdList = new List<string>();

            Debug.Assert((masterDt.Rows.Count == x.Count()), "要素数が違います。");

            int i = 0;
            foreach (DataRow masterRow in masterDt.Rows)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ST_SCORE_INDEX");
                sb.AppendLine("     ( INDEX_NAME");
                sb.AppendLine("     , INDEX_NO");
                sb.AppendLine("     , STAGE");
                sb.AppendLine("     , SCORE");
                sb.AppendLine("     )");
                sb.AppendLine("VALUES");
                sb.AppendLine("     ( " + STR.GetSqlString(masterRow["INDEX_NAME"].ToString()));
                sb.AppendLine("     , " + masterRow["NORMALIZE_INDEX_NO"].ToString());
                sb.AppendLine("     , " + stage.ToString());
                sb.AppendLine("     , " + x[i].ToString("R"));
                sb.AppendLine("     )");
                sb.AppendLine("    ON DUPLICATE KEY UPDATE SCORE = SCORE");
                cmdList.Add(sb.ToString());
                i++;
            }

            // 更新処理
            SQL.ExecuteCommand(cmdList);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// カンマで連結したターンのリストを取得する
        /// </summary>
        /// <param name="turnList"></param>
        /// <returns></returns>
        private string GetJoinTurnList(List<int> turnList)
        {
            return string.Join<int>(",", turnList);
        }
        #endregion
        #endregion
    }
}
