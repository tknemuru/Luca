using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using STR = TKCommon.Utility.StringUtility;

namespace LucaGraph.Logic
{
    /// <summary>
    /// グラフロジッククラス
    /// </summary>
    public class GraphLogic
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
        /// 分析対象のデータを取得する
        /// </summary>
        public DataTable GetAnalysisData(string indexName, int stage)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT AKI.INDEX_NO");
            sb.AppendLine("     , ARK.THEORY_BLACK_SCORE");
            sb.AppendLine("  FROM AR_KIFU_INDEX AS AKI");
            sb.AppendLine(" INNER JOIN MS_TURN_STAGE AS MTS");
            sb.AppendLine("    ON AKI.TURN_NO = MTS.TURN_NO");
            sb.AppendLine(" INNER JOIN AR_RAW_KIFU AS ARK");
            sb.AppendLine("    ON ARK.KIFU_ID = AKI.KIFU_ID");
            sb.AppendLine(" WHERE AKI.INDEX_NAME = " + STR.GetSqlString(indexName));
            sb.AppendLine("   AND MTS.STAGE = " + stage.ToString());
            sb.AppendLine(" GROUP BY AKI.INDEX_NO");
            sb.AppendLine("     , ARK.THEORY_BLACK_SCORE");
            sb.AppendLine("HAVING COUNT(*) > 10");

            return SQL.ReadDataAdapter(sb.ToString());
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
