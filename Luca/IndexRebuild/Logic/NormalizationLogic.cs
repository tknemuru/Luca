using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using ReversiCommon.Collections;
using STR = TKCommon.Utility.StringUtility;

namespace Luca.IndexRebuild.Logic
{
    /// <summary>
    /// インデックス正規化ロジック
    /// </summary>
    public class NormalizationLogic
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// SQLコマンドリスト
        /// </summary>
        private List<string> m_SqlCommandList;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NormalizationLogic()
        {
            this.m_SqlCommandList = new List<string>();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>正規化インデックスインサート用のSQLコマンドを追加する</para>
        /// </summary>
        public void AddCommand(int index, int digit, int normalizeIndex, int normalizeType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO MS_INDEX_NORMALIZE");
            sb.AppendLine("     ( INDEX_NO");
            sb.AppendLine("     , DIGIT");
            sb.AppendLine("     , NORMALIZE_INDEX_NO");
            sb.AppendLine("     , NORMALIZE_TYPE");
            sb.AppendLine("     )");
            sb.AppendLine("VALUES");
            sb.AppendLine("     ( " + index.ToString());
            sb.AppendLine("     , " + digit.ToString());
            sb.AppendLine("     , " + normalizeIndex.ToString());
            sb.AppendLine("     , " + normalizeType.ToString());
            sb.AppendLine("     )");

            this.m_SqlCommandList.Add(sb.ToString());
        }

        /// <summary>
        /// <para>正規化したインデックスをDBに記録する</para>
        /// </summary>
        public void ExecuteCommand()
        {
            SQL.ExecuteCommand(this.m_SqlCommandList);
            this.m_SqlCommandList = new List<string>();
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
