using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface Drawable
{
   void Draw(Graphics gfx, Pen pen);
}

namespace Jesture
{
   class Line : Drawable
   {
      Point startPoint;
      Point endPoint;

      public Line(Point start, Point end)
      {
         startPoint = start;
         endPoint = end;
      }

      public void Draw(Graphics gfx, Pen pen)
      {
         gfx.DrawLine(pen, startPoint, endPoint);
      }
   }

   class UseCase : Drawable
   {
      public static Size? Size;
 
      Rectangle _ellipse = new Rectangle();

      public UseCase(Point location, Size size)
      {
         _ellipse.Location = location;
         if (!Size.HasValue) Size = new Size((int)(size.Height * 1.4), size.Height);
         _ellipse.Size = Size.Value;
      }

      public void Draw(Graphics gfx, Pen pen)
      {
         gfx.DrawEllipse(pen, _ellipse);
      }
   }

   class SystemBox : Drawable
   {
      Rectangle _box = new Rectangle();

      public SystemBox(Point location, Size size)
      {
         _box.Location = location;
         _box.Size = size;
      }

      public void Draw(Graphics gfx, Pen pen)
      {
         gfx.DrawRectangle(pen, _box);
      }
   }
}
