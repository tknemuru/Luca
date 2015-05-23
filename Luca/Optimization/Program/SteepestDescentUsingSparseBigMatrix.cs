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
    public class SteepestDescentUsingSparseBigMatrix
    {
        #region "定数"
        /// <summary>
        /// 反復計算をする最大回数
        /// </summary>
        private readonly int MAX_CALC_COUNT;

        /// <summary>
        /// 同一許容値で実行する回数
        /// </summary>
        private const int ONE_SPAN = 100000;

        /// <summary>
        /// 許容値
        /// </summary>
        private static readonly List<double> EPS_LIST = new List<double>() { 1, 5 };
        //private static readonly List<double> EPS_LIST = new List<double>() { 10, 20, 30, 40, 50, 80, 120 };
        //private static readonly List<double> EPS_LIST = new List<double>() { 10, 22 };

        /// <summary>
        /// MSEを求める単位
        /// </summary>
        private const int MSE_ONE_UNIT = 1000;

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
        /// <summary>
        /// Aファイルリスト
        /// </summary>
        private IEnumerable<string> m_AFiles;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SteepestDescentUsingSparseBigMatrix(IEnumerable<string> aFiles)
        {
            MAX_CALC_COUNT = (ONE_SPAN * EPS_LIST.Count);
            this.InitializeMseChecker();
            this.m_AFiles = aFiles;
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
            double alpha = 1;
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
                    A = new SparseBigMatrix(this.m_AFiles, A.Width, 0);
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
            double minErr = double.MaxValue;
            int minPosition = 0;
            for (int i = 1; i < MAX_CALC_COUNT; i++)
            {
                SparseVector<double> N = new SparseVector<double>(x.Count(), 0);
                double mse = 0;
                int count = 0;

                foreach (SparseVector<double> vector in A)
                {
                    double score = 0;
                    count++;
                    // 推定スコアを計算
                    foreach (KeyValuePair<int, double> keyValue in vector.NoSparseKeyValues)
                    {
                        score += x[keyValue.Key] * keyValue.Value;
                        N[keyValue.Key] += Math.Abs(keyValue.Value);
                    }

                    // ある局面の誤差
                    double err = b[A.Height - 1] - score;
                    mse += err * err;

                    // 評価関数の修正
                    foreach (KeyValuePair<int, double> keyValue in vector.NoSparseKeyValues)
                    {
                        double normalizeAlpha = MathUtility.Min<double>((alpha / 100), (alpha / N[keyValue.Key]));
                        x[keyValue.Key] += (normalizeAlpha * err * keyValue.Value);
                    }

                    if ((count % 100) == 0)
                    {
                        DebugUtility.OutputStringToConsole("TRY : " + A.Height.ToString() + " UNIT TRY：" + count.ToString() + " Mse : " + mse.ToString());
                        DebugUtility.OutputStringToConsole("TRY : " + A.Height.ToString() + " UNIT TRY：" + count.ToString() + " Avg.err^2 : " + (mse / (double)count).ToString());
                    }

                    if (count > MSE_ONE_UNIT)
                    {
                        if ((mse / (double)count) < minErr)
                        {
                            minErr = (mse / (double)count);
                            minPosition = A.Height;
                        }
                        DebugUtility.OutputStringToConsole("MIN ERROR : " + minErr.ToString() + " MIN POSITION：" + minPosition.ToString());

                        //if (EPS_LIST.Count <= (A.Height / ONE_SPAN))
                        //{
                        //    throw new ApplicationException("収束しませんでした。");
                        //}

                        if (this.GetEps(A.Height) > (mse / (double)count))
                        {
                            DebugUtility.OutputStringToConsole("END EPS : " + this.GetEps(A.Height).ToString());
                            return x;
                        }
                        mse = 0;
                        count = 0;
                    }
                }
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
            if (EPS_LIST.Count <= (i / ONE_SPAN))
            {
                return EPS_LIST.Last();
            }
            else
            {
                return EPS_LIST[(i / ONE_SPAN)];
            }
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
