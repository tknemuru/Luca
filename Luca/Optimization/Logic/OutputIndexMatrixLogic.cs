using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using ReversiCommon.Collections;
using STR = TKCommon.Utility.StringUtility;
using TKCommon.Utility;

namespace Luca.Optimization.Logic
{
    /// <summary>
    /// インデックス行列出力ロジッククラス
    /// </summary>
    public class OutputIndexMatrixLogic
    {
        #region "定数"
        /// <summary>
        /// CSVファイルパス
        /// </summary>
        private const string CSV_FILE_PATH = @"C:\work\visualstudio\Luca_TRUNK\Luca\Optimization\Csv\index\";

        /// <summary>
        /// CSVファイル名
        /// </summary>
        private const string CSV_FILE_NAME = @"index_matrix_{0}.csv";
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// CSVファイル名
        /// </summary>
        private string m_CsvFileName;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stage"></param>
        public OutputIndexMatrixLogic(int stage)
        {
            this.m_CsvFileName = CSV_FILE_PATH + string.Format(CSV_FILE_NAME, stage.ToString());
        }
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
            sb.AppendLine("   AND MSI.INDEX_NAME <> 'corner2X5'");
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
            sb.AppendLine("SELECT DISTINCT RAW.KIFU_ID");
            sb.AppendLine("     , RAW.THEORY_BLACK_SCORE");
            sb.AppendLine("     , RAW.THEORY_WHITE_SCORE");
            sb.AppendLine("     , RAW.KIFU");
            sb.AppendLine("  FROM AR_KIFU_INDEX AS AKI");
            sb.AppendLine(" INNER JOIN AR_RAW_KIFU AS RAW");
            sb.AppendLine("    ON AKI.KIFU_ID = RAW.KIFU_ID");
            sb.AppendLine(" WHERE AKI.TURN_NO = " + turn.ToString());
            sb.AppendLine(" ORDER BY RAW.KIFU_ID");

            return SQL.ReadDataAdapter(sb.ToString());
        }

        /// <summary>
        /// CSVにヘッダを書き込む
        /// </summary>
        /// <param name="dt"></param>
        public void WriteCsvHeader(DataTable dt)
        {
            List<string> list = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(row["INDEX_NAME"].ToString() + "." + row["NORMALIZE_INDEX_NO"].ToString());
            }

            list.Add("SCORE");

            CsvUtility.OutputList(list, this.m_CsvFileName, false);
        }

        /// <summary>
        /// CSVに行を書き込む
        /// </summary>
        /// <param name="list"></param>
        public void WriteCsvLine(List<string> list)
        {
            CsvUtility.OutputList(list, this.m_CsvFileName);
        }

        /// <summary>
        /// パリティを取得する
        /// </summary>
        /// <param name="kifu"></param>
        public void GetParity(string kifu)
        {

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
