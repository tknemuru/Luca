using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucaDevelop.Collections
{
    /// <summary>
    /// 生棋譜データ
    /// </summary>
    public class RawKifu
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// 棋譜
        /// </summary>
        public string Kifu { get; private set; }

        /// <summary>
        /// 石差数（黒 - 白）
        /// </summary>
        public int BlackScore { get; private set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; private set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="blackScore"></param>
        public RawKifu(string kifu, int blackScore, string fileName)
        {
            this.Kifu = kifu;
            this.BlackScore = blackScore;
            this.FileName = fileName;
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
