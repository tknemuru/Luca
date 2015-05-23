using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Luca.Optimization.Logic;

namespace Luca.Optimization.Test
{
    /// <summary>
    /// 共役勾配法 テストクラス
    /// </summary>
    [TestFixture]
    public class ConjugateGradientTest
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
        /// 共役勾配法が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase()]
        public void TestExecute()
        {
            List<List<double>> A = ConjugateGradientTest.GetTestA();
            List<double> b = ConjugateGradientTest.GetTestB();
            List<double> expectedX = ConjugateGradientTest.GetTestExpectedX();
            List<double> x = new List<double> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            ConjugateGradientLogic logic = new ConjugateGradientLogic();
            List<double> modifydX = logic.Execute(A, b, x);
            int i = 0;
            foreach(double value in modifydX)
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
        #endregion
        #endregion
    }
}
