using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.Windows.Threading;

namespace R_Track_Controller
{


    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// int 형식의 숫자를 범위 내로 구속
        /// </summary>
        /// <param name="num">원본</param>
        /// <param name="min">최소값</param>
        /// <param name="max">최대값</param>
        /// <returns>새츄레이션된 값</returns>
        public int constraintNum(int num, int min, int max)
        {
            if (num < min)
            {
                num = min;
            }
            else if (num > max)
            {
                num = max;
            }

            return num;
        }

        /// <summary>
        /// 슬라이더들을 장전
        /// </summary>
        public void initialize_Sliders()
        {
            List<Slider> tempSlider_1 = new List<Slider>();
            tempSlider_1.Add(Slider_1_Vel);
            tempSlider_1.Add(Slider_1_Head_Pos);
            tempSlider_1.Add(Slider_1_Head_Amp);
            tempSlider_1.Add(Slider_1_Tail_Pos);
            tempSlider_1.Add(Slider_1_Tail_Amp);
            Sliders_main.Add(tempSlider_1);

            List<Slider> tempSlider_2 = new List<Slider>();
            tempSlider_2.Add(Slider_2_Vel);
            tempSlider_2.Add(Slider_2_Head_Pos);
            tempSlider_2.Add(Slider_2_Head_Amp);
            tempSlider_2.Add(Slider_2_Tail_Pos);
            tempSlider_2.Add(Slider_2_Tail_Amp);
            Sliders_main.Add(tempSlider_2);

            List<Slider> tempSlider_3 = new List<Slider>();
            tempSlider_3.Add(Slider_3_Vel);
            tempSlider_3.Add(Slider_3_Head_Pos);
            tempSlider_3.Add(Slider_3_Head_Amp);
            tempSlider_3.Add(Slider_3_Tail_Pos);
            tempSlider_3.Add(Slider_3_Tail_Amp);
            Sliders_main.Add(tempSlider_3);

            List<Slider> tempSlider_4 = new List<Slider>();
            tempSlider_4.Add(Slider_all_vel);
            Sliders_main.Add(tempSlider_4);
        }

        /// <summary>
        /// 슬라이더의 값을 컨트롤
        /// </summary>
        /// <param name="robot_num">로봇 번호</param>
        /// <param name="parts">로봇 부위</param>
        /// <param name="diretion">변경 방향, None일시에는 값을 가져옴</param>
        /// <param name="mode">읽기모드인지 쓰기모드인지 지정. direction이 none일때만 가능</param>
        /// <param name="newValue">쓰기모드일때 써줄 값. 인트만 들어감</param>
        public int SliderControl(RobotNum robotNum, Parts part, Direction direction = Direction.None, Mode mode = Mode.Write, int newValue = 0)
        {
            int robot_number = (int)robotNum;
            int part_ = (int)part;
            var slider = Sliders_main[robot_number][part_];

            int freq = (int)slider.TickFrequency;
            int curValue = (int)slider.Value;

            if (direction == Direction.None)
            {
                if (mode == Mode.Write)
                {
                    slider.Value = newValue;
                    return curValue;
                }
                else if (mode == Mode.Read)
                {
                    return curValue;
                }
            }
            else if (direction == Direction.Up)
            {
                curValue = curValue + freq;
            }
            else if (direction == Direction.Down)
            {
                curValue = curValue - freq;
            }
            curValue = constraintNum(curValue, (int)slider.Minimum, (int)slider.Maximum);
            slider.Value = curValue;
            return curValue;
        }

        /// <summary>
        /// 슬라이더들 넣어놓는 배열
        /// </summary>
        public List<List<Slider>> Sliders_main = new List<List<Slider>>();

        /// <summary>
        /// 화살표 색깔 담당하는 클래스
        /// </summary>
        public visual_binding_arrows arrow_colors;

        /// <summary>
        /// 로봇의 숫자 열거형
        /// </summary>
        public enum RobotNum { one = 0, two, three, all };

        /// <summary>
        /// 로봇의 파츠 열거형
        /// </summary>
        public enum Parts { Vel = 0, Head_Pos = 1, Head_Amp, Tail_Pos, Tail_Amp, None };

        /// <summary>
        /// 로봇의 명령 방향 열거형
        /// </summary>
        public enum Direction { Up, Down, Left, Right, None };

        /// <summary>
        /// 로봇 슬라이더 읽기 모드 열거형
        /// </summary>
        public enum Mode { Read, Write, ON, OFF };

        /// <summary>
        /// 콘솔에 온 메세지들 버퍼
        /// </summary>
        public List<string> console_buffer = new List<string>();

        /// <summary>
        /// 콘솔에 출력할 커맨드들 버퍼
        /// </summary>
        public List<string> console_commands_buffer = new List<string>();

