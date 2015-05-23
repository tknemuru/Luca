using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Luca.Optimization.Logic;
using TKCommon.Utility;

namespace Luca.Optimization.Test
{
    /// <summary>
    /// BiCGSTAB法テストクラス
    /// </summary>
    [TestFixture]
    public class BiCgStabTest
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
        /// BiCGSTAB法が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase()]
        public void TestExecute()
        {
            List<List<double>> A = BiCgStabTest.GetTestA();
            List<double> b = BiCgStabTest.GetTestB();
            List<double> expectedX = BiCgStabTest.GetTestExpectedX();
            List<double> x = new List<double> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            BiCgStabLogic logic = new BiCgStabLogic();
            List<double> modifydX = logic.Execute(A, b, x);
            int i = 0;
            foreach (double value in modifydX)
            {
                Assert.AreEqual(expectedX[i], value, 0.001);
                i++;
            }
        }

        /// <summary>
        /// BiCGSTAB法が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase()]
        public void TestEasyExecute()
        {
            List<List<double>> A = BiCgStabTest.GetEasyTestA();
            List<double> b = BiCgStabTest.GetEasyTestB();
            List<double> expectedX = BiCgStabTest.GetEasyTestExpectedX();
            List<double> x = new List<double> { 1, 1 };

            BiCgStabLogic logic = new BiCgStabLogic();
            List<double> modifydX = logic.Execute(A, b, x);
            int i = 0;
            foreach (double value in modifydX)
            {
                Assert.AreEqual(expectedX[i], value, 0.001);
                i++;
            }
        }

        /// <summary>
        /// BiCGSTAB法が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase()]
        public void TestExtractExecute()
        {
            List<List<double>> A = BiCgStabTest.GetExtractTestA();
            List<double> b = BiCgStabTest.GetExtractTestB();
            List<double> expectedX = BiCgStabTest.GetExtractTestExpectedX();
            List<double> x = new List<double> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            BiCgStabLogic logic = new BiCgStabLogic();
            List<double> modifydX = logic.Execute(A, b, x);
            int i = 0;
            foreach (double value in modifydX)
            {
                Assert.AreEqual(expectedX[i], value, 0.001);
                i++;
            }
        }

