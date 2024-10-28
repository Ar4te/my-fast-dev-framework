using System.Diagnostics;
using System.IO.Compression;

namespace updater;

public class Program
{
    static void Main(string[] args)
    {
        Thread.Sleep(5000);
        string updateZipPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update.zip");
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

        if (File.Exists(updateZipPath))
        {
            try
            {
                ZipFile.ExtractToDirectory(updateZipPath, appDirectory, true);
                File.Delete(updateZipPath);
                Process.Start(Path.Combine(appDirectory, "desktop.exe"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("更新失败：" + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
