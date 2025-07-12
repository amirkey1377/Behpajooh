using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Data;

namespace Skydat.classes2065
{
    public static class clasglobal
    {
        public static int Graph_Color
        {
            set; get;
        }

        //نام مخفف یا مستعار تکنیک ها درون آرایه قرار داده میشود و در قسمتهایی از برنامه با شماره اندیس فراخوانی می شود
        public static string[] TechName = { "DCV", "NPV", "DPV", "SWV",
                                            "CV", "LSV", "DCS","DPS",
                                            "CCC", "CPC","CP","CA",
                                            "CHC","","CD","OCP","RT","CDtest"};       
       
        //نام کامل تکنیکها
        public static string[] Tech_FullName = { "DC Voltammetry","Normal Pulse Voltammetry","Diff. Pulse Voltammetry","Square Wave Voltammetry",
                                                 "Cyclic Voltammetry","Liner Sweep Voltammetry","Stripping DC Voltammetry","Stripping Diff. Pulse Voltammetry",
                                                 "","Controlled Potential Coulometry","Chrono Ptential  Col","Chrono Amperometry",
                                                 "Chrono Coulometry","","Charge and Discharge","Open Circuit Potential","resistance trim"};

        //واحد های اندازه گیری هر تکنیک روی نمودار محور افقی 
        public static string[] HAxisTitle = { "Potential(V)", "Potential(V)", "Potential(V)", "Potential(V)",
                                              "Potential(V)","Potential(V)","Potential(V)","Potential(V)",
                                               "Time(S)", "Time(S)", "Time(S)", "Time(S)",
                                               "Time(S)" ,"","Time(S)","Time(S)","Time(S)"};

        //واحد های اندازه گیری هر تکنیک روی نمودار محور عمودی
        public static string[] VAxisTitle = {"Current(UA)", "Current(UA)", "Current(UA)", "Current(UA)",
                                             "Current(UA)", "Current(UA)", "Current(UA)", "Current(UA)",
                                             "Current(UA)","Current(UA)", "Potential(mV)", "Current(UA)",
                                             "Q(UC)" ,"","Potential(mV)","Potential(mV)","Current(UA)"};
        
        //مخفف دستگاه 2080
        public static string[] TechName2080 ={ "", "DCV", "NPV", "DPV",
                                                "SWV", "CV", "", "DCS", 
                                                "DPS", "OCP", "CPC", "CHA",
                                                "CCC", " SC", "LSV", "CHP", }; 

        //نام کامل 2080
        public static string[] Tech_FullName2080 = { "","DC Voltammetry","Normal Pulse Voltammetry","Diff. Pulse Voltammetry",
                                                     "Square Wave Voltammetry","Cyclic Voltammetry","","Stripping DC Voltammetry",
                                                     "Stripping Diff. Pulse Voltammetry","Open Circuit Potential","Controlled Potential Coulometry","Chrono Amperometry",
                                                     "","","Liner Sweep Voltammetry","Chrono Ptential  Col"};
      
        // واحد محور افقی 2080
        public static string[] HAxisTitle2080 = { "","Potential(V)", "Potential(V)", "Potential(V)",
                                                  "Potential(V)", "Potential(V)","","Potential(V)", 
                                                  "Potential(V)", "Time(S)", "Time(S)", "Time(S)",
                                                  "Time(S)", "Time(S)" , "Potential(V)","Time(S)"};
        
        //واحد محور عمودی 2080
        public static string[] VAxisTitle2080 = {"","Current(UA)", "Current(A)", "Current(A)", "Current(A)",
                                             "Current(A)", "Current(UA)","","Current(UA)",
                                             "Potential(V)","Q(C)","Current(A)","Current(UA)",
                                             "Current(UA)", "Current(A)","Current(UA)","Potential(mV)"};
        
