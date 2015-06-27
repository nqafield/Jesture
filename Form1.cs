using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Jesture
{
   public partial class Form1 : Form
   {
      Graphics _gfx;

      bool _panning = false;
      Point _lastPanLocation = new Point();
      Matrix _panTransform = new Matrix();

      bool _drawing = false;

      DateTime _strokeStartTime = DateTime.Now;
      DateTime _strokeFinishTime = DateTime.Now;
      Stroke _currentStroke = null;

      List<Stroke> _currentGesture = new List<Stroke>();
      Timer _gestureTimer = new Timer();

      Point _segmentStart = new Point();
      Point _segmentEnd = new Point();

      Pen _pen = new Pen(Color.Black);

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

         _gestureTimer.Tick += _gestureTimer_Tick;
         _gestureTimer.Interval = 200;
      }

      void _gestureTimer_Tick(object sender, EventArgs e)
      {
         var elapsedMilliSeconds = (DateTime.Now.Ticks - _strokeFinishTime.Ticks) / 10000;
         if (elapsedMilliSeconds > 1000)
         {
            //This is where the gesture will be recognised.
            _currentGesture.Clear();
            this.Invalidate();
         }
      }

      private void paper_Paint(object sender, PaintEventArgs e)
      {
         _gfx = e.Graphics;
         _gfx.Transform = _panTransform;

         //Make some drawable types: System, Actor, Line
         //Just draw the hand-drawn thing first and then once recognised
         //swap it for the real thing.

         //The hand-drawn thing is just a collection of strokes, each of which
         //is a collection of (line) segments.

         //They have interesting properties like size, shape and postion
         //of the collection of the strokes, and of the individual strokes,
         //and the direction of each segment

         foreach (var stroke in _currentGesture)
         {
            stroke.Draw(_gfx, _pen);
         }

         if (_currentStroke != null)
            _currentStroke.Draw(_gfx, _pen);

         //         _box.Draw(_gfx, _pen);
      }

      private void paper_MouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
            _drawing = true;
            _strokeStartTime = DateTime.Now;
            _currentStroke = new Stroke();
            _segmentStart = e.Location;
            _gestureTimer.Stop();
         }
         else if (e.Button == MouseButtons.Right)
         {
            _panning = true;
            _lastPanLocation = e.Location;
         }
      }

      private void paper_MouseUp(object sender, MouseEventArgs e)
      {
         if (_drawing)
         {
            _currentGesture.Add(_currentStroke);
            _currentStroke = null;
            _drawing = false;
            _strokeFinishTime = DateTime.Now;
            _gestureTimer.Start();
         }

         _panning = false;
         _segmentEnd = e.Location;
      }

      private void paper_MouseMove(object sender, MouseEventArgs e)
      {
         var elapsedMilliSeconds = (DateTime.Now.Ticks - _strokeStartTime.Ticks) / 10000;
         if (_drawing && elapsedMilliSeconds > 50)
         {
            _segmentEnd = e.Location;

            Point[] location = { _segmentStart, _segmentEnd };
            var transform = _panTransform.Clone();
            transform.Invert();
            transform.TransformPoints(location);

            _currentStroke.Add(new Segment(location[0], location[1]));

            _segmentStart = e.Location;
            _strokeStartTime = DateTime.Now;
         }
         else if (_panning)
         {
            _panTransform.Translate(
               e.Location.X - _lastPanLocation.X,
               e.Location.Y - _lastPanLocation.Y);

            _lastPanLocation = e.Location;
         }

         this.Invalidate();
      }

      private void _paper_Resize(object sender, EventArgs e)
      {
      }
   }
}
