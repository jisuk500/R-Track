using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Windows.Threading;

namespace R_Track_Controller
{

    delegate void Serial_call(object sender, SerialDataReceivedEventArgs e);

    static class Serial_static
    {
        /// <summary>
        /// 현재 연결 상태인지 프로퍼티
        /// </summary>
        public static bool isConnected { get; set; }
        /// <summary>
        /// 현재 연결된 포트 번호
        /// </summary>
        public static int portNum { get; set; }

        /// <summary>
        /// 내부적인 SerialPort클래스
        /// </summary>
        private static SerialPort Serial = new SerialPort();
        /// <summary>
        /// UI쓰레드에 invoke요청을 보내기 위해서 저장하는 메인윈도우 창.
        /// initialze 함수를 통해 꼭 할당을 해야 한다.
        /// </summary>
        private static MainWindow mainWin { get; set; }

        /// <summary>
        /// 현재 가능한 시리얼 포트 번호들을 불러온다.
        /// </summary>
        /// <returns>시리얼 포트 번호들의 List. COM이 붙은 스트링이다.</returns>
        public static List<string> getPortsAvailable()
        {
            return SerialPort.GetPortNames().ToList();
        }

        /// <summary>
        /// 특정 포트에 시리얼 연결을 시도하는 함수
        /// </summary>
        /// <param name="portname">연결할 포트의 이름. COM숫자 로 표시된다.</param>
        /// <returns>연결에 성공하면 true, 아니면 false가 반환된다.</returns>
        public static bool setConnection(string portname)
        {
            try
            {
                Serial.PortName = portname;
                Serial.BaudRate = 57600;

                Serial.Open();


                portNum = Convert.ToInt32(portname.Substring(3));
                isConnected = true;
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 현재 포트와의 연결을 끊는 함수
        /// </summary>
        /// <returns>연결 해제에 성공하면 true, 아니면 false</returns>
        public static bool delConnection()
        {
            try
            {
                Serial.Close();

                portNum = -1;
                isConnected = false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 현재 열려있는 포트로 메세지를 보내는 함수
        /// </summary>
        /// <param name="line">보낼 메세지</param>
        /// <returns>전송에 성공하면 true, 아니면 false</returns>
        public static bool sendMsg(string line)
        {
            try
            {
                Serial.WriteLine(line);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 초기화 함수. 프로그램을 처음 시작할때 꼭 사용할것
        /// </summary>
        /// <param name="mainWin_">반드시 메인윈도우의 this를 넣을것</param>
        public static void Initialize(MainWindow mainWin_)
        {
            Serial.NewLine = "\n";
            mainWin = mainWin_;
            Serial.DataReceived += Serial_DataReceived;
        }

        /// <summary>
        /// 시리얼 데이터가 도착했을때 트리거되는 이벤트. DataRecived 이벤트의 핸들러
        /// </summary>
        private static void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort SP = (SerialPort)sender;
            string msg = SP.ReadExisting();

            mainWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
             {
                 mainWin.addConsoleLine(msg);
             }));


        }

        /// <summary>
        /// 각종 시리얼 메세지를 정리해서 보내는 클래스
        /// </summary>
        public static class serialRule
        {
            /// <summary>
            /// 이전에 있던 값을 기억하여 명령을 덜 보내는 변수
            /// </summary>
            private static int[] previousValues { get; set; }


            /// <summary>
            /// 로봇을 특정 방향으로 움직이도록 함
            /// </summary>
            /// <param name="robotNum_">로봇 넘버</param>
            /// <param name="direction">방향</param>
            /// <param name="speed">움직이게 할 속도</param>
            public static bool robot_move(visual_binding_arrows.robotNum robotNum_, visual_binding_arrows.arrow direction, int speed)
            {
                if (direction == visual_binding_arrows.arrow.stop)
                {
                    speed = 0;
                }

                string line = "";
                int robotNumI = (int)robotNum_ + 1;
                string robotNumStr = robotNumI.ToString();
                string move = "";

                if (direction == visual_binding_arrows.arrow.forward)
                {
                    move = "MOVE";
                    line = robotNumStr + "," + move + "," + speed.ToString() + "/";
                }
                else if (direction == visual_binding_arrows.arrow.backward)
                {
                    move = "MOVE";
                    speed = -speed;
                    line = robotNumStr + "," + move + "," + speed.ToString() + "/";
                }
                else if (direction == visual_binding_arrows.arrow.left)
                {
                    move = "TURN";
                    line = robotNumStr + "," + move + ",L/";
                }
                else if (direction == visual_binding_arrows.arrow.right)
                {
                    move = "TURN";
                    line = robotNumStr + "," + move + ",R/";
                }
                else if (direction == visual_binding_arrows.arrow.stop)
                {
                    line = robotNumStr + ",MOVE,D/";
                }

                mainWin.addSystemMsgLine("sending :" + line);
                return sendMsg(line);
            }

            /// <summary>
            /// 로봇의 헤드와 테일 관련 값을 보내주는 함수
            /// </summary>
            /// <param name="robotNum_">로봇의 번호</param>
            /// <param name="part">파트 </param>
            /// <param name="value">해당 값</param>
            /// <returns></returns>
            public static bool robot_apply_value(MainWindow.RobotNum robotNum_, MainWindow.Parts part, int value)
            {
                string line = "";

                int robotNumI = (int)robotNum_ + 1;
                string robotNumStr = robotNumI.ToString();

                string partStr = "";
                switch (part)
                {
                    case MainWindow.Parts.Head_Pos:
                        {
                            partStr = "HEAD-POS";
                            break;
                        }
                    case MainWindow.Parts.Head_Amp:
                        {
                            partStr = "HEAD-AMP";
                            break;
                        }
                    case MainWindow.Parts.Tail_Pos:
                        {
                            partStr = "TAIL-POS";
                            break;
                        }
                    case MainWindow.Parts.Tail_Amp:
                        {
                            partStr = "TAIL-AMP";
                            break;
                        }
                }

                line = robotNumStr + "," + partStr + "," + value.ToString() + "/";
                mainWin.addSystemMsgLine("sending :" + line);
                return sendMsg(line);

            }

            /// <summary>
            /// 로봇의 값을 읽어오는 모드 설정하는 함수
            /// </summary>
            /// <param name="mode">true면 읽어오기 시작, false면 읽어오기 종료</param>
            /// <returns>값을 보내는데 성공하면 true, 아니면 false 반환</returns>
            public static bool robot_get_value_mode(bool mode)
            {
                string line = "";

                switch (mode)
                {
                    case true:
                        {
                            line = "255,VALUE,GIVE/";
                            break;
                        }
                    case false:
                        {
                            line = "255,VALUE,STOP/";
                            break;
                        }
                }

                mainWin.addSystemMsgLine("sending :" + line);
                return sendMsg(line);
            }

            /// <summary>
            /// 로봇의 연결이 멀쩡한지 보여주는 함수
            /// </summary>
            public static bool robot_check()
            {
                string line = "255,LED,TEST/";
                mainWin.addSystemMsgLine("sending :" + line);
                return sendMsg(line);
            }

            /// <summary>
            /// 로봇한테 배열로 읽어들인 프로파일 값을 전해주는 함수
            /// </summary>
            /// <param name="line">읽어들인 프로파일 텍스트</param>
            /// <returns>전송에 성공하면 true</returns>
            public static bool send_arrayProfile(string line, out int timeSpan)
            {
                var splitedLine = line.Split(',');

                timeSpan = Int32.Parse(splitedLine[0]);

                int temp = 0;
                for(int r=0;r<3;r++)
                {
                    for(int p=0;p<5;p++)
                    {
                        temp = Int32.Parse(splitedLine[r*5+p + 1]);
                        if (temp != previousValues[r*5+p])
                        {
                            previousValues[r * 5 + p] = temp;
                            switch(p)
                            {
                                case 0:
                                    {
                                        robot_move((visual_binding_arrows.robotNum)r, visual_binding_arrows.arrow.forward, temp);
                                        break;
                                    }
                                case 1:
                                    {
                                        robot_apply_value((MainWindow.RobotNum)r, MainWindow.Parts.Head_Pos, temp);
                                        break;
                                    }
                                case 2:
                                    {
                                        robot_apply_value((MainWindow.RobotNum)r, MainWindow.Parts.Head_Amp, temp);
                                        break;
                                    }
                                case 3:
                                    {
                                        robot_apply_value((MainWindow.RobotNum)r, MainWindow.Parts.Tail_Pos, temp);
                                        break;
                                    }
                                case 4:
                                    {
                                        robot_apply_value((MainWindow.RobotNum)r, MainWindow.Parts.Tail_Amp, temp);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    

                }


                try
                {
                    
                }
                catch
                {
                    return false;
                }

                return true;
            }


            /// <summary>
            /// 생성자
            /// </summary>
            static serialRule()
            {
                previousValues = new int[15];
                for(int i=0;i<15;i++)
                {
                    previousValues[i] = 0;
                }
            }
        }

    }
}