        //آرایه ای از رنگ ها که به صورت تصادفی از بین اینها برای رنگ گرافها انتخاب می شود
        public static Color[] color_Graph = {
                                 Color.Blue,Color.Aqua,Color.Aquamarine,Color.Bisque,Color.BurlyWood,Color.CadetBlue,Color.Blue,Color.Red,Color.MediumSeaGreen,Color.DarkTurquoise,
                                 Color.Chartreuse,Color.Chocolate,Color.Coral,Color.CornflowerBlue,Color.Cornsilk,Color.Crimson,Color.Cyan,Color.DarkGoldenrod,Color.Blue,Color.MediumSeaGreen,
                                 Color.DarkMagenta,Color.DarkOrange,Color.DarkOrchid,Color.DarkSalmon,Color.DarkSeaGreen,Color.DarkTurquoise,Color.DeepPink,Color.DeepSkyBlue,Color.Fuchsia,
                                 Color.Gold,Color.Goldenrod,Color.GreenYellow,Color.HotPink,Color.IndianRed,Color.Khaki,Color.LavenderBlush,Color.LawnGreen,Color.LemonChiffon,Color.Red,
                                 Color.LightCoral,Color.LightGoldenrodYellow,Color.LightGreen,Color.LightPink,Color.LightSalmon,Color.LightSeaGreen,Color.LightSkyBlue,Color.LightSteelBlue,
                                 Color.Lime,Color.Magenta,Color.MediumAquamarine,Color.MediumOrchid,Color.MediumPurple,Color.MediumSeaGreen,Color.MediumSpringGreen,Color.MediumVioletRed,
                                 Color.MistyRose,Color.NavajoWhite,Color.OliveDrab,Color.Orange,Color.OrangeRed,Color.Orchid,Color.PaleGoldenrod,Color.PaleGreen,Color.PaleTurquoise,
                                 Color.PaleVioletRed,Color.PeachPuff,Color.Peru,Color.Pink,Color.Plum,Color.PowderBlue,Color.Red,Color.Salmon,Color.SandyBrown,Color.SeaGreen,
                                 Color.SkyBlue,Color.SpringGreen,Color.SteelBlue,Color.Tomato,Color.Turquoise,Color.Violet,Color.YellowGreen,Color.MediumTurquoise,Color.Blue,
                                 Color.Red,Color.MediumSeaGreen,Color.DarkTurquoise,Color.PaleVioletRed,Color.Pink,Color.MediumSpringGreen,Color.PeachPuff,
        };

        //بعد از تمام شدن هر نکنیک به ترتیب رنگ های زیر به رنگ چارت اعمال می شود
        public static Color[] color_Graph2065 = {
                Color.Blue,Color.Red,Color.Green,Color.Yellow,Color.Orange,Color.Violet,Color.DarkRed,Color.DarkBlue,Color.DarkOrange,Color.DarkViolet,
                Color.DarkGreen,Color.DarkCyan,Color.DarkOliveGreen,Color.DarkMagenta,Color.Peru,Color.OliveDrab,Color.Salmon,Color.YellowGreen,Color.PaleGreen,Color.DarkMagenta,
                Color.DarkTurquoise,Color.Goldenrod,Color.Coral,Color.DarkOrchid,Color.CadetBlue,Color.DarkSalmon,Color.DarkSeaGreen,Color.CornflowerBlue,Color.Cyan,Color.MediumVioletRed,
                Color.Blue,Color.Red,Color.Green,Color.Yellow,Color.Orange,Color.Violet,Color.DarkRed,Color.DarkBlue,Color.DarkOrange,Color.DarkViolet,
                Color.DarkGreen,Color.DarkCyan,Color.DarkOliveGreen,Color.DarkMagenta,Color.Peru,Color.OliveDrab,Color.Salmon,Color.YellowGreen,Color.PaleGreen,Color.DarkMagenta,
                Color.DarkTurquoise,Color.Goldenrod,Color.Coral,Color.DarkOrchid,Color.CadetBlue,Color.DarkSalmon,Color.DarkSeaGreen,Color.CornflowerBlue,Color.Cyan,Color.MediumVioletRed,
                Color.Blue,Color.Red,Color.Green,Color.Yellow,Color.Orange,Color.Violet,Color.DarkRed,Color.DarkBlue,Color.DarkOrange,Color.DarkViolet,
                Color.DarkGreen,Color.DarkCyan,Color.DarkOliveGreen,Color.DarkMagenta,Color.Peru,Color.OliveDrab,Color.Salmon,Color.YellowGreen,Color.PaleGreen,Color.DarkMagenta,
                Color.DarkTurquoise,Color.Goldenrod,Color.Coral,Color.DarkOrchid,Color.CadetBlue,Color.DarkSalmon,Color.DarkSeaGreen,Color.CornflowerBlue,Color.Cyan,Color.MediumVioletRed
        };


        //جهت ست کردن مقدار به پارامترهای هر تکنیک یک مقدار به تکنیک داده شده تا نام مخفف آن با عدد صدا زده شود
        public const byte DCV = 0, NPV = 1, DPV = 2, SWV = 3, CV = 4, LSV = 5, DCs = 6, DPs = 7, CCC = 8, CPC = 9, CHP = 10, CA = 11, CHC = 12, CD = 14, OCP = 15, RT = 16,CDtest = 17;/////, CA = 11
        
        //2080
        public const byte NPV2080 = 1, DPV2080 = 2, SWV2080 = 3, CV2080 = 4, OCP2080 = 15, CPC2080 = 9, CHA2080 = 11, CCC2080 = 12, LSV2080 = 5, CHP2080 = 10;/////, CA = 11