        /// <summary>
        /// 배열 출력에 쓰는 타이머
        /// </summary>
        public DispatcherTimer arrayTimer = new DispatcherTimer();

        public int arrayTimerIntervalMs = -1;

        /// <summary>
        /// 메인윈도우 코드 비하인드 생성자
        /// </summary>
        public MainWindow()
        { 

            InitializeComponent();
            Serial_static.Initialize(this);

            mode_preset_io.initialize();
            mode_preset_io.readPresets();

            arrow_colors = new visual_binding_arrows(Key.W, Key.S, Key.A, Key.D);

            initialize_Sliders();

            Listbox_preset_names.ItemsSource = mode_preset_io.Presets.modes;


            Grid_1_controls.DataContext = arrow_colors.Robots[0];
            Grid_2_controls.DataContext = arrow_colors.Robots[1];
            Grid_3_controls.DataContext = arrow_colors.Robots[2];

            Grid_robotValues.DataContext = robot_values.value_indicator_;

            ArrayInput_static.readArray("ProfileArray.txt");
            ArrayInput_static.setReadingInterval(1);
            var textBlock_ = (TextBlock)(butt_array_interval_set.Content);
            textBlock_.Text = "줄간격: " + (ArrayInput_static.getReadingInterval()).ToString();

            arrayTimerIntervalMs = ArrayInput_static.getReadingInterval();
            arrayTimer.Interval = System.TimeSpan.FromMilliseconds(arrayTimerIntervalMs);
            arrayTimer.Tick += ArrayTimer_Tick;
            textBlock_ = (TextBlock)(butt_time_inteval_set.Content);
            textBlock_.Text = "시간간격: " + arrayTimerIntervalMs.ToString();

        }



        /// <summary>
        /// 콘솔 라인에 한줄을 추가한다. \n과 \r은 없애고 한줄로 만든다.
        /// </summary>
        /// <param name="msg">추가할 메세지</param>
        public void addConsoleLine(string msg)
        {
            int indexSlash = msg.IndexOf('/');

            if ((indexSlash != -1))
            {
                string line = "";

                foreach (string line_ in console_buffer)
                {
                    line += line_;
                }
                console_buffer.Clear();


                var refined_line = string.Concat(msg.Split('\n', '\r'));

                indexSlash = refined_line.IndexOf('/');

                while (true)
                {
                    if (indexSlash == -1)
                    {
                        console_buffer.Add(refined_line);
                        break;
                    }

                    line = line + refined_line.Substring(0, indexSlash + 1);
                    console_commands_buffer.Add(line);
                    line = "";

                    if (indexSlash == refined_line.Length - 1)
                    {
                        break;
                    }
                    else
                    {
                        refined_line = refined_line.Substring(indexSlash + 1);
                        indexSlash = refined_line.IndexOf('/');
                    }
                }

                var temp = Listbox_log.Items;
                foreach (var line_ in console_commands_buffer)
                {
                    //라인에서 로봇 모터들 값을 캐치하는 함수
                    if (robot_values.catch_value_from_log(line_, true) == false)
                    {

                    }

                    temp.Add(new TextBlock { Text = line_ });
                }

                console_commands_buffer.Clear();
                int index = temp.Count;
                Listbox_log.ScrollIntoView(temp.GetItemAt(index - 1));

            }
            else
            {
                var refined_line = string.Concat(msg.Split('\n', '\r'));

                console_buffer.Add(refined_line);
            }


        }

        /// <summary>
        /// 시스템 메세지에 한줄 추가하는 함수
        /// </summary>
        /// <param name="line">추가할 시스템 메세지</param>
        public void addSystemMsgLine(string line)
        {
            Listbox_systemMsg.Items.Add(new TextBlock { Text = line });
            Listbox_systemMsg.ScrollIntoView(Listbox_systemMsg.Items.GetItemAt(Listbox_systemMsg.Items.Count - 1));


        }

        /// <summary>
        /// 연결하기 버튼 클릭 이벤트
        /// </summary>
        private void Butt_Do_connect_Click(object sender, RoutedEventArgs e)
        {
            Serial_Select_window tempwin = new Serial_Select_window();
            tempwin.ShowDialog();

            Textblock_portnum.Text = (Serial_static.portNum).ToString();
        }

