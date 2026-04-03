using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
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
            Vector v1 = new Vector(Lines_master[0].pointx, Lines_master[0].pointy);
            Vector v2 = new Vector(Lines_master[1].pointx, Lines_master[1].pointy);
            Vector v3 = new Vector(Lines_master[2].pointx, Lines_master[2].pointy);
            Vector v4 = new Vector(Lines_master[3].pointx, Lines_master[3].pointy);
            Vector v5 = new Vector();
            return Vector.LineSegementsIntersect(v1,v2,v3,v4,out v5);

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
    public  class Vector
    {
        public double X;
        public double Y;

        // Constructors.
        public Vector(double x, double y) { X = x; Y = y; }
        public Vector() : this(double.NaN, double.NaN) { }

        public static Vector operator -(Vector v, Vector w)
        {
            return new Vector(v.X - w.X, v.Y - w.Y);
        }

        public static Vector operator +(Vector v, Vector w)
        {
            return new Vector(v.X + w.X, v.Y + w.Y);
        }

        public static double operator *(Vector v, Vector w)
        {
            return v.X * w.X + v.Y * w.Y;
        }

        public static Vector operator *(Vector v, double mult)
        {
            return new Vector(v.X * mult, v.Y * mult);
        }

        public static Vector operator *(double mult, Vector v)
        {
            return new Vector(v.X * mult, v.Y * mult);
        }

        public double Cross(Vector v)
        {
            return X * v.Y - Y * v.X;
        }

        public override bool Equals(object obj)
        {
            var v = (Vector)obj;
            return (X - v.X).IsZero() && (Y - v.Y).IsZero();
        }
        public static bool LineSegementsIntersect(Vector p, Vector p2, Vector q, Vector q2, out Vector intersection)
        {
            intersection = new Vector();

            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.Cross(s);
            var qpxr = (q - p).Cross(r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                
                    if ((0 <= (q - p) * r && (q - p) * r <= r * r) || (0 <= (p - q) * s && (p - q) * s <= s * s))
                        return true;

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return false;
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero() && !qpxr.IsZero())
                return false;

            // t = (q - p) x s / (r x s)
            var t = (q - p).Cross(s) / rxs;

            // u = (q - p) x r / (r x s)

            var u = (q - p).Cross(r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                intersection = p + t * r;

                // An intersection was found.
                return true;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }
    }
}
