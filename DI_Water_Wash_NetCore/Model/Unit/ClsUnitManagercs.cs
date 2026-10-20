using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ClsUnitManagercs
{
    public static ClsUnit[] cls_Units;
    public static string AssyPN { get; internal set; }
    public static bool IsInitSucess { get; internal set; }
    public static async Task Initialize(string assyPN,string WorkOder,Form mainForm,string StationID, string OP)
    {
        AssyPN = assyPN;
        cls_Units = new ClsUnit[3];
        for (int i = 0; i < cls_Units.Length; i++)
        {
            cls_Units[i] = new ClsUnit(assyPN, WorkOder, mainForm, StationID,i, OP);
            await cls_Units[i].clsSequenceTest.InitializeAsync();
        }
        IsInitSucess = true;
    }
}

