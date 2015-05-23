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
    /// 定石分析クラス
    /// </summary>
    public class BookAnalyzer
    {
        #region "定数"
        /// <summary>
        /// 記録開始手
        /// </summary>
        private const int START_LENGTH = 40;
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
        private Dictionary<int, Dictionary<string, BookInfo>> bookInfoDictionary;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        public BookAnalyzer(string path, string pattern)
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
        private void Analyze(Color color)
        {
            this.bookInfoDictionary = new Dictionary<int, Dictionary<string, BookInfo>>();
            for (int i = (START_LENGTH + 2); i <= (START_LENGTH + 20); i += 2)
            {
                bookInfoDictionary.Add(i, new Dictionary<string, BookInfo>());
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
                List<RawKifu> kifuList = reader.ConvertKifuDataOnlyLine(strKifuList, file, (START_LENGTH + 20));

                // 棋譜リストを登録用フォーマットに変換する
                this.Make(kifuList, file);
            }

            foreach (KeyValuePair<int, Dictionary<string, BookInfo>> turnBookDic in bookInfoDictionary)
            {
                // ファイルの出力
                string outputPath = string.Format(ConfigurationManager.AppSettings["Joseki Analysis File Path"], this.m_Color, "disp", (turnBookDic.Key / 2).ToString());

                foreach (KeyValuePair<string, BookInfo> bookInfo in turnBookDic.Value.OrderBy(item => item.Value.Disperse))
                {
                    string item = string.Format("{0},{1},{2},{3}", bookInfo.Key, bookInfo.Value.SumCount, bookInfo.Value.Avg, bookInfo.Value.Disperse);

                    FileUtility.WriteLine(item, outputPath);
                }

                //outputPath = string.Format(ConfigurationManager.AppSettings["Joseki Analysis File Path"], this.m_Color, "count", (turnBookDic.Key / 2).ToString());
                //foreach (KeyValuePair<string, BookInfo> bookInfo in turnBookDic.Value.OrderBy(item => item.Value.SumCount))
                //{
                //    string item = string.Format("{0},{1},{2},{3}", bookInfo.Key, bookInfo.Value.SumCount, bookInfo.Value.Avg, bookInfo.Value.Disperse);

                //    FileUtility.WriteLine(item, outputPath);
                //}
            }
        }

        /// <summary>
        /// 定石を作成する
        /// </summary>
        /// <param name="color"></param>
        private void Make(Color color)
        {
            this.bookInfoDictionary = new Dictionary<int, Dictionary<string, BookInfo>>();
            for (int i = (START_LENGTH + 2); i <= (START_LENGTH + 20); i += 2)
            {
                bookInfoDictionary.Add(i, new Dictionary<string, BookInfo>());
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
                List<RawKifu> kifuList = reader.ConvertKifuDataOnlyLine(strKifuList, file, (START_LENGTH + 20));

                // 棋譜リストを登録用フォーマットに変換する
                this.Make(kifuList, file);
            }

            foreach (KeyValuePair<int, Dictionary<string, BookInfo>> turnBookDic in bookInfoDictionary)
            {
                // 出力文字列の作成
                string csv = string.Empty;
                foreach (KeyValuePair<string, BookInfo> bookInfo in turnBookDic.Value)
                {
                    if (!this.IsAddBook(bookInfo.Value)) { continue; }

                    string item = bookInfo.Key + "," + bookInfo.Value.Avg.ToString("R");
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
        /// 定石に加える場合はtrueを返す
        /// </summary>
        /// <returns></returns>
        private bool IsAddBook(BookInfo info)
        {
            if (info.Avg <= 1)
            {
                return false;
            }

            return ((info.SumCount >= 2D && info.Disperse <= 9D) || (info.SumCount >= 3D && info.Disperse <= 100D));
        }

        /// <summary>
        /// 定石を作成する
        /// </summary>
        /// <param name="kifuList"></param>
        private void Make(List<RawKifu> kifuList, string file)
        {
            foreach (RawKifu kifu in kifuList)
            {
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
                    }

                    if (key.Length < START_LENGTH)
                    {
                        key += RotateUtility.GetRotatePoint(int.Parse(point)).ToString();
                        continue;
                    }

                    key += RotateUtility.GetRotatePoint(int.Parse(point)).ToString();

                    if (bookInfoDictionary[key.Length].ContainsKey(key))
                    {
                        bookInfoDictionary[key.Length][key].Values.Add((double)score);
                    }
                    else
                    {
                        bookInfoDictionary[key.Length][key] = new BookInfo(key, (double)score);
                    }

                    // test
                    if (key.Length == 20 && bookInfoDictionary[key.Length][key].Disperse != 0D)
                    {
                        double dummy = bookInfoDictionary[key.Length][key].Disperse;
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}
