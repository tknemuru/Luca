using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using TKCommon.Utility;
using System.Diagnostics;
using ReversiCommon.Collections;

namespace ReversiCommon.Test
{
    /// <summary>
    /// デバッグ用ボード
    /// </summary>
    public static class DebugBoard
    {
        #region "定数"
        /// <summary>
        /// 盤面状態を示すファイルパス
        /// </summary>
        private const string BOARD_STATE_FILE_PATH = @"C:\work\visualstudio\Luca_TRUNK\ReversiCommon\Test\board_state";

        /// <summary>
        /// 盤面状態ファイルのパターン
        /// </summary>
        private const string BOARD_STATE_FILE_PATTERN = "*.bs";

        /// <summary>
        /// 一つの盤面状態の行数
        /// </summary>
        private const int ONE_UNIT_LINE = 9;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// 状態リスト
        /// </summary>
        private static Dictionary<string, string> m_StateDictionary;

        /// <summary>
        /// <para>カラーリスト</para>
        /// </summary>
        private static Dictionary<int, Color> m_Colors;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static DebugBoard()
        {
            m_StateDictionary = new Dictionary<string, string>();
            IEnumerable<string> files = Directory.EnumerateFiles(BOARD_STATE_FILE_PATH, BOARD_STATE_FILE_PATTERN, SearchOption.AllDirectories);
            foreach (string file in files)
            {
                List<string> lines = FileUtility.ReadListData(file);
                int i = 1;
                string key = string.Empty;
                foreach (string line in lines)
                {
                    if (i == 1)
                    {
                        Debug.Assert(!m_StateDictionary.ContainsKey(line), "既に同名の盤面状態が存在しています。 名称：" + line);
                        m_StateDictionary.Add(line, "");
                        key = line;
                    }
                    else
                    {
                        m_StateDictionary[key] += line;
                    }
                    i++;
                    if (i > ONE_UNIT_LINE)
                    {
                        i = 1;
                        key = string.Empty;
                    }
                }
            }
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 盤面状態を読み込む
        /// </summary>
        /// <param name="key"></param>
        public static void Read(string key)
        {
            m_Colors = new Dictionary<int, Color>();
            List<string> pointList = StringUtility.ToListSplitCount(m_StateDictionary[key], 1);
            int i = 0;
            for (int h = 1; h <= 8; h++)
            {
                for (int w = 1; w <= 8; w++)
                {
                    int point = ((h * 10) + w);
                    m_Colors.Add(point, ConvertColor(pointList[i]));
                    i++;
                }
            }

            Board.SetBoardState(m_Colors);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 色の記号をカラークラスに変換する
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static Color ConvertColor(string color)
        {
            Color retColor;
            switch (color)
            {
                case "●" :
                    retColor = Color.Black;
                    break;
                case "○" :
                    retColor = Color.White;
                    break;
                case "　" :
                    retColor = Color.Space;
                    break;
                default :
                    throw new ApplicationException("不正な盤面状態です。");
            }
            return retColor;
        }
        #endregion
        #endregion
    }
}
