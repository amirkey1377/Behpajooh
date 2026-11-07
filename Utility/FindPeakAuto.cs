using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Skydat
{
    public class FindPeakAuto
    {
        public double ProminenceRatio = 0.10;
        public double MinProminence = 0;
        public int Resolution = 0;
        public int SmoothWindow = 5;
        public int MaxWindow = 200;

        public Color ColorBaseline = Color.Red;
        public Color ColorHeight = Color.Gold;
        public Color ColorWidth = Color.SpringGreen;
        public Color ColorArea = Color.Purple;

        public int[] HeightIndices = new int[0];
        public int[] WidthIndices = new int[0];
        public int[] AreaIndices = new int[0];

        public bool? PreferMaxOverride = null;

        public bool Draw(Chart chart, double[] volt, double[] current)
        {
            HeightIndices = new int[0];
            WidthIndices = new int[0];
            AreaIndices = new int[0];

            if (chart == null || volt == null || current == null) return false;
            if (volt.Length != current.Length || volt.Length < 30) return false;

            int n = volt.Length;

            int vertex = 0;
            double maxV = volt[0];
            for (int i = 1; i < n; i++)
                if (volt[i] > maxV) { maxV = volt[i]; vertex = i; }

            bool isCv = (vertex > n * 0.15 && vertex < n * 0.85);

            if (isCv)
            {
                ProcessRegionCv(chart, volt, current, 0, vertex, true);
                ProcessRegionCv(chart, volt, current, vertex, n - 1, false);
            }
            else
            {
                bool preferMax;
                if (PreferMaxOverride.HasValue)
                    preferMax = PreferMaxOverride.Value;
                else
                    preferMax = (volt[0] < volt[n - 1]);

                ProcessRegionNonCv(chart, volt, current, 0, n - 1, true, preferMax);
            }

            return true;
        }

        private void ProcessRegionCv(Chart chart, double[] volt, double[] current,
                                      int from, int to, bool isForward)
        {
            int len = to - from + 1;
            if (len < 30) return;

            int margin = Math.Max(5, len / 20);
            int validFrom = from + margin;
            int validTo = to - margin;

            if (validTo - validFrom < 15) return;

            int maxIdx = validFrom, minIdx = validFrom;
            for (int i = validFrom + 1; i <= validTo; i++)
            {
                if (current[i] > current[maxIdx]) maxIdx = i;
                if (current[i] < current[minIdx]) minIdx = i;
            }

            bool validMax = (maxIdx > validFrom + 5 && maxIdx < validTo - 5);
            bool validMin = (minIdx > validFrom + 5 && minIdx < validTo - 5);

            if (!validMax && !validMin) return;

            double heightMax = 0, heightMin = 0;
            int lMax = 0, rMax = 0, lMin = 0, rMin = 0;
            double baseMax = 0, baseMin = 0;

            if (validMax)
            {
                GetFeet(current, maxIdx, true, validFrom, validTo, out lMax, out rMax, true);
                baseMax = GetBaselineValue(volt, current, maxIdx, lMax, rMax);
                heightMax = Math.Abs(current[maxIdx] - baseMax);
            }

            if (validMin)
            {
                GetFeet(current, minIdx, false, validFrom, validTo, out lMin, out rMin, true);
                baseMin = GetBaselineValue(volt, current, minIdx, lMin, rMin);
                heightMin = Math.Abs(current[minIdx] - baseMin);
            }

            if (validMax && validMin)
            {
                if (heightMax >= heightMin && heightMax > 0)
                    DrawPeak(chart, volt, current, maxIdx, lMax, rMax, baseMax, isForward);
                else if (heightMin > 0)
                    DrawPeak(chart, volt, current, minIdx, lMin, rMin, baseMin, isForward);
            }
            else if (validMax && heightMax > 0)
            {
                DrawPeak(chart, volt, current, maxIdx, lMax, rMax, baseMax, isForward);
            }
            else if (validMin && heightMin > 0)
            {
                DrawPeak(chart, volt, current, minIdx, lMin, rMin, baseMin, isForward);
            }
        }

        private void ProcessRegionNonCv(Chart chart, double[] volt, double[] current,
                                         int from, int to, bool isForward, bool preferMax)
        {
            int len = to - from + 1;
            if (len < 30) return;

            int margin = 2;
            int validFrom = from + margin;
            int validTo = to - margin;

            if (validTo - validFrom < 15) return;

            int maxIdx = validFrom, minIdx = validFrom;
            for (int i = validFrom + 1; i <= validTo; i++)
            {
                if (current[i] > current[maxIdx]) maxIdx = i;
                if (current[i] < current[minIdx]) minIdx = i;
            }

            double heightMax = 0, heightMin = 0;
            int lMax = 0, rMax = 0, lMin = 0, rMin = 0;
            double baseMax = 0, baseMin = 0;

            GetFeet(current, maxIdx, true, validFrom, validTo, out lMax, out rMax, false);
            baseMax = GetBaselineValue(volt, current, maxIdx, lMax, rMax);
            heightMax = Math.Abs(current[maxIdx] - baseMax);

            GetFeet(current, minIdx, false, validFrom, validTo, out lMin, out rMin, false);
            baseMin = GetBaselineValue(volt, current, minIdx, lMin, rMin);
            heightMin = Math.Abs(current[minIdx] - baseMin);

            if (preferMax)
            {
                if (heightMax > 0)
                    DrawPeak(chart, volt, current, maxIdx, lMax, rMax, baseMax, isForward);
            }
            else
            {
                if (heightMin > 0)
                    DrawPeak(chart, volt, current, minIdx, lMin, rMin, baseMin, isForward);
            }
        }

        private double GetBaselineValue(double[] volt, double[] current, int peakIdx, int leftFoot, int rightFoot)
        {
            if (rightFoot <= leftFoot) return current[peakIdx];
            double xA = volt[leftFoot], yA = current[leftFoot];
            double xB = volt[rightFoot], yB = current[rightFoot];
            double slope = (xB - xA) != 0 ? (yB - yA) / (xB - xA) : 0;
            return yA + slope * (volt[peakIdx] - xA);
        }

        private void DrawPeak(Chart chart, double[] volt, double[] current,
                              int peakIdx, int leftFoot, int rightFoot, double baseAtPeak,
                              bool isForward)
        {
            double peakVal = current[peakIdx];
            double height = Math.Abs(peakVal - baseAtPeak);
            if (height <= 0) return;

            bool isMax = peakVal > baseAtPeak;
            double halfH = isMax ? baseAtPeak + height / 2 : baseAtPeak - height / 2;

            // ═══════════════════════════════════════════════════════════
            // ★★★ جستجوی خط نصف ارتفاع با درون‌یابی خطی ★★★
            // ═══════════════════════════════════════════════════════════
            double p1 = volt[peakIdx];
            double p2 = volt[peakIdx];
            bool foundL = false, foundR = false;

            // جستجوی چپ با interpolation
            for (int k = peakIdx - 1; k >= leftFoot; k--)
            {
                bool reached = isMax ? (current[k] <= halfH) : (current[k] >= halfH);
                if (reached)
                {
                    double xa = volt[k + 1], ya = current[k + 1];
                    double xb = volt[k], yb = current[k];
                    if (Math.Abs(yb - ya) > 1e-12)
                    {
                        double t = (halfH - ya) / (yb - ya);
                        p1 = xa + t * (xb - xa);
                    }
                    else p1 = xb;
                    foundL = true;
                    break;
                }
            }

            // جستجوی راست با interpolation
            for (int k = peakIdx + 1; k <= rightFoot; k++)
            {
                bool reached = isMax ? (current[k] <= halfH) : (current[k] >= halfH);
                if (reached)
                {
                    double xa = volt[k - 1], ya = current[k - 1];
                    double xb = volt[k], yb = current[k];
                    if (Math.Abs(yb - ya) > 1e-12)
                    {
                        double t = (halfH - ya) / (yb - ya);
                        p2 = xa + t * (xb - xa);
                    }
                    else p2 = xb;
                    foundR = true;
                    break;
                }
            }

            double width = (foundL && foundR) ? Math.Abs(p2 - p1) : 0;

            double xA = volt[leftFoot], yA = current[leftFoot];
            double xB = volt[rightFoot], yB = current[rightFoot];
            double slope = (xB - xA) != 0 ? (yB - yA) / (xB - xA) : 0;
            double area = 0;
            for (int k = leftFoot + 1; k <= rightFoot; k++)
            {
                double b1 = yA + slope * (volt[k - 1] - xA);
                double b2 = yA + slope * (volt[k] - xA);
                double f1 = isMax ? (current[k - 1] - b1) : (b1 - current[k - 1]); if (f1 < 0) f1 = 0;
                double f2 = isMax ? (current[k] - b2) : (b2 - current[k]); if (f2 < 0) f2 = 0;
                area += Math.Abs(volt[k] - volt[k - 1]) * (f1 + f2) / 2;
            }

            int idx = AddSeries(chart, ColorBaseline, 2);
            chart.Series[idx].Points.AddXY(xA, yA);
            chart.Series[idx].Points.AddXY(xB, yB);

            idx = AddSeries(chart, ColorHeight, 3);
            chart.Series[idx].Points.AddXY(volt[peakIdx], current[peakIdx]);
            chart.Series[idx].Points.AddXY(volt[peakIdx], baseAtPeak);
            chart.Series[idx].Points[0].Label = "H: " + height.ToString("F1");
            Array.Resize(ref HeightIndices, HeightIndices.Length + 1);
            HeightIndices[HeightIndices.Length - 1] = idx;

            if (width > 0 && foundL && foundR)
            {
                idx = AddSeries(chart, ColorWidth, 3);
                chart.Series[idx].Points.AddXY(p1, halfH);
                chart.Series[idx].Points.AddXY(p2, halfH);
                chart.Series[idx].Points[0].Label = "W: " + width.ToString("F1");
                Array.Resize(ref WidthIndices, WidthIndices.Length + 1);
                WidthIndices[WidthIndices.Length - 1] = idx;
            }

            idx = AddSeries(chart, ColorArea, 1);
            chart.Series[idx].Points.AddXY(volt[peakIdx], current[peakIdx]);
            chart.Series[idx].Points[0].Label = (isForward ? "Aria Fwd: " : "Aria Bwd: ") + area.ToString("F1");
            Array.Resize(ref AreaIndices, AreaIndices.Length + 1);
            AreaIndices[AreaIndices.Length - 1] = idx;
        }

        private int AddSeries(Chart chart, Color color, int width)
        {
            chart.Series.Add(new Series());
            int idx = chart.Series.Count - 1;
            chart.Series[idx].ChartType = SeriesChartType.Line;
            chart.Series[idx].Color = color;
            chart.Series[idx].BorderWidth = width;
            return idx;
        }

        private void GetFeet(double[] current, int peakIdx, bool isMax, int from, int to,
                              out int leftFoot, out int rightFoot, bool isCv)
        {
            leftFoot = peakIdx;
            rightFoot = peakIdx;

            double peakVal = current[peakIdx];

            double rangeMax = current[from];
            double rangeMin = current[from];
            for (int i = from; i <= to; i++)
            {
                if (current[i] > rangeMax) rangeMax = current[i];
                if (current[i] < rangeMin) rangeMin = current[i];
            }
            double range = Math.Abs(rangeMax - rangeMin);

            double riseThreshold = isCv ? 0.15 * range : 0.03 * range;
            int maxSearch = isCv ? 300 : 150;

            if (isMax)
            {
                double lowestL = peakVal;
                int lowestIdxL = peakIdx;
                int limitL = Math.Max(from, peakIdx - maxSearch);
                for (int i = peakIdx - 1; i >= limitL; i--)
                {
                    if (current[i] < lowestL) { lowestL = current[i]; lowestIdxL = i; }
                    else if (current[i] - lowestL > riseThreshold) break;
                }
                leftFoot = lowestIdxL;

                double lowestR = peakVal;
                int lowestIdxR = peakIdx;
                int limitR = Math.Min(to, peakIdx + maxSearch);
                for (int i = peakIdx + 1; i <= limitR; i++)
                {
                    if (current[i] < lowestR) { lowestR = current[i]; lowestIdxR = i; }
                    else if (current[i] - lowestR > riseThreshold) break;
                }
                rightFoot = lowestIdxR;
            }
            else
            {
                double highestL = peakVal;
                int highestIdxL = peakIdx;
                int limitL = Math.Max(from, peakIdx - maxSearch);
                for (int i = peakIdx - 1; i >= limitL; i--)
                {
                    if (current[i] > highestL) { highestL = current[i]; highestIdxL = i; }
                    else if (highestL - current[i] > riseThreshold) break;
                }
                leftFoot = highestIdxL;

                double highestR = peakVal;
                int highestIdxR = peakIdx;
                int limitR = Math.Min(to, peakIdx + maxSearch);
                for (int i = peakIdx + 1; i <= limitR; i++)
                {
                    if (current[i] > highestR) { highestR = current[i]; highestIdxR = i; }
                    else if (highestR - current[i] > riseThreshold) break;
                }
                rightFoot = highestIdxR;
            }
        }
    }
}