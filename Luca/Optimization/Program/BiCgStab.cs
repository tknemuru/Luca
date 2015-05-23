using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Optimization.Logic;
using TKCommon.Utility;

namespace Luca.Optimization.Program
{
    /// <summary>
    /// BiCGSTAB法　実行クラス
    /// </summary>
    public class BiCgStab
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// BiCGSTAB法を実行する
        /// </summary>
        /// <returns></returns>
        public List<double> Execute(List<List<double>> A, List<double> b, List<double> x)
        {
            List<List<double>> cnvA = MathUtility.ConvertSquareMatrix(A);
            List<double> cnvb = MathUtility.VectorByMatrix(MathUtility.TranspositionMatrix(A), b);

            return new BiCgStabLogic().Execute(cnvA, cnvb, x);
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
