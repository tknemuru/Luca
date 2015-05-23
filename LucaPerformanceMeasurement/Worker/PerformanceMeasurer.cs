using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LucaDevelop.Worker;
using LucaDevelop.Collections;
using ReversiCommon.Collections;
using TKCommon.Utility;
using ReversiCommon.Utility;
using ReversiCommon.Players;
using ReversiCommon.Strategys;
using System.Configuration;
using TKCommon.Debugger;

namespace LucaPerformanceMeasurement.Worker
{
    /// <summary>
    /// パフォーマンス測定クラス
    /// </summary>
    public class PerformanceMeasurer
    {
        #region "定数"
        /// <summary>
        /// 測定する行数
        /// </summary>
        private const int MEASURMENT_LINE = 1;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>プレイヤーリスト</para>
        /// </summary>
        private static Dictionary<ReversiCommon.Collections.Color, Player> m_PlayerList;

        /// <summary>
        /// 測定対象の概要
        /// </summary>
        private string m_Description;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PerformanceMeasurer(string description)
        {
            this.m_Description = description;
            m_PlayerList = new Dictionary<ReversiCommon.Collections.Color, Player>();
            m_PlayerList.Add(ReversiCommon.Collections.Color.White, new PlayerCpu("CPU1", ReversiCommon.Collections.Color.White, new StrategyCpu()));
            m_PlayerList.Add(ReversiCommon.Collections.Color.Black, new PlayerCpu("CPU2", ReversiCommon.Collections.Color.Black, new StrategyCpu()));
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 測定を実行する
        /// </summary>
        public void Excute()
        {
            VsOthaVsThellKifuReader reader = new VsOthaVsThellKifuReader("hoge", "hoge");
            string file = ConfigurationManager.AppSettings["Kifu File Path"];

            // 棋譜データの取得
            List<string> strKifuList = reader.Read(file);

            // 棋譜データを汎用フォーマットに変換
            List<RawKifu> kifuList = reader.ConvertKifuData(strKifuList, file);

            // ダミーリバーシを実行する
            StopWatchLogger.StartEventWatch("Game");
            this.DummyExecute(kifuList);
            StopWatchLogger.StopEventWatch("Game");

            // 測定結果をファイルに出力する
            string resultPath = string.Format(ConfigurationManager.AppSettings["Result File Path"], DateTime.Now.ToString("yyyyMMddhhmmss"),  this.m_Description);
            FileUtility.WriteLine(this.m_Description, resultPath, false);
            FileUtility.WriteLine(string.Format("{0}戦目まで実行", MEASURMENT_LINE), resultPath);
            StopWatchLogger.WriteAllEventTimes(resultPath);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// ダミーリバーシを実行する
        /// </summary>
        private void DummyExecute(List<RawKifu> kifuList)
        {
            // 対局していく
            int i = 1;
            foreach (RawKifu kifu in kifuList)
            {
                Console.WriteLine(string.Format("◇{0}戦目 Start◇", i));
                TurnKeeper.InitializeTurnKeeper();
                Board.InitializeBoard();

                List<string> list = StringUtility.ToListSplitCount(kifu.Kifu, 2);
                var cnvList = from a in list
                              select ReversiConvertUtility.ConvertToThorPoint(a);

                int turn = 0;
                foreach (string point in cnvList)
                {
                    Console.WriteLine(string.Format("- {0}ターン目 -", turn));
                    if (Board.IsReversible())
                    {
                        // ダミーで手を選択
                        Disc dummyDisc = m_PlayerList[TurnKeeper.NowTurnColor].GetNextPointer();

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
                            // ダミーで手を選択
                            Disc dummyDisc = m_PlayerList[TurnKeeper.NowTurnColor].GetNextPointer();

                            // 裏返す
                            Board.Reverse(int.Parse(point));

                            // ターンをまわす
                            TurnKeeper.ChangeTurn();
                        }
                        else
                        {
                            // ゲーム終了
                            break;
                        }
                    }
                    turn++;
                }
                i++;

                // デバッグ
                if ((i % 10) == 0)
                {
                    Console.WriteLine("完了行：" + i.ToString() + "/" + kifuList.Count().ToString());
                }

                if (i > MEASURMENT_LINE) { return; }
            }
        }
        #endregion
        #endregion
    }
}
