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

namespace Tracing_Test_Program
{
    /// <summary>
    /// Result.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Result : Window
    {
        int score;  //결과값
        public Result(double count)
        {
            InitializeComponent();
            score = (int)count;
            
        }
        //확인 버튼
        private void b_Click(object sender, RoutedEventArgs e)
        {
            b.Visibility = System.Windows.Visibility.Hidden;
            for (int i = 0; i <= score; i += 1)
            {
                //결과값 출력
                if (score - i > 10)
                    Delay(50);
                else if (score - i > 5)
                    Delay(150);
                else if (score - i > 2)
                    Delay(300);
                else Delay(700);
                result.Content = i;
                result.Content += "점 입니다.";
            }
            result.Content = score + "점 입니다!";
        }
        //시간 지연
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }
    }
}
