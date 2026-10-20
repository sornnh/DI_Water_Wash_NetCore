using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public static class API
{
    public static ProcessCheckService processService;
    public static GoldenSampleService goldenService;
    public static ToolLifeTimeService toolLifeTimeService;
    public static CommonFunctionService commonFunctionService;
    public static ProductionLimitService productionLimitService;
    public static UpdateVersionService updateVersionService;
    public static void Initialize()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://10.102.4.20/")
        };
        processService = new ProcessCheckService(client);
        goldenService = new GoldenSampleService(client);
        toolLifeTimeService = new ToolLifeTimeService(client);
        commonFunctionService = new CommonFunctionService(client);
        updateVersionService = new UpdateVersionService(client);
        productionLimitService = new ProductionLimitService(client);
    }
}
