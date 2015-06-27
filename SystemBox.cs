﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface Drawable
{
   void Draw(Graphics g, Pen p);
}

namespace Jesture
{
   class SystemBox : Drawable
   {
      Rectangle _box = new Rectangle();

      public SystemBox(Point location, Size size)
      {
         _box.Location = location;
         _box.Size = size;
      }

      public void Draw(Graphics g, Pen pen)
      {
         g.DrawRectangle(pen, _box);
      }
   }
}