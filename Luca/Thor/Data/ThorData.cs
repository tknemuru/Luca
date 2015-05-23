using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Collections;

namespace Luca.Thor.Data
{
    /// <summary>
    /// <para>Thorデータクラス</para>
    /// </summary>
    public class ThorData
    {
        #region "定数"
        /// <summary>
        /// <para>バイト数：ヘッダ</para>
        /// </summary>
        public const int BYTE_NUM_HEAD = 17;

        /// <summary>
        /// <para>バイト数：ボディ（インデックス情報）</para>
        /// </summary>
        public const int BYTE_NUM_BODY_INDEX = 6;

        /// <summary>
        /// <para>バイト数：ボディ（重要な情報）</para>
        /// </summary>
        public const int BYTE_NUM_BODY_ESSENTIAL = 62;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>大会名</para>
        /// </summary>
        public string CompetitionName { get; private set; }

        /// <summary>
        /// <para>大会名のインデックス</para>
        /// </summary>
        public int CompetitionIndex { get; private set; }

        /// <summary>
        /// <para>黒番の対戦者のインデックス</para>
        /// </summary>
        public int BlackIndex { get; private set; }

        /// <summary>
        /// <para>白番の対戦者のインデックス</para>
        /// </summary>
        public int WhiteIndex { get; private set; }

        /// <summary>
        /// <para>対戦結果（黒石の数）</para>
        /// </summary>
        public int ActualBlackScore { get; private set; }

        /// <summary>
        /// <para>対戦結果（白石の数）</para>
        /// </summary>
        public int ActualWhiteScore { get; private set; }

        /// <summary>
        /// <para>理論スコア（黒石の数）</para>
        /// </summary>
        public int TheoryBlackScore { get; private set; }

        /// <summary>
        /// <para>理論スコア（白石の数）</para>
        /// </summary>
        public int TheoryWhiteScore { get; private set; }

        /// <summary>
        /// <para>棋譜</para>
        /// </summary>
        public string Kifu { get; private set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        /// <param name="bytes"></param>
        public ThorData(byte[] bytes)
        {
            this.SetByteData(bytes);
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// <para>バイトデータをセットする</para>
        /// </summary>
        private void SetByteData(byte[] bytes)
        {
            this.ActualBlackScore = bytes[0];
            this.ActualWhiteScore = Board.SQURE_NUMBER_SUM - this.ActualBlackScore;
            this.TheoryBlackScore = bytes[1];
            this.TheoryWhiteScore = Board.SQURE_NUMBER_SUM - this.TheoryBlackScore;

            string kifu = string.Empty;
            for (int i = 2; i < bytes.Length; i++)
            {
                kifu += bytes[i].ToString();
            }
            this.Kifu = kifu;
        }
        #endregion
        #endregion
    }
}
