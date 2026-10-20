using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Cls_ScannerManager
{
    public static Cls_SmartCodeReader[] Scanner;
    public static void InitialScanner()
    {
        Scanner = new Cls_SmartCodeReader[4];
        for (int i = 0; i < 4; i++)
        {
            Scanner[i] = new Cls_SmartCodeReader($"192.168.0.1{i}");
        }
    }
}
