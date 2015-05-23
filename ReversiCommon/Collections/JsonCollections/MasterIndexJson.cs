using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ReversiCommon.Collections.JsonCollections
{
    /// <summary>
    /// マスターインデックスJSONクラス
    /// </summary>
    [DataContract]
    public class MasterIndexJson
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// インデックス名
        /// </summary>
        [DataMember]
        public string IndexName { get; set; }

        /// <summary>
        /// 正規化インデックスNo
        /// </summary>
        [DataMember]
        public string NormalizeIndexNo { get; set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MasterIndexJson()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="sparseItem"></param>
        public MasterIndexJson(string indexName, string normalizeIndexNo)
        {
            this.IndexName = indexName;
            this.NormalizeIndexNo = normalizeIndexNo;
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
