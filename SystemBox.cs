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

   class Actor : Drawable
   {
      public static Size? Size;

      Rectangle _circle = new Rectangle();

      public Actor(Point location, Size size)
      {
         _circle.Location = location;
         if (!Size.HasValue) Size = new Size((int)(size.Height), size.Height);
         _circle.Size = Size.Value;
      }

      public void Draw(Graphics gfx, Pen pen)
      {
         gfx.DrawEllipse(pen, _circle);
         gfx.DrawLine(
            pen,
            new Point(_circle.Left + _circle.Width / 2, _circle.Bottom),
            new Point(_circle.Left + _circle.Width / 2, _circle.Bottom + _circle.Height));
         gfx.DrawLine(
            pen,
            new Point(_circle.Left - _circle.Width / 5, _circle.Bottom + _circle.Width / 3),
            new Point(_circle.Right + _circle.Width / 5, _circle.Bottom + _circle.Width / 3));
         gfx.DrawLine(
            pen,
            new Point(_circle.Left + _circle.Width / 2, _circle.Bottom + _circle.Height),
            new Point(_circle.Left, _circle.Bottom + _circle.Height * 2));
         gfx.DrawLine(
            pen,
            new Point(_circle.Left + _circle.Width / 2, _circle.Bottom + _circle.Height),
            new Point(_circle.Right, _circle.Bottom + _circle.Height * 2));
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
