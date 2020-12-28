using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;

namespace R_Track_Controller
{

    static class robot_values
    {
        /// <summary>
        /// 전체 값들의 배열
        /// </summary>
        public static List<value> values { get; set; }

        /// <summary>
        /// 값을 화면에 표시하는 배열
        /// </summary>
        public static value_indicator value_indicator_ { get; set; }

        public static string savePath = "Robot_values.csv";

        /// <summary>
        /// 생성자
        /// </summary>
        public class value_indicator : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;


            public value R_1 { get; set; }
            public value R_2 { get; set; }
            public value R_3 { get; set; }

            public value_indicator()
            {
                R_1 = new value();
                R_2 = new value();
                R_3 = new value();
            }

            /// <summary>
            /// value 클래스 값을 받아서 인디케이터에 표시해줌
            /// </summary>
            /// <param name="value_">받는 value값</param>
            public void value2indicator(value value_)
            {
                switch (value_.robotNum_)
                {
                    case 1:
                        {
                            R_1 = value_;
                            break;
                        }
                    case 2:
                        {
                            R_2 = value_;
                            break;
                        }

                    case 3:
                        {
                            R_3 = value_;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 값 묶음 하나의 클래스
        /// </summary>
        public class value : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// 왼쪽 바퀴에 대한 값
            /// </summary>
            public part wheel_L { get; set; }

            /// <summary>
            /// 오른쪽 바퀴에 대한 값
            /// </summary>
            public part wheel_R { get; set; }

            /// <summary>
            /// 헤드 모터에 대한 값
            /// </summary>
            public part Head { get; set; }

            /// <summary>
            /// 꼬리 모터에 대한 값
            /// </summary>
            public part Tail { get; set; }

            /// <summary>
            /// 이 로봇의 넘버
            /// </summary>
            public int robotNum_ { get; set; }


            public class part
            {
                /// <summary>
                /// 이 로봇의 속도값
                /// </summary>
                public string Vel { get; set; }

                /// <summary>
                /// 이 로봇의 포지션 값
                /// </summary>
                public string Pos { get; set; }

                /// <summary>
                /// 이 로봇의 암페어 값
                /// </summary>
                public string Amp { get; set; }

                public part()
                {
                    Vel = "0";
                    Pos = "0";
                    Amp = "0";
                }
            }


            /// <summary>
            /// 생성자. 4개의 모터 부위와 로봇 넘버
            /// </summary>
            public value()
            {
                wheel_L = new part();
                wheel_R = new part();
                Head = new part();
                Tail = new part();
                robotNum_ = -1;
            }
        }


        /// <summary>
        /// 생성자. 인스턴스만 생성한다.
        /// </summary>
        static robot_values()
        {
            values = new List<value>();
            value_indicator_ = new value_indicator();
        }

        /// <summary>
        /// 라인 메시지로부터 값을 읽어들이는 함수
        /// </summary>
        /// <param name="line">라인의 존재</param>
        /// <param name="toIndicator">인디케이터에 바로 반영할지 여부</param>
        /// <returns>성공 여부</returns>
        public static bool catch_value_from_log(string line, bool toIndicator = false)
        {
            var split = line.Split(',','/');

            if (split[0] == "0")
            {
                var temp = new value();

                if (split[1] == "1")
                {
                    temp.robotNum_ = 1;
                }
                else if (split[1] == "2")
                {
                    temp.robotNum_ = 2;
                }
                else if (split[1] == "3")
                {
                    temp.robotNum_ = 3;
                }
                else
                {
                    return false;
                }

                var split_2 = split[2].Split('|');

                try
                {
                    temp.wheel_L.Vel = split_2[0];
                    temp.wheel_L.Pos = split_2[1];
                    temp.wheel_L.Amp = split_2[2];

                    temp.wheel_R.Vel = split_2[3];
                    temp.wheel_R.Pos = split_2[4];
                    temp.wheel_R.Amp = split_2[5];

                    temp.Head.Vel = split_2[6];
                    temp.Head.Pos = split_2[7];
                    temp.Head.Amp = split_2[8];

                    temp.Tail.Vel = split_2[9];
                    temp.Tail.Pos = split_2[10];
                    temp.Tail.Amp = split_2[11];

                }
                catch
                {
                    return false;
                }

                values.Add(temp);

                if (toIndicator == true)
                {
                    value_indicator_.value2indicator(temp);
                }

                return true;

            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 값들 나눠서 세이브 하는 함수
        /// </summary>
        /// <returns></returns>
        public static bool save_values_to_csv()
        {
            try
            {
                List<value> R_1_temp = new List<value>();
                List<value> R_2_temp = new List<value>();
                List<value> R_3_temp = new List<value>();

                List<string> total_lines = new List<string>();

                foreach(var value in values)
                {
                    switch(value.robotNum_)
                    {
                        case 1:
                            {
                                R_1_temp.Add(value);
                                break;
                            }
                        case 2:
                            {
                                R_2_temp.Add(value);
                                break;
                            }
                        case 3:
                            {
                                R_3_temp.Add(value);
                                break;
                            }
                    }
                   
                }

                int maxCount = 0;
                if(R_1_temp.Count<=R_2_temp.Count)
                {
                    if (R_2_temp.Count <= R_3_temp.Count)
                    {
                        maxCount = R_3_temp.Count;
                    }
                    else
                    {
                        maxCount = R_2_temp.Count;
                    }
                }
                else if(R_1_temp.Count<=R_3_temp.Count)
                {
                    maxCount = R_3_temp.Count;
                }
                else
                {
                    maxCount = R_1_temp.Count;
                }

                total_lines.Add("1,,,,,,,,,,,,2,,,,,,,,,,,,3,,,,,,,,,,,,RobotNum");
                total_lines.Add("Left,,,Right,,,Head,,,Tail,,,Left,,,Right,,,Head,,,Tail,,,Left,,,Right,,,Head,,,Tail,,,RobotPart");
                total_lines.Add("Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,Pos,Vel,Amp,ValueType");

                string tempLine = "";
                for (int i=0;i<maxCount;i++)
                {
                    tempLine = "";

                    if(R_1_temp.Count>=i+1)
                    {
                        tempLine += R_1_temp[i].wheel_L.Pos + "," + R_1_temp[i].wheel_L.Vel + "," + R_1_temp[i].wheel_L.Amp + ","
                            + R_1_temp[i].wheel_R.Pos + "," + R_1_temp[i].wheel_R.Vel + "," + R_1_temp[i].wheel_R.Amp + ","
                            + R_1_temp[i].Head.Pos + "," + R_1_temp[i].Head.Vel + "," + R_1_temp[i].Tail.Amp + ","
                            + R_1_temp[i].Tail.Pos + "," + R_1_temp[i].Tail.Vel + "," + R_1_temp[i].Tail.Amp + ",";
                    }
                    else
                    {
                        tempLine += ",,,,,,,,,,,,";
                    }

                    if (R_2_temp.Count >= i + 1)
                    {
                        tempLine += R_2_temp[i].wheel_L.Pos + "," + R_2_temp[i].wheel_L.Vel + "," + R_2_temp[i].wheel_L.Amp + ","
                            + R_2_temp[i].wheel_R.Pos + "," + R_2_temp[i].wheel_R.Vel + "," + R_2_temp[i].wheel_R.Amp + ","
                            + R_2_temp[i].Head.Pos + "," + R_2_temp[i].Head.Vel + "," + R_2_temp[i].Tail.Amp + ","
                            + R_2_temp[i].Tail.Pos + "," + R_2_temp[i].Tail.Vel + "," + R_2_temp[i].Tail.Amp+ ",";
                    }
                    else
                    {
                        tempLine += ",,,,,,,,,,,,";
                    }

                    if (R_3_temp.Count >= i + 1)
                    {
                        tempLine += R_3_temp[i].wheel_L.Pos + "," + R_3_temp[i].wheel_L.Vel + "," + R_3_temp[i].wheel_L.Amp + ","
                            + R_3_temp[i].wheel_R.Pos + "," + R_3_temp[i].wheel_R.Vel + "," + R_3_temp[i].wheel_R.Amp + ","
                            + R_3_temp[i].Head.Pos + "," + R_3_temp[i].Head.Vel + "," + R_3_temp[i].Tail.Amp + ","
                            + R_3_temp[i].Tail.Pos + "," + R_3_temp[i].Tail.Vel + "," + R_3_temp[i].Tail.Amp + ",";
                    }
                    else
                    {
                        tempLine += ",,,,,,,,,,,,";
                    }

                    total_lines.Add(tempLine);

                }

                File.WriteAllLines(savePath, total_lines);


                return true;
            }
            catch(Exception e)
            {
                
                return false;
            }
        }

        /// <summary>
        /// csv 파일을 바로 열어준다
        /// </summary>
        /// <returns>열기에 성공하면 true</returns>
        public static bool show_csv_value()
        {
            try
            {
                Process.Start(savePath);
            }
            catch
            {
                return false;
            }

            return true;
        }

    }
}
