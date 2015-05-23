using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Codeplex.Data;
using System.Web.Script.Serialization;

namespace TKCommon.Utility
{
    /// <summary>
    /// Jsonユーティリティ
    /// </summary>
    public static class JsonUtility
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
        /// <para>JSONファイルからリストを取得する</para>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string file)
        {
            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("utf-8")))
            {
                string text = sr.ReadToEnd();
                return (T)DynamicJson.Parse(text);
            }
        }

        /// <summary>
        /// <para>JSONファイルにデータを出力する</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        [Obsolete("最大文字数が限られているので未使用")]
        public static void SerializeOld(dynamic data, string file)
        {
            var ser = new JavaScriptSerializer();
            string json = ser.Serialize(data);

            //書き込むファイルが既に存在している場合は、上書きする
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("utf-8")))
            {
                sw.Write(json);
            }
        }

        /// <summary>
        /// <para>JSONファイルにデータを出力する</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public static void Serialize(dynamic data, string file, bool append = false)
        {
            // ディレクトリの作成
            FileUtility.CreateDirectory(FileUtility.GetFileDirectory(file));

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, append, System.Text.Encoding.GetEncoding("utf-8")))
            {
                var json = DynamicJson.Serialize(data);
                sw.Write(json);
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
