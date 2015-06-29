using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jesture
{
   class Segment
   {
      public Point Start { get; set; }
      public Point End { get; set; }

      public Segment(Point start, Point end)
      {
         Start = start;
         End = end;

         var vector = new Point(Start.X - End.X, Start.Y - End.Y);

         var i = Math.Abs(vector.X);
         var j = Math.Abs(vector.Y);

         IsAxisAligned = i > j * 3 || j > i * 3;
      }

      public bool IsAxisAligned { get; private set; }
   }

   class Stroke : Drawable
   {
      public List<Segment> Segments = new List<Segment>();

      public void Add(Segment segment)
      {
         Segments.Add(segment);
      }

      public void Draw(Graphics gfx, Pen pen)
      {
         foreach (var segment in Segments)
         {
            gfx.DrawLine(pen, segment.Start, segment.End);
         }
      }

      internal bool IsSquarey()
      {
         int matchCount = 0;

         foreach (var segment in Segments)
         {
            if(segment.IsAxisAligned)
            {
               matchCount++;
            }
         }
         return matchCount > Segments.Count * 0.8;
      }

      internal bool IsEllipsey()
      {
         int matchCount = 0;

         foreach (var segment in Segments)
         {
            if (segment.IsAxisAligned)
            {
               matchCount++;
            }
         }

         var rightSize =
            UseCase.Size.HasValue ?
            Size().Height < UseCase.Size.Value.Height * 2 &&
            Size().Height > UseCase.Size.Value.Height * 0.5 &&
            Size().Width < UseCase.Size.Value.Width * 2 &&
            Size().Width > UseCase.Size.Value.Width * 0.5 :
            true;

         return matchCount < Segments.Count * 0.7 &&
                Size().Width > Size().Height * 1.2 &&
                rightSize;
      }

      internal bool IsStrikeOut()
      {
         int matchCount = 0;

         foreach (var segment in Segments)
         {
            if (segment.IsAxisAligned)
            {
               matchCount++;
            }
         }

         var rightSize = UseCase.Size.HasValue ?
            Size().Height > UseCase.Size.Value.Height * 3 &&
            Size().Width > UseCase.Size.Value.Width * 3 :
            true;

         return matchCount < Segments.Count * 0.4 && rightSize;
      }

      internal bool IsLiney()
      {
         if (Math.Abs(Segments[0].Start.X - Segments[Segments.Count - 1].End.X) > 50 ||
             Math.Abs(Segments[0].Start.Y - Segments[Segments.Count - 1].End.Y) > 50)
         {
            return true;
         }

         return false;
      }

      public Point Location()
      {
         List<Point> points = new List<Point>();

         foreach (var segment in Segments)
         {
            points.Add(segment.Start);
            points.Add(segment.End);
         }

         var min = new Point((int)points.Min(p => p.X), (int)points.Min(p => p.Y));
         var max = new Point((int)points.Max(p => p.X), (int)points.Max(p => p.Y));

         return new Point(min.X, min.Y);
      }

      public Size Size()
      {
         List<Point> points = new List<Point>();

         foreach (var segment in Segments)
         {
            points.Add(segment.Start);
            points.Add(segment.End);
         }

         return new Size(
            (int)(points.Max(p => p.X) - points.Min(p => p.X)),
            (int)(points.Max(p => p.Y) - points.Min(p => p.Y)));
      }
   }
}
