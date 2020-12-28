using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;

namespace R_Track_Controller
{
    /// <summary>
    /// 배열 값을 읽어들여 일정 간격마다 행동을 지시하는 정적 클래스
    /// </summary>
    public static class ArrayInput_static
    {
        public static int currLineIndex { get; set; }
        public static List<string> lines { get; set; }
        private static int readingInterval { get; set; }
        private static string readingAddress { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        static ArrayInput_static()
        {
            readingInterval = 1;
            lines = new List<string>();
            currLineIndex = 1;
        }

        /// <summary>
        /// 읽는 줄 번호를 다시 1번으로 돌리는 함수
        /// </summary>
        public static void initialize()
        {
            currLineIndex = 1;
        }

        /// <summary>
        /// 배열이 기록된 txt파일을 읽어들이는 함수
        /// </summary>
        /// <param name="textAddress">읽어들일 텍스트 파일 주소값</param>
        /// <returns>읽어들이기 성공하면 true</returns>
        public static bool readArray(string textAddress)
        {
            lines.Clear();
            string[] temp;
            try
            {
                temp = File.ReadAllLines(textAddress);
            }
            catch
            {
                return false;
            }

            readingAddress = textAddress;
            lines.AddRange(temp);
            return true;
        }

        /// <summary>
        /// 읽는 간격을 설정하는 함수
        /// </summary>
        /// <param name="interval">간격, 1이면 1줄마다 바로바로 읽음</param>
        public static void setReadingInterval(int interval)
        {
            readingInterval = interval;
        }

        /// <summary>
        /// 읽는 간격값을 가져오는 함수
        /// </summary>
        /// <returns>간격값 int임</returns>
        public static int getReadingInterval()
        {
            return readingInterval;
        }

        /// <summary>
        /// 다음 줄을 읽어오는 함수
        /// </summary>
        /// <param name="line">읽은 값이 저장될 곳</param>
        /// <returns>성공하면 true, 끝에 다다랐으면 false</returns>
        public static bool readNextLine(ref string line)
        {
            if (lines.Count < currLineIndex)
            {
                return false;
            }
            else
            {
                line = lines[currLineIndex-1];
                currLineIndex += readingInterval;
                return true;
            }
        }

        /// <summary>
        /// 현재 읽고있는 텍스트 파일을 열어줌
        /// </summary>
        /// <returns>성공하면 true</returns>
        public static bool openTextEditor()
        {
            try
            {
                Process.Start(readingAddress);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 배열 값을 리로드 하는 함수
        /// </summary>
        /// <returns>리로드에 성공하면 true</returns>
        public static bool reloadLines()
        {
            try
            {
                lines.Clear();
                string[] temp;
                temp = File.ReadAllLines(readingAddress);
                lines.AddRange(temp);
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
