using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Text.RegularExpressions;

namespace R_Track_Controller
{
    /// <summary>
    /// reading_interval_window.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class reading_interval_window : Window
    {

        public int currInterval { get; set; }
        public int nowInterval { get; set; }
        public bool isOked { get; set; }
       
        public string titleName { get; set; }

        public reading_interval_window()
        {
            InitializeComponent();

            

            isOked = false;
        }

        /// <summary>
        /// 각각의 프로퍼티들을 실제 ui에 반영하는 함수
        /// </summary>
        /// <param name="currInterval_">현재 인터벌 값</param>
        /// <param name="titleName_">창의 제목</param>
        public void setproperty2UI(int currInterval_,string titleName_)
        {
            currInterval = currInterval_;
            Textbox_input.Text = currInterval.ToString();

            titleName = titleName_;
            this.Title = titleName;
        }

        /// <summary>
        /// 텍스트 박스에서 숫자만 입력할 수 있도록 제한하는 함수
        /// </summary>
        private void Textbox_input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int a = 0;
            if(int.TryParse(e.Text,out a) == false)
            {
                e.Handled = true;
            }
            
        }

        /// <summary>
        /// 확인 버튼 누르면 나옴
        /// </summary>
        private void Butt_ok_Click(object sender, RoutedEventArgs e)
        {
            int parsedInt = 0;
            if (int.TryParse(Textbox_input.Text, out parsedInt) == true)
            {
                isOked = true;
                nowInterval = parsedInt;
                this.Close();
            }
            else
            {
                MessageBox.Show("숫자가 아닙니다", "오류", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        
    }
}
