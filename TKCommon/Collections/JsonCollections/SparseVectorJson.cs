using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace TKCommon.Collections.JsonCollections
{
    /// <summary>
    /// 疎ベクトルJsonクラス
    /// </summary>
    [DataContract]
    public class SparseVectorJson<T>
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
        public Dictionary<int, T> NoSparseKeyValues { get; set; }

        /// <summary>
        /// 要素数
        /// </summary>
        public int Count { get; set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SparseVectorJson()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sparseItem"> 疎ベクターの大半の成分を構成する要素</param>
        public SparseVectorJson(SparseVector<T> vector, T sparseItem)
        {
            this.SparseItem = sparseItem;
            this.NoSparseKeyValues = vector.NoSparseKeyValues;
            this.Count = vector.Count();
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
