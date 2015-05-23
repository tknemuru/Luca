using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Configuration;
using LucaDevelop.Collections;
using ReversiCommon.Utility;
using TKCommon.Utility;
using ReversiCommon.Collections;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// 定石作成クラス
    /// </summary>
    public class BookMaker
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// パリティ
        /// </summary>
        private int m_Parity;

        /// <summary>
        /// 色
        /// </summary>
        private string m_Color;

        /// <summary>
        /// パス
        /// </summary>
        private string m_Path;

        /// <summary>
        /// パターン
        /// </summary>
        private string m_Pattern;

        /// <summary>
        /// Bookディクショナリ
        /// </summary>
        private Dictionary<int, Dictionary<string, int>> bookDictionary;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        public BookMaker(string path, string pattern)
        {
            this.m_Path = path;
            this.m_Pattern = pattern;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 定石を作成する
        /// </summary>
        public void Make()
        {
            this.Make(Color.Black);
            this.Make(Color.White);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 定石を作成する
        /// </summary>
        /// <param name="color"></param>
        private void Make(Color color)
        {
            this.bookDictionary = new Dictionary<int, Dictionary<string, int>>();
            for (int i = 4; i <= 20; i += 2)
            {
                bookDictionary.Add(i, new Dictionary<string, int>());
            }

            IEnumerable<string> files = Directory.EnumerateFiles(this.m_Path, this.m_Pattern, SearchOption.AllDirectories);
            VsOthaVsThellKifuReader reader = new VsOthaVsThellKifuReader("hoge", "hoge");

            this.m_Parity = (color == Color.Black) ? 1 : -1;
            this.m_Color = (color == Color.Black) ? "black" : "white";

            foreach (string file in files)
            {
                // 棋譜データの取得
                List<string> strKifuList = reader.Read(file);

                // 棋譜データを汎用フォーマットに変換
                List<RawKifu> kifuList = reader.ConvertKifuDataOnlyLine(strKifuList, file, 20);

                // 棋譜リストを登録用フォーマットに変換する
                this.Make(kifuList, file);
            }

            foreach (KeyValuePair<int, Dictionary<string, int>> turnBookDic in bookDictionary)
            {
                // 出力文字列の作成
                string csv = string.Empty;
                foreach (KeyValuePair<string, int> keyValue in turnBookDic.Value)
                {
                    string item = keyValue.Key + "," + keyValue.Value;
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
                string outputPath = string.Format(ConfigurationManager.AppSettings["Joseki File Path"], this.m_Color, (turnBookDic.Key / 2).ToString());
                FileUtility.Write(csv, outputPath, false);
            }
        }

        /// <summary>
        /// 定石を作成する
        /// </summary>
        /// <param name="kifuList"></param>
        private void Make(List<RawKifu> kifuList, string file)
        {
            foreach (RawKifu kifu in kifuList)
            {
                if ((kifu.BlackScore * this.m_Parity) <= 0) { continue; }

                List<string> list = StringUtility.ToListSplitCount(kifu.Kifu, 2);
                var cnvList = from a in list
                              select ReversiConvertUtility.ConvertToThorPoint(a);

                string key = string.Empty;
                int score = (kifu.BlackScore * this.m_Parity);
                foreach (string point in cnvList)
                {
                    if (key == string.Empty)
                    {
                        RotateUtility.SetRotateMethod(int.Parse(point));
                        key = RotateUtility.GetRotatePoint(int.Parse(point)).ToString();
                        continue;
                    }

                    key += RotateUtility.GetRotatePoint(int.Parse(point)).ToString();

                    if (bookDictionary[key.Length].ContainsKey(key))
                    {
                        if (bookDictionary[key.Length][key] < score)
                        {
                            bookDictionary[key.Length][key] = score;
                        }
                    }
                    else
                    {
                        bookDictionary[key.Length].Add(key, score);
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}
