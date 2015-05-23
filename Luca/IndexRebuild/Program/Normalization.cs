using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Utility;
using Luca.IndexRebuild.Logic;
using TKCommon.Utility;

namespace Luca.IndexRebuild.Program
{
    /// <summary>
    /// インデックス正規化クラス
    /// </summary>
    public class Normalization
    {
        #region "定数"
        /// <summary>
        /// インデックスの桁数（3進数）の最小値と最大値
        /// </summary>
        private static readonly Tuple<int, int> INDEX_DIGIT_MIN_MAX = new Tuple<int, int>(4, 10);

        /// <summary>
        /// 正規化タイプ
        /// </summary>
        private enum NORMALIZE_TYPE
        {
            NORMAL = 1,
            CHANGE_COLOR = -1
        }
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// MS_INDEX_NORMALIZEテーブルのデータを作成する
        /// </summary>
        public void CreateIndexNormalize()
        {
            NormalizationLogic logic = new NormalizationLogic();

            for (int digit = INDEX_DIGIT_MIN_MAX.Item1; digit <= INDEX_DIGIT_MIN_MAX.Item2; digit++)
            {
                List<int> registerdList = new List<int>();
                int limit = ReversiMathUtility.GetMaxIndex(digit);

                for (int i = 0; i <= limit; i++)
                {
                    // 既に登録済なら何もしない
                    if (registerdList.Contains(i)) { continue; }

                    // DBに登録していく
                    // 自分自身
                    int normalizeIndex = i;
                    logic.AddCommand(normalizeIndex, digit, i, (int)NORMALIZE_TYPE.NORMAL);
                    registerdList.Add(normalizeIndex);

                    // 色変換
                    normalizeIndex = NormalizationUtility.ChangeColor(i, digit);
                    if (!registerdList.Contains(normalizeIndex))
                    {
                        logic.AddCommand(normalizeIndex, digit, i, (int)NORMALIZE_TYPE.CHANGE_COLOR);
                        registerdList.Add(normalizeIndex);
                    }
                    
                    // 9桁以上のインデックスはリバースをすると状況が変わってしまうので、ここで終了
                    if (digit >= 9) { continue; }

                    // リバース
                    normalizeIndex = NormalizationUtility.Reverse(i, digit);
                    if (!registerdList.Contains(normalizeIndex))
                    {
                        logic.AddCommand(normalizeIndex, digit, i, (int)NORMALIZE_TYPE.NORMAL);
                        registerdList.Add(normalizeIndex);
                    }

                    // 色変換＆リバース
                    normalizeIndex = NormalizationUtility.ChangeColorReverse(i, digit);
                    if (!registerdList.Contains(normalizeIndex))
                    {
                        logic.AddCommand(normalizeIndex, digit, i, (int)NORMALIZE_TYPE.CHANGE_COLOR);
                        registerdList.Add(normalizeIndex);
                    }
                }

                // SQL実行
                logic.ExecuteCommand();
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
