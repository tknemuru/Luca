using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Thor.Logic;
using System.IO;

namespace Luca.Thor
{
    /// <summary>
    /// <para>Thor分析メインプログラム</para>
    /// </summary>
    class PG_THR001
    {
        #region "定数"
        /// <summary>
        /// <para>Thorファイルの場所</para>
        /// </summary>
        private const string FILE_PATH_THOR_ARCHIEVE = @"C:\work\Reversi\kifu\";
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>ロジッククラス</para>
        /// </summary>
        private THR001Logic m_logic;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public PG_THR001()
        {
            this.m_logic = new THR001Logic();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>Thorファイルを読み込んでDBに登録する</para>
        /// </summary>
        public void RegisterThorData()
        {
            IEnumerable<string> files = Directory.EnumerateFiles(FILE_PATH_THOR_ARCHIEVE, "*.wtb", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                // バイトデータの取得
                List<byte> bytes = this.m_logic.GetThorByteData(file);

                // Thorデータのセット
                this.m_logic.SetThorData(bytes, file);
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
