using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TKCommon.Utility;
using System.Diagnostics;
using ReversiCommon.Utility;
using TKCommon.Collections;

namespace Luca.Optimization.Logic
{
    /// <summary>
    /// BiCGSTAB法 ロジッククラス
    /// </summary>
    public class BiCgStabUsingSparseVectorLogic
    {
        #region "定数"
        /// <summary>
        /// 反復計算をする最大回数
        /// </summary>
        private readonly int MAX_CALC_COUNT;

        /// <summary>
        /// 同一許容値で実行する回数
        /// </summary>
        private const int ONE_SPAN = 20;

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
        public BiCgStabUsingSparseVectorLogic()
        {
            MAX_CALC_COUNT = (ONE_SPAN * EPS_LIST.Count);
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// BiCGSTAB法を実行する
        /// </summary>
        /// <returns></returns>
        public SparseVector<double> Execute(SparseMatrix<double> A, SparseVector<double> b, SparseVector<double> x)
        {
            //static Vector r0;  // 初期残差ベクトル
            //static Vector r;   // 残差ベクトル
            //static Vector p;   // 探索方向ベクトル

            //static Vector ax;
            //matrix_x_vector(ax, a, x);
            SparseVector<double> Ax = SparseVectorUtility.VectorByMatrix(A, x);

            //// r0,r,pを計算 r0 := b - Ax
            //// p = r = r0
            //for(int i = 0; i < N; i++) {
            //r0[i] = b[i] - ax[i];
            //p[i]  = r[i] = r0[i];
            //}
            Debug.Assert(Ax.Count() == b.Count(), "Axとbのベクトルの成分数が一致しません。");
            long limit = Ax.Count();
            SparseVector<double> r0 = new SparseVector<double>(0);
            SparseVector<double> p = new SparseVector<double>(0);
            SparseVector<double> r = new SparseVector<double>(0);
            for (int i = 0; i < limit; i++)
            {
                r0.Add(b[i] - Ax[i]);
                p.Add(r0[i]);
                r.Add(r0[i]);
            }

            //// (r0, r0*) != 0,   r0* = r0
            //double c1 = inner_product(r0, r0);
            //if(c1 == 0.0) {
            //fprintf(stderr, "(r0, r0*) == 0!!\n");
            //exit(1);
            //}
            double c1 = SparseVectorUtility.DotProduct(r0, r0);
            Debug.Assert((c1 != 0), "(r0, r0*) == 0!!");

            //for(int iter = 1; iter < ITRMAX; iter++) {
            for (int i = 1; i < MAX_CALC_COUNT; i++)
            {
                //static Vector ap;
                //matrix_x_vector(ap, a, p);
                //double c2    = inner_product(r0, ap);
                //double alpha = c1 / c2;
                SparseVector<double> Ap = SparseVectorUtility.VectorByMatrix(A, p);
                double c2 = SparseVectorUtility.DotProduct(r0, Ap);
                double alpha = (c1 / c2);

                //static Vector e;
                //for(int i = 0; i < N; i++) {
                //    e[i] = r[i] - alpha*ap[i];
                //}
                SparseVector<double> e = new SparseVector<double>(0);
                Debug.Assert(Ap.Count() == r.Count(), "Apとrのベクトルの成分数が一致しません。");
                limit = Ap.Count();
                for (int j = 0; j < limit; j++)
                {
                    e.Add(r[j] - (alpha * Ap[j]));
                }

                //static Vector ae;
                //matrix_x_vector(ae, a, e);
                //double e_dot_ae  = inner_product(e, ae);
                //double ae_dot_ae = inner_product(ae, ae);
                //double c3        = e_dot_ae / ae_dot_ae;
                SparseVector<double> Ae = SparseVectorUtility.VectorByMatrix(A, e);
                double eDotAe = SparseVectorUtility.DotProduct(e, Ae);
                double AeDotAe = SparseVectorUtility.DotProduct(Ae, Ae);
                double c3 = (eDotAe / AeDotAe);

                //for(int i = 0; i < N; i++) {
                //    x[i] += alpha*p[i] + c3*e[i];
                //    r[i]  = e[i] - c3*ae[i];
                //}
                Debug.Assert((x.Count() == p.Count()) && (p.Count() == e.Count()) && (e.Count() == r.Count()) && (r.Count() == Ae.Count()), "ベクトルの成分数が一致しません。");
                limit = Ae.Count();
                for (int j = 0; j < limit; j++)
                {
                    x[j] += ((alpha * p[j]) + (c3 * e[j]));
                    r[j] = (e[j] - (c3 * Ae[j]));
                }

                //double err = vector_norm(r)/vector_norm(b);
                //printf("LOOP : %d\t Error : %g\n", iter, err);
                //if(EPS > err) break;
                double error = (SparseVectorUtility.VectorNorm(r) / SparseVectorUtility.VectorNorm(b));
                DebugUtility.OutputStringToConsole("LOOP : " + i.ToString() + " Error : " + error.ToString());
                //if (this.GetEps(i) > error) { this.WriteLineVectorX(x); return x; }
                if (this.GetEps(i) > error) { return x; }

                //c1          = inner_product(r, r0);
                //double beta = c1 / (c2*c3);
                c1 = SparseVectorUtility.DotProduct(r, r0);
                double beta = (c1 / (c2 * c3));

                //for(int i = 0; i < N; i++) {
                //    p[i] = r[i] + beta*(p[i] - c3*ap[i]);
                //} 
                //}
                Debug.Assert((p.Count() == r.Count()) && (r.Count() == Ap.Count()), "ベクトルの成分数が一致しません。");
                limit = p.Count();
                for (int j = 0; j < limit; j++)
                {
                    p[j] = r[j] + (beta * (p[j] - c3 * Ap[j]));
                }
            }

            //this.WriteLineVectorX(x);
            return x;
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// ベクトルXを出力する
        /// </summary>
        /// <param name="x"></param>
        private void WriteLineVectorX(SparseVector<double> x)
        {
            Console.Write("x(");
            foreach (double value in x)
            {
                Console.Write(value.ToString() + " ");
            }
            Console.WriteLine(")");
        }

        /// <summary>
        /// 許容値を取得する
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private double GetEps(int i)
        {
            return EPS_LIST[(i / ONE_SPAN)];
        }
        #endregion
        #endregion
    }
}
