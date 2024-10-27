using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FancyControls
{
    public partial class FlowLine : UserControl
    {
        private readonly static double _dashLength = 20;

        private double _totalLength = 0;

        public FlowLine()
        {
            InitializeComponent();
        }

        public PointCollection PolyLinePoints
        {
            get => (PointCollection)GetValue(PolyLinePointsProperty);
            set => SetValue(PolyLinePointsProperty, value);
        }

        public static readonly DependencyProperty PolyLinePointsProperty =
            DependencyProperty.Register(
                "PolyLinePoints",
                typeof(PointCollection),
                typeof(FlowLine),
                new PropertyMetadata(null, OnPolyLinePointsPropertyChanged)
            );

        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public static readonly DependencyProperty IsAnimatingProperty =
            DependencyProperty.Register("IsAnimating", typeof(bool), typeof(FlowLine), new PropertyMetadata(false, OnIsAnimatingChanged));



        private static void OnIsAnimatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FlowLine;
            if (control != null)
            {
                control.UpdateAnimation();
            }
        }

        private void UpdateAnimation()
        {
            if (MovingLine != null)
            {
                if (IsAnimating)
                {
                    // 启动动画  
                    StartAnimation();
                }
                else
                {
                    // 停止动画  
                    StopAnimation();
                }
            }
        }

        private void StartAnimation()
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = -_totalLength,
                Duration = new Duration(TimeSpan.FromSeconds(3)),
                RepeatBehavior = RepeatBehavior.Forever
            };

            MovingLine.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));

            MovingLine.BeginAnimation(Polyline.StrokeDashOffsetProperty, animation);
        }

        private void StopAnimation()
        {
            MovingLine.Stroke.Opacity = 0;
            MovingLine.BeginAnimation(Polyline.StrokeDashOffsetProperty, null);
        }

        private static void OnPolyLinePointsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlowLine flowLine = d as FlowLine;

            PointCollection points = e.NewValue as PointCollection;

            flowLine.UpdatePoints(points);
        }

        private void UpdatePoints(PointCollection points)
        {
            double totalLength = 0;

            for (int i = 0; i < points.Count - 1; i += 1)
            {
                var point1 = points[i];

                var point2 = points[i + 1];

                var segmentLength =
                    Math.Sqrt(Math.Pow((point1.X - point2.X), 2) +
                    Math.Pow((point1.Y - point2.Y), 2));

                totalLength += segmentLength;
            }

            _totalLength = totalLength;

            BottomLine.Points = points;

            MovingLine.Points = points;

            MovingLine.StrokeDashArray = new DoubleCollection { _dashLength, totalLength - _dashLength };
        }
    }
}
