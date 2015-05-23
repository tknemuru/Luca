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
    /// 正規化インデックスロジッククラス
    /// </summary>
    public static class NormalizeIndexLogic
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
        /// 正規化インデックスリストを取得する
        /// </summary>
        /// <returns></returns>
        public static DataTable GetNormalizeIndexList()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            sb.AppendLine("  FROM MS_INDEX_NORMALIZE");

            return SQL.ReadDataAdapter(sb.ToString());
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
