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
      bool _zooming = false;
      float _scale = 1;
      Point _lastPanLocation = new Point();
      Point _lastZoomLocation = new Point();
      Matrix _panTransform = new Matrix();
      Matrix _zoomTransform = new Matrix();

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
      List<Drawable> _drawingElements = new List<Drawable>();

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

         _gfx = this.CreateGraphics();

         _gfx.SmoothingMode = SmoothingMode.HighQuality;
      }

      void _gestureTimer_Tick(object sender, EventArgs e)
      {
         var elapsedMilliSeconds = (DateTime.Now.Ticks - _strokeFinishTime.Ticks) / 10000;
         if (elapsedMilliSeconds > 1000)
         {
            if (_currentGesture.Count == 1)
            {
               var gesture = _currentGesture[0];
               if (gesture.IsSquarey())
               {
                  _drawingElements.Add(
                     new SystemBox(gesture.Location(), gesture.Size()));
               }

               if (gesture.IsEllipsey())
               {
                  _drawingElements.Add(
                     new UseCase(gesture.Location(), gesture.Size()));
               }
            }
            else if (_currentGesture.Count == 2)
            {
               if (_currentGesture[0].IsStrikeOut() &&
                   _currentGesture[0].IsStrikeOut())
               {
                  _drawingElements.Clear();
                  UseCase.Size = null;
               }
            }

            _currentGesture.Clear();
            this.Invalidate();
         }
      }

      private void paper_Paint(object sender, PaintEventArgs e)
      {
         _gfx = e.Graphics;
         _gfx.Transform = _zoomTransform;
         _gfx.MultiplyTransform(_panTransform);

         _gfx.SmoothingMode = SmoothingMode.HighQuality;

         //Make some drawable types: System, Actor, Line
         //Just draw the hand-drawn thing first and then once recognised
         //swap it for the real thing.

         //The hand-drawn thing is just a collection of strokes, each of which
         //is a collection of (line) segments.

         //They have interesting properties like size, shape and postion
         //of the collection of the strokes, and of the individual strokes,
         //and the direction of each segment


         foreach (var element in _drawingElements)
         {
            element.Draw(_gfx, _pen);
         }

         foreach (var stroke in _currentGesture)
         {
            stroke.Draw(_gfx, _pen);
         }

         if (_currentStroke != null)
            _currentStroke.Draw(_gfx, _pen);
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
         else if (e.Button == MouseButtons.Middle)
         {
            _zooming = true;
            _lastZoomLocation = e.Location;
         }
      }

      private void paper_MouseUp(object sender, MouseEventArgs e)
      {
         if (_drawing)
         {
            _segmentEnd = e.Location;
            Point[] location = { _segmentStart, _segmentEnd };
            var transform = _panTransform.Clone();
            transform.Invert();
            transform.TransformPoints(location);

            _currentGesture.Add(_currentStroke);

            _currentStroke = null;
            _drawing = false;
            _strokeFinishTime = DateTime.Now;
            _gestureTimer.Start();
         }

         _panning = false;
         _zooming = false;
      }

      private void paper_MouseMove(object sender, MouseEventArgs e)
      {
         var elapsedMilliSeconds = (DateTime.Now.Ticks - _strokeStartTime.Ticks) / 10000;
         if (_drawing &&
             elapsedMilliSeconds > 50 &&
             (Math.Abs(_segmentStart.X - e.Location.X) > 3 ||
              Math.Abs(_segmentStart.Y - e.Location.Y) > 3))
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
         else if (_zooming)
         {
            _scale -= (float)(e.Location.Y - _lastZoomLocation.Y)/1000;

            _zoomTransform.Scale(_scale, _scale);

            _lastZoomLocation = e.Location;
         }

         this.Invalidate();
      }

      private void _paper_Resize(object sender, EventArgs e)
      {
      }
   }
}
