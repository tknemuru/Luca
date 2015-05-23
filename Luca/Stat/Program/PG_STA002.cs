using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Stat.Logic;
using ReversiCommon.Collections;
using System.Data;
using ReversiCommon.Utility;
using TKCommon.Utility;
using STR = TKCommon.Utility.StringUtility;
using System.Diagnostics;
using ReversiCommon.Evaluators;


namespace Luca.Stat.Program
{
    /// <summary>
    /// <para>評価値統計プログラム（高速バージョン）</para>
    /// </summary>
    public class PG_STA002
    {
        #region "定数"
        /// <summary>
        /// <para>更新の度合い</para>
        /// </summary>
        private const double UPDATE_RATIO = 0.003;

        /// <summary>
        /// <para>１石あたりの評価値</para>
        /// </summary>
        private const double DISC_VALUE = 1000;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>ロジッククラス</para>
        /// </summary>
        private STA002Logic m_logic;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public PG_STA002()
        {
            this.m_logic = new STA002Logic();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>ダミーリバーシを実行する</para>
        /// </summary>
        public void DummyExecute()
        {
            // ThorファイルをDBから読み込む
            DataTable dt = this.m_logic.GetRawKifuData();

            // 対局していく
            foreach (DataRow row in dt.Rows)
            {
                TurnKeeper.InitializeTurnKeeper();
                Board.InitializeBoard();
                List<string> list = STR.ToListSplitCount(row["KIFU"].ToString(), 2);
                ScoreIndexEvaluator ev = new ScoreIndexEvaluator();
                foreach (string point in list)
                {
                    //DebugUtility.OutputStringToConsole(point + "：" + TurnKeeper.NowTurnColor.ColorId + "：" + TurnKeeper.NowTurnNumber.ToString() + "ターン目");
                    //DebugUtility.OutputBoardToConsole();

                    if (Board.IsReversible())
                    {
                        // 裏返す
                        Board.Reverse(int.Parse(point));

                        int theoryScore = int.Parse(row["THEORY_BLACK_SCORE"].ToString());
                        double nowScore = ev.GetEvaluate();

                        // 更新値の算出
                        int updateScore = (int)((double)((theoryScore * DISC_VALUE) - nowScore) * UPDATE_RATIO);

                        // 評価値の更新
                        this.m_logic.UpdateScoreIndex(row["KIFU_ID"].ToString(), TurnKeeper.NowTurnNumber.ToString(), updateScore);

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
                            // 裏返す
                            Board.Reverse(int.Parse(point));

                            int theoryScore = int.Parse(row["THEORY_BLACK_SCORE"].ToString());
                            double nowScore = ev.GetEvaluate();

                            // 更新値の算出
                            int updateScore = (int)((double)((theoryScore * DISC_VALUE) - nowScore) * UPDATE_RATIO);

                            // 評価値の更新
                            this.m_logic.UpdateScoreIndex(row["KIFU_ID"].ToString(), TurnKeeper.NowTurnNumber.ToString(), updateScore);

                            // ターンをまわす
                            TurnKeeper.ChangeTurn();
                        }
                        else
                        {
                            // ゲーム終了
                            break;
                        }
                    }
                }

                // 棋譜データを計算済にする
                this.m_logic.UpdateCalcdFlg(row["KIFU_ID"].ToString());
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
