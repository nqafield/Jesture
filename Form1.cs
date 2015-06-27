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

      SystemBox _box = new SystemBox(new Point(), new Size(200, 200));


      public Form1()
      {
         InitializeComponent();

         this.DoubleBuffered = true;
         this.SetStyle(
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
            true);
      }

      private void paper_Paint(object sender, PaintEventArgs e)
      {
         _g = e.Graphics;
         _g.Transform = _panTransform;

         //Make some drawable types: System, Actor, Line
         //Just draw the hand-drawn thing first and then once recognised
         //swap it for the real thing.

         //The hand-drawn thing is just a collection of strokes, each of which
         //is a collection of (line) segments.

         //They have interesting properties like size, shape and postion
         //of the collection of the strokes, and of the individual strokes,
         //and the direction of each segment

         foreach (var line in _lines)
         {
            _g.DrawLine(_pen, line.Item1, line.Item2);
         }

         _box.Draw(_g, _pen);
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

         //this.Invalidate();
      }

      private void paper_MouseMove(object sender, MouseEventArgs e)
      {
         _endPoint = e.Location;

         var elapsedMilliSeconds = (DateTime.Now.Ticks - _strokeStartTime.Ticks) / 10000;
         if (_drawing && elapsedMilliSeconds > 50)
         {
            if (_startPoint.HasValue)
            {
               Point[] location = { _startPoint.Value, _endPoint };
               var transform = _panTransform.Clone();
               transform.Invert();
               transform.TransformPoints(location);

               _lines.Add(new Tuple<Point, Point>(location[0], location[1]));
               _startPoint = e.Location;
            } 
            _strokeStartTime = DateTime.Now;
            this.Invalidate();
         }
         else if (_panning)
         {
            _panTransform.Translate(
               e.Location.X - _startPoint.Value.X,
               e.Location.Y - _startPoint.Value.Y);

            _startPoint = e.Location;
            this.Invalidate();
         }
      }

      private void _paper_Resize(object sender, EventArgs e)
      {
      }
   }
}
