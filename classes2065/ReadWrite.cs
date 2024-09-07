using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skydat.classes2065
{
    class ReadWrite
    {
        public static bool WritingFlag;
        private datasets.Dschart1 dschart1;
        public static void ReadFromFile(string tchName, string fileName)//فراخوانی اطلاعات تکنیک ها و پارامترها از فایل پروژه
        {
            try
            {
                StreamReader Fl = new StreamReader(fileName, Encoding.ASCII);
                string s = "";
                int tech = -1;
                int num_row = 0;
                bool flag1 = false;
                if (tchName == "All" || tchName == "Singel")
                    while (!Fl.EndOfStream)
                    {
                        s = Fl.ReadLine();
                        if (s.LastIndexOf("Technic") >= 0)
                        {
                            if ((s.Substring(9) == "NPV") && s.LastIndexOf("Technic") >= 0 && s != "")
                            {
                                tech = clasglobal.NPV;
                                s = Fl.ReadLine(); clasglobal.npvprms.E1 = Convert.ToDouble(s.Substring(3));
                                s = Fl.ReadLine(); clasglobal.npvprms.E2 = Convert.ToDouble(s.Substring(3));
                                s = Fl.ReadLine(); clasglobal.npvprms.HStep = Convert.ToDouble(s.Substring(6));
                                s = Fl.ReadLine(); clasglobal.npvprms.PulseWidth = Convert.ToDouble(s.Substring(12));
                                s = Fl.ReadLine(); clasglobal.npvprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                s = Fl.ReadLine(); clasglobal.npvprms.ScanRate = Convert.ToDouble(s.Substring(10));
                                s = Fl.ReadLine(); clasglobal.npvprms.TStep = Convert.ToDouble(s.Substring(6));
                                s = Fl.ReadLine();
                                if (s.Trim().Length >= 7)
                                {
                                    if (s.Trim().Substring(0, 7) == "comment")
                                    {
                                        clasglobal.npvprms.comment1 = s.Substring(8);
                                    }
                                    else
                                        clasglobal.npvprms.comment1 = "";

                                }
                                else
                                {
                                    clasglobal.npvprms.comment1 = "";
                                }
                                s = "            ";
                            }
                            if ((s.Substring(9) == "DPV") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.DPV;
                                    s = Fl.ReadLine(); clasglobal.dpvprms.E1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.dpvprms.E2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.dpvprms.HStep = Convert.ToDouble(s.Substring(6));
                                    s = Fl.ReadLine(); clasglobal.dpvprms.PulseHeight = Convert.ToDouble(s.Substring(13));
                                    s = Fl.ReadLine(); clasglobal.dpvprms.PulseWidth = Convert.ToDouble(s.Substring(12));
                                    s = Fl.ReadLine(); clasglobal.dpvprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.dpvprms.ScanRate = Convert.ToDouble(s.Substring(10));
                                    s = Fl.ReadLine(); clasglobal.dpvprms.TStep = Convert.ToDouble(s.Substring(6));
                                    s = Fl.ReadLine();
                                    if (s.Trim().Length >= 7)
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.dpvprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.dpvprms.comment1 = "";

                                    }
                                    else
                                    {
                                        clasglobal.dpvprms.comment1 = "";
                                    }
                                    s = "            ";
                                }
                                catch { }
                            if ((s.Substring(9) == "SWV") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.SWV;
                                    s = Fl.ReadLine(); clasglobal.swvprms.E1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.swvprms.E2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.swvprms.Frequency = Convert.ToDouble(s.Substring(10));
                                    s = Fl.ReadLine(); clasglobal.swvprms.HStep = Convert.ToDouble(s.Substring(6));
                                    s = Fl.ReadLine(); clasglobal.swvprms.PulseHeight = Convert.ToDouble(s.Substring(13));
                                    s = Fl.ReadLine(); clasglobal.swvprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.swvprms.ScanRate = Convert.ToDouble(s.Substring(10));
                                    s = Fl.ReadLine();
                                    if (s.Trim().Length >= 7)
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.swvprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.swvprms.comment1 = "";

                                    }
                                    else
                                    {
                                        clasglobal.swvprms.comment1 = "";
                                    }
                                    s = "            ";
                                }
                                catch { }
                            if ((s.Substring(9) == "CV") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.CV;
                                    s = Fl.ReadLine(); clasglobal.cvprms.Cycles = Convert.ToDouble(s.Substring(7));
                                    s = Fl.ReadLine(); clasglobal.cvprms.E1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.cvprms.E2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.cvprms.E3 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); //clasglobal.cvprms.HoldTime = Convert.ToDouble(s.Substring(9));
                                    s = Fl.ReadLine(); clasglobal.cvprms.HStep = Convert.ToDouble(s.Substring(6));
                                    s = Fl.ReadLine(); clasglobal.cvprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.cvprms.ScanRate = Convert.ToDouble(s.Substring(10));
                                    s = Fl.ReadLine(); //clasglobal.cvprms.ScanRate_R = Convert.ToDouble(s.Substring(11));
                                    s = Fl.ReadLine(); clasglobal.cvprms.TStep = Convert.ToDouble(s.Substring(6));
                                    s = Fl.ReadLine();
                                    if (s.Trim().Length >= 7)
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.cvprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.cvprms.comment1 = "";

                                    }
                                    else
                                    {
                                        clasglobal.cvprms.comment1 = "";
                                    }
                                    s = "            ";
                                }
                                catch {
                                    return;
                                }
                            if ((s.Substring(9) == "LSV") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.LSV;
                                    s = Fl.ReadLine(); clasglobal.lsvprms.E1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.lsvprms.E2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.lsvprms.HStep = Convert.ToDouble(s.Substring(6));
                                    s = Fl.ReadLine(); clasglobal.lsvprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.lsvprms.ScanRate = Convert.ToDouble(s.Substring(10));
                                    s = Fl.ReadLine(); clasglobal.lsvprms.TStep = Convert.ToDouble(s.Substring(6));
                                    s = Fl.ReadLine();
                                    if (s.Trim().Length >= 7)
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.lsvprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.lsvprms.comment1 = "";

                                    }
                                    else
                                    {
                                        clasglobal.lsvprms.comment1 = "";
                                    }
                                    s = "            ";
                                }
                                catch { }
                            if ((s.Substring(9) == "CPC") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.CPC;
                                    s = Fl.ReadLine(); clasglobal.cpcprms.E1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.cpcprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.cpcprms.T1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); //clasglobal.cpcprms.Cycles = Convert.ToDouble(s.Substring(7));
                                    s = Fl.ReadLine();
                                    if (s.Trim().Length >= 7)
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.cpcprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.cpcprms.comment1 = "";

                                    }
                                    else
                                    {
                                        clasglobal.cpcprms.comment1 = "";
                                    }
                                    s = "            ";
                                }
                                catch { }
                            if ((s.Substring(9) == "CA") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.CA;
                                    s = Fl.ReadLine(); clasglobal.chaprms.E1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chaprms.E2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chaprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.chaprms.T1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chaprms.T2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chaprms.Cycle = Convert.ToDouble(s.Substring(7));
                                    s = Fl.ReadLine();
                                    if (s.Trim().Length >= 7)
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.chaprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.chaprms.comment1 = "";

                                    }
                                    else
                                    {
                                        clasglobal.chaprms.comment1 = "";
                                    }
                                    s = "            ";
                                }
                                catch { }
                            if ((s.Substring(9) == "CC") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.CHC;
                                    s = Fl.ReadLine(); clasglobal.chcprms.E1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chcprms.E2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chcprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.chcprms.T1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chcprms.T2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); //caprms.Cycles = Convert.ToDouble(s.Substring(7));
                                    s = Fl.ReadLine();
                                    if (s.Trim().Length < 7)
                                    {
                                        clasglobal.chcprms.comment1 = "";

                                    }
                                    else
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.chcprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.chcprms.comment1 = "";

                                    }
                                    s = "            ";
                                }
                                catch { }
                            if ((s.Substring(9) == "CP") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.CHP;
                                    s = Fl.ReadLine(); clasglobal.chpprms.I1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chpprms.I2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chpprms.EUP = Convert.ToDouble(s.Substring(4));
                                    s = Fl.ReadLine(); clasglobal.chpprms.EDO = Convert.ToDouble(s.Substring(4));
                                    s = Fl.ReadLine(); clasglobal.chpprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.chpprms.T1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chpprms.T2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.chpprms.Cycle = Convert.ToDouble(s.Substring(7));
                                    s = Fl.ReadLine();
                                    if (s != null && s.Trim().Length >= 7)
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.chpprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.chpprms.comment1 = "";

                                    }
                                    else
                                    {
                                        clasglobal.chpprms.comment1 = "";
                                    }
                                    s = "            ";
                                }
                                catch { }
                            if ((s.Substring(9) == "CD") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.CD;
                                    s = Fl.ReadLine(); clasglobal.cdprms.I1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.cdprms.I2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.cdprms.EUP = Convert.ToDouble(s.Substring(4));
                                    s = Fl.ReadLine(); clasglobal.cdprms.EDO = Convert.ToDouble(s.Substring(4));
                                    s = Fl.ReadLine(); clasglobal.cdprms.EquilibriumTime = Convert.ToDouble(s.Substring(17));
                                    s = Fl.ReadLine(); clasglobal.cdprms.T1 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.cdprms.T2 = Convert.ToDouble(s.Substring(3));
                                    s = Fl.ReadLine(); clasglobal.cdprms.Cycle = Convert.ToDouble(s.Substring(7));
                                    s = Fl.ReadLine();
                                    if (s.Trim().Length >= 7)
                                    {
                                        if (s.Trim().Substring(0, 7) == "comment")
                                        {
                                            clasglobal.chpprms.comment1 = s.Substring(8);
                                        }
                                        else
                                            clasglobal.chpprms.comment1 = "";

                                    }
                                    else
                                    {
                                        clasglobal.chpprms.comment1 = "";
                                    }
                                    s = "            ";
                                }
                                catch { }
                            if ((s.Substring(9) == "OCP") && s.LastIndexOf("Technic") >= 0 && s != "")
                                try
                                {
                                    tech = clasglobal.OCP;
                                    s = Fl.ReadLine(); clasglobal.ocpprms.Time = Convert.ToDouble(s.Substring(5));
                                    //s = Fl.ReadLine(); //clasglobal.ocpprms.Cycles = Convert.ToDouble(s.Substring(7));
                                    //s = "            ";
                                }
                                catch { }
                            flag1 = true;
                        }
                        if (tchName == "Singel" && flag1)
                        {
                            num_row++;
                        }
                    }
                Fl.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show($"مقادیر اولیه به درستی فراخانی نشد \n{x.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        public static void WriteToFile(string tchName, string fileName)
        {

            if (tchName == "All")
            {
                // پاک کردن محتوای فایل
                File.WriteAllText(fileName, string.Empty);
            }
            StreamWriter Fl = new StreamWriter(fileName, true, Encoding.ASCII);
            if (WritingFlag)
            {
                Fl.WriteLine("Settings for Parameters:");
                Fl.WriteLine("Date:" + DateTime.Today.ToString());
            }

            if (tchName == "NPV" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: NPV");
                Fl.WriteLine("E1:" + clasglobal.npvprms.E1.ToString());
                Fl.WriteLine("E2:" + clasglobal.npvprms.E2.ToString());
                Fl.WriteLine("HStep:" + clasglobal.npvprms.HStep.ToString());
                Fl.WriteLine("Pulse Width:" + clasglobal.npvprms.PulseWidth.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.npvprms.EquilibriumTime.ToString());
                Fl.WriteLine("Scan Rate:" + clasglobal.npvprms.ScanRate.ToString());
                Fl.WriteLine("TStep:" + clasglobal.npvprms.TStep.ToString());
            }

            if (tchName == "DPV" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: DPV");
                //Fl.WriteLine("TchNo: " + DPV.ToString());
                Fl.WriteLine("E1:" + clasglobal.dpvprms.E1.ToString());
                Fl.WriteLine("E2:" + clasglobal.dpvprms.E2.ToString());
                Fl.WriteLine("HStep:" + clasglobal.dpvprms.HStep.ToString());
                Fl.WriteLine("Pulse Height:" + clasglobal.dpvprms.PulseHeight.ToString());
                Fl.WriteLine("Pulse Width:" + clasglobal.dpvprms.PulseWidth.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.dpvprms.EquilibriumTime.ToString());
                Fl.WriteLine("Scan Rate:" + clasglobal.dpvprms.ScanRate.ToString());
                Fl.WriteLine("TStep:" + clasglobal.dpvprms.TStep.ToString());
            }

            if (tchName == "SWV" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: SWV");
                Fl.WriteLine("E1:" + clasglobal.swvprms.E1.ToString());
                Fl.WriteLine("E2:" + clasglobal.swvprms.E2.ToString());
                Fl.WriteLine("Frequency:" + clasglobal.swvprms.Frequency.ToString());
                Fl.WriteLine("HStep:" + clasglobal.swvprms.HStep.ToString());
                Fl.WriteLine("Pulse Height:" + clasglobal.swvprms.PulseHeight.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.swvprms.EquilibriumTime.ToString());
                Fl.WriteLine("Scan Rate:" + clasglobal.swvprms.ScanRate.ToString());
            }

            if (tchName == "CV" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: CV");
                //Fl.WriteLine("TchNo: " + CV.ToString());
                Fl.WriteLine("Cycles:" + clasglobal.cvprms.Cycles.ToString());
                Fl.WriteLine("E1:" + clasglobal.cvprms.E1.ToString());
                Fl.WriteLine("E2:" + clasglobal.cvprms.E2.ToString());
                Fl.WriteLine("E3:" + clasglobal.cvprms.E3.ToString());
                Fl.WriteLine("Hold Time:0");
                Fl.WriteLine("HStep:" + clasglobal.cvprms.HStep.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.cvprms.EquilibriumTime.ToString());
                Fl.WriteLine("Scan Rate:" + clasglobal.cvprms.ScanRate.ToString());
                Fl.WriteLine("ScanRate_R:" + clasglobal.cvprms.ScanRate.ToString());
                Fl.WriteLine("TStep:" + clasglobal.cvprms.TStep.ToString());
            }

            if (tchName == "LSV" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: LSV");
                //Fl.WriteLine("TchNo: " + LSV.ToString());
                Fl.WriteLine("E1:" + clasglobal.lsvprms.E1.ToString());
                Fl.WriteLine("E2:" + clasglobal.lsvprms.E2.ToString());
                Fl.WriteLine("HStep:" + clasglobal.lsvprms.HStep.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.lsvprms.EquilibriumTime.ToString());
                Fl.WriteLine("Scan Rate:" + clasglobal.lsvprms.ScanRate.ToString());
                Fl.WriteLine("TStep:" + clasglobal.lsvprms.TStep.ToString());
            }

            if (tchName == "CPC" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: CPC");
                //Fl.WriteLine("TchNo: " + CPC.ToString());
                Fl.WriteLine("E1:" + clasglobal.cpcprms.E1.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.cpcprms.EquilibriumTime.ToString());
                Fl.WriteLine("T1:" + clasglobal.cpcprms.T1.ToString());
                Fl.WriteLine("Cycles:" + "1"); //clasglobal.cpcprms.Cycles.ToString());
            }

            if (tchName == "CA" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: CA");
                //Fl.WriteLine("TchNo: " + CA.ToString());
                Fl.WriteLine("E1:" + clasglobal.chaprms.E1.ToString());
                Fl.WriteLine("E2:" + clasglobal.chaprms.E2.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.chaprms.EquilibriumTime.ToString());
                Fl.WriteLine("T1:" + clasglobal.chaprms.T1.ToString());
                Fl.WriteLine("T2:" + clasglobal.chaprms.T2.ToString());
                Fl.WriteLine("Cycles:" + clasglobal.chaprms.Cycle.ToString());
            }

            if (tchName == "CP" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: CP");
                //Fl.WriteLine("TchNo: " + CA.ToString());
                Fl.WriteLine("I1:" + clasglobal.chpprms.I1.ToString());
                Fl.WriteLine("I2:" + clasglobal.chpprms.I2.ToString());
                Fl.WriteLine("EUP:" + clasglobal.chpprms.EUP.ToString());
                Fl.WriteLine("EDO:" + clasglobal.chpprms.EDO.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.chpprms.EquilibriumTime.ToString());
                Fl.WriteLine("T1:" + clasglobal.chpprms.T1.ToString());
                Fl.WriteLine("T2:" + clasglobal.chpprms.T2.ToString());
                Fl.WriteLine("Cycles:" + clasglobal.chpprms.Cycle.ToString());
            }

            if (tchName == "CC" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: CC");
                //Fl.WriteLine("TchNo: " + CA.ToString());
                Fl.WriteLine("E1:" + clasglobal.chcprms.E1.ToString());
                Fl.WriteLine("E2:" + clasglobal.chcprms.E2.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.chcprms.EquilibriumTime.ToString());
                Fl.WriteLine("T1:" + clasglobal.chcprms.T1.ToString());
                Fl.WriteLine("T2:" + clasglobal.chcprms.T2.ToString());
                Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());
            }

            if (tchName == "CD" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: CD");
                //Fl.WriteLine("TchNo: " + CA.ToString());
                Fl.WriteLine("I1:" + clasglobal.cdprms.I1.ToString());
                Fl.WriteLine("I2:" + clasglobal.cdprms.I2.ToString());
                Fl.WriteLine("EUP:" + clasglobal.cdprms.EUP.ToString());
                Fl.WriteLine("EDO:" + clasglobal.cdprms.EDO.ToString());
                Fl.WriteLine("Equilibrium Time:" + clasglobal.cdprms.EquilibriumTime.ToString());
                Fl.WriteLine("T1:" + clasglobal.cdprms.T1.ToString());
                Fl.WriteLine("T2:" + clasglobal.cdprms.T2.ToString());
                Fl.WriteLine("Cycles:" + clasglobal.cdprms.Cycle.ToString());
            }

            if (tchName == "OCP" || tchName == "All")
            {
                Fl.WriteLine("");
                Fl.WriteLine("Technic: OCP");
                Fl.WriteLine("Time:" + clasglobal.ocpprms.Time.ToString());
            }

            Fl.Close();

        }

        public void WriteToFile(string fileName, int i)
        {
            StreamWriter Fl = new StreamWriter(fileName, false, Encoding.ASCII);
            switch (int.Parse(dschart1.chartlist.Rows[i][1].ToString()))
            {
                case 0:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DCV");
                    //Fl.WriteLine("TchNo: "+ DCV.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("HStep:" + double.Parse(dschart1.chartlist.Rows[i][6].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("Scan Rate:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("TStep:" + double.Parse(dschart1.chartlist.Rows[i][9].ToString()));
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());
                    break;
                case 1:
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: NPV");
                    //Fl.WriteLine("TchNo: " + NPV.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("HStep:" + double.Parse(dschart1.chartlist.Rows[i][6].ToString()));
                    Fl.WriteLine("Pulse Width:" + double.Parse(dschart1.chartlist.Rows[i][10].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("Scan Rate:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("TStep:" + double.Parse(dschart1.chartlist.Rows[i][9].ToString()));
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 2:
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DPV");
                    //Fl.WriteLine("TchNo: " + DPV.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("HStep:" + double.Parse(dschart1.chartlist.Rows[i][6].ToString()));
                    Fl.WriteLine("Pulse Height:" + double.Parse(dschart1.chartlist.Rows[i][11].ToString()));
                    Fl.WriteLine("Pulse Width:" + double.Parse(dschart1.chartlist.Rows[i][10].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("Scan Rate:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("TStep:" + double.Parse(dschart1.chartlist.Rows[i][9].ToString()));
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;


                case 3:
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: SWV");
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("Frequency:" + double.Parse(dschart1.chartlist.Rows[i][12].ToString()));
                    Fl.WriteLine("HStep:" + double.Parse(dschart1.chartlist.Rows[i][6].ToString()));
                    Fl.WriteLine("Pulse Height:" + double.Parse(dschart1.chartlist.Rows[i][11].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("Scan Rate:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 4:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CV");
                    //Fl.WriteLine("TchNo: " + CV.ToString());
                    Fl.WriteLine("Cycles:" + double.Parse(dschart1.chartlist.Rows[i][2].ToString()));
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("E3:" + double.Parse(dschart1.chartlist.Rows[i][5].ToString()));
                    Fl.WriteLine("Hold Time:0");
                    Fl.WriteLine("HStep:" + double.Parse(dschart1.chartlist.Rows[i][6].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("Scan Rate:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("ScanRate_R:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("TStep:" + double.Parse(dschart1.chartlist.Rows[i][9].ToString()));
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;


                case 5:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: LSV");
                    //Fl.WriteLine("TchNo: " + LSV.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("HStep:" + double.Parse(dschart1.chartlist.Rows[i][6].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("Scan Rate:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("TStep:" + double.Parse(dschart1.chartlist.Rows[i][9].ToString()));
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 6:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DCS");
                    //Fl.WriteLine("TchNo: " + DCs.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("HStep:" + double.Parse(dschart1.chartlist.Rows[i][6].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("Scan Rate:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("TStep:" + double.Parse(dschart1.chartlist.Rows[i][9].ToString()));
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;


                case 7:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DPS");
                    //Fl.WriteLine("TchNo: " + DPs.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("HStep:" + double.Parse(dschart1.chartlist.Rows[i][6].ToString()));
                    Fl.WriteLine("Pulse Height:" + double.Parse(dschart1.chartlist.Rows[i][11].ToString()));
                    Fl.WriteLine("Pulse Width:" + double.Parse(dschart1.chartlist.Rows[i][10].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("Scan Rate:" + double.Parse(dschart1.chartlist.Rows[i][8].ToString()));
                    Fl.WriteLine("TStep:" + double.Parse(dschart1.chartlist.Rows[i][9].ToString()));
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 8:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CCC");
                    //Fl.WriteLine("TchNo: " + CCC.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("T1:" + double.Parse(dschart1.chartlist.Rows[i][15].ToString()));
                    Fl.WriteLine("Cycles:" + "1"); //clasglobal.cpcprms.Cycles.ToString());
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 9:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CPC");
                    //Fl.WriteLine("TchNo: " + CPC.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("T1:" + double.Parse(dschart1.chartlist.Rows[i][15].ToString()));
                    Fl.WriteLine("Cycles:" + "1"); //clasglobal.cpcprms.Cycles.ToString());
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 10:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CHP");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("I1:" + double.Parse(dschart1.chartlist.Rows[i][17].ToString()));
                    Fl.WriteLine("I2:" + double.Parse(dschart1.chartlist.Rows[i][18].ToString()));
                    Fl.WriteLine("EUP:" + double.Parse(dschart1.chartlist.Rows[i][5].ToString()));
                    Fl.WriteLine("EDO:" + double.Parse(dschart1.chartlist.Rows[i][13].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("T1:" + double.Parse(dschart1.chartlist.Rows[i][15].ToString()));
                    Fl.WriteLine("T2:" + double.Parse(dschart1.chartlist.Rows[i][16].ToString()));
                    Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 11:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CHA");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("T1:" + double.Parse(dschart1.chartlist.Rows[i][15].ToString()));
                    Fl.WriteLine("T2:" + double.Parse(dschart1.chartlist.Rows[i][16].ToString()));
                    Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 12:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CHC");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("E1:" + double.Parse(dschart1.chartlist.Rows[i][3].ToString()));
                    Fl.WriteLine("E2:" + double.Parse(dschart1.chartlist.Rows[i][4].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("T1:" + double.Parse(dschart1.chartlist.Rows[i][15].ToString()));
                    Fl.WriteLine("T2:" + double.Parse(dschart1.chartlist.Rows[i][16].ToString()));
                    Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;
                case 14:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CD");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("I1:" + double.Parse(dschart1.chartlist.Rows[i][17].ToString()));
                    Fl.WriteLine("I2:" + double.Parse(dschart1.chartlist.Rows[i][18].ToString()));

                    Fl.WriteLine("EUP:" + double.Parse(dschart1.chartlist.Rows[i][5].ToString()));
                    Fl.WriteLine("EDO:" + double.Parse(dschart1.chartlist.Rows[i][13].ToString()));
                    Fl.WriteLine("Equilibrium Time:" + double.Parse(dschart1.chartlist.Rows[i][7].ToString()));
                    Fl.WriteLine("T1:" + double.Parse(dschart1.chartlist.Rows[i][15].ToString()));
                    Fl.WriteLine("T2:" + double.Parse(dschart1.chartlist.Rows[i][16].ToString()));
                    Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());
                    Fl.WriteLine("comment:" + dschart1.chartlist.Rows[i]["comment"].ToString());

                    break;

                case 15:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: OCP");
                    //Fl.WriteLine("TchNo: " + OCP.ToString());
                    Fl.WriteLine("Time:" + double.Parse(dschart1.chartlist.Rows[i][15].ToString()));
                    Fl.WriteLine("Cycles:" + "1"); //clasglobal.ocpprms.Cycles.ToString());
                    break;
                case 16:

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: RT");
                    //Fl.WriteLine("TchNo: " + OCP.ToString());
                    Fl.WriteLine("T1:" + double.Parse(dschart1.chartlist.Rows[i][15].ToString()));

                    break;

            }

        }


        public static void WriteToFile(DataRow dr, string tchName, string fileName, int row, int typedevice)//ذخیره اطلاعات و دیتای تکنیک درون فایل متنی
        {
            StreamWriter Fl = new StreamWriter(fileName, false, Encoding.ASCII);

            if (tchName != "APP" && tchName != "SEQ")
            {
                Fl.WriteLine("Settings for Parameters:");
                Fl.WriteLine("Date:" + DateTime.Now.ToString());
                clasglobal.SetChartColor();
                Fl.WriteLine("Color:" + clasglobal.Graph_Color);

                if (tchName == "NPV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: NPV");
                    //Fl.WriteLine("TchNo: " + NPV.ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Pulse Width:" + dr[10].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    Fl.WriteLine("TStep:" + dr[9].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "DPV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DPV");
                    //Fl.WriteLine("TchNo: " + DPV.ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Pulse Height:" + dr[11].ToString());
                    Fl.WriteLine("Pulse Width:" + dr[10].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    Fl.WriteLine("TStep:" + dr[9].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "SWV" || tchName == "All")
                {

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: SWV");
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("Frequency:" + dr[12].ToString());
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Pulse Height:" + dr[11].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "CV" || tchName == "All")
                {

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CV");
                    //Fl.WriteLine("TchNo: " + CV.ToString());
                    //Fl.WriteLine("Cycles:" + dr[13].ToString());   // i dont knowb why cycle 1 lesses than from user set thise here
                    Fl.WriteLine("Cycles:" + movedata.main_cycle);
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("E3:" + dr[5].ToString());
                    Fl.WriteLine("Hold Time:0");
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    Fl.WriteLine("ScanRate_R:" + dr[8].ToString());
                    Fl.WriteLine("TStep:" + dr[9].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "LSV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: LSV");
                    //Fl.WriteLine("TchNo: " + LSV.ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    Fl.WriteLine("TStep:" + dr[9].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }

                }

                if (tchName == "CPC" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CPC");
                    //Fl.WriteLine("TchNo: " + CPC.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("T1:" + dr[15].ToString());
                    Fl.WriteLine("Cycles:1");
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "CP" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CP");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("I1:" + dr[17].ToString());
                    Fl.WriteLine("I2:" + dr[18].ToString());
                    Fl.WriteLine("Vup:" + dr[5].ToString());
                    Fl.WriteLine("Vdo:" + dr[13].ToString());
                    Fl.WriteLine("T1:" + double.Parse(dr[15].ToString()) * 1000);
                    Fl.WriteLine("T2:" + double.Parse(dr[16].ToString()) * 1000);
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Cycle:" + double.Parse(dr[12].ToString()));
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "CA" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CA");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    if (typedevice == 1)
                        Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    if (typedevice == 1)
                        Fl.WriteLine("T1:" + double.Parse(dr[15].ToString()) * 1000);
                    else
                        Fl.WriteLine("T1:" + double.Parse(dr[15].ToString()));

                    if (typedevice == 1)
                        Fl.WriteLine("T2:" + double.Parse(dr[16].ToString()) * 1000);

                    Fl.WriteLine("Cycle:" + double.Parse(dr[12].ToString()) * 1000);
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());

                    }

                }

                if (tchName == "CC" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CHC");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("T1:" + double.Parse(dr[15].ToString()) * 1000);
                    Fl.WriteLine("T2:" + double.Parse(dr[16].ToString()) * 1000);
                    Fl.WriteLine("Cycle:" + double.Parse(dr[15].ToString()));
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "CD" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CD");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("I1:" + dr[17].ToString());
                    Fl.WriteLine("I2:" + dr[18].ToString());
                    Fl.WriteLine("Vup:" + dr[5].ToString());
                    Fl.WriteLine("Vdo:" + dr[13].ToString());
                    Fl.WriteLine("T1:" + double.Parse(dr[15].ToString()) * 1000);
                    Fl.WriteLine("T2:" + double.Parse(dr[16].ToString()) * 1000);
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Cycle:" + double.Parse(dr[12].ToString()));
                    //Fl.WriteLine("Cycles:" + clasglobal.cdprms.Cycle.ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "OCP" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: OCP");
                    Fl.WriteLine("Time:" + double.Parse(dr[15].ToString()));
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

            }
            Fl.Close();

        }

    }
}
