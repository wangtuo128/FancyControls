using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FancyControls
{
    public partial class FlowLine : UserControl
    {
        private readonly static double _dashLength = 20;
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

            BottomLine.Points = points;

            MovingLine.Points = points;

            MovingLine.StrokeDashArray = new DoubleCollection { _dashLength, totalLength - _dashLength };
        }
    }
}
