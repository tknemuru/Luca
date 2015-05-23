using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Collections;
using System.Diagnostics;

namespace ReversiCommon.Utility
{
    /// <summary>
    /// <para>リバーシデバッグクラス</para>
    /// </summary>
    public static class DebugUtility
    {
        #region "定数"
        /// <summary>
        /// <para>デバッグモードの場合はTrue</para>
        /// </summary>
        public const bool IS_DEBUG = true;

        /// <summary>
        /// <para>コンソールに出力する場合はTrue</para>
        /// </summary>
        private const bool IS_CONSOLE = false;
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        static DebugUtility()
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>ボード情報をコンソールに出力する</para>
        /// </summary>
        public static void OutputBoardToConsole()
        {
            if (!IS_DEBUG) { return; }
            for (int i = 10; i <= 80; i += 10)
            {
                string outLine = string.Empty;
                for (int s = 1; s <= 8; s++)
                {
                    Color color = Board.GetSquareColor(i + s);
                    if(color == Color.Black)
                    {
                        outLine += "○";
                    }
                    else if (color == Color.White)
                    {
                        outLine += "●";
                    }
                    else if (color == Color.Space)
                    {
                        outLine += (s + i).ToString();
                    }
                    else
                    {
                        outLine += "？";
                    }
                }
                Debug.WriteLine(outLine);
            }
        }

        /// <summary>
        /// <para>ボード情報をコンソールに出力する</para>
        /// </summary>
        public static void OutputBoardScoreToConsole()
        {
            if (!IS_DEBUG) { return; }
            List<string> scoreList = new List<string>();
            for (int i = 10; i <= 80; i += 10)
            {
                string outLine = string.Empty;
                for (int s = 1; s <= 8; s++)
                {
                    Color color = Board.GetSquareColor(i + s);
                    if (color == Color.Black)
                    {
                        outLine += (IS_CONSOLE) ? "○" : "●";
                    }
                    else if (color == Color.White)
                    {
                        outLine += (IS_CONSOLE) ? "●" : "○";
                    }
                    else if (color == Color.Space)
                    {
                        if (Board.Scores.ContainsKey(i + s))
                        {
                            outLine += (s + i).ToString();
                            scoreList.Add((s + i).ToString() + "：" + Board.Scores[i + s]);
                        }
                        else
                        {
                            outLine += (IS_CONSOLE) ? "■" : "□";
                        }
                    }
                    else
                    {
                        outLine += "？";
                    }
                }
                Debug.WriteLine(outLine);
            }
            Debug.WriteLine("");
            foreach (string score in scoreList)
            {
                Debug.WriteLine(score);
            }
            Board.ClearScores();
        }

        /// <summary>
        /// <para>ボードの開放度情報をコンソールに出力する</para>
        /// </summary>
        public static void OutputBoardOpenScoreToConsole(Dictionary<int, int> openDic)
        {
            if (!IS_DEBUG) { return; }
            for (int i = 10; i <= 80; i += 10)
            {
                string outLine = string.Empty;
                for (int s = 1; s <= 8; s++)
                {
                    Color color = Board.GetSquareColor(i + s);
                    if (color == Color.Black)
                    {
                        outLine += (IS_CONSOLE) ? "○" : "●";
                    }
                    else if (color == Color.White)
                    {
                        outLine += (IS_CONSOLE) ? "●" : "○";
                    }
                    else if (color == Color.Space)
                    {
                        outLine += " " + openDic[i + s].ToString();
                    }
                    else
                    {
                        outLine += "？";
                    }
                }
                Debug.WriteLine(outLine);
            }
            Debug.WriteLine("");
        }

        /// <summary>
        /// <para>ボード情報をコンソールに出力する</para>
        /// </summary>
        public static void OutputBoardToConsole(Dictionary<int, Color> colors)
        {
            if (!IS_DEBUG) { return; }
            for (int i = 10; i <= 80; i += 10)
            {
                string outLine = string.Empty;
                for (int s = 1; s <= 8; s++)
                {
                    Color color = colors[i + s];
                    if (color == Color.Black)
                    {
                        outLine += (IS_CONSOLE) ? "○" : "●";
                    }
                    else if (color == Color.White)
                    {
                        outLine += (IS_CONSOLE) ? "●" : "○";
                    }
                    else if (color == Color.Space)
                    {
                        outLine += (s + i).ToString();
                    }
                    else
                    {
                        outLine += "？";
                    }
                }
                Debug.WriteLine(outLine);
            }
        }

        /// <summary>
        /// <para>文字列をコンソールに出力する</para>
        /// </summary>
        /// <param name="str"></param>
        public static void OutputStringToConsole(string str)
        {
            if (!IS_DEBUG) { return; }
            Debug.WriteLine(str);
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
