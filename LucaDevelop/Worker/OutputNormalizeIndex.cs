using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using ReversiCommon.Collections;
using TKCommon.Utility;
using ReversiCommon.CollectionsLogic;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// 正規化インデックス出力クラス
    /// </summary>
    public class OutputNormalizeIndex
    {
        #region "定数"
        /// <summary>
        /// 出力パス
        /// </summary>
        private const string OUTPUT_PATH = @"C:\work\visualstudio\Luca_TRUNK\LucaDevelop\csv\normalize_index\{0}";
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 正規化インデックス出力処理を行う
        /// </summary>
        public void Execute()
        {
            DataTable dt = NormalizeIndexLogic.GetNormalizeIndexList();

            this.OutputIndexCsv(dt);
            this.OutputTypeCsv(dt);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// CSVをファイルに出力する
        /// </summary>
        private void OutputIndexCsv(DataTable dt)
        {
            string csv = string.Empty;
            foreach(DataRow row in dt.Rows)
            {
                // 出力文字列の作成
                string item = NormalizeIndex.GetIndexKey(row["INDEX_NO"].ToString(), row["DIGIT"].ToString()) + "," + row["NORMALIZE_INDEX_NO"].ToString();
                if (string.IsNullOrEmpty(csv))
                {
                    csv = item;
                }
                else
                {
                    csv += ("," + item);
                }
            }

            // ファイルの出力
            string outputPath = string.Format(OUTPUT_PATH, "normalize_index.txt");
            FileUtility.Write(csv, outputPath, false);
        }

        /// <summary>
        /// CSVをファイルに出力する
        /// </summary>
        private void OutputTypeCsv(DataTable dt)
        {
            string csv = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                // 出力文字列の作成
                string item = NormalizeIndex.GetIndexKey(row["INDEX_NO"].ToString(), row["DIGIT"].ToString()) + "," + row["NORMALIZE_TYPE"].ToString();
                if (string.IsNullOrEmpty(csv))
                {
                    csv = item;
                }
                else
                {
                    csv += ("," + item);
                }
            }

            // ファイルの出力
            string outputPath = string.Format(OUTPUT_PATH, "normalize_type.txt");
            FileUtility.Write(csv, outputPath, false);
        }
        #endregion
        #endregion
    }
}
