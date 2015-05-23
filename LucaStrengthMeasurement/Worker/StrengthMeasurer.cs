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
using LucaStrengthMeasurement.Collections;

namespace LucaStrengthMeasurement.Worker
{
    /// <summary>
    /// パフォーマンス測定クラス
    /// </summary>
    public class StrengthMeasurer
    {
        #region "定数"
        /// <summary>
        /// 測定する回数
        /// </summary>
        private const int MEASURMENT_LINE = 5;

        /// <summary>
        /// スコアインデックスファイルA
        /// </summary>
        private const string SCORE_INDEX_FILE_PATH_A = @"C:\work\visualstudio\Luca_TRUNK\LucaDevelop\csv\score_index\score.{0}";

        /// <summary>
        /// スコアインデックスファイルB
        /// </summary>
        private const string SCORE_INDEX_FILE_PATH_B = @"C:\work\visualstudio\bk\20131120_1_indexファイル退避\score_index\score.{0}";
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
        public StrengthMeasurer(string description)
        {
            this.m_Description = description;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 測定を実行する
        /// </summary>
        public void Excute()
        {
            // 初期化
            this.Initialize();

            // 対戦を実行する
            this.DummyExecute();

            // 測定結果をファイルに出力する
            string resultPath = string.Format(ConfigurationManager.AppSettings["Result File Path"], DateTime.Now.ToString("yyyyMMddhhmmss"),  this.m_Description);
            FileUtility.WriteLine(this.m_Description, resultPath, false);
            FileUtility.WriteLine(BattleResult.ToString(), resultPath);

            // 逆で初期化
            this.Initialize(true);

            // 対戦を実行する
            this.DummyExecute();

            // 測定結果をファイルに出力する
            FileUtility.WriteLine(this.m_Description, resultPath, true);
            FileUtility.WriteLine(BattleResult.ToString(), resultPath);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 初期化する
        /// </summary>
        private void Initialize(bool isReverse = false)
        {
            if (!isReverse)
            {
                m_PlayerList = new Dictionary<ReversiCommon.Collections.Color, Player>();
                m_PlayerList.Add(ReversiCommon.Collections.Color.White, new PlayerCpu("旧バージョン", ReversiCommon.Collections.Color.White, new StrategyMeasurementCpu(SCORE_INDEX_FILE_PATH_A, 1, 0)));
                m_PlayerList.Add(ReversiCommon.Collections.Color.Black, new PlayerCpu("新バージョン", ReversiCommon.Collections.Color.Black, new StrategyMeasurementCpu(SCORE_INDEX_FILE_PATH_B, 5, -3)));

                BattleResult.SetPlayerName(m_PlayerList[Color.Black].Name, m_PlayerList[Color.White].Name);
            }
            else
            {
                m_PlayerList = new Dictionary<ReversiCommon.Collections.Color, Player>();
                m_PlayerList.Add(ReversiCommon.Collections.Color.White, new PlayerCpu("新バージョン", ReversiCommon.Collections.Color.White, new StrategyMeasurementCpu(SCORE_INDEX_FILE_PATH_B, 5, -3)));
                m_PlayerList.Add(ReversiCommon.Collections.Color.Black, new PlayerCpu("旧バージョン", ReversiCommon.Collections.Color.Black, new StrategyMeasurementCpu(SCORE_INDEX_FILE_PATH_A, 1, 0)));

                BattleResult.SetPlayerName(m_PlayerList[Color.Black].Name, m_PlayerList[Color.White].Name);
            }
        }

        /// <summary>
        /// ダミーリバーシを実行する
        /// </summary>
        private void DummyExecute()
        {
            // 対局していく
            int i = 1;
            while(true)
            {
                Console.WriteLine(string.Format("◇{0}戦目 Start◇", i));
                TurnKeeper.InitializeTurnKeeper();
                Board.InitializeBoard();

                int turn = 0;
                while(true)
                {
                    Console.WriteLine(string.Format("- {0}ターン目 -", turn));
                    if (Board.IsReversible())
                    {
                        // 手を選択
                        Disc disc = m_PlayerList[TurnKeeper.NowTurnColor].GetNextPointer();
                        Console.WriteLine(ReversiInformation.ToString());

                        // 裏返す
                        Board.Reverse(disc.Point);

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
                            // 手を選択
                            Disc disc = m_PlayerList[TurnKeeper.NowTurnColor].GetNextPointer();
                            Console.WriteLine(ReversiInformation.ToString());

                            // 裏返す
                            Board.Reverse(disc.Point);

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

                // 結果を記録
                BattleResult.Register(i);
                Console.WriteLine(BattleResult.ToString());
                i++;

                if (i > MEASURMENT_LINE) { return; }
            }
        }
        #endregion
        #endregion
    }
}
