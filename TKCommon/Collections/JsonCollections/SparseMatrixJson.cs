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
    /// 疎行列Jsonクラス
    /// </summary>
    [DataContract]
    public class SparseMatrixJson<T>
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
        public Dictionary<int, T> NoSparseItemDictionary { get; set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SparseMatrixJson()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="sparseItem"></param>
        public SparseMatrixJson(SparseMatrix<T> matrix, T sparseItem)
        {
            this.Width = matrix.Width;
            this.Height = matrix.Height;
            this.SparseItem = sparseItem;

            this.NoSparseItemDictionary = matrix.NoSparseKeyValues;
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
