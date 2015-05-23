using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TKCommon.Utility;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// 実行ファイル配布クラス
    /// </summary>
    public class FileDistributer
    {
        #region "定数"
        /// <summary>
        /// コピー元ディレクトリ
        /// </summary>
        private const string SOURCE_DIRECTORY = @"C:\work\visualstudio\Luca_TRUNK\LucaDevelop\bin\Debug";

        /// <summary>
        /// コピー元ディレクトリ
        /// </summary>
        private const string DESTINATION_DIRECTORY = @"C:\work\visualstudio\Luca_TRUNK\bk\20131110_IOS実行場所\";
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 実行ファイルを配布する
        /// </summary>
        public void Distribute()
        {
            for (int i = 1; i <= 37; i++)
            {
                FileUtility.CopyDirectory(SOURCE_DIRECTORY, DESTINATION_DIRECTORY + i.ToString());
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
