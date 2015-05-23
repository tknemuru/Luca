using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Collections;

namespace ReversiCommon.Test
{
    /// <summary>
    /// <para>評価テスト</para>
    /// </summary>
    public static class EvaluateTest
    {
        #region "定数"
        /// <summary>
        /// <para>角の座標</para>
        /// </summary>
        private static readonly List<int> CORNER_POINT = new List<int>() { 11, 18, 81, 88 };

        /// <summary>
        /// <para>C打ち、X打ちの座標</para>
        /// </summary>
        private static readonly List<int> CX_POINT = new List<int>() { 12, 21, 22, 17, 27, 28, 71, 72, 82, 78, 77, 87 };
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>適正と思われる値の場合はTrueを返す</para>
        /// </summary>
        /// <returns></returns>
        public static bool IsValidateEvaluate(int score)
        {
            //var cornerMySelf = from a in Board.Colors
            //                   where CORNER_POINT.Contains(a.Key)
            //                   && a.Value == TurnKeeper.NowTurnColor
            //                   select a;

            //var cornerEnemy = from a in Board.Colors
            //                  where CORNER_POINT.Contains(a.Key)
            //                  && a.Value == TurnKeeper.NowTurnColor.OppositeColor
            //                  select a;

            //if (cornerMySelf.ToList().Count > 0 && cornerEnemy.ToList().Count <= 0 && score < 0)
            //{
            //    return false;
            //}
            //else if (cornerEnemy.ToList().Count > 0 && cornerMySelf.Count() <= 0 && score > 0)
            //{
            //    return false;
            //}

            return true;
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
