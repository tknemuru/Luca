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
    /// αβ法探索クラス
    /// </summary>
    public class AlphaBeta : AlphaBetaBase<int>
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>評価クラスのインスタンスリスト</para>
        /// </summary>
        private static List<IEvaluator> m_Evaluators;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public AlphaBeta(int limit, List<IEvaluator> evaluator)
            : base(limit)
        {
            m_Evaluators = evaluator;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 深さ制限に達した場合にはTrueを返す
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected override bool IsLimit(int limit)
        {
            return (limit == this.m_LimitDepth || TurnKeeper.IsGameEnd());
        }

        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        protected override KeyValuePair<int, double> GetEvaluate()
        {
            StopWatchLogger.StartEventWatch("SearchBestPointer-GetEvaluate");
            double score = 0;
            foreach (IEvaluator ev in m_Evaluators)
            {
                score += ev.GetEvaluate();
            }
            StopWatchLogger.StopEventWatch("SearchBestPointer-GetEvaluate");

            return new KeyValuePair<int,double>(99, (double)score);
        }

        /// <summary>
        /// 全てのリーフを取得する
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<int, double> GetAllLeaf()
        {
            return Board.GetAllPutableIndex();
        }

        /// <summary>
        /// 探索の前処理を行う
        /// </summary>
        protected override void SearchSetUp(KeyValuePair<int, double> leaf)
        {
            // 手を打つ
            Board.Reverse(leaf.Key);
            // 初回は回転方法を決める
            if (TurnKeeper.NowTurnNumber == 1)
            {
                RotateUtility.SetRotateMethod(leaf.Key);
            }

            // ターンの変更
            TurnKeeper.ChangeTurn();
        }

        /// <summary>
        /// 探索の後処理を行う
        /// </summary>
        protected override void SearchTearDown()
        {
            // 手を戻す
            Board.UndoReverse();
            // ターンの変更
            TurnKeeper.UndoTurn();
        }

        /// <summary>
        /// パスの前処理を行う
        /// </summary>
        protected override void PassSetUp()
        {
            // 色の変更
            TurnKeeper.ChangeOnlyColor();
        }

        /// <summary>
        /// パスの後処理を行う
        /// </summary>
        protected override void PassTearDown()
        {
            // 色を戻しておく
            TurnKeeper.ChangeOnlyColor();
        }
        #endregion
        #endregion
    }
}