        /// <summary>
        /// 연결끊기 버튼 클릭 이벤트
        /// </summary>
        private void Butt_no_connection_Click(object sender, RoutedEventArgs e)
        {
            bool result = Serial_static.delConnection();

            if (result)
            {
                MessageBox.Show("성공적으로 연결을 해제함", "굳", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("먼가꼬임; 이미 끊어진거아닌가", "ㄷㄷ;", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Textblock_portnum.Text = "-1";
        }

        /// <summary>
        /// 연결체크 버튼 클릭 이벤트
        /// </summary>
        private void Butt_check_connection_Click(object sender, RoutedEventArgs e)
        {
            bool result = Serial_static.serialRule.robot_check();
            if (result == false) addSystemMsgLine("send failed");
        }



        /// <summary>
        /// 콘솔 체크박스 클릭 이벤트. 체크박스와 버튼 껐다 킴
        /// </summary>
        private void Checkbox_console_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)Checkbox_console.IsChecked)
            {
                textbox_console.IsEnabled = true;
                Butt_manually_serial_send.IsEnabled = true;
            }
            else
            {
                textbox_console.IsEnabled = false;
                Butt_manually_serial_send.IsEnabled = false;
            }
        }

        /// <summary>
        /// 수동으로 시리얼 데이터 보내기 버튼 클릭 이벤트
        /// </summary>
        private void Butt_manually_serial_send_Click(object sender, RoutedEventArgs e)
        {
            bool result = Serial_static.sendMsg(textbox_console.Text);
            if (result == false) addSystemMsgLine("send failed");

            textbox_console.Text = "";
        }



