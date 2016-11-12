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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tracing_Test_Program
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 타이틀 수정
            this.Title = "Tracing Test Program";
        }
        // 시작버튼
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Load_Image a = new Load_Image();
            this.Close();
            a.Show();
        }
        //게임방법 버튼
        private void how_Click(object sender, RoutedEventArgs e)
        {
            How_To_Play a = new How_To_Play();
            a.Show();
        }
    }
}
