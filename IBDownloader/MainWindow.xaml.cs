using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Collections;

namespace IBDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private const string msgSuccessful = "Завершено успешно";
        private const string msgInQueue = "В очереди";
        private const string msgInProgress = "Скачиваем";
        private const string msgError = "Ошибка скачивания";

        private ProgressBar prbProgress;
        private List<Thread> _Threads = new List<Thread>();
        private int CurrentThreadProcessing = 0;
        private int LinksCount = 0;
        private bool IsDownloading = false;

        public List<Thread> Threads
        {
            get { return _Threads; }
            set
            {
                AddThreadToList();
                _Threads = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            //chkAutoRefresh.Visibility = Visibility.Hidden;
            //chkFullThread.Visibility = Visibility.Hidden;
        }

        public void UpdateListView(int downloadedFilesCounter)
        {
            Threads[CurrentThreadProcessing].ProgressBarVal = downloadedFilesCounter;

            if (downloadedFilesCounter == LinksCount)
                Threads[CurrentThreadProcessing].Status = msgSuccessful;

            // Обращение к основному потоку
            this.Dispatcher.Invoke((Action)(() =>
            {
                lstViewURLs.Items[CurrentThreadProcessing] = new Thread()
                {
                    Link = Threads[CurrentThreadProcessing].Link,
                    OutputDir = Threads[CurrentThreadProcessing].OutputDir,
                    Progress = Threads[CurrentThreadProcessing].ProgressBarVal + "/" + LinksCount,
                    Status = Threads[CurrentThreadProcessing].Status
                };
                prbProgress.Value = Threads[CurrentThreadProcessing].ProgressBarVal;
            }));
        }

        private async void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstViewURLs.HasItems)
                {
                    int i = 0;
                    CurrentThreadProcessing = 0;
                    IsDownloading = true;
                    BlockButtons();
                    // Скачиваем каждый тред из списка
                    foreach (var Thread in Threads)
                    {
                        if (Thread.Status != msgSuccessful)
                        {
                            Threads[i].Status = msgInProgress;
                            Parser Parser = new Parser(false);
                            Downloader Downloader = new Downloader(this);
                            List<string> Links = await Parser.GetLinksToDownload(Thread.Link);
                            LinksCount = Links.Count;

                            // Установка максимального значения ProgressBar
                            prbProgress = GetProgressBar(i);
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                prbProgress.Maximum = LinksCount;
                            }));
                            // Задаём папку для сохранения текущего треда, обрамляя путь C:\folder —> "C:\folder"
                            Downloader.SavePath = Utils.AddQuoteMark(Thread.OutputDir);
                            if (await Downloader.DownloadList(Links))
                                Threads[i].Status = msgSuccessful;
                            else
                                Threads[i].Status = msgError;
                        }
                        i++;
                        CurrentThreadProcessing++;
                    }
                    IsDownloading = false;
                    UnlockButtons();
                }
                else
                {
                    btnAddThreadURL_Click(sender, e);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnAddThreadURL_Click(object sender, RoutedEventArgs e)
        {
            AddThreadURL AddThreadURL = new AddThreadURL(this);
            AddThreadURL.Owner = this;
            AddThreadURL.Show();
        }

        private void AddThreadToList()
        {
            lstViewURLs.Items.Add(new Thread()
            {
                Link = Threads[Threads.Count - 1].Link,
                OutputDir = Threads[Threads.Count - 1].OutputDir,
                Progress = "0/0",
                Status = msgInQueue
            });
        }

        private void BlockButtons()
        {
            btnDownload.IsEnabled = false;
            btnAddThreadURL.IsEnabled = false;
            btnRemoveThreadURL.IsEnabled = false;
        }

        private void UnlockButtons()
        {
            btnDownload.IsEnabled = true;
            btnAddThreadURL.IsEnabled = true;
            btnRemoveThreadURL.IsEnabled = true;
        }


        private void ClearAllURLs()
        {
            CurrentThreadProcessing = 0;
            LinksCount = 0;
            Threads.Clear();
            lstViewURLs.Items.Clear();
        }

        private void btnRemoveThreadURL_Click(object sender, RoutedEventArgs e)
        {
            // Удаляем выделенные треды из списка
            if (lstViewURLs.SelectedItems.Count > 0)
            {
                foreach (ListViewItem eachItem in lstViewURLs.SelectedItems)
                {
                    lstViewURLs.Items.Remove(eachItem);
                    int index = lstViewURLs.Items.IndexOf(eachItem);
                    Threads.RemoveAt(index);
                }
            }
            
            else
            {
                //ClearAllURLs();
            }
        }

        private ProgressBar GetProgressBar(int index)
        {
            return Utils.FindVisualChildren<ProgressBar>(lstViewURLs).ToArray()[index];
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProcessHelper.KillProcessByName("aria2c");
        }
    }
}
