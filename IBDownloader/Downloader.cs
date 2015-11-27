using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IBDownloader
{
    public class Downloader
    {
        private MainWindow _MainWindow;
        private Options _Options;
        private string savePath;
        private int downloadedFilesCounter = 0;
        private string Aria2cPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\aria2c.exe";
        public string SavePath
        {
            get { return savePath; }
            set { savePath = value; }
        }

        public Downloader(MainWindow MainWindow, Options Options)
        {
            _MainWindow = MainWindow;
            _Options = Options;
        }

        private bool Aria2cExists()
        {
            return File.Exists(Aria2cPath);
        }

        public Task<bool> DownloadList(List<string> LinksList)
        {
            return Task.Run(() =>
            {
                if (Aria2cExists())
                {
                    string FileListPath = CreateFileList(LinksList);
                    var proc = new Process();
                    proc.StartInfo.FileName = Aria2cPath;
                    proc.StartInfo.Arguments =
                    "--max-connection-per-server=" + _Options.MaxConnectionPerServer +
                    " --max-concurrent-downloads=" + _Options.MaxConcurrentDownloads +
                    " --uri-selector=inorder " +
                    "--user-agent=Chrome/46.0.2490.86" +
                    "--check-certificate=false" +
                    "--conditional-get=true " +
                    "-d " + savePath +
                    " -i " + FileListPath;

                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    // set up output redirection
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.EnableRaisingEvents = true;
                    // see below for output handler
                    proc.ErrorDataReceived += proc_DataReceived;
                    proc.OutputDataReceived += proc_DataReceived;

                    proc.Start();

                    proc.BeginErrorReadLine();
                    proc.BeginOutputReadLine();

                    proc.WaitForExit();

                    File.Delete(FileListPath);
                    return true;
                }
                else
                {
                    MessageBox.Show("Не найден файл aria2c.exe в папке с программой" + Environment.NewLine + "Можно скачать отсюда: http://aria2.sourceforge.net/", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            });
        }

        private string CreateFileList(List<string> LinksList)
        {
            string FileListPath = Path.GetTempPath() + "ListLinks.txt";
            try
            {
                File.WriteAllLines(FileListPath, LinksList);
                return FileListPath;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                return String.Empty;
            }
        }

        private void proc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                if (e.Data.IndexOf("Download complete:", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    downloadedFilesCounter++;
                    _MainWindow.UpdateListView(downloadedFilesCounter);
                }
            }
        }
    }
}
