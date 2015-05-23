using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;

namespace ReversiCommon.CollectionsLogic
{
    /// <summary>
    /// <para>ターンキーパーロジック</para>
    /// </summary>
    public static class TurnKeeperLogic
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
        /// <para>ターンステージのリストを取得する</para>
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTurnStageList()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            sb.AppendLine("  FROM MS_TURN_STAGE");

            return SQL.ReadDataAdapter(sb.ToString());
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
