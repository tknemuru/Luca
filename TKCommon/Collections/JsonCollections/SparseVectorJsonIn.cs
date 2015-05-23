using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKCommon.Collections.JsonCollections
{
    /// <summary>
    /// 疎ベクトルJsonクラス
    /// </summary>
    public class SparseVectorJsonIn<T>
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// 疎ベクターの大半の成分を構成する要素
        /// </summary>
        public T SparseItem { get; set; }

        /// <summary>
        /// 要素を構成するディクショナリ
        /// </summary>
        public List<JsonKeyValuePair<int, T>> NoSparseKeyValues { get; set; }

        /// <summary>
        /// 要素数
        /// </summary>
        public int Count { get; set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SparseVectorJsonIn()
        {
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
