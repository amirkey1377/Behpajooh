using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Skydat.classes2065
{
    public class drawline_chart
    {
        public Pen pen_ch;
        public bool EnableDraw = false, Active = false;
        public System.Drawing.Point FromPoint, ToPoint;
        public struct lines
        {
            public System.Drawing.Point point1, point2;
        };
        public struct multipoint
        {
            public double pointx,pointy;
        };
        public List<lines> Lines = new List<lines>();
        public List<multipoint> Lines_master = new List<multipoint>();

        //public int select(int x, int y) {
        //    for (int i = 0; i < Lines.Count; i++) {
        //        if ((((double)y - Lines[i].point1.Y)) / ((double)(x - Lines[i].point1.X)) == ((double)(Lines[i].point2.Y - Lines[i].point1.Y)) / ((double)(Lines[i].point2.X - Lines[i].point1.X)))
        //            return i;
        //    }

        //    return -1;
        //}
        public void remove(int i)
        {
            Lines.RemoveAt(i);
          
        }
        public void removeall()
        {
            for (int k = Lines.Count - 1; k >= 0; k--)
                Lines.RemoveAt(k);

        }

        public void removeall_master()
        {
            for (int k = Lines_master.Count - 1; k >= 0; k--)
                Lines_master.RemoveAt(k);
        }

        public bool intersect_line()
        {
            try
            {
                Vector v1 = new Vector(Lines_master[0].pointx, Lines_master[0].pointy);
                Vector v2 = new Vector(Lines_master[1].pointx, Lines_master[1].pointy);
                Vector v3 = new Vector(Lines_master[2].pointx, Lines_master[2].pointy);
                Vector v4 = new Vector(Lines_master[3].pointx, Lines_master[3].pointy);
                Vector v5 = new Vector();
                return Vector.LineSegementsIntersect(v1, v2, v3, v4, out v5);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return false;
            }
        }
    }
    public static class Extensions
    {
        private const double Epsilon = 1e-10;

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
        }
    }
}
