using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Drawing;
using Microsoft.Win32;
using System.Drawing.Imaging;


namespace Tracing_Test_Program
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1(System.Windows.Controls.Image input_image)
        {
            InitializeComponent();
            input.Source = input_image.Source;       //흑백이미지 출력
            this.Title = "Let’s draw!";
            
        }

        System.Windows.Point currentPoint = new System.Windows.Point();
        // 캔버스에 그림그리는 부분
        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
        }
        //마우스 움직임 감지
        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)    //그림 그리는 코드
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();

                SolidColorBrush brush = new SolidColorBrush(Colors.Black);
                line.StrokeThickness = 3;
                line.X1 = currentPoint.X - 517;
                line.Y1 = currentPoint.Y - 10;
                line.X2 = e.GetPosition(this).X - 517;
                line.Y2 = e.GetPosition(this).Y - 10;
                line.Stroke = brush;
                currentPoint = e.GetPosition(this);

                paintSurface.Children.Add(line);
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)     // 캔버스 초기화
        {
            paintSurface.Children.Clear();
        }
        
        private void end_Click(object sender, RoutedEventArgs e)   //End버튼
        {

            SaveCanvas(this, paintSurface, 96, "paint.jpg");    //캔버스 저장

            CutImage(new BitmapImage(new Uri(@"C:\Users\hfuie\Documents\visual studio 2015\Projects\Tracing_Test_Program\Tracing_Test_Program\bin\Debug\paint.jpg")), 517, 10, 417, 472);


            Bitmap bmp1 = new Bitmap("grayimage2.jpg");
            Bitmap bmp2 = new Bitmap("paint2.jpg");

            List<bool> iHash1 = GetHash(bmp1);
            List<bool> iHash2 = GetHash(bmp2);
            double count = 0;
            for (int i = 0; i < 10000; i++)
            {
                if (iHash1[i] == iHash2[i])
                    count++;
            }
            count /= 100;
            end.Content = count;

            Result r = new Result(count);  //새 윈도우 생성
            this.Close();
            r.Show();
            
        }

        //이미지 흑,백으로 변환
        public static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            //create new image with 16x16 pixel
            Bitmap bmpMin = new Bitmap(bmpSource, new System.Drawing.Size(100, 100));
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true and false
                    lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                }
            }
            return lResult;
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

            FileStream stream = new FileStream((@"C:\Users\hfuie\Documents\visual studio 2015\Projects\Tracing_Test_Program\Tracing_Test_Program\bin\Debug\paint2.jpg"), FileMode.Create);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);
            stream.Close();
        }
        //캔버스 저장
        public static void SaveCanvas(Window window, Canvas canvas, int dpi, string filename)
        {

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width 
                (int)window.Height, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(canvas);

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
    }
}