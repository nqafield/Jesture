using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Jesture
{
   public partial class Form1 : Form
   {
      Graphics _g;

      bool _drawing = false;
      Point? _startPoint = null;
      Point _endPoint = new Point();
      Pen _pen = new Pen(Color.Black);
      List<Tuple<Point, Point>> _lines = new List<Tuple<Point, Point>>();
      bool _panning = false;
      Matrix _panTransform = new Matrix();

      DateTime _strokeStartTime = DateTime.Now;


      public Form1()
      {
         InitializeComponent();

         _g = _paper.CreateGraphics();
      }

      private void paper_Paint(object sender, PaintEventArgs e)
      {
         _g = _paper.CreateGraphics();
         _g.Transform = _panTransform;


         //Make some drawable types: System, Actor, Line

         foreach (var line in _lines)
         {
            _g.DrawLine(_pen, line.Item1, line.Item2);
         }
      }

      private void paper_MouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
            _drawing = true;
            _strokeStartTime = DateTime.Now;
         }
         else if (e.Button == MouseButtons.Right)
         {
            _panning = true;
         }
         _startPoint = e.Location;
      }

      private void paper_MouseUp(object sender, MouseEventArgs e)
      {
         _panning = false;

         _drawing = false;
         _endPoint = e.Location;

         //if (_startPoint.HasValue)
         //{
         //   Point[] location = { _startPoint.Value, _endPoint };
         //   var transform = _panTransform.Clone();
         //   transform.Invert();
         //   transform.TransformPoints(location);

         //   _lines.Add(new Tuple<Point, Point>(location[0], location[1]));
         //   _startPoint = null;
         //}

         //_paper.Invalidate();
      }

      private void paper_MouseMove(object sender, MouseEventArgs e)
      {
         _endPoint = e.Location;

         var elapsedMilliSeconds = (DateTime.Now.Ticks - _strokeStartTime.Ticks) / 10000;
         if (_drawing && elapsedMilliSeconds > 50)
         {
            Point[] location = { e.Location };
            var transform = _panTransform.Clone();
            transform.Invert();
            transform.TransformPoints(location);
            _g.DrawEllipse(_pen, new Rectangle(location[0], new Size(5, 5)));
            
            if (_startPoint.HasValue)
            {
               Point[] location2 = { _startPoint.Value, _endPoint };
               var transform2 = _panTransform.Clone();
               transform2.Invert();
               transform2.TransformPoints(location2);

               _lines.Add(new Tuple<Point, Point>(location2[0], location2[1]));
               _startPoint = e.Location;
            } 
            _strokeStartTime = DateTime.Now;
            _paper.Invalidate();
         }
         else if (_panning)
         {
            _panTransform.Translate(
               e.Location.X - _startPoint.Value.X,
               e.Location.Y - _startPoint.Value.Y);

            _startPoint = e.Location;
            _paper.Invalidate();
         }
      }

      private void _paper_Resize(object sender, EventArgs e)
      {
      }
   }
}
