using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;    //C:\Users\hfuie\Desktop\3D\b.PNG
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Microsoft.Win32;
using System.IO;

namespace Tracing_Test_Program
{
    /// <summary>
    /// Load_Image.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Load_Image : Window
    {
        public Load_Image()
        {
            InitializeComponent();
            this.Title = "Image Load!";
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            BitmapImage firstBitmapImage = new BitmapImage();

            firstBitmapImage.BeginInit();
            firstBitmapImage.UriSource = new Uri(textBox.Text);  // image Source 불러옴

            firstBitmapImage.EndInit();

            firstimage.Source = firstBitmapImage;   //기존 이미지 출력
            
            StartButton.Visibility = Visibility;     //Start 버튼 활성화
            //흑백 이미지로 변환
            FormatConvertedBitmap monoBitmap = new FormatConvertedBitmap(firstBitmapImage, PixelFormats.Gray2, BitmapPalettes.Gray4, 0);
            //흑백 이미지 출력
            grayimage.Source = monoBitmap;
            

        }
        //start버튼
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SaveWindow(this, 96, "grayimage.jpg");
            CutImage(new BitmapImage(new Uri(@"C:\Users\hfuie\Documents\visual studio 2015\Projects\Tracing_Test_Program\Tracing_Test_Program\bin\Debug\grayimage.jpg")), 470, 49, 417, 472);
            Window1 start = new Window1(grayimage);  //새 윈도우 생성
            this.Close();
            start.Show();
        }
        //파일 경로 찾아옴
        private void open_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "이미지를 선택하세요";
            op.Filter = "All supported graphics |*.jpg;*.jpeg;*.png";
            if (op.ShowDialog() == true)
            {
                BitmapImage firstBitmapImage = new BitmapImage();

                firstBitmapImage.BeginInit();
                firstBitmapImage.UriSource = new Uri(op.FileName);  // image Source 불러옴

                firstBitmapImage.EndInit();

                firstimage.Source = firstBitmapImage;   //기존 이미지 출력

                StartButton.Visibility = Visibility;     //Start 버튼 활성화


                FormatConvertedBitmap monoBitmap = new FormatConvertedBitmap(firstBitmapImage, PixelFormats.Gray32Float, null, 0);
                grayimage.Source = monoBitmap;
            }
        }
        //윈도우 캡쳐
        public static void SaveWindow(Window window, int dpi, string filename)
        {

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width 
                (int)window.Width, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(window);

            SaveRTBAsPNG(rtb, filename);

        }
        //이미지 저장
        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
        //이미지 자름
        public void CutImage(BitmapImage sourceImage,
                               int startX,
                               int startY,
                               int width,
                               int height)
        {
            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = -startX;
            translateTransform.Y = -startY;
            transformGroup.Children.Add(translateTransform);

            DrawingVisual vis = new DrawingVisual();
            DrawingContext cont = vis.RenderOpen();
            cont.PushTransform(transformGroup);
            cont.DrawImage(sourceImage, new Rect(new System.Windows.Size(sourceImage.PixelWidth, sourceImage.PixelHeight)));
            cont.Close();

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            rtb.Render(vis);

            FileStream stream = new FileStream((@"C:\Users\hfuie\Documents\visual studio 2015\Projects\Tracing_Test_Program\Tracing_Test_Program\bin\Debug\grayimage2.jpg"), FileMode.Create);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);
            stream.Close();
        }
    }
}