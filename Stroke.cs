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
      }
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
   }
}