        /// <summary>
        /// BiCGSTAB法が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase()]
        public void TestTranspositionExecute()
        {
            List<List<double>> A = BiCgStabTest.GetTranspositionTestA();
            List<double> expectedX = BiCgStabTest.GetTranspositionTestExpectedX();
            List<double> x = new List<double> { 1, 1 };

            // 正方行列に変換
            List<List<double>> AtA = MathUtility.ConvertSquareMatrix(A);

            // Atx(b)を求める
            List<double> Atx = MathUtility.VectorByMatrix(AtA, expectedX);
            Console.Write("Atx(");
            foreach (double value in Atx)
            {
                Console.Write(value.ToString() + " ");
            }
            Console.WriteLine(")");

            BiCgStabLogic logic = new BiCgStabLogic();
            List<double> modifydX = logic.Execute(AtA, Atx, x);
            int i = 0;
            foreach (double value in modifydX)
            {
                Assert.AreEqual(expectedX[i], value, 0.001);
                i++;
            }
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// テスト用のA（説明変数の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<List<double>> GetTestA()
        {
            List<List<double>> A = new List<List<double>>();
            A.Add(new List<double> { 5.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            A.Add(new List<double> { 2.0, 5.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            A.Add(new List<double> { 0.0, 2.0, 5.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            A.Add(new List<double> { 0.0, 0.0, 2.0, 5.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            A.Add(new List<double> { 0.0, 0.0, 0.0, 2.0, 5.0, 2.0, 0.0, 0.0, 0.0, 0.0 });
            A.Add(new List<double> { 0.0, 0.0, 0.0, 0.0, 2.0, 5.0, 2.0, 0.0, 0.0, 0.0 });
            A.Add(new List<double> { 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 5.0, 2.0, 0.0, 0.0 });
            A.Add(new List<double> { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 5.0, 2.0, 0.0 });
            A.Add(new List<double> { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 5.0, 2.0 });
            A.Add(new List<double> { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 5.0 });

            return A;
        }

        /// <summary>
        /// テスト用のb（結果の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetTestB()
        {
            return new List<double> { 3.0, 1.0, 4.0, 0.0, 5.0, -1.0, 6.0, -2.0, 7.0, -15.0 };
        }

        /// <summary>
        /// テスト用のx（正しい重み係数）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetTestExpectedX()
        {
            return new List<double> { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5 };
        }

        /// <summary>
        /// テスト用のA（説明変数の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<List<double>> GetExtractTestA()
        {
            List<List<double>> A = new List<List<double>>();
            //                        -1  0  1  2   3   4  5   6  7   8
            A.Add(new List<double> { 0, 0, 3, 0, -4, -1, 6, 1, 0, 0 });
            A.Add(new List<double> { 1, 0, 0, 0, 0, 3, 2, 1, 0, 3 });
            A.Add(new List<double> { 0, 0, 0, 2, 0, 1, 0, 3, 0, -2 });
            A.Add(new List<double> { 0, 2, 0, 1, 0, 5, 1, 0, 0, -1 });
            A.Add(new List<double> { 0, 0, 1, 0, 0, 0, 1, 0, 0, 3 });
            A.Add(new List<double> { 0, 0, 0, 0, 0, 1, 2, 0, 1, 0 });
            A.Add(new List<double> { 2, 0, 1, 0, 0, 0, 0, 1, 0, 0 });
            A.Add(new List<double> { 0, 0, 2, 0, 0, 0, 0, -1, 0, 0 });
            A.Add(new List<double> { 0, 3, 0, 0, 0, 3, 0, 0, 0, 0 });
            A.Add(new List<double> { 0, 0, 0, 1, 0, 0, 0, 0, 0, 2 });

            return A;
        }

        /// <summary>
        /// テスト用のb（結果の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetExtractTestB()
        {
            // x（正しい重み係数）が{-1, 0, 1, 2, 3, 4, 5, 6, 7, 8}とすると…
            //return new List<double> { 10, 25, 17, 12, 4, 37, 7, 1, 4, 21 };
            return new List<double> { 23, 51, 10, 19, 30, 21, 5, -4, 12, 18 };
        }

        /// <summary>
        /// テスト用のx（正しい重み係数）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetExtractTestExpectedX()
        {
            return new List<double> { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        }

        /// <summary>
        /// テスト用のA（説明変数の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<List<double>> GetEasyTestA()
        {
            List<List<double>> A = new List<List<double>>();
            //A.Add(new List<double> { 4, 5, 6 });
            //A.Add(new List<double> { 7, 8, 9 });
            A.Add(new List<double> { 35, 44 });
            A.Add(new List<double> { 44, 56 });

            return A;
        }

        /// <summary>
        /// テスト用のb（結果の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetEasyTestB()
        {
            // x（正しい重み係数）が{1, 2, 3}とすると…
            return new List<double> { 597, 756 };
        }

        /// <summary>
        /// テスト用のx（正しい重み係数）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetEasyTestExpectedX()
        {
            return new List<double> { 7, 8 };
        }

        /// <summary>
        /// テスト用のA（説明変数の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<List<double>> GetTranspositionTestA()
        {
            List<List<double>> A = new List<List<double>>();
            A.Add(new List<double> { 1, 2 });
            A.Add(new List<double> { 3, 4 });
            A.Add(new List<double> { 5, 6 });

            return A;
        }

        /// <summary>
        /// テスト用のx（正しい重み係数）を取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetTranspositionTestExpectedX()
        {
            return new List<double> { 7, 8 };
        }
        #endregion
        #endregion
    }
}
