using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;
using Accord.Math;
using Accord.Math.Transforms;


namespace Skydat.eis_form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(36,36,35);
            
            double[] mag1;
            double[] xi1 = new double[10];
            double[] xi2 = new double[10];
            double[] xr1 = new double[10];
            double[] xr2 = new double[10];
            double[] angle1 = new double[10];
            double[] data = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0,9,10 };
            double[] data2 = { 10, 9,8,7,6,5,4,3,2,1};
            int sampleRate = 10000;
            double[] buffer_voltage = new double[10];
            double[] buffer_current = new double[10];
            double amplitude = 0.25;
            double frequency = 1000;
            for(int j = 0; j < buffer_voltage.Length; j++)
            {

                buffer_voltage[j] = (float)(amplitude * Math.Sin((2 * Math.PI * j * frequency / sampleRate + Math.PI / 3)));
                buffer_current[j] = (float)(amplitude * Math.Sin((2 * Math.PI * j * frequency / sampleRate )));
            }
            calculateDFT(buffer_voltage, xr1,xi1);
            calculateDFT(buffer_current, xr2,xi2);
            calculate_angle_beetween(angle1, xr1, xi1,xr2,xi2);


            Console.WriteLine("Original data:");
            Console.WriteLine("\nFFT result:");
            
            
           
        }
        void calculate_angle_beetween(double [] angle,double []xr,double[] xi,double [] xr2,double [] xi2)
        {
            double[] divide1 = new double[10];
            double[] divide2 = new double[10];
            for (int i = 0; i < 10; i++) {
                angle[i]=Math.Acos(((xr[i] * xr2[i]) + (xi[i] * xi2[i])) / ((Math.Abs(Math.Sqrt(Math.Pow(xr[i], 2) + Math.Pow(xi[i], 2)))) * (Math.Abs(Math.Sqrt(Math.Pow(xr2[i], 2) + Math.Pow(xi2[i], 2))))))*(180 / Math.PI);
                //divide1[i] = (xr[i] * xr2[i] + xi[i] * xi2[i]) / (Math.Pow(xr2[i], 2) + Math.Pow(xi2[i], 2));
                //divide2[i] = (xi[i] * xr2[i] - xr[i] * xi2[i]) / (Math.Pow(xr2[i], 2) + Math.Pow(xi2[i], 2));
                //angle[i] = Math.Atan(divide2[i]/divide1[i])*(180/Math.PI);
                //angle[i]= Math.Atan2(divide1[i], divide2[i]);
            }

        }
        void magnitude_complex(double [] xr,double []xi,double []mag)
        {

            for (int i = 0; i < 10; i++)
            {
                mag[i] = Math.Sqrt(Math.Pow(xr[i], 2) + Math.Pow(xi[i], 2));


            }
        }

        void calculateDFT(double []xn, double[] xr, double[] xi)
        {
            //int xn[len];
            int i, k, n, N = 0;            
            for (k = 0; k < 10; k++)
            {
                xr[k] = 0;
                xi[k] = 0;
                for (n = 0; n < 10; n++)
                {
                    xr[k] = (xr[k]+ xn[n] * Math.Cos(2 * 3.141592 * k * n / 10));
                    xi[k] = (xi[k]- xn[n] * Math.Sin(2 * 3.141592 * k * n / 10));
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
