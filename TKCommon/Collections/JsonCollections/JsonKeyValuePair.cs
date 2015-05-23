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
    /// JSON用KeyValuePair
    /// </summary>
    [DataContract]
    public class JsonKeyValuePair<TKey, TValue>
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// Key
        /// </summary>
        [DataMember]
        public TKey Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public TValue Value { get; set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonKeyValuePair() { }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        public void Add(dynamic obj)
        {
            this.Key = obj.Key;
            this.Value = obj.Value;
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
