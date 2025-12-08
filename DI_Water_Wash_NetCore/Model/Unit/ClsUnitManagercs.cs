using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public static class ClsUnitManagercs
    {
        public static ClsUnit[] cls_Units;

        public static void Initialize(string[] assyPN,string [] WorkOder,Form mainForm,string StationID)
        {
            cls_Units = new ClsUnit[assyPN.Length];
            for (int i = 0; i < assyPN.Length; i++)
            {
                cls_Units[i] = new ClsUnit(assyPN[i], WorkOder[i], mainForm, StationID,i);
            }
        }
    }

