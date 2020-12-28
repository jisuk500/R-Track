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
    /// change_mode_name.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class change_mode_name : Window
    {

        public bool pressOK { get; set; }
        public string name { get; set; }
        public change_mode_name(string name_)
        {
            pressOK = false;
            name = name_;
            InitializeComponent();
            Textbox_input.Text = name;
        }

        private void Butt_ok_Click(object sender, RoutedEventArgs e)
        {
            name = Textbox_input.Text;
            pressOK = true;

            this.Close();
        }
    }
}
