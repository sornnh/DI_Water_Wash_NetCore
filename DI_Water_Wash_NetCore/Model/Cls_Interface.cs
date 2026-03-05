using log4net;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Cls_Interface
{
    public static Cls_ASPcontrol cls_ASPcontrol;
    private static readonly ILog log = LogManager.GetLogger("ASPBoardLogger");
    public delegate void DelShow_UpdateAnalog_Request();
    public static event DelShow_UpdateAnalog_Request OnRequestUpdateAnalog;
    public static void Initialize()
    {
        cls_ASPcontrol = new Cls_ASPcontrol();
    }
    public static async Task PollScanAnalog(CancellationToken token,int ticks)
    {
        while (!token.IsCancellationRequested)
        {
            if(!StateCommon.bAuto) 
            {
                await Task.Delay(ticks);
                continue;
            }
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    await cls_ASPcontrol.GetADIAsync(i + 1);
                    await Task.Delay(100);
                    await cls_ASPcontrol.GetADVAsync(i + 1);
                    await Task.Delay(100);
                }
                for (int i = 0; i < 8; i++)
                {
                    await cls_ASPcontrol.GetADCAsync(i + 1);
                    await Task.Delay(100);
                }
                OnRequestUpdateAnalog?.Invoke();
            }
            catch (Exception ex)
            {
                log.Error("Error in PollScanAnalog: " + ex.Message);
            }
            await Task.Delay(ticks); // Poll every second
        }
    }
    public static async void SetAllRelayTurnOff()
    {
        await cls_ASPcontrol.SetAllRelayOFF();
    }
}

