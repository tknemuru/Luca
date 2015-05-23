using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
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
    /// αβ法探索ベースクラス
    /// </summary>
    public abstract class AlphaBetaBase<T> : ISearch<KeyValuePair<T, double>>
    {
        #region "定数"
        /// <summary>
        /// <para>初期アルファ値</para>
        /// </summary>
        protected const double DEFAULT_ALPHA = double.MinValue;

        /// <summary>
        /// <para>初期ベータ値</para>
        /// </summary>
        protected const double DEFAULT_BETA = double.MaxValue;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>探索する深さ</para>
        /// </summary>
        protected int m_LimitDepth;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public AlphaBetaBase(int limit)
        {
            m_LimitDepth = limit;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<T, double> SearchBestValue()
        {
            KeyValuePair<T, double> ret = this.AlphaBetaMax(1, DEFAULT_ALPHA, DEFAULT_BETA);
            return ret;
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        protected KeyValuePair<T, double> AlphaBetaMax(int depth, double alpha, double beta)
        {
            Debug.Assert(TurnKeeper.NowTurnColor == Color.Black, "黒じゃないのにBetaMaxになっています");

            // 深さ制限に達した
            if (this.IsLimit(depth)) { return this.GetEvaluate(); }

            // 可能な手をすべて生成
            KeyValuePair<T, double> alphaKeyValue = new KeyValuePair<T, double>();
            Dictionary<T, double> leafList = this.GetAllLeaf();
            if (leafList.Count > 0)
            {
                foreach (KeyValuePair<T, double> leaf in leafList)
                {
                    // 前処理
                    this.SearchSetUp(leaf);

                    double value = this.AlphaBetaMin(depth + 1, alpha, beta).Value;
                    
                    // 後処理
                    this.SearchTearDown();

                    // β刈り
                    if (value > alpha)
                    {
                        alpha = value;
                        alphaKeyValue = new KeyValuePair<T, double>(leaf.Key, alpha);
                        if (alpha >= beta) { return new KeyValuePair<T, double>(leaf.Key, beta); }
                    }
                }
            }
            else
            {
                // ▼パスの場合▼
                // 前処理
                this.PassSetUp();

                double value = this.AlphaBetaMin(depth + 1, alpha, beta).Value;

                // 後処理
                this.PassTearDown();
            }

            return alphaKeyValue;
        }

        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        protected KeyValuePair<T, double> AlphaBetaMin(int depth, double alpha, double beta)
        {
            Debug.Assert(TurnKeeper.NowTurnColor == Color.White, "白じゃないのにAlphaMinになっています");

            // 深さ制限に達した
            if (this.IsLimit(depth)) { return this.GetEvaluate(); }

            // 可能な手をすべて生成
            KeyValuePair<T, double> betaKeyValue = new KeyValuePair<T, double>();
            Dictionary<T, double> leafList = this.GetAllLeaf();
            if (leafList.Count > 0)
            {
                foreach (KeyValuePair<T, double> leaf in leafList)
                {
                    // 前処理
                    this.SearchSetUp(leaf);

                    double value = this.AlphaBetaMax(depth + 1, alpha, beta).Value;

                    // 後処理
                    this.SearchTearDown();

                    // α刈り
                    if (value > beta)
                    {
                        beta = value;
                        betaKeyValue = new KeyValuePair<T, double>(leaf.Key, beta);
                        if (beta <= alpha) { return new KeyValuePair<T, double>(leaf.Key, alpha); }
                    }
                }
            }
            else
            {
                // ▼パスの場合▼
                // 前処理
                this.PassSetUp();

                double value = this.AlphaBetaMax(depth + 1, alpha, beta).Value;

                // 後処理
                this.PassTearDown();
            }

            return betaKeyValue;
        }

        /// <summary>
        /// 深さ制限に達した場合にはTrueを返す
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected abstract bool IsLimit(int limit);

        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract KeyValuePair<T, double> GetEvaluate();

        /// <summary>
        /// 全てのリーフを取得する
        /// </summary>
        /// <returns></returns>
        protected abstract Dictionary<T, double> GetAllLeaf();

        /// <summary>
        /// 探索の前処理を行う
        /// </summary>
        protected abstract void SearchSetUp(KeyValuePair<T, double> leaf);

        /// <summary>
        /// 探索の後処理を行う
        /// </summary>
        protected abstract void SearchTearDown();

        /// <summary>
        /// パスの前処理を行う
        /// </summary>
        protected abstract void PassSetUp();

        /// <summary>
        /// パスの後処理を行う
        /// </summary>
        protected abstract void PassTearDown();
        #endregion
        #endregion
    }
}
