using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Skydat
{
    public class FindPeakManual
    {
        public int Resolution = 0;
        public int[] HeightIndices = new int[0];
        public int[] WidthIndices = new int[0];
        public int[] AreaIndices = new int[0];

        public bool? PreferMaxOverride = null;

        private void AddIndex(ref int[] arr, int val)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = val;
        }

        public bool Draw(Chart chart,
                         double[] volt, double[] current,
                         int startSeriesIndex, int userSeriesIndex,
                         double startX, double startY, double endX, double endY,
                         double E1 = 9999, double E2 = 9999)
        {
            HeightIndices = new int[0];
            WidthIndices = new int[0];
            AreaIndices = new int[0];

            if (chart == null || volt == null || current == null) return false;
            if (volt.Length != current.Length || volt.Length < 3) return false;

            int n = volt.Length;
            double[] currentSmooth = MovingAverage(current, 7);

            int forwardCount = n;
            for (int i = 1; i < n; i++)
                if (volt[i] < volt[i - 1]) { forwardCount = i - 1; break; }
            bool isCv = (forwardCount < n);

            bool preferMax = true;
            if (!isCv)
            {
                if (PreferMaxOverride.HasValue)
                    preferMax = PreferMaxOverride.Value;
                else if (E1 != 9999 && E2 != 9999 && E1 != E2)
                    preferMax = (E1 < E2);
                else
                    preferMax = (volt[0] < volt[n - 1]);
            }

            if (startX > endX)
            {
                double tempX = startX; startX = endX; endX = tempX;
                double tempY = startY; startY = endY; endY = tempY;
            }

            double userSlope = (endX - startX) != 0 ? (endY - startY) / (endX - startX) : 0;
            double userIntercept = startY - userSlope * startX;

            double minVolt = volt[0], maxVolt = volt[0];
            for (int i = 1; i < n; i++)
            {
                if (volt[i] < minVolt) minVolt = volt[i];
                if (volt[i] > maxVolt) maxVolt = volt[i];
            }

            if (userSeriesIndex >= 0 && userSeriesIndex < chart.Series.Count)
            {
                chart.Series[userSeriesIndex].Points.Clear();
                chart.Series[userSeriesIndex].ChartType = SeriesChartType.Line;
                chart.Series[userSeriesIndex].Color = Color.Orange;
                chart.Series[userSeriesIndex].BorderWidth = 2;
                chart.Series[userSeriesIndex].Points.AddXY(minVolt, userSlope * minVolt + userIntercept);
                chart.Series[userSeriesIndex].Points.AddXY(maxVolt, userSlope * maxVolt + userIntercept);
            }

            int ser = startSeriesIndex;

            if (!isCv)
            {
                int mainPeakIdx = -1;
                if (preferMax)
                {
                    double maxVal = double.MinValue;
                    for (int i = 0; i < n; i++)
                        if (current[i] > maxVal) { maxVal = current[i]; mainPeakIdx = i; }
                }
                else
                {
                    double minVal = double.MaxValue;
                    for (int i = 0; i < n; i++)
                        if (current[i] < minVal) { minVal = current[i]; mainPeakIdx = i; }
                }
                if (mainPeakIdx == -1) return false;

                DrawOneNewRule(chart, volt, current, ref ser, mainPeakIdx,
                               userSlope, userIntercept, startX, endX,
                               preferMax, "Aria: ");
                return true;
            }

            DrawOne(chart, volt, currentSmooth, ref ser, 0, forwardCount - 1,
                    userSlope, minVolt, userSlope * minVolt + userIntercept, true, "Aria Forward: ");
            DrawOne(chart, volt, currentSmooth, ref ser, forwardCount, n - 1,
                    userSlope, minVolt, userSlope * minVolt + userIntercept, false, "Aria Backward: ");

            return true;
        }

        private void DrawOneNewRule(Chart chart, double[] volt, double[] current,
                             ref int ser, int peakIdx,
                             double userSlope, double userIntercept,
                             double startX, double endX,
                             bool isMax, string areaLabel)
        {
            double baseAtPeak = userSlope * volt[peakIdx] + userIntercept;
            double height = Math.Abs(current[peakIdx] - baseAtPeak);
            if (height <= 0) return;

            double targetHeight = height / 2;

            // خط ارتفاع (زرد)
            chart.Series.Add(new Series());
            chart.Series[ser].ChartType = SeriesChartType.Line;
            chart.Series[ser].Color = Color.Gold;
            chart.Series[ser].BorderWidth = 1;
            chart.Series[ser].Points.AddXY(volt[peakIdx], current[peakIdx]);
            chart.Series[ser].Points.AddXY(volt[peakIdx], baseAtPeak);
            chart.Series[ser].Points[0].Label = "H: " + height.ToString("F3");
            AddIndex(ref HeightIndices, ser);
            ser++;

            // مساحت
            double area = 0;
            for (int k = 1; k < volt.Length; k++)
            {
                if (volt[k] >= startX && volt[k] <= endX)
                {
                    double b1 = userSlope * volt[k - 1] + userIntercept;
                    double b2 = userSlope * volt[k] + userIntercept;
                    double f1, f2;
                    if (isMax)
                    {
                        f1 = current[k - 1] - b1; if (f1 < 0) f1 = 0;
                        f2 = current[k] - b2; if (f2 < 0) f2 = 0;
                    }
                    else
                    {
                        f1 = b1 - current[k - 1]; if (f1 < 0) f1 = 0;
                        f2 = b2 - current[k]; if (f2 < 0) f2 = 0;
                    }
                    area += Math.Abs(volt[k] - volt[k - 1]) * (f1 + f2) / 2;
                }
            }

            chart.Series.Add(new Series());
            chart.Series[ser].ChartType = SeriesChartType.Spline;
            chart.Series[ser].Color = Color.Purple;
            chart.Series[ser].BorderWidth = 1;
            chart.Series[ser].Points.AddXY(volt[peakIdx], current[peakIdx]);
            chart.Series[ser].Points[0].Label = areaLabel + area.ToString("F5");
            AddIndex(ref AreaIndices, ser);
            ser++;

            // ═══════════════════════════════════════════════════════════
            // ★★★ FWHM با خط پایه شیب‌دار محلی + درون‌یابی خطی ★★★
            // ═══════════════════════════════════════════════════════════
            double p1 = volt[peakIdx];
            double p2 = volt[peakIdx];
            bool foundL = false, foundR = false;

            // جستجوی چپ با interpolation
            for (int k = peakIdx - 1; k >= 0; k--)
            {
                if (volt[k] < startX) break;
                double lb_prev = userSlope * volt[k + 1] + userIntercept;
                double lb_curr = userSlope * volt[k] + userIntercept;
                double hh_prev = isMax ? lb_prev + targetHeight : lb_prev - targetHeight;
                double hh_curr = isMax ? lb_curr + targetHeight : lb_curr - targetHeight;
                double d_prev = current[k + 1] - hh_prev;
                double d_curr = current[k] - hh_curr;

                if ((d_prev >= 0 && d_curr <= 0) || (d_prev <= 0 && d_curr >= 0))
                {
                    double xa = volt[k + 1], xb = volt[k];
                    double t = Math.Abs(d_prev - d_curr) > 1e-12 ? d_prev / (d_prev - d_curr) : 0.5;
                    p1 = xa + t * (xb - xa);
                    foundL = true;
                    break;
                }
            }

            // جستجوی راست با interpolation
            for (int k = peakIdx + 1; k < volt.Length; k++)
            {
                if (volt[k] > endX) break;
                double lb_prev = userSlope * volt[k - 1] + userIntercept;
                double lb_curr = userSlope * volt[k] + userIntercept;
                double hh_prev = isMax ? lb_prev + targetHeight : lb_prev - targetHeight;
                double hh_curr = isMax ? lb_curr + targetHeight : lb_curr - targetHeight;
                double d_prev = current[k - 1] - hh_prev;
                double d_curr = current[k] - hh_curr;

                if ((d_prev >= 0 && d_curr <= 0) || (d_prev <= 0 && d_curr >= 0))
                {
                    double xa = volt[k - 1], xb = volt[k];
                    double t = Math.Abs(d_prev - d_curr) > 1e-12 ? d_prev / (d_prev - d_curr) : 0.5;
                    p2 = xa + t * (xb - xa);
                    foundR = true;
                    break;
                }
            }

            if (foundL && foundR)
            {
                double width = Math.Abs(p2 - p1);
                double halfHAtPeak = isMax ? baseAtPeak + targetHeight : baseAtPeak - targetHeight;

                chart.Series.Add(new Series());
                chart.Series[ser].ChartType = SeriesChartType.Line;
                chart.Series[ser].Color = Color.SpringGreen;
                chart.Series[ser].BorderWidth = 2;
                chart.Series[ser].Points.AddXY(p1, halfHAtPeak);
                chart.Series[ser].Points.AddXY(p2, halfHAtPeak);
                chart.Series[ser].Points[0].Label = "W: " + width.ToString("F4");
                AddIndex(ref WidthIndices, ser);
                ser++;
            }
        }

        // ══════════ متدهای CV (دست‌نخورده) ══════════
        private void DrawOne(Chart chart, double[] volt, double[] current, ref int ser,
                             int fromIdx, int toIdx, double slope, double x0, double y0,
                             bool isForward, string areaLabel)
        {
            if (toIdx <= fromIdx) return;
            int len = toIdx - fromIdx + 1;
            double[] devArr = new double[len];
            for (int i = 0; i < len; i++)
            {
                int idx = fromIdx + i;
                double baseAtI = (slope * volt[idx]) - (slope * x0) + y0;
                devArr[i] = isForward ? (current[idx] - baseAtI) : (baseAtI - current[idx]);
            }
            int res = Resolution > 0 ? Resolution : Math.Max(1, len / 80);
            if (res < 1) res = 1;
            if (res > len / 4) res = Math.Max(1, len / 10);
            int startSearch = Math.Max(2, len / 50);
            int endSearch = len;
            List<int> candidates = new List<int>();
            for (int i = startSearch; i < endSearch; i += res)
            {
                int bestIdx = i;
                double bestVal = devArr[i];
                int endW = Math.Min(i + res, endSearch);
                for (int k = i; k < endW; k++)
                    if (devArr[k] > bestVal) { bestVal = devArr[k]; bestIdx = k; }
                candidates.Add(bestIdx);
            }
            if (candidates.Count == 0) return;
            candidates.Sort((a, b) => devArr[b].CompareTo(devArr[a]));
            int peakLocalIdx = candidates[0];
            double maxDev = devArr[peakLocalIdx];
            for (int i = startSearch; i < endSearch; i++)
            {
                bool l1 = (i < 1) || (devArr[i] >= devArr[i - 1]);
                bool l2 = (i < 2) || (devArr[i] >= devArr[i - 2]);
                bool r1 = (i >= len - 1) || (devArr[i] >= devArr[i + 1]);
                bool r2 = (i >= len - 2) || (devArr[i] >= devArr[i + 2]);
                if (l1 && l2 && r1 && r2 && devArr[i] > maxDev) { maxDev = devArr[i]; peakLocalIdx = i; }
            }
            if (maxDev <= 0)
                for (int i = 0; i < len; i++) if (devArr[i] > maxDev) { maxDev = devArr[i]; peakLocalIdx = i; }

            double threshold = maxDev * 0.20;
            int peakStartLocal = peakLocalIdx;
            for (int i = peakLocalIdx - 1; i >= 0; i--)
            {
                if (devArr[i] < threshold) { peakStartLocal = i; break; }
                peakStartLocal = i;
            }
            int peakEndLocal = peakLocalIdx;
            for (int i = peakLocalIdx + 1; i < len; i++)
            {
                if (devArr[i] < threshold) { peakEndLocal = i; break; }
                peakEndLocal = i;
            }
            int peakIdx = fromIdx + peakLocalIdx;
            double basePeak = (slope * volt[peakIdx]) - (slope * x0) + y0;
            double height = Math.Abs(current[peakIdx] - basePeak);
            if (height <= 0) return;

            chart.Series.Add(new Series());
            chart.Series[ser].ChartType = SeriesChartType.Line;
            chart.Series[ser].Color = Color.Gold;
            chart.Series[ser].BorderWidth = 1;
            chart.Series[ser].Points.AddXY(volt[peakIdx], current[peakIdx]);
            chart.Series[ser].Points.AddXY(volt[peakIdx], basePeak);
            chart.Series[ser].Points[0].Label = "H: " + height.ToString("F3");
            AddIndex(ref HeightIndices, ser);
            ser++;

            double area = 0;
            int areaFrom = fromIdx + peakStartLocal;
            int areaTo = fromIdx + peakEndLocal;
            if (isForward)
                for (int k = areaFrom + 1; k <= areaTo; k++)
                {
                    double b1 = (slope * volt[k - 1]) - (slope * x0) + y0;
                    double b2 = (slope * volt[k]) - (slope * x0) + y0;
                    double f1 = current[k - 1] - b1; if (f1 < 0) f1 = 0;
                    double f2 = current[k] - b2; if (f2 < 0) f2 = 0;
                    area += (volt[k] - volt[k - 1]) * (f1 + f2) / 2;
                }
            else
                for (int k = areaTo; k > areaFrom; k--)
                {
                    double b1 = (slope * volt[k]) - (slope * x0) + y0;
                    double b2 = (slope * volt[k - 1]) - (slope * x0) + y0;
                    double f1 = b1 - current[k]; if (f1 < 0) f1 = 0;
                    double f2 = b2 - current[k - 1]; if (f2 < 0) f2 = 0;
                    area += Math.Abs(volt[k] - volt[k - 1]) * (f1 + f2) / 2;
                }

            chart.Series.Add(new Series());
            chart.Series[ser].ChartType = SeriesChartType.Spline;
            chart.Series[ser].Color = Color.Red;
            chart.Series[ser].BorderWidth = 1;
            chart.Series[ser].Points.AddXY(volt[peakIdx], current[peakIdx]);
            chart.Series[ser].Points[0].Label = areaLabel + area.ToString("F5");
            AddIndex(ref AreaIndices, ser);
            ser++;

            double halfH = isForward ? basePeak + (height / 2) : basePeak - (height / 2);
            double p1, p2;
            bool foundL = FindLeft(volt, current, peakIdx, areaFrom, halfH, isForward, out p1);
            bool foundR = FindRight(volt, current, peakIdx, areaTo, halfH, isForward, out p2);
            if (!foundL || !foundR) return;
            double width = Math.Abs(p2 - p1);
            chart.Series.Add(new Series());
            chart.Series[ser].ChartType = SeriesChartType.Line;
            chart.Series[ser].Color = Color.SpringGreen;
            chart.Series[ser].BorderWidth = 2;
            chart.Series[ser].Points.AddXY(p1, halfH);
            chart.Series[ser].Points.AddXY(p2, halfH);
            chart.Series[ser].Points[0].Label = "W: " + width.ToString("F4");
            AddIndex(ref WidthIndices, ser);
            ser++;
        }

        private bool FindLeft(double[] volt, double[] current, int peak, int limit, double halfH, bool isForward, out double leftX)
        {
            leftX = volt[limit];
            for (int k = peak; k > limit; k--)
            {
                bool reached = isForward ? (current[k] <= halfH) : (current[k] >= halfH);
                if (reached)
                {
                    bool crossed = isForward ? (current[k] < halfH) : (current[k] > halfH);
                    if (crossed)
                    {
                        double xa = volt[k], ya = current[k];
                        double xb = volt[k + 1], yb = current[k + 1];
                        double s = (yb - ya) / (xb - xa);
                        leftX = ((s * xa) + (halfH - ya)) / s;
                    }
                    else leftX = volt[k];
                    return true;
                }
            }
            return false;
        }

        private bool FindRight(double[] volt, double[] current, int peak, int limit, double halfH, bool isForward, out double rightX)
        {
            rightX = volt[limit];
            for (int k = peak; k < limit; k++)
            {
                bool reached = isForward ? (current[k] <= halfH) : (current[k] >= halfH);
                if (reached)
                {
                    bool crossed = isForward ? (current[k] < halfH) : (current[k] > halfH);
                    if (crossed)
                    {
                        double xa = volt[k], ya = current[k];
                        double xb = volt[k - 1], yb = current[k - 1];
                        double s = (yb - ya) / (xb - xa);
                        rightX = ((s * xa) + (halfH - ya)) / s;
                    }
                    else rightX = volt[k];
                    return true;
                }
            }
            return false;
        }

        private double[] MovingAverage(double[] data, int window)
        {
            if (window < 3) return (double[])data.Clone();
            if (window % 2 == 0) window++;
            int half = window / 2;
            int n = data.Length;
            double[] r = new double[n];
            for (int i = 0; i < n; i++)
            {
                double s = 0; int c = 0;
                for (int j = i - half; j <= i + half; j++)
                    if (j >= 0 && j < n) { s += data[j]; c++; }
                r[i] = s / c;
            }
            return r;
        }
    }
}