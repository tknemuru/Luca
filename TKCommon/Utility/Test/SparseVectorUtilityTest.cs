using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using TKCommon.Utility;
using TKCommon.Collections;

namespace TKCommon.Utility.Test
{
    /// <summary>
    /// SparseVectorUtilityテストクラス
    /// </summary>
    [TestFixture]
    public class SparseVectorUtilityTest
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
        /// ベクトルに行列を作用させる y = Ax
        /// </summary>
        /// <param name="vectorX"></param>
        /// <param name="matrixA"></param>
        /// <param name="width"></param>
        /// <param name="expectedVectorY"></param>
        [TestCase()]
        public void TestVectorByMatrix()
        {
            SparseVector<double> vectorX = SparseVectorUtilityTest.GetTestVectorX();
            SparseMatrix<double> matrixA = SparseVectorUtilityTest.GetTestMatrixA();
            SparseVector<double> expectedVectorY = SparseVectorUtilityTest.GetTestExpectedVectorY();
            Assert.That(SparseVectorUtility.VectorByMatrix(matrixA, vectorX), Is.EqualTo(expectedVectorY));
        }

        /// <summary>
        /// 内積のテストを行う
        /// </summary>
        [TestCase()]
        public void TestDotProduct()
        {
            SparseVector<double> vectorX = SparseVectorUtilityTest.GetTestVectorX();
            SparseVector<double> vectorY = SparseVectorUtilityTest.GetTestVectorY();
            Assert.That(SparseVectorUtility.DotProduct(vectorX, vectorY), Is.EqualTo(32));
        }

        /// <summary>
        /// ベクトルノルムのテストを行う
        /// </summary>
        [TestCase()]
        public void TestVectorNorm()
        {
            SparseVector<double> vector = new SparseVector<double>(0) { 1, -2, 3 };
            Assert.That(SparseVectorUtility.VectorNorm(vector), Is.EqualTo(6));
        }

        /// <summary>
        /// 行列転置のテストを行う
        /// </summary>
        [TestCase()]
        public void TestTranspositionMatrix()
        {
            // 1 2
            // 3 4
            // 5 6
            SparseMatrix<double> matrix = new SparseMatrix<double>(3, 2, 0);
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;
            matrix[1, 0] = 3;
            matrix[1, 1] = 4;
            matrix[2, 0] = 5;
            matrix[2, 1] = 6;

            // 1 3 5
            // 2 4 6
            SparseMatrix<double> expectedMatrix = new SparseMatrix<double>(2, 3, 0);
            expectedMatrix[0, 0] = 1;
            expectedMatrix[0, 1] = 3;
            expectedMatrix[0, 2] = 5;
            expectedMatrix[1, 0] = 2;
            expectedMatrix[1, 1] = 4;
            expectedMatrix[1, 2] = 6;

            SparseMatrix<double> retTranA = SparseVectorUtility.TranspositionMatrix(matrix);
            for (int y = 0; y < expectedMatrix.Height; y++)
            {
                for (int x = 0; x < expectedMatrix.Width; x++)
                {
                    Assert.That(retTranA[y, x], Is.EqualTo(expectedMatrix[y, x]));
                }
            }
        }

