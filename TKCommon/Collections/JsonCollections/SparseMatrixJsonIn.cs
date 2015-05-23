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
    /// 疎行列Jsonクラス(入力専用)
    /// </summary>
    [DataContract]
    public class SparseMatrixJsonIn<T>
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// 幅
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// 高さ
        /// </summary>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// 疎行列の大半の成分を構成する要素
        /// </summary>
        [DataMember]
        public T SparseItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<JsonKeyValuePair<int, T>> NoSparseItemDictionary { get; set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SparseMatrixJsonIn()
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
