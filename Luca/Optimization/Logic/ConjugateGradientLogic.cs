using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TKCommon.Utility;
using System.Diagnostics;
using ReversiCommon.Utility;

namespace Luca.Optimization.Logic
{
    /// <summary>
    /// 共役勾配法　ロジッククラス
    /// </summary>
    public class ConjugateGradientLogic
    {
        #region "定数"
        /// <summary>
        /// 反復計算をする最大回数
        /// </summary>
        private const int MAX_CALC_COUNT = 100;

        /// <summary>
        /// 許容値
        /// </summary>
        private const double EPS = (1.0e-6);
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 共役勾配法を実行する
        /// </summary>
        /// <param name="A"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public List<double> Execute(List<List<double>> A, List<double> b, List<double> x)
        {
            //static Vector ax;
            //static Vector ap;
            //int           i, iter;

            //// Axを計算
            //vector_x_matrix(ax, a, x);
            List<double> Ax = MathUtility.VectorByMatrix(A, x);


            //static Vector p;   // 探索方向ベクトル
            //static Vector r;   // 残差ベクトル
            //// pとrを計算 p = r := b - Ax
            //for(i = 0; i < N; i++){
            //p[i] = b[i] - ax[i];
            //r[i] = p[i];
            //}
            List<double> p = new List<double>();
            List<double> r = new List<double>();
            int i = 0;
            foreach (double axValue in Ax)
            {
                p.Add(b[i] - axValue);
                r.Add(p[i]);
                i++;
            }

            //// 反復計算
            //for(iter = 1; iter < TMAX; iter++){
            //double alpha, beta, err = 0;
            for (i = 1; i < MAX_CALC_COUNT; i++)
            {
                //// alphaを計算
                //vector_x_matrix(ap, a, p);
                //alpha = +dot_product(p, r)/dot_product(p, ap);
                List<double> ap = MathUtility.VectorByMatrix(A, p);
                double alpha = MathUtility.DotProduct(p, r) / MathUtility.DotProduct(p, ap);

                //for(i = 0; i < N; i++){
                //    x[i] += +alpha*p[i];
                //    r[i] += -alpha*ap[i];
                //}
                Debug.Assert((x.Count() == p.Count()) && (p.Count() == r.Count()) && (r.Count() == ap.Count()), "要素数が一致していません。");

                int limit = ap.Count();
                for (int j = 0; j < limit; j++)
                {
                    x[j] += (alpha * p[j]);
                    r[j] += -(alpha * ap[j]);
                }

                //err = vector_norm(r);   // 誤差を計算
                //printf("LOOP : %d\t Error : %g\n", iter, err);
                //if(EPS > err) break;
                double error = MathUtility.VectorNorm(r);
                DebugUtility.OutputStringToConsole("LOOP : " + i.ToString() + " Error : " + error.ToString());
                if (EPS > error) { return x; }

                //// EPS < err ならbetaとpを計算してループを継続
                //beta = -dot_product(r, ap)/dot_product(p, ap);
                //for(i = 0; i < N; i++){
                //    p[i] = r[i] + beta*p[i];
                //}
                Debug.Assert((r.Count() == ap.Count()) && (ap.Count() == p.Count()), "要素数が一致していません。");
                double beta = -(MathUtility.DotProduct(r, ap) / MathUtility.DotProduct(p, ap));
                limit = ap.Count();
                for (int t = 0; t < limit; t++)
                {
                    p[t] = r[t] + beta * p[t];
                }
            }

            return x;
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