        /// <summary>
        /// 行列同士の積のテストを行う
        /// </summary>
        [TestCase()]
        public void TestMatrixByMatrix()
        {
            SparseMatrix<double> matrixX = new SparseMatrix<double>(2, 3, 0);
            matrixX[0, 0] = 1;
            matrixX[0, 1] = 3;
            matrixX[0, 2] = 5;
            matrixX[1, 0] = 2;
            matrixX[1, 1] = 4;
            matrixX[1, 2] = 6;

            SparseMatrix<double> matrixY = new SparseMatrix<double>(3, 2, 0);
            matrixY[0, 0] = 1;
            matrixY[0, 1] = 2;
            matrixY[1, 0] = 3;
            matrixY[1, 1] = 4;
            matrixY[2, 0] = 5;
            matrixY[2, 1] = 6;

            SparseMatrix<double> expectedMatrix = new SparseMatrix<double>(2, 2, 0);
            expectedMatrix[0, 0] = 35;
            expectedMatrix[0, 1] = 44;
            expectedMatrix[1, 0] = 44;
            expectedMatrix[1, 1] = 56;

            SparseMatrix<double> retMatrix = SparseVectorUtility.MatrixByMatrix(matrixX, matrixY);
            for (int y = 0; y < expectedMatrix.Height; y++)
            {
                for (int x = 0; x < expectedMatrix.Width; x++)
                {
                    Assert.That(retMatrix[y, x], Is.EqualTo(expectedMatrix[y, x]));
                }
            }

            matrixX = new SparseMatrix<double>(2, 3, 0);
            matrixX[0, 0] = 1;
            //matrixX[0, 1] = 0;
            //matrixX[0, 2] = 0;
            //matrixX[1, 0] = 0;
            matrixX[1, 1] = 4;
            matrixX[1, 2] = 6;

            matrixY = new SparseMatrix<double>(3, 2, 0);
            //matrixY[0, 0] = 0;
            matrixY[0, 1] = 2;
            //matrixY[1, 0] = 0;
            //matrixY[1, 1] = 0;
            matrixY[2, 0] = 5;
            //matrixY[2, 1] = 0;

            expectedMatrix = new SparseMatrix<double>(2, 2, 0);
            expectedMatrix[0, 0] = 0;
            expectedMatrix[0, 1] = 2;
            expectedMatrix[1, 0] = 30;
            expectedMatrix[1, 1] = 0;

            retMatrix = SparseVectorUtility.MatrixByMatrix(matrixX, matrixY);
            for (int y = 0; y < expectedMatrix.Height; y++)
            {
                for (int x = 0; x < expectedMatrix.Width; x++)
                {
                    Assert.That(retMatrix[y, x], Is.EqualTo(expectedMatrix[y, x]));
                }
            }
        }

        /// <summary>
        /// 行列の正方化のテストを行う
        /// </summary>
        [TestCase()]
        public void TestConvertSquareMatrix()
        {
            SparseMatrix<double> A = new SparseMatrix<double>(3, 2, 0);
            A[0, 0] = 1;
            A[0, 1] = 2;
            A[1, 0] = 3;
            A[1, 1] = 4;
            A[2, 0] = 5;
            A[2, 1] = 6;

            SparseMatrix<double> expectedMatrix = new SparseMatrix<double>(2, 2, 0);
            expectedMatrix[0, 0] = 35;
            expectedMatrix[0, 1] = 44;
            expectedMatrix[1, 0] = 44;
            expectedMatrix[1, 1] = 56;

            SparseMatrix<double> retCnvA = SparseVectorUtility.ConvertSquareMatrix(A);
            Assert.That(retCnvA[0, 0], Is.EqualTo(35));
            Assert.That(retCnvA[0, 1], Is.EqualTo(44));
            Assert.That(retCnvA[1, 0], Is.EqualTo(44));
            Assert.That(retCnvA[1, 1], Is.EqualTo(56));

            //Assert.That(SparseVectorUtility.ConvertSquareMatrix(A), Is.EqualTo(expectedMatrix));
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// テスト用のベクトルxを取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetTestVectorX()
        {
            return new SparseVector<double>(0) { 1, 2, 3 };
        }

        /// <summary>
        /// テスト用のベクトルyを取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetTestVectorY()
        {
            return new SparseVector<double>(0) { 4, 5, 6 };
        }

        /// <summary>
        /// テスト用の行列Aを取得する
        /// </summary>
        /// <returns></returns>
        public static SparseMatrix<double> GetTestMatrixA()
        {
            SparseMatrix<double> A = new SparseMatrix<double>(2, 3, 0);
            A[0, 0] = 4;
            A[0, 1] = 5;
            A[0, 2] = 6;
            A[1, 0] = 7;
            A[1, 1] = 8;
            A[1, 2] = 9;

            return A;
        }

        /// <summary>
        /// テスト用の期待する行列Yを取得する
        /// </summary>
        /// <returns></returns>
        public static SparseVector<double> GetTestExpectedVectorY()
        {
            return new SparseVector<double>(0) { 32, 50 };
        }
        #endregion
        #endregion
    }
}
