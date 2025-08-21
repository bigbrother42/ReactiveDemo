using BaseDemo.Util;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveDemo.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ReactiveDemo.ViewModels.ComputerVision
{
    public class OpenCVViewModel : ViewModelBase
    {
        #region Fields



        #endregion

        #region ReactiveProperty

        public ReactiveProperty<string> FilePath { get; set; }

        public ReactiveProperty<BitmapSource> OriginalImageSource { get; set; }

        #endregion

        #region ReactiveCommand

        public ReactiveCommand DemoCommand { get; set; }

        public ReactiveCommand ShapeDetectionCommand { get; set; }

        public ReactiveCommand SelectImageCommand { get; set; }

        #endregion

        #region Requests



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            FilePath = new ReactiveProperty<string>(string.Empty).AddTo(DisposablePool);
            OriginalImageSource = new ReactiveProperty<BitmapSource>().AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            DemoCommand = new ReactiveCommand().AddTo(DisposablePool);
            DemoCommand.Subscribe(Demo).AddTo(DisposablePool);

            ShapeDetectionCommand = new ReactiveCommand().AddTo(DisposablePool);
            ShapeDetectionCommand.Subscribe(ShapeDetection).AddTo(DisposablePool);

            SelectImageCommand = new ReactiveCommand().AddTo(DisposablePool);
            SelectImageCommand.Subscribe(SelectImage).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Methods

        private void SelectImage()
        {
            using (var ofd = new OpenFileDialog())
            {
                var result = ofd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {
                    if (!FileUtil.IsImageFile(ofd.FileName))
                    {
                        System.Windows.MessageBox.Show("Please choose image file!", MessageBoxConstant.TITLE_WARNING, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    }

                    FilePath.Value = ofd.FileName;

                    OriginalImageSource.Value = FileUtil.FilePath2BitmapSource(ofd.FileName, 400);
                }
            }
        }

        private void Demo()
        {
            var win1 = "Test Window";
            CvInvoke.NamedWindow(win1);

            using (var frame = new Mat(200, 400, Emgu.CV.CvEnum.DepthType.Cv8U, 3)) 
            {
                frame.SetTo(new Bgr(255, 0, 0).MCvScalar);

                CvInvoke.PutText(frame, "Hello World", new System.Drawing.Point(10, 80), Emgu.CV.CvEnum.FontFace.HersheyComplex, 1.0, new Bgr(0, 255, 0).MCvScalar);

                CvInvoke.Imshow(win1, frame);
                CvInvoke.WaitKey(0);
                CvInvoke.DestroyWindow(win1);

                /*using (var capture = new VideoCapture())
                {
                    while (CvInvoke.WaitKey(1) == -1)
                    {
                        capture.Read(frame);
                        CvInvoke.Imshow(win1, frame);
                    }
                }*/
            }
        }

        private void ShapeDetection()
        {
            if (string.IsNullOrEmpty(FilePath.Value)) return;

            using (var img = new Mat(FilePath.Value))
            {
                var frame = ProcessImageForShapeDetection(img);

                OriginalImageSource.Value = BitmapSource.Create(
                            frame.Width, frame.Height,
                            96, 96,
                            System.Windows.Media.PixelFormats.Bgr24,
                            null,
                            frame.DataPointer,
                            frame.Step * frame.Height,
                            frame.Step);
            }
        }

        public Mat ProcessImageForShapeDetection(Mat img)
        {
            using (UMat gray = new UMat())
            using (UMat cannyEdges = new UMat())
            using (Mat triangleRectangleImage = new Mat(img.Size, DepthType.Cv8U, 3)) //image to draw triangles and rectangles on
            using (Mat circleImage = new Mat(img.Size, DepthType.Cv8U, 3)) //image to draw circles on
            using (Mat lineImage = new Mat(img.Size, DepthType.Cv8U, 3)) //image to drtaw lines on
            {
                //Convert the image to grayscale and filter out the noise
                CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

                //Remove noise
                CvInvoke.GaussianBlur(gray, gray, new Size(3, 3), 1);

                #region circle detection
                double cannyThreshold = 180.0;
                double circleAccumulatorThreshold = 120;
                CircleF[] circles = CvInvoke.HoughCircles(gray, HoughModes.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5);
                #endregion

                #region Canny and edge detection
                double cannyThresholdLinking = 120.0;
                CvInvoke.Canny(gray, cannyEdges, cannyThreshold, cannyThresholdLinking);
                LineSegment2D[] lines = CvInvoke.HoughLinesP(
                    cannyEdges,
                    1, //Distance resolution in pixel-related units
                    Math.PI / 45.0, //Angle resolution measured in radians.
                    20, //threshold
                    30, //min Line width
                    10); //gap between lines
                #endregion

                #region Find triangles and rectangles
                List<Triangle2DF> triangleList = new List<Triangle2DF>();
                List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle
                using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                {
                    CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                    int count = contours.Size;
                    for (int i = 0; i < count; i++)
                    {
                        using (VectorOfPoint contour = contours[i])
                        using (VectorOfPoint approxContour = new VectorOfPoint())
                        {
                            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                            if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contours with area greater than 250
                            {
                                if (approxContour.Size == 3) //The contour has 3 vertices, it is a triangle
                                {
                                    Point[] pts = approxContour.ToArray();
                                    triangleList.Add(new Triangle2DF(pts[0], pts[1], pts[2]));
                                }
                                else if (approxContour.Size == 4) //The contour has 4 vertices.
                                {
                                    #region determine if all the angles in the contour are within [80, 100] degree
                                    bool isRectangle = true;
                                    Point[] pts = approxContour.ToArray();
                                    LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                    for (int j = 0; j < edges.Length; j++)
                                    {
                                        double angle = Math.Abs(edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                        if (angle < 80 || angle > 100)
                                        {
                                            isRectangle = false;
                                            break;
                                        }
                                    }

                                    #endregion

                                    if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                                }
                            }
                        }
                    }
                }
                #endregion

                #region draw triangles and rectangles
                triangleRectangleImage.SetTo(new MCvScalar(0));
                foreach (Triangle2DF triangle in triangleList)
                {
                    CvInvoke.Polylines(triangleRectangleImage, Array.ConvertAll(triangle.GetVertices(), Point.Round), true, new Bgr(Color.DarkBlue).MCvScalar, 2);
                }

                foreach (RotatedRect box in boxList)
                {
                    CvInvoke.Polylines(triangleRectangleImage, Array.ConvertAll(box.GetVertices(), Point.Round), true, new Bgr(Color.DarkOrange).MCvScalar, 2);
                }

                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(triangleRectangleImage,
                    new Rectangle(Point.Empty,
                        new Size(triangleRectangleImage.Width - 1, triangleRectangleImage.Height - 1)),
                    new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(triangleRectangleImage, "Triangles and Rectangles", new Point(20, 20), FontFace.HersheyDuplex, 0.5, new MCvScalar(120, 120, 120));
                #endregion

                #region draw circles
                circleImage.SetTo(new MCvScalar(0));
                foreach (CircleF circle in circles)
                    CvInvoke.Circle(circleImage, Point.Round(circle.Center), (int)circle.Radius, new Bgr(Color.Brown).MCvScalar, 2);

                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(circleImage, new Rectangle(Point.Empty, new Size(circleImage.Width - 1, circleImage.Height - 1)), new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(circleImage, "Circles", new Point(20, 20), FontFace.HersheyDuplex, 0.5, new MCvScalar(120, 120, 120));
                #endregion

                #region draw lines
                lineImage.SetTo(new MCvScalar(0));
                foreach (LineSegment2D line in lines)
                    CvInvoke.Line(lineImage, line.P1, line.P2, new Bgr(Color.Green).MCvScalar, 2);
                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(lineImage, new Rectangle(Point.Empty, new Size(lineImage.Width - 1, lineImage.Height - 1)), new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(lineImage, "Lines", new Point(20, 20), FontFace.HersheyDuplex, 0.5, new MCvScalar(120, 120, 120));
                #endregion

                Mat result = new Mat();
                var row1 = new Mat();
                CvInvoke.HConcat(new Mat[] { img, triangleRectangleImage }, row1);
                var row2 = new Mat();
                CvInvoke.HConcat(new Mat[] { circleImage, lineImage }, row2);
                CvInvoke.VConcat(new Mat[] { row1, row2 }, result);
                return result;
            }
        }

        #endregion
    }
}
