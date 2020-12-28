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

namespace R_Track_Controller
{
    /// <summary>
    /// Serial_Select_window.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Serial_Select_window : Window
    {
        /// <summary>
        /// 이 창의 포트번호 리스트
        /// </summary>
        private List<string> portlist = new List<string>();

        public Serial_Select_window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 포트 새로고침 버튼 클릭 이벤트
        /// </summary>
        private void Butt_port_refresh_Click(object sender, RoutedEventArgs e)
        {
            Combo_ports.Items.Clear();
            portlist.Clear();

            var asdf = Serial_static.getPortsAvailable();

            foreach (string line in asdf)
            {
                addComboBox(line);
                portlist.Add(line);
            }

            if (asdf.Count >= 1)
            {
                Combo_ports.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 콤보박스에 포트번호들 추가하기
        /// </summary>
        /// <param name="line_">추가할 한줄 포트 이름</param>
        private void addComboBox(string line_)
        {
            var asdf = Combo_ports.Items;
            asdf.Add(new TextBlock { Text = line_ });
        }

        /// <summary>
        /// 연결하기 버튼 클릭 이벤트
        /// </summary>
        private void Butt_begin_connect_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            try
            {
                result = Serial_static.setConnection(portlist[Combo_ports.SelectedIndex]);
            }
            catch
            {
                MessageBox.Show("포트를 선택하지 않았습니다.", "ㄷㄷ", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (result)
            {
                MessageBox.Show(portlist[Combo_ports.SelectedIndex] + " 에 연결 성공", "ㅇㅋ", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("먼가 연결에 실패했습니다.", "ㄷㄷ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}