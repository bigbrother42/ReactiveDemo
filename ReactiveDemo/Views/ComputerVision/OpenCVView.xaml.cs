using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MahApps.Metro.Controls;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace ReactiveDemo.Views.ComputerVision
{
    /// <summary>
    /// Interaction logic for OpenCVView.xaml
    /// </summary>
    public partial class OpenCVView : MetroWindow 
    {
        private VideoCapture _capture;
        private Mat _frame;
        private bool _isCapture;

        public OpenCVView()
        {
            InitializeComponent();

            StartCamera();
        }

        private void StartCamera()
        {
            try
            {
                _capture = new VideoCapture(0);
                _frame = new Mat();

                /*_capture.ImageGrabbed += ProcessFrame;
                _capture.Start();*/ 
                _isCapture = true;

                CompositionTarget.Rendering += OnRendering;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Start Capture failed! " + ex.Message);
            }
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (_isCapture && _capture != null && _capture.IsOpened)
            {
                _capture.Read(_frame);
                if (!_frame.IsEmpty)
                {
                    //imageBox.Source = ConvertMatToBitmapSource(_frame);
                }
            }
        }

        /*private void ProcessFrame(object sender, EventArgs e)
        {
            if (!_isCapture) return;

            try
            {
                Mat frame = new Mat();
                _capture.Retrieve(frame);

                var bitmapSource = ConvertMatToBitmapSource(frame);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    imageBox.Source = bitmapSource;
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }*/

        private BitmapSource ConvertMatToBitmapSource(Mat mat)
        {
            using (var bitmap = mat.ToBitmap())
            {
                var hBitmap = bitmap.GetHbitmap();

                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    //DeleteObject(hBitmap);
                }
            }   
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _isCapture = false; 
            CompositionTarget.Rendering -= OnRendering;
            _capture?.Dispose();
            _frame?.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_frame != null && !_frame.IsEmpty)
            {
                string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "capture.png");
                _frame.Save(path);

                MessageBox.Show($"Save successful! {path}");
            }
        }
    }
}
