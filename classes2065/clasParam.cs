using System.ComponentModel;
using System.Drawing;


namespace Skydat.classes2065
{
    //CV Properties *******************************************************************
    [DefaultPropertyAttribute("E1")]
    public class CV_params
    {
        //private CV2_params _AX;
        private double _Cycles;
        private double _E1;
        private double _E2;
        private double _E3;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private double _EquilibriumTime;
        //private double _ScanRateR;
        private double _HoldTime;
        private string _comment1;

        [CategoryAttribute("CV Values"), DescriptionAttribute("Number of complete cycle(s) scan\n 1    100")]
        public double Cycles
        {
            get
            {
                return _Cycles;
            }

            set
            {
                _Cycles = value;
            }
        }

        [CategoryAttribute("CV Values"), DescriptionAttribute("Initial potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("CV Values"), DescriptionAttribute("High limit of potential scan(V)\n -8     +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }

        [CategoryAttribute("CV Values"), DescriptionAttribute("Low limit of potential scan(V)\n -8    +8")]
        public double E3
        {
            get
            {
                return _E3;
            }

            set
            {
                _E3 = value;
            }
        }

        [CategoryAttribute("CV Values"), DescriptionAttribute("Step height (scan increment)(V)\n 0.001    0.025")]
        public double HStep
        {
            get
            {
                return _HStep;
            }

            set
            {
                _HStep = value;
            }
        }

        [CategoryAttribute("CV Values"), DescriptionAttribute("Step time(Hstep/ScanRate)(S)\n 0.0001    10")]
        [ReadOnlyAttribute(true)]
        public double TStep
        {
            get
            {
               return _TStep;
            }

            set
            {
                _TStep = value;
            }
        }

        [CategoryAttribute("CV Values"), DescriptionAttribute(" Potential scan rate(V/S)\n 0.0001    250")]
        [DisplayName("Scan Rate")]
        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }

            set
            {
                _ScanRate = value;
            }
        }
        
        //[CategoryAttribute("CV Values"), DescriptionAttribute("Revers Potential scan rate(V/S)\n 0.0001    250")]
        //[DisplayName("Scan RateR")]
        //public double ScanRateR
        //{
        //    get
        //    {
        //        return _ScanRateR;
        //    }

        //    set
        //    {
        //        _ScanRateR = value;
        //    }
        //}
         
