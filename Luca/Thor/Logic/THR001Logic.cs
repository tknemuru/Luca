using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TKUtil = TKCommon.Utility.FileUtility;
using STR = TKCommon.Utility.StringUtility;
using SQL = TKCommon.Utility.MySqlUtility;
using Luca.Thor.Data;
using System.IO;

namespace Luca.Thor.Logic
{
    /// <summary>
    /// <para>Thor分析メインプログラムロジッククラス</para>
    /// </summary>
    public class THR001Logic
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
        /// <para>ファイルからThorフォーマットのバイト配列データを取得する</para>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<byte> GetThorByteData(string file)
        {
            return TKUtil.GetByteListData(file);
        }

        /// <summary>
        /// <para>ファイルからThorフォーマットのバイト配列データを取得する</para>
        /// </summary>
        public void SetThorData(List<byte> bytes, string fileName)
        {
            int seq = 1;
            for (int i = ThorData.BYTE_NUM_HEAD - 1; i < bytes.Count; i += ThorData.BYTE_NUM_BODY_INDEX + ThorData.BYTE_NUM_BODY_ESSENTIAL)
            {
                // Thorデータの作成
                ThorData thorData = new ThorData(bytes.Skip(i + ThorData.BYTE_NUM_BODY_INDEX).Take(ThorData.BYTE_NUM_BODY_ESSENTIAL).ToArray());

                // DBに登録
                this.InsertThorData(thorData, fileName, seq);
                seq++;
            }
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// <para>ThorデータをInsertする</para>
        /// </summary>
        /// <param name="thorData"></param>
        private void InsertThorData(ThorData thorData, string fileName, int seq)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AR_RAW_KIFU");
            sb.AppendLine("      ( KIFU_SOURCE");
            sb.AppendLine("      , ACTUAL_BLACK_SCORE");
            sb.AppendLine("      , ACTUAL_WHITE_SCORE");
            sb.AppendLine("      , THEORY_BLACK_SCORE");
            sb.AppendLine("      , THEORY_WHITE_SCORE");
            sb.AppendLine("      , KIFU");
            sb.AppendLine("      , FILE_NAME");
            sb.AppendLine("      , FILE_SEQ_NO");
            sb.AppendLine("      )");
            sb.AppendLine("VALUES( " + STR.GetSqlString("FFO"));
            sb.AppendLine("      , " + thorData.ActualBlackScore.ToString());
            sb.AppendLine("      , " + thorData.ActualWhiteScore.ToString());
            sb.AppendLine("      , " + thorData.TheoryBlackScore.ToString());
            sb.AppendLine("      , " + thorData.TheoryWhiteScore.ToString());
            sb.AppendLine("      , " + STR.GetSqlString(thorData.Kifu));
            sb.AppendLine("      , " + STR.GetSqlString(fileName));
            sb.AppendLine("      , " + seq.ToString());
            sb.AppendLine("      )");

            SQL.ExecuteCommand(sb.ToString());
        }
        #endregion
        #endregion
    }
}
