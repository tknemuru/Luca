using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TKCommon.Utility;
using System.Diagnostics;
using ReversiCommon.Utility;
using TKCommon.Collections;
using TKCommon.Debugger;

namespace Luca.Optimization.Program
{
    /// <summary>
    /// 最急降下法（巨大疎行列使用バージョン）
    /// </summary>
    public class SteepestDescentUsingSparseBigMatrixOld
    {
        #region "定数"
        /// <summary>
        /// 反復計算をする最大回数
        /// </summary>
        private readonly int MAX_CALC_COUNT;

        /// <summary>
        /// 同一許容値で実行する回数
        /// </summary>
        private const int ONE_SPAN = 5;

        /// <summary>
        /// 許容値
        /// </summary>
        private static readonly List<double> EPS_LIST = new List<double>() { 3, 6, 10, 15, 20, 30, 50, 80, 120 };

        /// <summary>
        /// 誤差が広がっている場合はtrueを格納していく
        /// </summary>
        private List<bool> m_MseCheckList;

        /// <summary>
        /// 前回の誤差
        /// </summary>
        private double m_LastMse;
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SteepestDescentUsingSparseBigMatrixOld()
        {
            MAX_CALC_COUNT = (ONE_SPAN * EPS_LIST.Count);
            this.InitializeMseChecker();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 最急降下法を実行する
        /// </summary>
        /// <returns></returns>
        public SparseVector<double> Execute(SparseBigMatrix A, List<double> b, SparseVector<double> x)
        {
            SparseVector<double> trueX = new SparseVector<double>(0);
            //double alpha = 0.00512D;
            double alpha = 0.01;
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    x = new SparseVector<double>(A.Width, 0);
                    this.InitializeMseChecker();
                    trueX = this.Execute(A, b, x, alpha);
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine(ex.Message + " " + i.ToString() + "回目 α：" + alpha.ToString());
                    alpha = alpha * 0.8;
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
        public SparseVector<double> Execute(SparseBigMatrix A, List<double> b, SparseVector<double> x, double alpha)
        {
            for (int i = 1; i < MAX_CALC_COUNT; i++)
            {
                SparseVector<double> derivatives;
                SparseVector<double> N;
                Dictionary<int, double> derivativesDic = new Dictionary<int, double>();
                Dictionary<int, double> NDic = new Dictionary<int, double>();
                double mse = 0;

                foreach (SparseVector<double> vector in A)
                {
                    double score = 0;
                    //StopWatchLogger.StartEventWatch("scoreの算出");
                    foreach (KeyValuePair<int, double> keyValue in vector.NoSparseKeyValues)
                    {
                        score += x[keyValue.Key] * keyValue.Value;
                    }
                    //StopWatchLogger.StopEventWatch("scoreの算出");

                    // ある局面の誤差
                    //StopWatchLogger.StartEventWatch("ある局面の誤差");
                    double err = b[A.Height - 1] - score;
                    mse += err * err;
                    //StopWatchLogger.StopEventWatch("ある局面の誤差");

                    //StopWatchLogger.StartEventWatch("ある局面の誤差 * ある局面のあるパラメータの合計を求めていく");
                    foreach (KeyValuePair<int, double> keyValue in vector.NoSparseKeyValues)
                    {
                        // ある局面の誤差 * ある局面のあるパラメータの合計を求めていく
                        //derivatives[keyValue.Key] += err * keyValue.Value;
                        //N[keyValue.Key] += 1;
                        if (derivativesDic.ContainsKey(keyValue.Key))
                        {
                            derivativesDic[keyValue.Key] += (err * keyValue.Value);
                        }
                        else
                        {
                            derivativesDic.Add(keyValue.Key, (err * keyValue.Value)); 
                        }

                        if (NDic.ContainsKey(keyValue.Key))
                        {
                            NDic[keyValue.Key] += 1;
                        }
                        else
                        {
                            NDic.Add(keyValue.Key, 1);
                        }
                    }
                    //StopWatchLogger.StopEventWatch("ある局面の誤差 * ある局面のあるパラメータの合計を求めていく");
                }
                derivatives = new SparseVector<double>(x.Count(), derivativesDic, 0);
                N = new SparseVector<double>(x.Count(), NDic, 0);

                StopWatchLogger.DisplayAllEventTimes();

                if (!this.MseValidation(mse / (double)A.Height))
                {
                    DebugUtility.OutputStringToConsole("EXPLOSION!! LOOP : " + i.ToString() + " Mse : " + mse.ToString());
                    DebugUtility.OutputStringToConsole("EXPLOSION!! LOOP : " + i.ToString() + " Avg.err^2 : " + ((mse / (double)A.Height).ToString()));
                    throw new ApplicationException("暴発しています。");
                }

                foreach (KeyValuePair<int, double> derivative in derivatives.NoSparseKeyValues)
                {
                    double normalizeAlpha = MathUtility.Min<double>((alpha / 100), (alpha / N[derivative.Key]));
                    x[derivative.Key] += (normalizeAlpha * derivative.Value);
                    //x[derivative.Key] += ((2 * alpha) / (double)A.Height) * derivative.Value;
                }

                DebugUtility.OutputStringToConsole("LOOP : " + i.ToString() + " Mse : " + mse.ToString());
                DebugUtility.OutputStringToConsole("LOOP : " + i.ToString() + " Avg.err^2 : " + (mse / (double)A.Height).ToString());
                if (this.GetEps(i) > (mse / (double)A.Height)) { return x; }
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
            if (this.m_LastMse < mse)
            {
                this.m_MseCheckList.Add(true);
            }
            else
            {
                this.m_MseCheckList = new List<bool>();
            }

            double errRange = 0D;
            if (this.m_LastMse != 0)
            {
                errRange = (mse / this.m_LastMse);
            }
            this.m_LastMse = mse;

            if ((this.m_MseCheckList.Count < 3) && (errRange < 1.5))
            {
                return true;
            }
            else
            {
                this.m_MseCheckList = new List<bool>();
                return false;
            }
        }

        /// <summary>
        /// MSEチェック関連の情報を初期化する
        /// </summary>
        private void InitializeMseChecker()
        {
            this.m_MseCheckList = new List<bool>();
            this.m_LastMse = 0;
        }
        #endregion
        #endregion
    }
}
