using System.Diagnostics;

namespace PlatformTools
{
    static public class Adb
    {
        static private Process process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.BaseDirectory + "adb",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        static public List<string>? GetDevices()
        {
            // > adb devices
            // "List of devices attached\n62d00e5a\tdevice\n\n"

            process.StartInfo.Arguments = "devices";
            process.Start();
            ReadOnlySpan<string> output = process.StandardOutput.ReadToEnd().Split("\n");
            process.WaitForExit();
            if (!output[0].Contains("List of devices attached"))
                return null;

            List<string> devices = new List<string>();

            for (int i = 0; i < output.Length - 1; i++)
                if (output[i + 1].Length > 7)
                    devices.Add(output[i + 1].Substring(0, output[i + 1].IndexOf("\t")));

            return devices;
        }

        static public List<string> GetDeviceIGeneralnfo(string deviceName)
        {
            /*
            - [ro.product.manufacturer]: [Xiaomi]
            - [ro.product.model]: [Redmi Note 10]
            - [ro.product.device]: [mojito]
            */
            string[] properties = { "ro.product.manufacturer", "ro.product.model", "ro.product.device" };
            List<string> values = new List<string>();
            foreach (var prop in properties)
            {
                // https://stackoverflow.com/a/21099766/12429279
                process.StartInfo.Arguments = $"-s {deviceName} shell getprop {prop}";
                process.Start();
                values.Add(process.StandardOutput.ReadToEnd().Split("\n")[0]);
                process.WaitForExit();
            }

            return values;
        }

        static public List<string> Get3rdApps(string deviceName)
        {
            List<string> values = new();
            process.StartInfo.Arguments = $"-s {deviceName} shell pm list packages -3";
            process.Start();
            values.AddRange(process.StandardOutput.ReadToEnd().Split("\n"));
            process.WaitForExit();
            values.RemoveAt(values.Count-1);
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = values[i].Substring(8);
            }
            return values;
        }

        static public void uninstallPackages(string deviceName, IEnumerable<string> packages)
        {
            foreach (string package in packages)
            {
                process.StartInfo.Arguments = $"-s {deviceName} shell pm uninstall {package}";
                process.Start();
                process.WaitForExit();
            }
        }
    }
}