using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TKCommon.Utility;
using System.Diagnostics;
using ReversiCommon.Utility;
using TKCommon.Collections;

namespace Luca.Optimization.Program
{
    /// <summary>
    /// 最急降下法
    /// </summary>
    public class SteepestDescent
    {
        #region "定数"
        /// <summary>
        /// 反復計算をする最大回数
        /// </summary>
        private readonly int MAX_CALC_COUNT;

        /// <summary>
        /// 同一許容値で実行する回数
        /// </summary>
        private const int ONE_SPAN = 100;

        /// <summary>
        /// 許容値
        /// </summary>
        private static readonly List<double> EPS_LIST = new List<double>() { (1.0e-6), (1.0e-4), (1.0e-2), 1 };
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SteepestDescent()
        {
            MAX_CALC_COUNT = (ONE_SPAN * EPS_LIST.Count);
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 最急降下法を実行する
        /// </summary>
        /// <returns></returns>
        public SparseVector<double> Execute(SparseMatrix<double> nA, SparseVector<double> b, SparseVector<double> x)
        {
            SparseVector<double> trueX = new SparseVector<double>(0);
            double alpha = 10D;
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    trueX = this.Execute(nA, b, x, alpha);
                }
                catch (ApplicationException ex)
                {
                    Console.Write(ex.Message + " " + i.ToString() + "回目 α：" + alpha.ToString());
                    alpha = alpha / 2D;
                    continue;
                }
                return trueX;
            }

            return trueX;
        }
        /// <summary>
        /// 最急降下法を実行する
        /// </summary>
        /// <returns></returns>
        public SparseVector<double> Execute(SparseMatrix<double> nA, SparseVector<double> b, SparseVector<double> x, double alpha)
        {
            Random rnd = new Random();
            for (int i = 0; i < x.Count(); i++)
            {
                x[i] = ((double)rnd.Next(10) / 10D) - 0.5;
            }

            ///* scaling the data */
            //for (l = 0; l < NSAMPLE; l++)
            //{
            //    /*    t[l] = t[l] / tmean;*/
            //    for (j = 0; j < XDIM; j++)
            //    {
            //        x[l][j] = x[l][j] / 100.0;
            //    }
            //}
            //SparseMatrix<double> nA = new SparseMatrix<double>(A.Height, A.Width, 0);
            //foreach (KeyValuePair<int, double> keyValue in A.NoSparseKeyValues)
            //{
            //    nA[A.DeserializeX(keyValue.Key), A.DeserializeY(keyValue.Key)] = ((keyValue.Value) / 100);
            //}

            ///* Learning the parameters */
            //for (i = 1; i < 20000; i++) { /* Learning Loop */
            for (int i = 1; i < MAX_CALC_COUNT; i++)
            {
                /* Compute derivatives */
                /* Initialize derivatives */
                //for (j = 0; j < XDIM + 1; j++)
                //{
                //    derivatives[j] = 0.0;
                //}
                SparseVector<double> derivatives = new SparseVector<double>(x.Count(), 0);
                double mse = 0;

                /* update derivatives */
                //for (l = 0; l < NSAMPLE; l++) {
                for (int y = 0; y < nA.Height; y++)
                {
                    /* prediction */
                    //y = a[0];
                    //for (j = 1; j < XDIM + 1; j++)
                    //{
                    //    y += a[j] * x[l][j - 1];
                    //}
                    double score = 0;
                    foreach (KeyValuePair<int, double> keyValue in nA.NoSparseKeyValuesOneRow(y))
                    {
                        score += x[nA.DeserializeX(keyValue.Key)] * keyValue.Value;
                    }

                    ///* error */
                    //err = t[l] - y;
                    ///*      printf("err[%d] = %f\n", l, err);*/
                    // ある局面の誤差
                    double err = b[y] - score;
                    mse += err * err;

                    /* update derivatives */
                    foreach (KeyValuePair<int, double> keyValue in nA.NoSparseKeyValuesOneRow(y))
                    {
                        // ある局面の誤差 * ある局面のあるパラメータの合計を求めていく
                        derivatives[nA.DeserializeX(keyValue.Key)] += err * keyValue.Value;
                    }
                }

                if(!this.MseValidation(mse))
                {
                    throw new ApplicationException("暴発しています。");
                }

                ///* update parameters */
                //for (j = 0; j < XDIM + 1; j++)
                //{
                //    a[j] = a[j] - alpha * derivatives[j];
                //}
                foreach (KeyValuePair<int, double> derivative in derivatives.NoSparseKeyValues)
                {
                    x[derivative.Key] += ((2 * alpha) / (double)nA.Height) * derivative.Value;
                }

                DebugUtility.OutputStringToConsole("LOOP : " + i.ToString() + " Mse : " + mse.ToString());
                DebugUtility.OutputStringToConsole("LOOP : " + i.ToString() + " Avg.err^2 : " + (mse / (double)x.Count()).ToString());
                if (this.GetEps(i) > (mse / (double)x.Count())) { return x; }
            }

            return x;
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 許容値を取得する
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private double GetEps(int i)
        {
            return EPS_LIST[(i / ONE_SPAN)];
        }

        /// <summary>
        /// MSEが暴発していないかチェックする
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        private bool MseValidation(double mse)
        {
            return Math.Abs(mse) < 100000000;
        }
        #endregion
        #endregion
    }
}
