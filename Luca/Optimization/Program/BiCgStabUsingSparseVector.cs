using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TKCommon.Collections;
using TKCommon.Utility;
using Luca.Optimization.Logic;

namespace Luca.Optimization.Program
{
    /// <summary>
    /// BiCGSTAB法　実行クラス
    /// </summary>
    public class BiCgStabUsingSparseVector
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
        public SparseVector<double> Execute(SparseMatrix<double> A, SparseVector<double> b, SparseVector<double> x)
        {
            SparseMatrix<double> cnvA = SparseVectorUtility.ConvertSquareMatrix(A);
            SparseVector<double> cnvb = SparseVectorUtility.VectorByMatrix(SparseVectorUtility.TranspositionMatrix(A), b);

            return new BiCgStabUsingSparseVectorLogic().Execute(cnvA, cnvb, x);
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
