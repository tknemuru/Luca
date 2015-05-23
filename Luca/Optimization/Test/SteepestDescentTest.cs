using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Luca.Optimization.Program;
using TKCommon.Utility;
using TKCommon.Collections;

namespace Luca.Optimization.Test
{
    /// <summary>
    /// 最急降下法テストクラス
    /// </summary>
    [TestFixture]
    public class SteepestDescentTest
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
        /// 最急降下法が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase()]
        public void TestExecute()
        {
            SparseMatrix<double> A = BiCgStabUsingSparseVectorTest.GetTestA();
            SparseVector<double> b = BiCgStabUsingSparseVectorTest.GetTestB();
            SparseVector<double> expectedX = BiCgStabUsingSparseVectorTest.GetTestExpectedX();
            SparseVector<double> x = new SparseVector<double>(0) { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            SteepestDescent logic = new SteepestDescent();
            SparseVector<double> modifydX = logic.Execute(A, b, x);
            int i = 0;
            foreach (double value in modifydX)
            {
                Assert.AreEqual(expectedX[i], value, 1);
                i++;
            }
        }

        /// <summary>
        /// 最急降下法が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase()]
        public void TestEasyExecute()
        {
            SparseMatrix<double> A = BiCgStabUsingSparseVectorTest.GetEasyTestA();
            SparseVector<double> b = BiCgStabUsingSparseVectorTest.GetEasyTestB();
            SparseVector<double> expectedX = BiCgStabUsingSparseVectorTest.GetEasyTestExpectedX();
            SparseVector<double> x = new SparseVector<double>(0) { 1, 1 };

            SteepestDescent logic = new SteepestDescent();
            SparseVector<double> modifydX = logic.Execute(A, b, x);
            int i = 0;
            foreach (double value in modifydX)
            {
                Assert.AreEqual(expectedX[i], value, 1);
                i++;
            }
        }

        /// <summary>
        /// 最急降下法が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase()]
        public void TestExtractExecute()
        {
            SparseMatrix<double> A = BiCgStabUsingSparseVectorTest.GetExtractTestA();
            SparseVector<double> b = BiCgStabUsingSparseVectorTest.GetExtractTestB();
            SparseVector<double> expectedX = BiCgStabUsingSparseVectorTest.GetExtractTestExpectedX();
            SparseVector<double> x = new SparseVector<double>(0) { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            SteepestDescent logic = new SteepestDescent();
            SparseVector<double> modifydX = logic.Execute(A, b, x);
            int i = 0;
            foreach (double value in modifydX)
            {
                Assert.AreEqual(expectedX[i], value, 1);
                i++;
            }
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// テスト用のA（説明変数の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseMatrix<double> GetTestA()
        {
            SparseMatrix<double> A = new SparseMatrix<double>(10, 10, 0);
            A[0, 0] = 5;
            A[0, 1] = 2;

            for (int i = 1; i < 9; i++)
            {
                A[i, (0 + (i - 1))] = 2;
                A[i, (1 + (i - 1))] = 5;
                A[i, (2 + (i - 1))] = 2;
            }

            A[9, 8] = 2;
            A[9, 9] = 5;

            //A.Add(new SparseVector<double>(0) { 5.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            //A.Add(new SparseVector<double>(0) { 2.0, 5.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            //A.Add(new SparseVector<double>(0) { 0.0, 2.0, 5.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            //A.Add(new SparseVector<double>(0) { 0.0, 0.0, 2.0, 5.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
            //A.Add(new SparseVector<double>(0) { 0.0, 0.0, 0.0, 2.0, 5.0, 2.0, 0.0, 0.0, 0.0, 0.0 });
            //A.Add(new SparseVector<double>(0) { 0.0, 0.0, 0.0, 0.0, 2.0, 5.0, 2.0, 0.0, 0.0, 0.0 });
            //A.Add(new SparseVector<double>(0) { 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 5.0, 2.0, 0.0, 0.0 });
            //A.Add(new SparseVector<double>(0) { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 5.0, 2.0, 0.0 });
            //A.Add(new SparseVector<double>(0) { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 5.0, 2.0 });
            //A.Add(new SparseVector<double>(0) { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, 5.0 });

            return A;
        }

        /// <summary>
        /// テスト用のb（結果の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetTestB()
        {
            return new SparseVector<double>(0) { 3.0, 1.0, 4.0, 0.0, 5.0, -1.0, 6.0, -2.0, 7.0, -15.0 };
        }

        /// <summary>
        /// テスト用のx（正しい重み係数）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetTestExpectedX()
        {
            return new SparseVector<double>(0) { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5 };
        }

        /// <summary>
        /// テスト用のA（説明変数の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseMatrix<double> GetExtractTestA()
        {
            SparseMatrix<double> A = new SparseMatrix<double>(10, 10, 0);
            A[0, 2] = 3;
            A[0, 4] = -4;
            A[0, 5] = -1;
            A[0, 6] = 6;
            A[0, 7] = 1;

            A[1, 0] = 1;
            A[1, 5] = 3;
            A[1, 6] = 2;
            A[1, 7] = 1;
            A[1, 9] = 3;

            A[2, 3] = 2;
            A[2, 5] = 1;
            A[2, 7] = 3;
            A[2, 9] = -2;

            A[3, 1] = 2;
            A[3, 3] = 1;
            A[3, 5] = 5;
            A[3, 6] = 1;
            A[3, 9] = -1;

            A[4, 2] = 1;
            A[4, 6] = 1;
            A[4, 9] = 3;

            A[5, 5] = 1;
            A[5, 6] = 2;
            A[5, 8] = 1;

            A[6, 0] = 2;
            A[6, 2] = 1;
            A[6, 7] = 1;

            A[7, 2] = 2;
            A[7, 7] = -1;

            A[8, 1] = 3;
            A[8, 5] = 3;

            A[9, 3] = 1;
            A[9, 9] = 2;

            //                                    0  1  2  3   4   5  6   7  8   9
            //A.Add(new SparseVector<double>(0) { 0, 0, 3, 0, -4, -1, 6,  1, 0,  0 }); // 0
            //A.Add(new SparseVector<double>(0) { 1, 0, 0, 0,  0,  3, 2,  1, 0,  3 }); // 1
            //A.Add(new SparseVector<double>(0) { 0, 0, 0, 2,  0,  1, 0,  3, 0, -2 }); // 2
            //A.Add(new SparseVector<double>(0) { 0, 2, 0, 1,  0,  5, 1,  0, 0, -1 }); // 3
            //A.Add(new SparseVector<double>(0) { 0, 0, 1, 0, 0, 0, 1, 0, 0, 3 }); // 4
            //A.Add(new SparseVector<double>(0) { 0, 0, 0, 0, 0, 1, 2, 0, 1, 0 }); // 5
            //A.Add(new SparseVector<double>(0) { 2, 0, 1, 0, 0, 0, 0, 1, 0, 0 }); // 6
            //A.Add(new SparseVector<double>(0) { 0, 0, 2, 0, 0, 0, 0, -1, 0, 0 }); // 7
            //A.Add(new SparseVector<double>(0) { 0, 3, 0, 0, 0, 3, 0, 0, 0, 0 }); // 8
            //A.Add(new SparseVector<double>(0) { 0, 0, 0, 1, 0, 0, 0, 0, 0, 2 }); // 9



            //                        -1  0  1  2   3   4  5   6  7   8

            return A;
        }

        /// <summary>
        /// テスト用のb（結果の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetExtractTestB()
        {
            // x（正しい重み係数）が{-1, 0, 1, 2, 3, 4, 5, 6, 7, 8}とすると…
            //return new SparseVector<double>(0) { 10, 25, 17, 12, 4, 37, 7, 1, 4, 21 };
            return new SparseVector<double>(0) { 23, 51, 10, 19, 30, 21, 5, -4, 12, 18 };
        }

        /// <summary>
        /// テスト用のx（正しい重み係数）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetExtractTestExpectedX()
        {
            return new SparseVector<double>(0) { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        }

        /// <summary>
        /// テスト用のA（説明変数の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseMatrix<double> GetEasyTestA()
        {
            SparseMatrix<double> A = new SparseMatrix<double>(2, 2, 0);
            A[0, 0] = 35;
            A[0, 1] = 44;
            A[1, 0] = 44;
            A[1, 1] = 56;

            return A;
        }

        /// <summary>
        /// テスト用のb（結果の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetEasyTestB()
        {
            // x（正しい重み係数）が{1, 2, 3}とすると…
            return new SparseVector<double>(0) { 597, 756 };
        }

        /// <summary>
        /// テスト用のx（正しい重み係数）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetEasyTestExpectedX()
        {
            return new SparseVector<double>(0) { 7, 8 };
        }

        /// <summary>
        /// テスト用のA（説明変数の行列）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseMatrix<double> GetTranspositionTestA()
        {
            // 1 2
            // 3 4
            // 5 6
            SparseMatrix<double> A = new SparseMatrix<double>(3, 2, 0);
            A[0, 0] = 1;
            A[0, 1] = 2;
            A[1, 0] = 3;
            A[1, 1] = 4;
            A[2, 0] = 5;
            A[2, 1] = 6;

            return A;
        }

        /// <summary>
        /// テスト用のx（正しい重み係数）を取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetTranspositionTestExpectedX()
        {
            return new SparseVector<double>(0) { 7, 8 };
        }
        #endregion
        #endregion
    }
}
