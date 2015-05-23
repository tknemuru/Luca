using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.TypedDataSet;
using TKCommon.Utility;
using LucaDevelop.Collections;
using System.Diagnostics;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// vsOthaVsThellの棋譜読み込みクラス
    /// </summary>
    public class VsOthaVsThellKifuReader : KifuReaderBase
    {
        #region "定数"
        /// <summary>
        /// ひとかたまりの行数
        /// </summary>
        private const int ONE_UNIT_LINE = 4;

        /// <summary>
        /// 棋譜の内容
        /// </summary>
        private enum KIFU
        {
            KIFU_1,
            KIFU_2,
            BLACK_SCORE,
            PROGRAM_1,
            PROGRAM_2,
            GAME_DATE
        }
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VsOthaVsThellKifuReader(string path, string pattern) : base(path, pattern)
        {
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 棋譜リストを登録用フォーマットに変換する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="kifuList"></param>
        /// <returns></returns>
        public override List<RawKifu> ConvertKifuData(List<string> kifuList, string fileName)
        {
            List<RawKifu> retList = new List<RawKifu>();

            int lineCount = 0;
            foreach (string line in kifuList)
            {
                lineCount++;
                List<string> kifuItem = line.Split(' ').ToList();

                //if ((kifuItem[(int)KIFU.KIFU_1].Length != 20)) { continue; }

                // ひとかたまりの先頭じゃなかったらファイルか処理がおかしい
                //Debug.Assert(((lineCount % ONE_UNIT_LINE) == 1), "ひとかたまりの先頭じゃありませんでした。 行：" + lineCount.ToString());

                // 棋譜を連結
                string kifu = kifuItem[(int)KIFU.KIFU_1] + kifuItem[(int)KIFU.KIFU_2];
                List<string> paths = fileName.Replace(@"\", "/").Split('/').ToList();
                retList.Add(new RawKifu(kifu, int.Parse(kifuItem[(int)KIFU.BLACK_SCORE]), paths.Last()));
            }

            return retList;
        }

        /// <summary>
        /// 棋譜リストを登録用フォーマットに変換する（訂正数指定）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="kifuList"></param>
        /// <returns></returns>
        public List<RawKifu> ConvertKifuDataOnlyLine(List<string> kifuList, string fileName, int noRepairLength)
        {
            List<RawKifu> retList = new List<RawKifu>();

            int lineCount = 0;
            foreach (string line in kifuList)
            {
                lineCount++;
                List<string> kifuItem = line.Split(' ').ToList();

                if ((kifuItem[(int)KIFU.KIFU_1].Length != noRepairLength)) { continue; }

                // 前半の棋譜のみ取得
                string kifu = kifuItem[(int)KIFU.KIFU_1];
                retList.Add(new RawKifu(kifu, int.Parse(kifuItem[(int)KIFU.BLACK_SCORE]), "hoge"));
            }

            return retList;
        }
        #endregion
        #endregion
    }
}