        //برای هر تکنیکی پارامترهایی تعریف شده است که داخل کلاس پارام کلاسهای تعریف هر تکنیک و پارامترهاشون قرار دارد
        public static DCV_params dcvprms = new DCV_params();
        public static NPV_params npvprms = new NPV_params();
        public static DPV_params dpvprms = new DPV_params();
        public static SWV_params swvprms = new SWV_params();
        public static CV_params cvprms = new CV_params();
        public static LSV_params lsvprms = new LSV_params();
        //public static DCS_params dcsprms = new DCS_params();
        //public static DPS_params dpsprms = new DPS_params();
        public static CPC_params cpcprms = new CPC_params();
        public static CCC_params cccprms = new CCC_params();
        public static CHP_params chpprms = new CHP_params();
        public static CHA_params chaprms = new CHA_params();
        public static CHC_params chcprms = new CHC_params();
        public static cpct_params cpctprms = new cpct_params();
        public static Cd_params cdprms = new Cd_params();
        public static OCP_params ocpprms = new OCP_params();
        public static RT_params rtprms = new RT_params();
        public static CDtest_params cdtestprms = new CDtest_params();
        //2080
        public static CCP_params_ocp cpctprms_ocp = new CCP_params_ocp();
        public static DCV_params_ocp dcvprms_ocp = new DCV_params_ocp();
        public static NPV_params_ocp npvprms_ocp = new NPV_params_ocp();
        public static DPV_params_ocp dpvprms_ocp = new DPV_params_ocp();
        public static SWV_params_ocp swvprms_ocp = new SWV_params_ocp();
        public static CV_params_ocp cvprms_ocp = new CV_params_ocp();
        public static LSV_params_ocp lsvprms_ocp = new LSV_params_ocp();
        public static OCP_params_ocp ocpprms_ocp = new OCP_params_ocp();
        public static CPC_params_opc cpcprms_ocp = new CPC_params_opc();
        public static CA_params_ocp chaprms_ocp = new CA_params_ocp();
        public static CCC_params_opc cccprms_ocp = new CCC_params_opc();
       
        public static DCS_params_ocp dcsprms_ocp = new DCS_params_ocp();
        public static DPS_params_ocp dpsprms_ocp = new DPS_params_ocp();


        public static void set_gridCells(int Tch, PropertyGrid pGrid)// هنگام انتخاب هر تکنیک پارامترهای آن درون کلاسش ست می شود 
        {
            switch (Tch)
            {
                case NPV: pGrid.SelectedObject = npvprms; break;
                case DPV: pGrid.SelectedObject = dpvprms; break;
                case SWV: pGrid.SelectedObject = swvprms; break;
                case CV: pGrid.SelectedObject =  cvprms; break;
                case LSV: pGrid.SelectedObject = lsvprms; break;;
                case CPC: pGrid.SelectedObject = cpcprms; break;
                case CHP: pGrid.SelectedObject = chpprms; break;
                case CA: pGrid.SelectedObject = chaprms; break;
                case CHC: pGrid.SelectedObject = chcprms; break;
                case CD: pGrid.SelectedObject = cdprms; break;
                case OCP: pGrid.SelectedObject = ocpprms; break;
                case RT: pGrid.SelectedObject = rtprms; break;
                default: break;
            }
           
        }
        public static void set_gridCells_ocp(int Tch, PropertyGrid pGrid)//در صورت داشتن او سی پی ست می شود
        {
            switch (Tch)
            {
                // case DCV: pGrid.SelectedObject = dcvprms_ocp; break;
                case NPV2080: pGrid.SelectedObject = npvprms_ocp; break;
                case DPV2080: pGrid.SelectedObject = dpvprms_ocp; break;
                case SWV2080: pGrid.SelectedObject = swvprms_ocp; break;
                case CV2080: pGrid.SelectedObject = cvprms_ocp; break;
                case LSV2080: pGrid.SelectedObject = lsvprms_ocp; break;
                //case DCs: pGrid.SelectedObject = dcsprms; break;
                //case DPs: pGrid.SelectedObject = dpsprms; break;
                case OCP2080: pGrid.SelectedObject = ocpprms_ocp; break;
                case CPC2080: pGrid.SelectedObject = cpcprms_ocp; break;
                //  case CHP: pGrid.SelectedObject = chpprms; break;
                case CHA2080: pGrid.SelectedObject = chaprms_ocp; break;
                //  case CHC: pGrid.SelectedObject = chcprms; break;
                default: break;
            }

        }

        //public static void Init_defualt_Params()
        //{
        //    // cvprms.Cycles = 1;
        //    cvprms.E1 = 1;
        //    cvprms.E2 = -1;
        //    cvprms.E3 = 1;
        //    cvprms.HStep = 0.1;
        //    cvprms.TStep = 0;
        //}
        public static void AppendText(RichTextBox box, string text, Color color)// متن وضعیت تکنیک در حال اجرا
        {
            box.SelectionStart = 0;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.SelectedText = text;
            // box.SelectionColor = box.ForeColor;
        }

        public static void ChangeColor(string fileName)
        {
            var lines = File.ReadAllLines(fileName).ToList();
            lines.RemoveAt(2);
            lines.Insert(2, ("Color:" + Graph_Color)); // افزودن محتوای جدید
            File.WriteAllLines(fileName, lines);

        }

        public static void SetChartColor()
        {
            Random rand = new Random();
            int RandomNumber = rand.Next(90);
            Graph_Color = clasglobal.color_Graph2065[RandomNumber].ToArgb();
        }

     }
    
}
