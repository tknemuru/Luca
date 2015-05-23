using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReversiCommon.Collections;
using ReversiCommon.Evaluators;

namespace ReversiCommon.Searchs
{
    /// <summary>
    /// <para>NegaScout法探索クラス</para>
    /// </summary>
    public class NegaScout : ISearch
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
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public NegaScout(int limit)
        {
            m_LimitDepth = limit;
        }
        #endregion

        #region "メソッド"
        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        public Disc SearchBestPointer()
        {
            return SearchBestPointer(m_LimitDepth, DEFAULT_ALPHA, DEFAULT_BETA);
        }

        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        private static Disc SearchBestPointer(int limit, int alpha, int beta)
        {
            // 深さ制限に達した
            if (limit == 0) { return Evaluator.GetInstance().GetEvaluate(); }

            // 可能な手をすべて生成
            List<Disc> putablePointerList = Board.GetAllPutablePointer();

            Disc resultAlpha = new Disc(0, Color.Space, DEFAULT_ALPHA);
            Disc resultBeta = new Disc(0, Color.Space, DEFAULT_BETA);
            Disc resultEvaluate = new Disc(0, Color.Space);

            foreach (Disc eachPointer in putablePointerList)
            {
                // 手を打つ
                Board.Reverse(eachPointer.Point);
                resultEvaluate = eachPointer;
                // ターンの変更
                TurnKeeper.ChangeTurn();
                resultEvaluate.SetEvaluate(NegaScout.SearchBestPointer(limit - 1, resultBeta.Evaluate * -1, resultAlpha.Evaluate * -1).OppositeSignEvaluate());
                if (resultEvaluate.Evaluate > resultAlpha.Evaluate && resultEvaluate.Evaluate < resultBeta.Evaluate && !IsFirstNode(limit) && limit <= 2)
                {
                    resultAlpha = eachPointer;
                    resultAlpha.SetEvaluate(NegaScout.SearchBestPointer(limit - 1, beta * -1, resultEvaluate.Evaluate * -1).OppositeSignEvaluate());
                }
                // 手を戻す
                Board.UndoReverse();
                // ターンの変更
                TurnKeeper.UndoTurn();

                resultAlpha = Disc.MaxEvaluate(resultAlpha, resultEvaluate);

                if (resultAlpha.Evaluate >= resultBeta.Evaluate)
                {
                    // ベータ刈り
                    return resultAlpha;
                }

                // 新しいnull windowを設定
                resultBeta.Evaluate = resultAlpha.Evaluate + 1;
            }

            return resultAlpha;
        }

        /// <summary>
        /// <para>最初のノードならばTrueを返す</para>
        /// </summary>
        /// <returns></returns>
        private static bool IsFirstNode(int nowNode)
        {
            return (m_LimitDepth == nowNode);
        }
        #endregion
    }
}
