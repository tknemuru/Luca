using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.TypedDataSet;
using System.IO;
using TKCommon.Utility;
using LucaDevelop.Collections;
using ReversiCommon.Collections;
using System.Diagnostics;
using ReversiCommon.Utility;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// 棋譜ファイル修復ファイル
    /// </summary>
    public class KifuFileRepair
    {
        #region "定数"
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
        /// <summary>
        /// ファイルパス
        /// </summary>
        protected string m_Path;

        /// <summary>
        /// ファイルパターン
        /// </summary>
        protected string m_Pattern;

        /// <summary>
        /// rowを生成するために使用するダミーテーブル
        /// </summary>
        private static ReversiCommonDataSet.AR_DEVELOP_KIFU_INDEXDataTable m_DummyDt;

        /// <summary>
        /// 棋譜書き込みインスタンス
        /// </summary>
        private KifuRegister m_Register;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KifuFileRepair(string path, string pattern)
        {
            this.m_Path = path;
            this.m_Pattern = pattern;
            m_DummyDt = new ReversiCommonDataSet.AR_DEVELOP_KIFU_INDEXDataTable();
            this.m_Register = new KifuRegister();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 棋譜ファイルを修復する
        /// </summary>
        public void Repair()
        {
            IEnumerable<string> files = Directory.EnumerateFiles(this.m_Path, this.m_Pattern, SearchOption.AllDirectories);

            foreach (string file in files)
            {
                // 棋譜データの取得
                List<string> orgKifuList = FileUtility.ReadListData(file);

                List<string> expKifuList = new List<string>();
                int i = 1;
                foreach (string line in orgKifuList)
                {
                    List<string> kifuItem = line.Split(' ').ToList();

                    //if ((kifuItem[(int)KIFU.KIFU_1].Length != 20)) { continue; }

                    // 棋譜を連結
                    string kifu = kifuItem[(int)KIFU.KIFU_1] + kifuItem[(int)KIFU.KIFU_2];
                    List<string> paths = file.Replace(@"\", "/").Split('/').ToList();
                    RawKifu rawKifu = new RawKifu(kifu, int.Parse(kifuItem[(int)KIFU.BLACK_SCORE]), paths.Last());

                    // ダミーリバーシ実行
                    if (this.DummyExecute(rawKifu))
                    {
                        expKifuList.Add(line);
                    }
                    else
                    {
                        Console.WriteLine("不正な行を見つけました。 行：" + i.ToString() + " ファイル：" + paths.Last());
                    }

                    i++;
                }

                // 修正したファイルを出力する
                FileUtility.WriteListData(expKifuList, file + ".repair");
            }
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// <para>ダミーリバーシを実行する</para>
        /// </summary>
        private bool DummyExecute(RawKifu rawKifu)
        {
            // 対局していく
            TurnKeeper.InitializeTurnKeeper();
            Board.InitializeBoard();

            List<string> list = StringUtility.ToListSplitCount(rawKifu.Kifu, 2);

            // 座標変換ができない箇所が含まれている場合はNG
            var chkList = from a in list
                          select ReversiConvertUtility.IsConvertableToThorPoint(a);
            if (chkList.Contains(false))
            {
                Console.WriteLine("座標変換できませんでした。");
                return false;
            }
            
            var cnvList = from a in list
                          select ReversiConvertUtility.ConvertToThorPoint(a);

            foreach (string point in cnvList)
            {
                if (Board.IsReversible())
                {
                    // 裏返せる？
                    if (!Board.IsReversibleThisPoint(int.Parse(point)))
                    {
                        Console.WriteLine("裏返せませんでした。");
                        return false;
                    }

                    // 裏返す
                    Board.Reverse(int.Parse(point));

                    // ターンをまわす
                    TurnKeeper.ChangeTurn();
                }
                else
                {
                    // パス
                    TurnKeeper.ChangeOnlyColor();

                    // 自分自身は裏返せる？
                    if (Board.IsReversible())
                    {
                        // 裏返せる？
                        if (!Board.IsReversibleThisPoint(int.Parse(point)))
                        {
                            Console.WriteLine("裏返せませんでした。");
                            return false;
                        }

                        // 裏返す
                        Board.Reverse(int.Parse(point));

                        // ターンをまわす
                        TurnKeeper.ChangeTurn();
                    }
                    else
                    {
                        // ゲーム終了
                        return true;
                    }
                }
            }

            return true;
        }
        #endregion
        #endregion
    }
}
