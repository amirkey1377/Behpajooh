using System.Windows.Forms.DataVisualization.Charting;

namespace Skydat
{
    public interface ITechniques
    {
        byte Number { get; }

        string Name { get; }

        string FullName { get; }

        string Discription { get; }

        string Comment { get; set; }

        int ColorCode { get; set; }

        int EquilibriumTime { get; set; }

        Series previewChart();

        string SeriesPath { get; set; }

        SeriesChartType SeriesType { get; set; }

        string[] XY_Axis_Title { get; }

        int[] SendToUsb(ref int TotalByteExpectedToRead, ref byte[] buffer);

        void SendToUsb_For_2066(ref string CommondRun);


        ITechniques Clone();
    }
}
