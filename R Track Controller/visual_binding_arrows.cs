using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Windows.Input;

namespace R_Track_Controller
{
    /// <summary>
    /// 키값을 비쥬얼적으로 바인딩 하기 위한 클래스
    /// </summary>
    public class visual_binding_arrows : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 로봇 3개의 리스트
        /// </summary>
        public List<robot> Robots { get; set; }

        public enum arrow { forward = 0, backward, left, right, stop, none }
        public enum robotNum { one = 0, two, three }
        public List<List<bool>> alreadyPressed { get; set; }

        public List<Key> mapKeys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public class robot : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public bool forward { get; set; }
            public bool backward { get; set; }
            public bool left { get; set; }
            public bool right { get; set; }
            public bool stop { get; set; }

            public robot()
            {
                forward = false;
                backward = false;
                left = false;
                right = false;
                stop = true;
            }

            public void reset()
            {
                forward = false;
                backward = false;
                left = false;
                right = false;
                stop = false;
            }
        }


        /// <summary>
        /// 화살 방향표를 제어하는 클래스 생성자
        /// </summary>
        /// <param name="forward_key">전진키</param>
        /// <param name="backward_key">후진키</param>
        /// <param name="left_key">좌로키</param>
        /// <param name="right_key">우로키</param>
        public visual_binding_arrows(Key forward_key, Key backward_key, Key left_key, Key right_key)
        {
            Robots = new List<robot>();
            for (int i = 0; i < 3; i++)
            {
                Robots.Add(new robot());
            }

            mapKeys = new List<Key>();
            mapKeys.Add(forward_key);
            mapKeys.Add(backward_key);
            mapKeys.Add(left_key);
            mapKeys.Add(right_key);

            alreadyPressed = new List<List<bool>>();
            for(int i=0;i<3;i++)
            {
                alreadyPressed.Add(new List<bool> { false, false, false,false });
            }
        }

        /// <summary>
        /// 다시 정지만 켜져있는 상태로 돌리는 함수
        /// </summary>
        /// <param name="robotNum_">로봇의 넘버</param>
        public void turnOff_stop(robotNum robotNum_)
        {
            int robotNumI = (int)robotNum_;
            Robots[robotNumI].forward = false;
            Robots[robotNumI].backward = false;
            Robots[robotNumI].left = false;
            Robots[robotNumI].right = false;
            Robots[robotNumI].stop = true;
        }

        /// <summary>
        /// 하나의 화살을 키고 나머지는 꺼버리는 함수
        /// </summary>
        /// <param name="robotNum__">로봇 넘버</param>
        /// <param name="arrow_">방향</param>
        /// <returns>킨 애로우</returns>
        public arrow turnOn_arrow(robotNum robotNum__, arrow arrow_)
        {
            int robotNum_ = (int)robotNum__;
            switch (arrow_)
            {
                case arrow.forward:
                    {
                        Robots[robotNum_].reset();
                        Robots[robotNum_].forward = true;
                        return arrow.forward;
                    }
                case arrow.backward:
                    {
                        Robots[robotNum_].reset();
                        Robots[robotNum_].backward = true;
                        return arrow.backward;
                    }
                case arrow.left:
                    {
                        Robots[robotNum_].reset();
                        Robots[robotNum_].left = true;
                        return arrow.left;
                    }
                case arrow.right:
                    {
                        Robots[robotNum_].reset();
                        Robots[robotNum_].right = true;
                        return arrow.right;
                    }
                case arrow.stop:
                    {
                        Robots[robotNum_].reset();
                        Robots[robotNum_].stop = true;
                        return arrow.stop;
                    }
                default:
                    {
                        return arrow.none;
                    }
            }

        }

        /// <summary>
        /// 키다운을 하면 여기로 연결됨
        /// </summary>
        /// <param name="key">어떤 키를 눌렀는지</param>
        /// <param name="checks">체크버튼들의 상태는 어떠한지</param>
        /// <returns>각각의 로봇에 눌린 버튼</returns>
        public arrow[] check_keydown(Key key, bool[] checks)
        {
            arrow[] arrows = new arrow[3] { arrow.none, arrow.none, arrow.none };
            if (key == mapKeys[(int)arrow.forward])
            {
                for (int i = 0; i < 3; i++)
                {
                    if (checks[i] == true)
                    {
                        if (alreadyPressed[i][(int)arrow.forward] == false)
                        {
                            arrows[i] = turnOn_arrow((robotNum)i, arrow.forward);
                            alreadyPressed[i][(int)arrow.forward] = true;
                        }
                    }
                }
            }
            else if (key == mapKeys[(int)arrow.backward])
            {
                for (int i = 0; i < 3; i++)
                {
                    if (checks[i] == true)
                    {
                        if (alreadyPressed[i][(int)arrow.backward] == false)
                        {
                            arrows[i] = turnOn_arrow((robotNum)i, arrow.backward);
                            alreadyPressed[i][(int)arrow.backward] = true;
                        }
                    }
                }
            }
            else if (key == mapKeys[(int)arrow.left])
            {
                for (int i = 0; i < 3; i++)
                {
                    if (checks[i] == true)
                    {
                        if (alreadyPressed[i][(int)arrow.left] == false)
                        {
                            arrows[i] = turnOn_arrow((robotNum)i, arrow.left);
                            alreadyPressed[i][(int)arrow.left] = true;
                        }
                    }
                }
            }
            else if (key == mapKeys[(int)arrow.right])
            {
                for (int i = 0; i < 3; i++)
                {
                    if (checks[i] == true)
                    {
                        if (alreadyPressed[i][(int)arrow.right] == false)
                        {
                            arrows[i] = turnOn_arrow((robotNum)i, arrow.right);
                            alreadyPressed[i][(int)arrow.right] = true;
                        }
                    }
                }
            }


            return arrows;
        }

        /// <summary>
        /// 키를 뗄때 발생하는 이벤트
        /// </summary>
        /// <param name="key">떼진 키</param>
        /// <param name="checks">각각의 체크버튼 상태</param>
        /// <returns>none이면 무시, 스탑이면 스탑</returns>
        public arrow[] check_keyup(Key key, bool[] checks)
        {
            arrow[] arrows = new arrow[3] { arrow.none, arrow.none, arrow.none };
            int indexMapping = mapKeys.FindIndex(x => x == key);
            arrow pressedArrow = (arrow)indexMapping;

            if ((key == mapKeys[0]) || (key == mapKeys[1]) || (key == mapKeys[2]) || (key == mapKeys[3]))
            {
                bool isStop = true;
                for (int i = 0; i < 4; i++)
                {
                    if (key == mapKeys[i])
                    {
                        alreadyPressed[0][(int)pressedArrow] = false;
                        alreadyPressed[1][(int)pressedArrow] = false;
                        alreadyPressed[2][(int)pressedArrow] = false;
                        continue;
                    }
                    else
                    {
                        if (Keyboard.IsKeyDown(mapKeys[i]) == true)
                        {
                            isStop = false;
                        }
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if ((checks[i] == true) && (isStop == true))
                    {
                        arrows[i] = arrow.stop;
                        turnOn_arrow((robotNum)i, arrow.stop);
                    }
                }
            }

            return arrows;

        }


    }
}
