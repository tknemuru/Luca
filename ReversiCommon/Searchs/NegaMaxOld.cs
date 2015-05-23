using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Collections;
using ReversiCommon.Evaluators;
using TKCommon.Utility;
using ReversiCommon.Test;
using System.Diagnostics;
using ReversiCommon.Utility;
using TKCommon.Debugger;

namespace ReversiCommon.Searchs
{
    /// <summary>
    /// <para>NegaMax法探索クラス</para>
    /// </summary>
    public class NegaMaxOld
    {
        #region "定数"
        /// <summary>
        /// <para>初期アルファ値</para>
        /// </summary>
        private const int DEFAULT_ALPHA = int.MinValue;

        /// <summary>
        /// <para>初期ベータ値</para>
        /// </summary>
        private const int DEFAULT_BETA = int.MaxValue;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>探索する深さ</para>
        /// </summary>
        private static int m_LimitDepth;

        /// <summary>
        /// <para>自分の色</para>
        /// </summary>
        private static Color m_MyColor;

        /// <summary>
        /// <para>評価クラスのインスタンスリスト</para>
        /// </summary>
        private static List<IEvaluator> m_Evaluators;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public NegaMaxOld(int limit, List<IEvaluator> evaluator)
        {
            m_LimitDepth = limit;
            m_MyColor = TurnKeeper.NowTurnColor;
            m_Evaluators = evaluator;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        public Disc SearchBestPointer()
        {
            StopWatchLogger.StartEventWatch("SearchBestPointer");
            Disc ret = this.SearchBestPointer(m_LimitDepth, DEFAULT_ALPHA, DEFAULT_BETA);
            // デバッグ
            DebugUtility.OutputStringToConsole("座標：" + ret.Point.ToString() + " スコア：" + ret.Evaluate.ToString());
            DebugUtility.OutputBoardScoreToConsole();
            StopWatchLogger.StopEventWatch("SearchBestPointer");
            return ret;
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        private Disc SearchBestPointer(int limit, int alpha, int beta)
        {
            // 深さ制限に達した
            if (limit == 0 || TurnKeeper.IsGameEnd()) { return this.GetEvaluate(); }

            // 可能な手をすべて生成
            List<Disc> putablePointerList = Board.GetAllPutablePointer();

            Disc scoreMax = new Disc(0, Color.Space, DEFAULT_ALPHA);
            if (putablePointerList.Count > 0)
            {
                foreach (Disc eachPointer in putablePointerList)
                {
                    // 手を打つ
                    Board.Reverse(eachPointer.Point);
                    // 初回は回転方法を決める
                    if (TurnKeeper.NowTurnNumber == 1)
                    {
                        RotateUtility.SetRotateMethod(eachPointer.Point);
                    }

                    // ターンの変更
                    TurnKeeper.ChangeTurn();

                    Disc tmpDisc = this.SearchBestPointer(limit - 1, -beta, -alpha).OppositeSignEvaluate();
                    eachPointer.Evaluate += tmpDisc.Evaluate;
                    eachPointer.BoardColors = tmpDisc.BoardColors;

                    // デバッグ
                    if (DebugUtility.IS_DEBUG && limit == m_LimitDepth)
                    {
                        Board.Scores.Add(eachPointer.Point, eachPointer.Evaluate);
                    }

                    // 手を戻す
                    Board.UndoReverse();
                    // ターンの変更
                    TurnKeeper.UndoTurn();

                    if (eachPointer.Evaluate >= beta)
                    {
                        // ベータ刈り
                        return eachPointer;
                    }

                    if (eachPointer.Evaluate > scoreMax.Evaluate)
                    {
                        // より良い手が見つかった
                        scoreMax = eachPointer;
                        // α値の更新
                        alpha = MathUtility.Max(alpha, scoreMax.Evaluate);
                    }
                }
            }
            else
            {
                // ▼パスの場合▼

                // 色の変更
                TurnKeeper.ChangeOnlyColor();

                // 次の相手の手をそのまま返す
                scoreMax = this.SearchBestPointer(limit - 1, -beta, -alpha).OppositeSignEvaluate();

                // 色を戻しておく
                TurnKeeper.ChangeOnlyColor();
            }

            return scoreMax;
        }

        /// <summary>
        /// <para>自分の色に従った評価値を返す</para>
        /// </summary>
        /// <returns></returns>
        private Disc GetEvaluate()
        {
            StopWatchLogger.StartEventWatch("SearchBestPointer-GetEvaluate");
            double score = 0;
            foreach (IEvaluator ev in m_Evaluators)
            {
                score += ev.GetEvaluate();
            }
            //return (m_MyColor == Color.Black) ? new Disc(0, Color.Space, score) : new Disc(0, Color.Space, score * -1);
            //score = (TurnKeeper.NowTurnColor == Color.Black) ? score : score * -1;
            //score = (m_MyColor == Color.Black) ? score : score * -1;
            //Debug.Assert(EvaluateTest.IsValidateEvaluate(score), "評価が間違っている可能性が高いです");
            StopWatchLogger.StopEventWatch("SearchBestPointer-GetEvaluate");

            Disc ret = new Disc(0, Color.Space, (int)score);

            if (DebugUtility.IS_DEBUG)
            {
                ret.SetBoardColors();
            }

            return ret;
        }

        /// <summary>
        /// <para>完全読みが評価関数に含まれている場合はTrueを返す</para>
        /// </summary>
        /// <returns></returns>
        private bool IsContainsFinalEvaluate()
        {
            foreach (IEvaluator ev in m_Evaluators)
            {
                if (ev is FinalEvaluator)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #endregion
    }
}
