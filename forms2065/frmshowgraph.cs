using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Skydat.classes2065;
using System.IO;
using System.Drawing.Printing;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace Skydat.forms2065
{
    public partial class frmshowgraph : Form
    {
        string strlegend,titles;
        double xminb, xmaxb, yminb, ymaxb;
        public frmshowgraph(string strlegend1,double xmin1,double xmax1,double ymin1,double ymax1,string titlegraph1,string titles1)
        {
           
        
            InitializeComponent();           
            strlegend = strlegend1;
            xminb = xmin1;
            yminb = ymin1;
            xmaxb = xmax1;
            ymaxb = ymax1;
            titles = titles1.Trim();
            Chart1.ChartAreas[0].BackColor = Color.White;
            Chart1.ChartAreas[0].Area3DStyle.Enable3D = false;

            Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            Chart1.ChartAreas[0].AxisX.LineColor = Color.Black;
            Chart1.ChartAreas[0].AxisY.LineColor = Color.Black;
             
            Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            if (titlegraph1.Trim() != "")
            {
                if(titlegraph1.Substring(0,1)=="V")
                    Chart1.ChartAreas[0].AxisY.Title = "Potential(mV)";
                else if(titlegraph1.Substring(0,1)=="I")
                    Chart1.ChartAreas[0].AxisY.Title = "Current(UA)";
                else if (titlegraph1.Substring(0, 1) == "Q")
                    Chart1.ChartAreas[0].AxisY.Title = "Q(UC)";
                if (titlegraph1.Substring(1, 1) == "V")
                    Chart1.ChartAreas[0].AxisX.Title = "Potential(mV)";
                else if (titlegraph1.Substring(1, 1) == "I")
                    Chart1.ChartAreas[0].AxisX.Title = "Time(S)"; 
            
         
            }
            if (titles.Trim() != "") {
                this.Chart1.Titles.Add("header");
                this.Chart1.Titles[0].Text =titles;
                this.Chart1.Titles[0].Alignment = ContentAlignment.TopCenter;
                this.Chart1.Titles[0].ForeColor = Color.Black;
            }
           // Chart1.ChartAreas[0].AxisX.Title = "";
            Chart1.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            Chart1.ChartAreas[0].AxisY.TitleForeColor = Color.Black;

            Chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#0.#e-0";
            Chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
            this.Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.Chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            this.Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            this.Chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            this.Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
            this.Chart1.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Black;
            this.Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Black;
            this.Chart1.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.Black;
            Chart1.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.Black;
            Chart1.ChartAreas[0].AxisY.ScrollBar.BackColor = Color.Black;

            Chart1.Series[0].Color = Color.SpringGreen;
           
        }

        private void frmshowgraph_Load(object sender, EventArgs e)
        {
            try
            {
                if(strlegend.Trim()!="")
                Chart1.Series[0].LegendText =strlegend.Trim();   

               
                Chart1.Legends[0].LegendStyle = LegendStyle.Table;
                Chart1.Legends[0].Docking = Docking.Left;
                Chart1.Legends[0].IsDockedInsideChartArea = false;
                Chart1.Legends[0].BorderColor = Color.Black;
                Chart1.Legends[0].BorderWidth = 1;
                Chart1.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
                Chart1.Printing.PrintDocument.PrinterSettings.DefaultPageSettings.Landscape = true;
                PaperSize paperSize = new PaperSize();
                paperSize.RawKind = (int)PaperKind.A4;
                Chart1.Printing.PrintDocument.DefaultPageSettings.PaperSize = paperSize;               
                Chart1.Printing.PrintDocument.PrinterSettings.DefaultPageSettings.PaperSize = paperSize;
               // Chart1.Printing.PrintPreview();
               
               
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex,1,1); }
        }

        private void Chart1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Chart1.Printing.Print(true);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;           

            sfd.DefaultExt = "bmp";
            sfd.Filter = "Bitmap Files(*.bmp)|*.bmp|JPeg Files(*.jpg)|*.jpg|Gif Files(*.gif)|*.gif|Png Files(*.png)|*.png|tiff Files(*.tiff)|*.tiff";
            sfd.ShowDialog();
            bool st = false;
            if (sfd.FileName != "")
                switch (sfd.FilterIndex)
                {
                    case 1:
                        Chart1.SaveImage(sfd.FileName, ChartImageFormat.Bmp);
                        st = true;
                        break;
                    case 2:
                        Chart1.SaveImage(sfd.FileName, ChartImageFormat.Jpeg);
                        st = true;
                        break;
                    case 3:
                        Chart1.SaveImage(sfd.FileName, ChartImageFormat.Gif);
                        st = true;
                        break;
                    case 4:
                        Chart1.SaveImage(sfd.FileName, ChartImageFormat.Png);
                        st = true;
                        break;
                    case 5:
                        Chart1.SaveImage(sfd.FileName, ChartImageFormat.Tiff);
                        st = true;
                        break;
                    default:
                        MessageBox.Show("Format not correct !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        st = false;
                        break;
                }
            if(st)
                MessageBox.Show("graph is successfully exported", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;

            sfd.DefaultExt = "pdf";
            sfd.Filter = "Acrobat reader Files(*.pdf)|*.pdf";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                var doc = new Document(iTextSharp.text.PageSize.A4.Rotate());

                PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                doc.Open();

               

                using (var chartimage = new MemoryStream())
                {
                    Chart1.SaveImage(chartimage, ChartImageFormat.Png);

                    byte[] cccc = chartimage.GetBuffer();
                    var image11 = iTextSharp.text.Image.GetInstance(cccc);
                    image11.ScalePercent(75f);
                    doc.Add(image11);
                    doc.Close();
                }
                MessageBox.Show("graph is successfully exported", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;        

            sfd.DefaultExt = "xml";
            sfd.Filter = "XML Files(*.xml)|*.xml";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                Chart1.Serializer.Save(sfd.FileName);
                MessageBox.Show("graph is successfully exported", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void changeDimensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmchangedimensions frm = new frmchangedimensions(xminb,xmaxb,yminb,ymaxb);
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() == DialogResult.OK) {
                Chart1.ChartAreas[0].AxisX.Minimum = frm.xmin;
                Chart1.ChartAreas[0].AxisY.Minimum = frm.ymin;
                Chart1.ChartAreas[0].AxisX.Maximum = frm.xmax;
                Chart1.ChartAreas[0].AxisY.Maximum = frm.ymax;

            
            
            }
        }

       
      
    }
}
