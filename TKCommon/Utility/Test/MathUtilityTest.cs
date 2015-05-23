using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using TKCommon.Utility;

namespace TKCommon.Utility.Test
{
    /// <summary>
    /// MathUtilityテストクラス
    /// </summary>
    [TestFixture]
    public class MathUtilityTest
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
            List<double> vectorX = MathUtilityTest.GetTestVectorX();
            List<List<double>> matrixA = MathUtilityTest.GetTestMatrixA();
            List<double> expectedVectorY = MathUtilityTest.GetTestExpectedVectorY();
            Assert.That(MathUtility.VectorByMatrix(matrixA, vectorX), Is.EqualTo(expectedVectorY));
        }

        /// <summary>
        /// 内積のテストを行う
        /// </summary>
        [TestCase()]
        public void TestDotProduct()
        {
            List<double> vectorX = MathUtilityTest.GetTestVectorX();
            List<double> vectorY = MathUtilityTest.GetTestVectorY();
            Assert.That(MathUtility.DotProduct(vectorX, vectorY), Is.EqualTo(32));
        }

        /// <summary>
        /// ベクトルノルムのテストを行う
        /// </summary>
        [TestCase()]
        public void TestVectorNorm()
        {
            List<double> vector = new List<double> { 1, -2, 3 };
            Assert.That(MathUtility.VectorNorm(vector), Is.EqualTo(6));
        }

        /// <summary>
        /// 行列転置のテストを行う
        /// </summary>
        [TestCase()]
        public void TestTranspositionMatrix()
        {
            List<List<double>> matrix = new List<List<double>>();
            matrix.Add(new List<double> { 1, 2 });
            matrix.Add(new List<double> { 3, 4 });
            matrix.Add(new List<double> { 5, 6 });

            List<List<double>> expectedMatrix = new List<List<double>>();
            expectedMatrix.Add(new List<double> { 1, 3, 5 });
            expectedMatrix.Add(new List<double> { 2, 4, 6 });
            Assert.That(MathUtility.TranspositionMatrix(matrix), Is.EqualTo(expectedMatrix));
        }

        /// <summary>
        /// 行列同士の積のテストを行う
        /// </summary>
        [TestCase()]
        public void TestMatrixByMatrix()
        {
            List<List<double>> matrixX = new List<List<double>>();
            matrixX.Add(new List<double> { 1, 3, 5 });
            matrixX.Add(new List<double> { 2, 4, 6 });

            List<List<double>> matrixY = new List<List<double>>();
            matrixY.Add(new List<double> { 1, 2 });
            matrixY.Add(new List<double> { 3, 4 });
            matrixY.Add(new List<double> { 5, 6 });

            List<List<double>> expectedMatrix = new List<List<double>>();
            expectedMatrix.Add(new List<double> { 35, 44 });
            expectedMatrix.Add(new List<double> { 44, 56 });
            Assert.That(MathUtility.MatrixByMatrix(matrixX, matrixY), Is.EqualTo(expectedMatrix));
        }

        /// <summary>
        /// 行列の正方化のテストを行う
        /// </summary>
        [TestCase()]
        public void TestConvertSquareMatrix()
        {
            List<List<double>> A = new List<List<double>>();
            A.Add(new List<double> { 1, 2 });
            A.Add(new List<double> { 3, 4 });
            A.Add(new List<double> { 5, 6 });

            List<List<double>> expectedMatrix = new List<List<double>>();
            expectedMatrix.Add(new List<double> { 35, 44 });
            expectedMatrix.Add(new List<double> { 44, 56 });
            Assert.That(MathUtility.ConvertSquareMatrix(A), Is.EqualTo(expectedMatrix));
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// テスト用のベクトルxを取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetTestVectorX()
        {
            return new List<double> { 1, 2, 3 };
        }

        /// <summary>
        /// テスト用のベクトルyを取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetTestVectorY()
        {
            return new List<double> { 4, 5, 6 };
        }

        /// <summary>
        /// テスト用の行列Aを取得する
        /// </summary>
        /// <returns></returns>
        public static List<List<double>> GetTestMatrixA()
        {
            List<List<double>> A = new List<List<double>>();
            A.Add(new List<double> { 4, 5, 6 });
            A.Add(new List<double> { 7, 8, 9 });

            return A;
        }

        /// <summary>
        /// テスト用の期待する行列Yを取得する
        /// </summary>
        /// <returns></returns>
        public static List<double> GetTestExpectedVectorY()
        {
            return new List<double> { 32, 50 };
        }
        #endregion
        #endregion
    }
}