        /// <summary>
        /// 윈도우 키다운 이벤트
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((textbox_console.IsFocused == false) && (Listbox_preset_names.IsFocused == false) && (Grid_mode_presets_display.IsFocused == false))
            {
                bool[] checks = new bool[3] { (bool)checkbox_1_available.IsChecked, (bool)checkbox_2_available.IsChecked, (bool)checkbox_3_available.IsChecked };
                var results = arrow_colors.check_keydown(e.Key, checks);


                for (int i = 0; i < 3; i++)
                {
                    if (results[i] != visual_binding_arrows.arrow.none)
                    {
                        int speed = SliderControl((RobotNum)i, Parts.Vel, Direction.None, Mode.Read);
                        Serial_static.serialRule.robot_move((visual_binding_arrows.robotNum)i, results[i], speed);
                    }
                }
            }
        }

        /// <summary>
        /// 윈도우 키업 이벤트
        /// </summary>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

            if ((textbox_console.IsFocused == false) && (Listbox_preset_names.IsFocused == false) && (Grid_mode_presets_display.IsFocused == false))
            {
                bool[] checks = new bool[3] { (bool)checkbox_1_available.IsChecked, (bool)checkbox_2_available.IsChecked, (bool)checkbox_3_available.IsChecked };
                var results = arrow_colors.check_keyup(e.Key, checks);

                for (int i = 0; i < 3; i++)
                {
                    if (results[i] != visual_binding_arrows.arrow.none)
                    {
                        int speed = SliderControl((RobotNum)i, Parts.Vel, Direction.None, Mode.Read);
                        Serial_static.serialRule.robot_move((visual_binding_arrows.robotNum)i, results[i], speed);
                    }
                }
            }
        }



        /// <summary>
        /// 윈도우가 닫힘 이벤트. 연결중이라면 꺼준다.
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Serial_static.isConnected == true)
            {
                bool result = Serial_static.delConnection();
                MessageBox.Show("시리얼 자동으로 끊고 종료합니다.", "ㅂㅂ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        /// <summary>
        /// 1번 로봇 속도 느리게 하는 버튼
        /// </summary>
        private void Butt_1_Vel_slower_Click(object sender, RoutedEventArgs e)
        {
            SliderControl(RobotNum.one, Parts.Vel, Direction.Down);
        }

        /// <summary>
        /// 1번 로봇 속도 빠르게 하는 버튼
        /// </summary>
        private void Butt_1_Vel_faster_Click(object sender, RoutedEventArgs e)
        {
            SliderControl(RobotNum.one, Parts.Vel, Direction.Up);
        }

        /// <summary>
        /// 2번 로봇 속도 느리게 하는 버튼
        /// </summary>
        private void Butt_2_Vel_slower_Click(object sender, RoutedEventArgs e)
        {
            SliderControl(RobotNum.two, Parts.Vel, Direction.Down);
        }

        /// <summary>
        /// 2번 로봇 속도 빠르게 하는 버튼
        /// </summary>
        private void Butt_2_Vel_faster_Click(object sender, RoutedEventArgs e)
        {
            SliderControl(RobotNum.two, Parts.Vel, Direction.Up);
        }

        /// <summary>
        /// 3번 로봇 속도 느리게 하는 버튼
        /// </summary>
        private void Butt_3_Vel_slower_Click(object sender, RoutedEventArgs e)
        {
            SliderControl(RobotNum.three, Parts.Vel, Direction.Down);
        }

        /// <summary>
        /// 3번 로봇 속도 빠르게 하는 버튼
        /// </summary>
        private void Butt_3_Vel_faster_Click(object sender, RoutedEventArgs e)
        {
            SliderControl(RobotNum.three, Parts.Vel, Direction.Up);
        }



        /// <summary>
        /// 전체 로봇 속도 내리는 버튼
        /// </summary>
        private void Butt_all_vel_Minus_Click(object sender, RoutedEventArgs e)
        {
            SliderControl(RobotNum.all, Parts.Vel, Direction.Down);
        }

        /// <summary>
        /// 전체 로봇 속도 올리는 버튼
        /// </summary>
        private void Butt_all_vel_Plus_Click(object sender, RoutedEventArgs e)
        {
            SliderControl(RobotNum.all, Parts.Vel, Direction.Up);
        }

        /// <summary>
        /// 전체 로봇 속도 조정 바를 바로바로 동기화할지 체크박스
        /// </summary>
        private void Checkbox_all_vel_sync_Click(object sender, RoutedEventArgs e)
        {
            if (Checkbox_all_vel_sync.IsChecked == true)
            {
                Butt_all_vel_apply.IsEnabled = false;
            }
            else
            {
                Butt_all_vel_apply.IsEnabled = true;
            }
        }

        /// <summary>
        /// 전체 로봇 속도 조정 바대로 전체 로봇의 속도를 지금 동기화하는 함수
        /// </summary>
        public void all_vel_apply()
        {
            int curVal = (int)Slider_all_vel.Value;
            SliderControl(RobotNum.one, Parts.Vel, Direction.None, Mode.Write, curVal);
            SliderControl(RobotNum.two, Parts.Vel, Direction.None, Mode.Write, curVal);
            SliderControl(RobotNum.three, Parts.Vel, Direction.None, Mode.Write, curVal);
        }

        /// <summary>
        /// 전체 로봇 속도 조정 바대로 전체 로봇의 속도를 지금 동기화할지 버튼
        /// </summary>
        private void Butt_all_vel_apply_Click(object sender, RoutedEventArgs e)
        {
            all_vel_apply();
        }

        /// <summary>
        /// 전체 로봇 속도 조정 바의 값이 변할때 이벤트. 체크되어있으면 즉시동기화함
        /// </summary>
        private void Slider_all_vel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsLoaded == true)
            {
                if (Checkbox_all_vel_sync.IsChecked == true)
                {
                    all_vel_apply();
                }
            }
        }



        /// <summary>
        /// 체크박스 1 허용할지 말지 클릭
        /// </summary>
        private void Checkbox_1_available_Click(object sender, RoutedEventArgs e)
        {
            var check = (CheckBox)sender;
            if (check.IsChecked == true)
            {
                Rect_1_curtain.Visibility = Visibility.Collapsed;
            }
            else
            {
                Rect_1_curtain.Visibility = Visibility.Visible;
                arrow_colors.turnOff_stop(visual_binding_arrows.robotNum.one);
                Serial_static.serialRule.robot_move(visual_binding_arrows.robotNum.one, visual_binding_arrows.arrow.stop, 0);
            }
        }

        /// <summary>
        /// 체크박스 2 허용할지 말지 클릭
        /// </summary>
        private void Checkbox_2_available_Click(object sender, RoutedEventArgs e)
        {
            var check = (CheckBox)sender;
            if (check.IsChecked == true)
            {
                Rect_2_curtain.Visibility = Visibility.Collapsed;
            }
            else
            {
                Rect_2_curtain.Visibility = Visibility.Visible;
                arrow_colors.turnOff_stop(visual_binding_arrows.robotNum.two);
                Serial_static.serialRule.robot_move(visual_binding_arrows.robotNum.two, visual_binding_arrows.arrow.stop, 0);
            }
        }

        /// <summary>
        /// 체크박스 3 허용할지 말지 클릭
        /// </summary>
        private void Checkbox_3_available_Click(object sender, RoutedEventArgs e)
        {
            var check = (CheckBox)sender;
            if (check.IsChecked == true)
            {
                Rect_3_curtain.Visibility = Visibility.Collapsed;
            }
            else
            {
                Rect_3_curtain.Visibility = Visibility.Visible;
                arrow_colors.turnOff_stop(visual_binding_arrows.robotNum.three);
                Serial_static.serialRule.robot_move(visual_binding_arrows.robotNum.three, visual_binding_arrows.arrow.stop, 0);
            }
        }



        /// <summary>
        /// 모드 바로 적용 및 즉시 실행까지 하는 코드 함수
        /// </summary>
        private void mode_apply_and_send_Click()
        {
            int selected_index = Listbox_preset_names.SelectedIndex;

            int robotNum;
            Parts parts_ = Parts.None;
            if (selected_index != -1)
            {
                var selected_mode = mode_preset_io.Presets.modes[selected_index];

                for (int r = 0; r < 3; r++)
                {
                    for (int n = 0; n < 5; n++)
                    {
                        if (selected_mode.Robots[r].nodes[n].value_available_new == true)
                        {
                            SliderControl((RobotNum)r, (Parts)n, Direction.None, Mode.Write,
                                selected_mode.Robots[r].nodes[n].value_new);

                            robotNum = r;

                            try
                            {
                                parts_ = (Parts)n;
                            }
                            catch
                            {

                            }

                            if ((parts_ != Parts.None) && (parts_ != Parts.Vel))
                            {
                                Serial_static.serialRule.robot_apply_value(
                                     (RobotNum)robotNum, parts_, selected_mode.Robots[r].nodes[n].value_new);
                            }
                        }
                    }
                }

            }

            if ((textbox_console.IsFocused == false) && (Listbox_preset_names.IsFocused == false))
            {
                bool[] checks = new bool[3] { (bool)checkbox_1_available.IsChecked, (bool)checkbox_2_available.IsChecked, (bool)checkbox_3_available.IsChecked };
                var results = arrow_colors.check_keydown(Key.W, checks);


                for (int i = 0; i < 3; i++)
                {
                    if (results[i] != visual_binding_arrows.arrow.none)
                    {
                        int speed = SliderControl((RobotNum)i, Parts.Vel, Direction.None, Mode.Read);
                        Serial_static.serialRule.robot_move((visual_binding_arrows.robotNum)i, results[i], speed);
                    }
                }
            }
        }

        /// <summary>
        /// 버튼 모드 적용 버튼 클릭 이벤트
        /// </summary>
        private void Butt_mode_apply_Click(object sender, RoutedEventArgs e)
        {
            int selected_index = Listbox_preset_names.SelectedIndex;
            if (selected_index != -1)
            {
                var selected_mode = mode_preset_io.Presets.modes[selected_index];

                for (int r = 0; r < 3; r++)
                {
                    for (int n = 0; n < 5; n++)
                    {
                        if (selected_mode.Robots[r].nodes[n].value_available_new == true)
                        {
                            SliderControl((RobotNum)r, (Parts)n, Direction.None, Mode.Write,
                                selected_mode.Robots[r].nodes[n].value_new);
                        }
                    }
                }

            }



        }

        /// <summary>
        /// 버튼 모드 바로 적용 및 즉시 실행까지 하는 이벤트
        /// </summary>
        private void Butt_mode_apply_and_send_Click(object sender, RoutedEventArgs e)
        {
            mode_apply_and_send_Click();
        }

        /// <summary>
        /// 버튼 모드 불러오기 버튼 클릭
        /// </summary>
        private void Butt_mode_get_Click(object sender, RoutedEventArgs e)
        {
            mode_preset_io.readPresets();
            Listbox_preset_names.ItemsSource = null;
            Listbox_preset_names.ItemsSource = mode_preset_io.Presets.modes;
        }

        /// <summary>
        /// 버튼 모드 저장하기 버튼 클릭
        /// </summary>
        private void Butt_mode_save_Click(object sender, RoutedEventArgs e)
        {
            mode_preset_io.updateNewtoNow();
            mode_preset_io.savePresets();
            Listbox_preset_names.ItemsSource = null;
            Listbox_preset_names.ItemsSource = mode_preset_io.Presets.modes;
        }

        /// <summary>
        /// 프리셋 모드 추가 버튼
        /// </summary>
        private void Butt_mode_add_Click(object sender, RoutedEventArgs e)
        {
            Mode_Presets.mode addingMode = new Mode_Presets.mode();

            addingMode.Robots[0].nodes[0].value = 0;
            int newVal = 0;

            for (int r = 0; r < 3; r++)
            {
                for (int n = 0; n < 5; n++)
                {
                    newVal = SliderControl((RobotNum)r, (Parts)n, Direction.None, Mode.Read);
                    addingMode.Robots[r].nodes[n].value = newVal;
                    addingMode.Robots[r].nodes[n].value_new = newVal;
                    addingMode.Robots[r].nodes[n].value_available = false;
                    addingMode.Robots[r].nodes[n].value_available_new = false;
                }
            }

            mode_preset_io.Presets.modes.Add(addingMode);
            Listbox_preset_names.ItemsSource = null;
            Listbox_preset_names.ItemsSource = mode_preset_io.Presets.modes;
        }

        /// <summary>
        /// 프리셋 모드 삭제 버튼
        /// </summary>
        private void Butt_mode_del_Click(object sender, RoutedEventArgs e)
        {
            int curTabIndex = Listbox_preset_names.SelectedIndex;
            if (curTabIndex != -1)
            {
                mode_preset_io.Presets.modes.RemoveAt(curTabIndex);
                Listbox_preset_names.ItemsSource = null;
                Listbox_preset_names.ItemsSource = mode_preset_io.Presets.modes;
            }
        }

        /// <summary>
        /// 프리셋 설정의 포커스를 잃게 해줌
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid_main.Focus();

        }

        /// <summary>
        /// 모드 이름 변경하는 버튼 클릭 이벤트
        /// </summary>
        private void Butt_change_modename_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            int current_index = Listbox_preset_names.SelectedIndex;
            if (current_index != -1)
            {
                name = mode_preset_io.Presets.modes[current_index].modeName;
                var tempWin = new change_mode_name(name);
                tempWin.ShowDialog();
                if (tempWin.pressOK == true)
                {
                    mode_preset_io.Presets.modes[current_index].modeName = tempWin.name;
                    Listbox_preset_names.ItemsSource = null;
                    Listbox_preset_names.ItemsSource = mode_preset_io.Presets.modes;
                }
            }

        }

        /// <summary>
        /// 프리셋 리스트박스의 선택된 아이템이 바뀔때 이벤트 함수
        /// </summary>
        private void Listbox_preset_names_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkbox_mode_instant_apply.IsChecked == true)
            {
                if (Listbox_preset_names.SelectedIndex >= 0)
                {
                    mode_apply_and_send_Click();
                }
            }
        }

        /// <summary>
        /// 모드 리스트 선택된 아이템 한칸 위로 순서 올리는 버튼 클릭 이벤트
        /// </summary>
        private void Butt_preset_list_selecteditem_up_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = Listbox_preset_names.SelectedIndex;

            if (selectedIndex >= 1)
            {
                var tempmode = mode_preset_io.Presets.modes[selectedIndex];
                mode_preset_io.Presets.modes[selectedIndex] = mode_preset_io.Presets.modes[selectedIndex - 1];
                mode_preset_io.Presets.modes[selectedIndex - 1] = tempmode;

                Listbox_preset_names.ItemsSource = null;
                Listbox_preset_names.ItemsSource = mode_preset_io.Presets.modes;
                Listbox_preset_names.SelectedIndex = selectedIndex - 1;
            }
            else
            {
                addSystemMsgLine("한칸 위로 올릴 수 없습니다.");
            }
        }

        /// <summary>
        /// 모드 리스트 선택된 아이템 한칸 아래로 순서 내리는 버튼 클릭 이벤트
        /// </summary>
        private void Butt_preset_list_selecteditem_down_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = Listbox_preset_names.SelectedIndex;

            if ((selectedIndex >= 0) && (selectedIndex < Listbox_preset_names.Items.Count - 1))
            {
                var tempmode = mode_preset_io.Presets.modes[selectedIndex];
                mode_preset_io.Presets.modes[selectedIndex] = mode_preset_io.Presets.modes[selectedIndex + 1];
                mode_preset_io.Presets.modes[selectedIndex + 1] = tempmode;

                Listbox_preset_names.ItemsSource = null;
                Listbox_preset_names.ItemsSource = mode_preset_io.Presets.modes;
                Listbox_preset_names.SelectedIndex = selectedIndex + 1;
            }
            else
            {
                addSystemMsgLine("한칸 아래로 내릴 수 없습니다.");
            }
        }




        /// <summary>
        /// 버튼들 이름에서 로봇 정보를 뽑아내는 함수
        /// </summary>
        /// <param name="name">버튼의 이름</param>
        /// <param name="robotnum_">로봇 번호</param>
        /// <param name="parts_">부분</param>
        /// <param name="direction_">방향</param>
        /// <returns></returns>
        private bool ComponentName2num_part_directions(string name, ref RobotNum robotnum_, ref Parts parts_, ref Direction direction_)
        {
            robotnum_ = RobotNum.all;
            parts_ = Parts.None;
            direction_ = Direction.None;

            var splited = name.Split('_');

            if (splited[1] == "1")
            {
                robotnum_ = RobotNum.one;
            }
            else if (splited[1] == "2")
            {
                robotnum_ = RobotNum.two;
            }
            else if (splited[1] == "3")
            {
                robotnum_ = RobotNum.three;
            }

            if (splited[2] == "Head")
            {
                if (splited[3] == "Pos")
                {
                    parts_ = Parts.Head_Pos;
                }
                else if (splited[3] == "Amp")
                {
                    parts_ = Parts.Head_Amp;
                }
            }
            else if (splited[2] == "Tail")
            {
                if (splited[3] == "Pos")
                {
                    parts_ = Parts.Tail_Pos;
                }
                else if (splited[3] == "Amp")
                {
                    parts_ = Parts.Tail_Amp;
                }
            }

            if (splited[4] == "Minus")
            {
                direction_ = Direction.Down;
            }
            else if (splited[4] == "Plus")
            {
                direction_ = Direction.Up;
            }

            if ((robotnum_ == RobotNum.all) || (parts_ == Parts.None) || (direction_ == Direction.None))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 버튼들 이름에서 로봇 정보를 뽑아내는 함수
        /// </summary>
        /// <param name="name">버튼의 이름</param>
        /// <param name="robotnum_">로봇 번호</param>
        /// <param name="parts_">부분</param>
        /// <returns></returns>
        private bool ComponentName2num_part_directions(string name, ref RobotNum robotnum_, ref Parts parts_)
        {
            robotnum_ = RobotNum.all;
            parts_ = Parts.None;

            var splited = name.Split('_');

            if (splited[1] == "1")
            {
                robotnum_ = RobotNum.one;
            }
            else if (splited[1] == "2")
            {
                robotnum_ = RobotNum.two;
            }
            else if (splited[1] == "3")
            {
                robotnum_ = RobotNum.three;
            }

            if (splited[2] == "head")
            {
                if (splited[3] == "pos")
                {
                    parts_ = Parts.Head_Pos;
                }
                else if (splited[3] == "amp")
                {
                    parts_ = Parts.Head_Amp;
                }
            }
            else if (splited[2] == "tail")
            {
                if (splited[3] == "pos")
                {
                    parts_ = Parts.Tail_Pos;
                }
                else if (splited[3] == "amp")
                {
                    parts_ = Parts.Tail_Amp;
                }
            }

            if ((robotnum_ == RobotNum.all) || (parts_ == Parts.None))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 통합 헤드테일 업 다운 버튼 클릭
        /// </summary>
        private void Butt_HeadTail_Arrow_Click(object sender, RoutedEventArgs e)
        {
            var clickedButt = (Button)sender;
            string name = clickedButt.Name;

            RobotNum robotnum_ = RobotNum.all;
            Parts parts_ = Parts.None;
            Direction direction_ = Direction.None;

            if (ComponentName2num_part_directions(name, ref robotnum_, ref parts_, ref direction_) == true)
            {
                SliderControl(robotnum_, parts_, direction_);
            }

        }

        /// <summary>
        /// 통합 헤드테일 어플라이 버튼 클릭
        /// </summary>
        private void Butt_HeadPos_apply_Click(object sender, RoutedEventArgs e)
        {
            var clickedButt = (Button)sender;
            string name = clickedButt.Name;
            int value = 0;

            RobotNum robotnum_ = RobotNum.all;
            Parts parts_ = Parts.None;


            if (ComponentName2num_part_directions(name, ref robotnum_, ref parts_) == true)
            {
                value = SliderControl(robotnum_, parts_, Direction.None, Mode.Read);
                Serial_static.serialRule.robot_apply_value(robotnum_, parts_, value);
            }
        }

        /// <summary>
        /// 로봇 값 가져오기 시작 버튼 클릭 이벤트
        /// </summary>
        private void Butt_robot_value_give_Click(object sender, RoutedEventArgs e)
        {
            if (Serial_static.serialRule.robot_get_value_mode(true) == false)
            {
                addSystemMsgLine("send Failed");
            }
        }

        /// <summary>
        /// 로봇 값 가져오기 종료 버튼 클릭 이벤트
        /// </summary>
        private void Butt_robot_value_stop_Click(object sender, RoutedEventArgs e)
        {
            if (Serial_static.serialRule.robot_get_value_mode(false) == false)
            {
                addSystemMsgLine("send Failed!");
            }

        }

        /// <summary>
        /// 로봇 값 가져온거 세이브 하는 버튼
        /// </summary>
        private void Butt_robot_value_save_Click(object sender, RoutedEventArgs e)
        {
            if (robot_values.save_values_to_csv() == true)
            {
                addSystemMsgLine("save completed to Robot_values.txt");
            }
            else
            {
                addSystemMsgLine("failed to save logs");
            }
        }

        /// <summary>
        /// 로봇 값 읽은거 초기화하기
        /// </summary>
        private void Butt_saved_values_delete_Click(object sender, RoutedEventArgs e)
        {
            Listbox_log.Items.Clear();
            robot_values.values.Clear();
            addSystemMsgLine("cleared the logs!");
        }

        /// <summary>
        /// csv 파일 저장된거 열어보기
        /// </summary>
        private void Butt_saved_values_show_Click(object sender, RoutedEventArgs e)
        {
            robot_values.show_csv_value();
        }



        /// <summary>
        /// 배열 타이머가 틱당 하는 작업
        /// </summary>
        private void ArrayTimer_Tick(object sender, System.EventArgs e)
        {
            string line = "";
            if (ArrayInput_static.readNextLine(ref line) == true)
            {
                int timeSpan;
                Serial_static.serialRule.send_arrayProfile(line,out timeSpan);
                //addSystemMsgLine(line);

                arrayTimerIntervalMs = timeSpan;
                arrayTimer.Interval = System.TimeSpan.FromMilliseconds(arrayTimerIntervalMs);
                var textBlock_ = (TextBlock)(butt_time_inteval_set.Content);
                textBlock_.Text = "시간간격: " + arrayTimerIntervalMs.ToString();
            }
            else
            {
                addSystemMsgLine("failed to array reading lines");
                arrayTimer.Stop();
                ArrayInput_static.initialize();
            }
        }

        /// <summary>
        /// 배열 실행 버튼 클릭 이벤트
        /// </summary>
        private void Butt_array_start_Click(object sender, RoutedEventArgs e)
        {
            string line = "";
            if (ArrayInput_static.readNextLine(ref line) == true)
            {
                addSystemMsgLine("array profile sending start!");
                int timeSpan;
                Serial_static.serialRule.send_arrayProfile(line,out timeSpan);
                //addSystemMsgLine(line);

                arrayTimerIntervalMs = timeSpan;
                arrayTimer.Interval = System.TimeSpan.FromMilliseconds(arrayTimerIntervalMs);
                var textBlock_ = (TextBlock)(butt_time_inteval_set.Content);
                textBlock_.Text = "시간간격: " + arrayTimerIntervalMs.ToString();

                arrayTimer.Start();
            }
            else
            {
                addSystemMsgLine("failed to start array profile sending!");
            }

        }

        /// <summary>
        /// 배열 중지 버튼 클릭 이벤트
        /// </summary>
        private void Butt_array_stop_Click(object sender, RoutedEventArgs e)
        {
            addSystemMsgLine("array reading stopped");
            arrayTimer.Stop();
            ArrayInput_static.initialize();
        }

        /// <summary>
        /// 배열 값 노트패드로 열기
        /// </summary>
        private void Butt_array_shownotepad_Click(object sender, RoutedEventArgs e)
        {
            if (ArrayInput_static.openTextEditor() == true)
            {
                addSystemMsgLine("opening the text editor..");
            }
            else
            {
                addSystemMsgLine("failed to open as text editor!");
            }
        }

        /// <summary>
        /// 배열값 리로드 하는 버튼 클릭 이벤트
        /// </summary>
        private void Butt_array_reload_Click(object sender, RoutedEventArgs e)
        {
            if (ArrayInput_static.reloadLines() == true)
            {
                addSystemMsgLine("array reloaded!");
            }
            else
            {
                addSystemMsgLine("array reload Failed!!");
            }
        }

        /// <summary>
        /// 타이머 시간간격 변경하는 버튼 클릭시
        /// </summary>
        private void Butt_time_inteval_set_Click(object sender, RoutedEventArgs e)
        {
            reading_interval_window valueChangingWin = new reading_interval_window();
            valueChangingWin.setproperty2UI(arrayTimerIntervalMs, "읽어들이는 시간 간격 설정");
            valueChangingWin.ShowDialog();

            if (valueChangingWin.isOked == true)
            {
                arrayTimerIntervalMs = valueChangingWin.nowInterval;
                arrayTimer.Interval = System.TimeSpan.FromMilliseconds(arrayTimerIntervalMs);
                var textBlock_ = (TextBlock)(butt_time_inteval_set.Content);
                textBlock_.Text = "시간간격: " + arrayTimerIntervalMs.ToString();
                addSystemMsgLine("changed the timer interval");
            }
            else
            {
                addSystemMsgLine("cancel timer interval chainging");
            }
        }

        /// <summary>
        /// 배열 줄간격 변경하는 버튼 클릭시
        /// </summary>
        private void Butt_array_interval_set_Click(object sender, RoutedEventArgs e)
        {
            reading_interval_window valueChangingWin = new reading_interval_window();
            valueChangingWin.setproperty2UI(ArrayInput_static.getReadingInterval(), "읽어들이는 줄간격 설정");
            valueChangingWin.ShowDialog();

            if (valueChangingWin.isOked == true)
            {
                ArrayInput_static.setReadingInterval(valueChangingWin.nowInterval);
                var textBlock_ = (TextBlock)(butt_array_interval_set.Content);
                textBlock_.Text = "줄간격: " + (ArrayInput_static.getReadingInterval()).ToString();
                addSystemMsgLine("changed the array reading interval");
            }
            else
            {
                addSystemMsgLine("cancel array reading interval chainging");
            }
        }

    }
}