        [CategoryAttribute("CV Values"), DescriptionAttribute("Quiescent time before potential scan(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("CV Values"), DescriptionAttribute("Holding time after reaching E2(S)\n 0    99")]
        [DisplayName("HoldTime")]
        public double HoldTime
        {
            get
            {
                return _HoldTime;
            }

            set
            {
                _HoldTime = value;
            }
        }

        [CategoryAttribute("CV Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        
        public CV_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    
    // LSV Properties *******************************************************************
    public class LSV_params
    {
        private double _E1;
        private double _E2;
        private double _ScanRate; 
        private double _HStep;
        private double _TStep;
        private double _EquilibriumTime;
        private string _comment1;
       [CategoryAttribute("LSV Values"), DescriptionAttribute("Initial potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("LSV Values"), DescriptionAttribute("Final potential(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }

        [CategoryAttribute("LSV Values"), DescriptionAttribute("Step height (scan increment)(V)\n 0.001    0.025")]
        public double HStep
        {
            get
            {
                return _HStep;
            }

            set
            {
                _HStep = value;
            }
        }

        [CategoryAttribute("LSV Values"), DescriptionAttribute("Step time(Hstep/ScanRate)(S)\n 0.0001    10")]
        [ReadOnlyAttribute(true)]
        public double TStep
        {
            get
            {
                return _TStep;
            }

            set
            {
                _TStep = value;
            }
        }

        [CategoryAttribute("LSV Values"), DescriptionAttribute("potential scan rate(V/S)\n 0.0001    250")]
        [DisplayName("Scan Rate")]
        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }

            set
            {
                _ScanRate = value;
            }
        }

        [CategoryAttribute("LSV Values"), DescriptionAttribute("Quiescent time before potential scan(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("LSV Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public LSV_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    // DCS Properties  ******************************************************************* 
    public class DCS_params
    {
        private double _E1;
        private double _E2;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private double _EquilibriumTime;
        private string _comment1;

        /// <summary>
        /// Change value of variable
        /// </summary>
        [CategoryAttribute("DCS Values"), DescriptionAttribute("Initial potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("DCS Values"), DescriptionAttribute("Final potential(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }

        [CategoryAttribute("DCS Values"), DescriptionAttribute("Step height (scan increment)(V)\n 0.001    0.025")]
        public double HStep
        {
            get
            {
                return _HStep;
            }

            set
            {
                _HStep = value;
            }
        }

        [CategoryAttribute("DCS Values"), DescriptionAttribute("Step time(Hstep/ScanRate)(S)\n 0.1    10")]
        [ReadOnlyAttribute(true)]
        public double TStep
        {
            get
            {
                return _TStep;
            }

            set
            {
                _TStep = value;
            }
        }

        [CategoryAttribute("DCS Values"), DescriptionAttribute("potential scan rate(V/S)\n 0.0001    0.25")]
        [DisplayName("Scan Rate")]
        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }

            set
            {
                _ScanRate = value;
            }
        }

        [CategoryAttribute("DCS Values"), DescriptionAttribute("Quiescent time before potential scan(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("DCS Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public DCS_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    //DCV Properties ******************************************************************
    public class DCV_params
    {
        private double _E1;
        private double _E2;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private double _EquilibriumTime;
        private string _comment1;
       
        [CategoryAttribute("DCV Values"), DescriptionAttribute("Initial potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("DCV Values"), DescriptionAttribute("Final potential(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }

        [CategoryAttribute("DCV Values"), DescriptionAttribute("Step height (scan increment)(V)\n 0.001    0.025")]
        public double HStep
        {
            get
            {
                return _HStep;
            }

            set
            {
                _HStep = value;
            }
        }

        [CategoryAttribute("DCV Values"), DescriptionAttribute("Step time(Hstep/ScanRate)(S)\n 0.1    10")]
        [ReadOnlyAttribute(true)]
        public double TStep
        {
            get
            {
                return _TStep;
            }

            set
            {
                _TStep = value;
            }
        }

        [CategoryAttribute("DCV Values"), DescriptionAttribute("Potential scan rate(V/S)\n 0.0001    0.25")]
        [DisplayName("Scan Rate")]
        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }

            set
            {
                _ScanRate = value;
            }
        }

        [CategoryAttribute("DCV Values"), DescriptionAttribute("Quiescent time before potential scan(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("DCV Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public DCV_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    //NPV Properties  ****************************************************************
    public class NPV_params
    {
        public double _E1;
        private double _E2;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private double _PulseWidth;
        private double _EquilibriumTime;
        private string _comment1;
       
       [CategoryAttribute("NPV Values"), DescriptionAttribute("Initial potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("NPV Values"), DescriptionAttribute("Final potential(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }

        [CategoryAttribute("NPV Values"), DescriptionAttribute("Step height (scan increment)(V)\n 0.002    0.025")]
        public double HStep
        {
            get
            {
                return _HStep;
            }

            set
            {
                _HStep = value;
            }
        }

        [CategoryAttribute("NPV Values"), DescriptionAttribute("Step time(Hstep/ScanRate)(S)\n 0.1    10")]
        [ReadOnlyAttribute(true)]
        public double TStep
        {
            get
            {
                return _TStep;
            }

            set
            {
                _TStep = value;
            }
        }

        [CategoryAttribute("NPV Values"), DescriptionAttribute("potential scan rate(V/S)\n 0.0001    0.25")]
        [DisplayName("Scan Rate")]
        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }

            set
            {
                _ScanRate = value;
            }
        }

        [CategoryAttribute("NPV Values"), DescriptionAttribute("Potential pulse time(S)\n 0.05    (TStep-0.05)")]
        [DisplayName("Pulse Width")]
        public double PulseWidth
        {
            get
            {
                return _PulseWidth;
            }

            set
            {
                _PulseWidth = value;
            }
        }
        
        [CategoryAttribute("NPV Values"), DescriptionAttribute("Quiescent time before potential scan(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("NPV Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public NPV_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    //DPV Properties  ****************************************************************
    public class DPV_params
    {
        private double _E1;
        private double _E2;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private double _PulseWidth;
        private double _PulseHeight;
        private double _EquilibriumTime;
        private string _comment1;

        [CategoryAttribute("DPV Values"), DescriptionAttribute("Initial potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("DPV Values"), DescriptionAttribute("Final potential(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }

        [CategoryAttribute("DPV Values"), DescriptionAttribute("Step height (scan increment)(V)\n 0.002    0.025")]
        public double HStep
        {
            get
            {
                return _HStep;
            }

            set
            {
                _HStep = value;
            }
        }

        [CategoryAttribute("DPV Values"), DescriptionAttribute("Step time(Hstep/ScanRate)(S)\n 0.1    10")]
        [ReadOnlyAttribute(true)]
        public double TStep
        {
            get
            {
                return _TStep;
            }

            set
            {
                _TStep = value;
            }
        }

        [CategoryAttribute("DPV Values"), DescriptionAttribute("Potential scan rate(V/S)\n 0.0001    0.25")]
        [DisplayName("Scan Rate")]
        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }

            set
            {
                _ScanRate = value;
            }
        }

        [CategoryAttribute("DPV Values"), DescriptionAttribute("Potential pulse time(S)\n 0.05    (TStep-0.05)")]
        [DisplayName("Pulse Width")]
        public double PulseWidth
        {
            get
            {
                return _PulseWidth;
            }

            set
            {
                _PulseWidth = value;
            }
        }

        [CategoryAttribute("DPV Values"), DescriptionAttribute("Potential Pulse Amplitude superimposed(V)\n 0.001    0.25")]
        [DisplayName("Pulse Height")]
        public double PulseHeight
        {
            get
            {
                return _PulseHeight;
            }

            set
            {
                _PulseHeight = value;
            }
        }
        
        [CategoryAttribute("DPV Values"), DescriptionAttribute("Quiescent time before potential scan(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("DPV Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public DPV_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    
    //SWV Properties  ****************************************************************
    public class SWV_params
    {
        private double _E1;
        private double _E2;
        private double _ScanRate;
        private double _Frequency;
        private double _HStep;
        private double _PulseHeight;
        private double _EquilibriumTime;
        private string _comment1;

       [CategoryAttribute("SWV Values"), DescriptionAttribute("Initial potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("SWV Values"), DescriptionAttribute("Final potential(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }

        [CategoryAttribute("SWV Values"), DescriptionAttribute("Square wave frequency(Hstep/ScanRate)(Hz)\n 1    1000")]
        [ReadOnlyAttribute(true)]
        public double Frequency
        {
            get
            {
                return _Frequency;
            }

            set
            {
                _Frequency = value;
            }
        }

        [CategoryAttribute("SWV Values"), DescriptionAttribute("Step height (scan increment)(V)\n 0.002    0.025")]
        public double HStep
        {
            get
            {
                return _HStep;
            }

            set
            {
                _HStep = value;
            }
        }

        [CategoryAttribute("SWV Values"), DescriptionAttribute("potential scan rate(V/S)\n 0.001    25")]
        [DisplayName("Scan Rate")]
        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }

            set
            {
                _ScanRate = value;
            }
        }

        [CategoryAttribute("SWV Values"), DescriptionAttribute("Potential Pulse Amplitude superimposed(V)\n .001    0.25")]
        [DisplayName("Pulse Height")]
        public double PulseHeight
        {
            get
            {
                return _PulseHeight;
            }

            set
            {
                _PulseHeight = value;
            }
        }

        [CategoryAttribute("SWV Values"), DescriptionAttribute("Quiescent time before potential scan(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("SWV Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public SWV_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    
    //DPS Properties  ****************************************************************
    public class DPS_params
    {
        private double _E1;
        private double _E2;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private double _PulseWidth;
        private double _PulseHeight;
        private double _EquilibriumTime;
        private string _comment1;

        [CategoryAttribute("DPS Values"), DescriptionAttribute("Initial potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("DPS Values"), DescriptionAttribute("Final potential(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }

        [CategoryAttribute("DPS Values"), DescriptionAttribute("Step height (scan increment)(V)\n 0.001    0.025")]
        public double HStep
        {
            get
            {
                return _HStep;
            }

            set
            {
                _HStep = value;
            }
        }

        [CategoryAttribute("DPS Values"), DescriptionAttribute("Step time(Hstep/ScanRate)(S)\n 0.1    10")]
        [ReadOnlyAttribute(true)]
        public double TStep
        {
            get
            {
                return _TStep;
            }

            set
            {
                _TStep = value;
            }
        }

        [CategoryAttribute("DPS Values"), DescriptionAttribute("potential scan rate(V/S)\n 0.0001    0.25")]
        [DisplayName("Scan Rate")]
        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }

            set
            {
                _ScanRate = value;
            }
        }

        [CategoryAttribute("DPS Values"), DescriptionAttribute("Potential pulse time(S)\n 0.05    (TStep-0.05)")]
        [DisplayName("Pulse Width")]
        public double PulseWidth
        {
            get
            {
                return _PulseWidth;
            }

            set
            {
                _PulseWidth = value;
            }
        }

        [CategoryAttribute("DPS Values"), DescriptionAttribute("Potential Pulse Amplitude superimposed(V)\n 0.001    0.25")]
        [DisplayName("Pulse Height")]
        public double PulseHeight
        {
            get
            {
                return _PulseHeight;
            }
            set
            {
                _PulseHeight = value;
            }
        }

        [CategoryAttribute("DPS Values"), DescriptionAttribute("Quiescent time before potential scan(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("DPS Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public DPS_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    
    //CPC Properties  ****************************************************************
    public class CPC_params
    {
        //private double _Cycles;
        private double _E1;
        private double _T1;
        private double _EquilibriumTime;
        private string _comment1;

       [CategoryAttribute("CPC Values"), DescriptionAttribute("Step potential(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }

        [CategoryAttribute("CPC Values"), DescriptionAttribute("Step time(S)\n 1    60000")]
        public double T1
        {
            get
            {
                return _T1;
            }

            set
            {
                _T1 = value;
            }
        }

        [CategoryAttribute("CPC Values"), DescriptionAttribute("Quiescent time before start measuring(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("CPC Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public CPC_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    //CPC Properties  ****************************************************************
    public class CCC_params
    {
        //private double _Cycles;
        private double _I1;
        private double _T1;
        private double _EquilibriumTime;
        private string _comment1;

        
        [CategoryAttribute("CCC Values"), DescriptionAttribute("Step potential(V)\n -8    +8")]
        public double I1
        {
            get
            {
                return _I1;
            }

            set
            {
                _I1 = value;
            }
        }

        [CategoryAttribute("CCC Values"), DescriptionAttribute("Step time(S)\n 1    65000")]
        public double T1
        {
            get
            {
                return _T1;
            }

            set
            {
                _T1 = value;
            }
        }

        [CategoryAttribute("CCC Values"), DescriptionAttribute("Quiescent time before start measuring(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("CCC Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public CCC_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    //CA Properties  ****************************************************************
    public class CHP_params
    {
        //private double _Cycles;
        private double _I1;
        private double _T1;
        private double _I2;
        private double _T2;
        private double _EUP;
        private double _EDO;
        private double _CYCLE;
        private double _EquilibriumTime;
        private string _comment1;

        [CategoryAttribute("CHP Values"), DescriptionAttribute("First current step(mA)\n -8    +8")]
        public double I1
        {
            get
            {
                return _I1;
            }

            set
            {
                _I1 = value;
            }
        }
        [CategoryAttribute("CHP Values"), DescriptionAttribute("First step time(S)\n 1    32000")]
        public double T1
        {
            get
            {
                return _T1;
            }

            set
            {
                _T1 = value;
            }
        }

        [CategoryAttribute("CHP Values"), DescriptionAttribute("Second current step(mA)\n -8    +8")]
        public double I2
        {
            get
            {
                return _I2;
            }

            set
            {
                _I2 = value;
            }
        }
        [CategoryAttribute("CHP Values"), DescriptionAttribute("Second step time(S)\n 1    32000-T1")]
        public double T2
        {
            get
            {
                return _T2;
            }

            set
            {
                _T2 = value;
            }
        }
        [CategoryAttribute("CHP Values"), DescriptionAttribute("Higger cutoff voltage(V)")]
        public double EUP
        {
            get
            {
                return _EUP;
            }

            set
            {
                _EUP = value;
            }
        }
        [CategoryAttribute("CHP Values"), DescriptionAttribute("lowwer cutoff voltage(V)")]
        public double EDO
        {
            get
            {
                return _EDO;
            }

            set
            {
                _EDO = value;
            }
        }
        [CategoryAttribute("Cycle Values"), DescriptionAttribute("CYCLE")]
        public double CYCLE
        {
            get
            {
                return _CYCLE;
            }

            set
            {
                _CYCLE = value;
            }
        }
        [CategoryAttribute("CHP Values"), DescriptionAttribute("Quiescent time before start measuring(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("CHP Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }

      

        public CHP_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    //CA Properties  ****************************************************************
    public class CHA_params
    {
        //private double _Cycles;
        private double _E1;
        private double _T1;
        private double _E2;
        private double _T2;
        private double _CYCLE;//
        private double _EquilibriumTime;
        private string _comment1;

        [CategoryAttribute("CHA Values"), DescriptionAttribute("First potential step(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }
        [CategoryAttribute("CHA Values"), DescriptionAttribute("First step time(S)\n 1    32000")]
        public double T1
        {
            get
            {
                return _T1;
            }

            set
            {
                _T1 = value;
            }
        }

        [CategoryAttribute("CHA Values"), DescriptionAttribute("second potential step(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }
        [CategoryAttribute("CHA Values"), DescriptionAttribute("second step time(S)\n 1    32000-T1")]
        public double T2
        {
            get
            {
                return _T2;
            }

            set
            {
                _T2 = value;
            }
        }

        [CategoryAttribute("CHA Values"), DescriptionAttribute("CYCLE")]
        public double CYCLE
        {
            get
            {
                return _CYCLE;
            }

            set
            {
                _CYCLE = value;
            }
        }

        [CategoryAttribute("CHA Values"), DescriptionAttribute("Quiescent time before start measuring(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("CHA Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public CHA_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    //CHA Properties  ****************************************************************
    public class CHC_params
    {
        //private double _Cycles;
        private double _E1;
        private double _T1;
        private double _E2;
        private double _T2;
        private double _EquilibriumTime;
        private string _comment1;
        
        [CategoryAttribute("CHC Values"), DescriptionAttribute("First potential step(V)\n -8    +8")]
        public double E1
        {
            get
            {
                return _E1;
            }

            set
            {
                _E1 = value;
            }
        }
        [CategoryAttribute("CHC Values"), DescriptionAttribute("First step time(S)\n 1    32000")]
        public double T1
        {
            get
            {
                return _T1;
            }

            set
            {
                _T1 = value;
            }
        }

        [CategoryAttribute("CHC Values"), DescriptionAttribute("Second potential step(V)\n -8    +8")]
        public double E2
        {
            get
            {
                return _E2;
            }

            set
            {
                _E2 = value;
            }
        }
        [CategoryAttribute("CHC Values"), DescriptionAttribute("Second step time(S)\n 1    32000-T1")]
        public double T2
        {
            get
            {
                return _T2;
            }

            set
            {
                _T2 = value;
            }
        }

        [CategoryAttribute("CHC Values"), DescriptionAttribute("Quiescent time before start measuring(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("CHC Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public CHC_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    public class OCP_params
    {
        //private double _Cycles;
        private double _Time;
        private string _comment1;
      
        [CategoryAttribute("OCP Values"), DescriptionAttribute("Time(S)\n 0    +65000")]
        public double Time
        {
            get
            {
                return _Time;
            }
            set
            {
                _Time = value;
            }
        }
        [CategoryAttribute("OCP Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public OCP_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    /*
        public class SC_params
        {
            private double _Cycles;
            private double _E1;
            private double _Q1;
            private double _E2;
            private double _Q2;
            private double _EquilibriumTime;
            private bool _vs_OCP;
            private double _OCPmeasurment;
            private double _Range;
            [CategoryAttribute("SC Values"), DescriptionAttribute("Number of complete cycle(s) scan\n 1    1000")]
            public double Cycles
            {
                get
                {
                    return _Cycles;
                }

                set
                {
                    _Cycles = value;
                }
            }

            [CategoryAttribute("SC Values"), DescriptionAttribute("First potential step(V)\n -5    +5")]
            public double E1
            {
                get
                {
                    return _E1;
                }

                set
                {
                    _E1 = value;
                }
            }

            [CategoryAttribute("SC Values"), DescriptionAttribute("Charge(uC)\n -65000    +65000")]
            public double Q1
            {
                get
                {
                    return _Q1;
                }

                set
                {
                    _Q1 = value;
                }
            }

            [CategoryAttribute("SC Values"), DescriptionAttribute("Sec. potential step(V)\n -5    +5")]
            public double E2
            {
                get
                {
                    return _E2;
                }

                set
                {
                    _E2 = value;
                }
            }

            [CategoryAttribute("SC Values"), DescriptionAttribute("Sec. Charge(uC)\n -65000    +65000")]
            public double Q2
            {
                get
                {
                    return _Q2;
                }

                set
                {
                    _Q2 = value;
                }
            }

            [CategoryAttribute("SC Values"), DescriptionAttribute("Quiescent time before start measuring(S)\n 0    2000")]
            [DisplayName("Equilibrium Time")]
            public double EquilibriumTime
            {
                get
                {
                    return _EquilibriumTime;
                }

                set
                {
                    _EquilibriumTime = value;
                }
            }

            [CategoryAttribute("SC Values"), DescriptionAttribute("vs OCP(True;False)")]
            [DisplayName("vs OCP")]
            public bool vs_OCP
            {
                get
                {
                    return _vs_OCP;
                }

                set
                {
                    _vs_OCP = value;
                }
            }

            [CategoryAttribute("SC Values"), DescriptionAttribute("Measurment of OCP Time(S)")]
            [DisplayName("OCP Measurment")]
            public double OCPmeasurment
            {
                get
                {
                    return _OCPmeasurment;
                }

                set
                {
                    _OCPmeasurment = value;
                }
            }
            [CategoryAttribute("SC Values"), DescriptionAttribute("0=Auto   1=0.4uA   2=4uA   3=40uA  \n              4=0.4mA  5=4mA  6=20mA ")]
            [DisplayName("Select Range ")]
            public double Range
            {
                get
                {
                    return _Range;
                }

                set
                {
                    _Range = value;
                }
            }
            public SC_params()
            {
                //
                // TODO: Add constructor logic here
                //
            }
        }
    */
    // LSV Properties *******************************************************************


    public class Cd_params
    {
        //private double _Cycles;
        private double _I1;
        private double _I2;
        private double _EUP;
        private double _EDO;
        private double _T1;
        private double _T2;
        private double _CYCLE;
        private double _EquilibriumTime;
        private string _comment1;

        [CategoryAttribute("Cd Values"), DescriptionAttribute("First current step(mA)\n -360    +360")]
        public double I1
        {
            get
            {
                return _I1;
            }

            set
            {
                _I1 = value;
            }
        }
        [CategoryAttribute("Cd Values"), DescriptionAttribute("Second current step(mA)\n -360    +360")]
        public double I2
        {
            get
            {
                return _I2;
            }
            set
            {
                _I2 = value;
            }
        }

        [CategoryAttribute("cycle Values"), DescriptionAttribute("cycle")]
        public double CYCLE
        {
            get
            {
                return _CYCLE;
            }

            set
            {
                _CYCLE = value;
            }
        }

            [CategoryAttribute("CD Values"), DescriptionAttribute("First step time(S)\n 1    32000")]
        public double T1
        {
            get
            {
                return _T1;
            }

            set
            {
                _T1 = value;
            }
        }
        [CategoryAttribute("CD Values"), DescriptionAttribute("First step time(S)\n 1    32000")]
        public double T2
        {
            get
            {
                return _T2;
            }

            set
            {
                _T2 = value;
            }
        }

        [CategoryAttribute("Cd Values"), DescriptionAttribute("Higger cutoff voltage(V)")]
        public double EUP
        {
            get
            {
                return _EUP;
            }

            set
            {
                _EUP = value;
            }
        }
        [CategoryAttribute("Cd Values"), DescriptionAttribute("lowwer cutoff voltage(V)")]
        public double EDO
        {
            get
            {
                return _EDO;
            }

            set
            {
                _EDO = value;
            }
        }

        [CategoryAttribute("Cd Values"), DescriptionAttribute("Quiescent time before start measuring(S)\n 0    2000")]
        [DisplayName("Equilibrium Time")]
        public double EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }

            set
            {
                _EquilibriumTime = value;
            }
        }
        [CategoryAttribute("Cd Values"), DescriptionAttribute("detail of expriment")]
        public string comment1
        {
            get
            {
                return _comment1;
            }

            set
            {
                _comment1 = value;
            }
        }
        public Cd_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    public partial class CCP_params
    {
        private double _CP1;
        private double _CP2;
        private double _CP3;
        private double _CP4;
        private double _CP5;
        private double _CT1;
        private double _CT2;
        private double _CT3;
        private double _CT4;
        private double _CT5;
        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Potential 1(V)")]
        public double CP1
        {
            get
            {
                return _CP1;
            }

            set
            {
                _CP1 = value;
            }
        }


        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Potential 2(V)")]
        public double CP2
        {
            get
            {
                return _CP2;
            }

            set
            {
                _CP2 = value;
            }
        }

        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Potential 3(V)")]
        public double CP3
        {
            get
            {
                return _CP3;
            }

            set
            {
                _CP3 = value;
            }
        }

        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Potential 4(V)")]
        public double CP4
        {
            get
            {
                return _CP4;
            }

            set
            {
                _CP4 = value;
            }
        }

        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Potential 5(V)")]
        public double CP5
        {
            get
            {
                return _CP5;
            }

            set
            {
                _CP5 = value;
            }
        }

        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Time 1(S)")]
        public double CT1
        {
            get
            {
                return _CT1;
            }

            set
            {
                _CT1 = value;
            }
        }

        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Time 2(S)")]
        public double CT2
        {
            get
            {
                return _CT2;
            }

            set
            {
                _CT2 = value;
            }
        }

        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Time 3(S)")]
        public double CT3
        {
            get
            {
                return _CT3;
            }

            set
            {
                _CT3 = value;
            }
        }

        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Time 4(S)")]
        public double CT4
        {
            get
            {
                return _CT4;
            }

            set
            {
                _CT4 = value;
            }
        }

        [CategoryAttribute("CCP Values"), DescriptionAttribute("Amount of Cond. Time 5(S)")]
        public double CT5
        {
            get
            {
                return _CT5;
            }

            set
            {
                _CT5 = value;
            }
        }
      
        public CCP_params()
        {

        }
    }
    [DefaultPropertyAttribute("BackGroundColor")]
    public class Color_params
    {
        //private CV2_params _AX;
        private Color _BackGround;
        //private Color _ForeColorButtons;
        //private Color _BackColorButtons;
        //private Color _ForeColorLable;
        private Color _ForeColorParameterGrid;
        private Color _BackColorParameterGrid;
        private Color _ForeColorDataGrid;
        private Color _BackColorDataGrid;
        private Color _ChartBackground;
        
        [CategoryAttribute("Color Values"), DescriptionAttribute("BackGround Color")]
        public Color BackGround
        {
            get
            {
                return _BackGround;
            }

            set
            {
                _BackGround = value;
            }
        }

        /*[CategoryAttribute("Color Values"), DescriptionAttribute("Buttons Color")]
        public Color ForeColorButtons
        {
            get
            {
                return _ForeColorButtons;
            }

            set
            {
                _ForeColorButtons = value;
            }
        }

        [CategoryAttribute("Color Values"), DescriptionAttribute("Back color of Buttons")]
        public Color BackColorButtons
        {
            get
            {
                return _BackColorButtons;
            }

            set
            {
                _BackColorButtons = value;
            }
        }

        [CategoryAttribute("Color Values"), DescriptionAttribute("ForeColor of Labels")]
        public Color ForeColorLabel
        {
            get
            {
                return _ForeColorLable;
            }

            set
            {
                _ForeColorLable = value;
            }
        }*/

        [CategoryAttribute("Color Values"), DescriptionAttribute("Parameter's Grid ForeColor")]
        public Color ForeColorParameterGrid
        {
            get
            {
                return _ForeColorParameterGrid;
            }

            set
            {
                _ForeColorParameterGrid = value;
            }
        }

        [CategoryAttribute("Color Values"), DescriptionAttribute("Parameter's Grid BackColor")]
        public Color BackColorParameterGrid
        {
            get
            {
                return _BackColorParameterGrid;
            }

            set
            {
                _BackColorParameterGrid = value;
            }
        }

        [CategoryAttribute("Color Values"), DescriptionAttribute("Data Grid's ForeColor")]
        public Color ForeColorDataGrid
        {
            get
            {
                return _ForeColorDataGrid;
            }

            set
            {
                _ForeColorDataGrid = value;
            }
        }

        [CategoryAttribute("Color Values"), DescriptionAttribute("Data Grid's BackColor")]
        public Color BackColorDataGrid
        {
            get
            {
                return _BackColorDataGrid;
            }

            set
            {
                _BackColorDataGrid = value;
            }
        }

        [CategoryAttribute("Color Values"), DescriptionAttribute("Chart Background Color")]
        public Color ChartBackground
        {
            get
            {
                return _ChartBackground;
            }

            set
            {
                _ChartBackground = value;
            }
        }
        public Color_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    //CA Properties  ****************************************************************
    public class FRA_params
    {
        private double _FirstFrequency;
        private double _SecondFrequency;
        private double _NumberPoints;
        private double _StepType;
        private double _Step;
        private double _V_DC;
        private double _V_AC;

        [CategoryAttribute("FRA Values"), DescriptionAttribute("First Frequency\n 1    1000")]
        public double FirstFrequency
        {
            get
            {
                return _FirstFrequency;
            }

            set
            {
                _FirstFrequency = value;
            }
        }

        [CategoryAttribute("FRA Values"), DescriptionAttribute("Second Frequency\n 0    +65000")]
        public double SecondFrequency
        {
            get
            {
                return _SecondFrequency;
            }

            set
            {
                _SecondFrequency = value;
            }
        }

        [CategoryAttribute("FRA Values"), DescriptionAttribute("Number of points between two frequency")]
        public double NumberPoints
        {
            get
            {
                return _NumberPoints;
            }

            set
            {
                _NumberPoints = value;
            }
        }

        [CategoryAttribute("FRA Values"), DescriptionAttribute("Type of calculation for Steps")]
        public double StepType
        {
            get
            {
                return _StepType;
            }

            set
            {
                _StepType = value;
            }
        }


        [CategoryAttribute("FRA Values"), DescriptionAttribute("Step of frequency")]
        public double Step
        {
            get
            {
                return _Step;
            }

            set
            {
                _Step = value;
            }
        }

        [CategoryAttribute("FRA Values"), DescriptionAttribute("Amount of DC voltage")]
        public double V_DC
        {
            get
            {
                return _V_DC;
            }

            set
            {
                _V_DC = value;
            }
        }

        [CategoryAttribute("FRA Values"), DescriptionAttribute("Amount of AC voltage")]
        public double V_AC
        {
            get
            {
                return _V_AC;
            }

            set
            {
                _V_AC = value;
            }
        }

        public FRA_params()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    public class cpct_params
    {
        private double _cp1;
        private double _ct1;
        private double _cp2;
        private double _ct2;
        private double _cp3;
        private double _ct3;

        [CategoryAttribute("cp/ct Values"), DescriptionAttribute("first potential precondition(V)\n -8    +8")]
        public double CP1
        {
            get
            {
                return _cp1;
            }

            set
            {
                _cp1 = value;
            }
        }
        [CategoryAttribute("cp/ct Values"), DescriptionAttribute("first time precondition(V)\n 0    1000")]
        public double CT1
        {
            get
            {
                return _ct1;
            }

            set
            {
                _ct1 = value;
            }
        }
        [CategoryAttribute("cp/ct Values"), DescriptionAttribute("second potential precondition(V)\n -8    +8")]
        public double CP2
        {
            get
            {
                return _cp2;
            }

            set
            {
                _cp2 = value;
            }
        }
        [CategoryAttribute("cp/ct Values"), DescriptionAttribute("second time precondition(V)\n 0    1000")]
        public double CT2
        {
            get
            {
                return _ct2;
            }

            set
            {
                _ct2= value;
            }
        }
        [CategoryAttribute("cp/ct Values"), DescriptionAttribute("last potential precondition(V)\n -8    +8")]
        public double CP3
        {
            get
            {
                return _cp3;
            }

            set
            {
                _cp3 = value;
            }
        }
        [CategoryAttribute("cp/ct Values"), DescriptionAttribute("last time precondition(V)\n 0    1000")]
        public double CT3
        {
            get
            {
                return _ct3;
            }

            set
            {
                _ct3 = value;
            }
        }


    }
}

