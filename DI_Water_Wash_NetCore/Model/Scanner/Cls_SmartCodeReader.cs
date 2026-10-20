using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Cls_SmartCodeReader
{
    private readonly string _ipAddress;
    private readonly int _triggerPort = 2001;
    private readonly int _dataPort = 2002;

    private readonly SemaphoreSlim _gate = new(1, 1);

    public Cls_SmartCodeReader(string ipAddress)
    {
        _ipAddress = ipAddress;
    }

    public async Task<(bool isOK,string? sCode)> TriggerAndReadAsync()
    {
        if (!await _gate.WaitAsync(0))
            return (false, null);
        try
        {
            using TcpClient dataClient = new TcpClient();
            using TcpClient triggerClient = new TcpClient();

            await ConnectWithTimeoutAsync(dataClient, _ipAddress, _dataPort, 3000);
            dataClient.ReceiveTimeout = 5000;
            dataClient.SendTimeout = 5000;

            using NetworkStream dataStream = dataClient.GetStream();

            await ConnectWithTimeoutAsync(triggerClient, _ipAddress, _triggerPort, 3000);
            triggerClient.ReceiveTimeout = 3000;
            triggerClient.SendTimeout = 3000;

            using NetworkStream triggerStream = triggerClient.GetStream();

            await SendTextAsync(triggerStream, "start");

            var result = await ReadBarcodeAsync(dataStream);

            if (!result.Success)
                return (false, null);
            if(result.Data!.Contains("NoRead"))
                return (false, result.Data!);
            return (true, result.Data!);
        }
        finally
        {
            _gate.Release();
        }
    }

    private static async Task ConnectWithTimeoutAsync(TcpClient client, string ip, int port, int timeoutMs)
    {
        using var cts = new CancellationTokenSource(timeoutMs);
        await client.ConnectAsync(ip, port, cts.Token);
    }

    private static async Task SendTextAsync(NetworkStream stream, string text)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(text);
        await stream.WriteAsync(bytes, 0, bytes.Length);
        await stream.FlushAsync();
    }

    private static async Task<(bool Success, string? Data, string? Error)> ReadBarcodeAsync(NetworkStream stream)
    {
        const int firstDataTimeoutMs = 3000;
        const int idleTimeoutMs = 500;
        const int maxTotalMs = 5000;
        const int bufferSize = 1024;

        byte[] buffer = new byte[bufferSize];
        using var ms = new MemoryStream();

        DateTime begin = DateTime.UtcNow;
        bool hasData = false;

        try
        {
            while (true)
            {
                int timeout = hasData ? idleTimeoutMs : firstDataTimeoutMs;

                using var cts = new CancellationTokenSource(timeout);

                int bytesRead;
                try
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    if (!hasData)
                        return (false, null, "No data received from scanner.");

                    break;
                }

                if (bytesRead <= 0)
                {
                    if (!hasData)
                        return (false, null, "Connection closed before receiving data.");

                    break;
                }

                hasData = true;
                await ms.WriteAsync(buffer, 0, bytesRead);

                if ((DateTime.UtcNow - begin).TotalMilliseconds > maxTotalMs)
                    break;

                byte[] raw = ms.ToArray();
                if (EndsWithTerminator(raw))
                    break;
            }

            if (ms.Length == 0)
                return (false, null, "Empty barcode data.");

            byte[] finalRaw = ms.ToArray();

            Console.WriteLine("HEX: " + BitConverter.ToString(finalRaw));
            Console.WriteLine("ASCII: " + Encoding.ASCII.GetString(finalRaw));

            string code = ParseBarcode(finalRaw);

            if (string.IsNullOrWhiteSpace(code))
                return (false, null, "Barcode is empty after parsing.");

            return (true, code, null);
        }
        catch (Exception ex)
        {
            return (false, null, $"Read error: {ex.Message}");
        }
    }

    private static bool EndsWithTerminator(byte[] data)
    {
        if (data.Length == 0) return false;

        byte last = data[^1];

        return last == 0x0D || // CR
               last == 0x0A || // LF
               last == 0x03 || // ETX
               last == 0x00;   // NUL
    }

    private static string ParseBarcode(byte[] rawData)
    {
        byte[] cleaned = rawData
            .Where(b => b != 0x02 && b != 0x03 && b != 0x0D && b != 0x0A && b != 0x00)
            .ToArray();

        return Encoding.ASCII.GetString(cleaned).Trim();
    }
}