using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using IBDownloader.Parser;


namespace IBDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private const string msgInQueue = "В очереди";
        private const string msgInProgress = "Скачиваем";
        private const string msgSuccessful = "Завершено успешно";
        private const string msgAutoRefresh = "Автообновление";
        private const string msgError = "Ошибка скачивания";

        private DispatcherTimer dspTimer = new DispatcherTimer();
        private ProgressBar prbProgress;
        private List<Thread> _Threads = new List<Thread>();
        private int CurrentThreadProcessing = 0;
        private int LinksCount = 0;
        private bool IsDownloading = false;

        public Options Options = new Options();

        public List<Thread> Threads
        {
            get { return _Threads; }
            set
            {
                AddThreadToList();
                _Threads = value;
            }
        }

        private void ReStartTimer()
        {
            if (dspTimer.IsEnabled) dspTimer.Stop();

            Options.AutoUpdateInterval = (int)numAutoUpdateTime.Value;

            dspTimer.Tick += new EventHandler(dspTimer_Tick);
            dspTimer.Interval = new TimeSpan(0, Options.AutoUpdateInterval, 0);
            dspTimer.Start();
        }

        public MainWindow()
        {
            InitializeComponent();

            // Загружаем настройки
            Options.Load();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Options.AutoRefresh)
                chkAutoRefresh.IsEnabled = true;

            chkAutoRefresh.IsChecked = Options.AutoRefresh;
            numAutoUpdateTime.Value = Options.AutoUpdateInterval;
        }

        public void UpdateListView(int downloadedFilesCounter)
        {
            Threads[CurrentThreadProcessing].ProgressBarVal = downloadedFilesCounter;
            // Если все ссылки скачаны — устанавливаем статус
            if (downloadedFilesCounter == LinksCount)
            {
                if (Options.AutoRefresh)
                    Threads[CurrentThreadProcessing].Status = msgAutoRefresh;
                else
                    Threads[CurrentThreadProcessing].Status = msgSuccessful;
            }

            // Обращение к основному потоку
            this.Dispatcher.Invoke((Action)(() =>
            {
                lstViewURLs.Items[CurrentThreadProcessing] = new Thread()
                {
                    Link = Threads[CurrentThreadProcessing].Link,
                    OutputDir = Threads[CurrentThreadProcessing].OutputDir,
                    DownloadEntirePage = Threads[CurrentThreadProcessing].DownloadEntirePage,
                    Progress = Threads[CurrentThreadProcessing].ProgressBarVal + "/" + LinksCount,
                    Status = Threads[CurrentThreadProcessing].Status
                };
                prbProgress.Value = Threads[CurrentThreadProcessing].ProgressBarVal;
            }));
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DownloadThreads(Threads, sender, e);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void DownloadThreads(List<Thread> Threads, object sender, RoutedEventArgs e)
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
                    //if (Thread.Status != msgSuccessful)
                    //{
                    // Обновляем статус закачки
                    Threads[i].Status = msgInProgress;
                    // Извлекаем ссылки для скачивания
                    List<string> Links = await GetLinks(Thread);
                    // Находим нужный ProgressBar в колонке ListView
                    prbProgress = GetProgressBar(i);
                    // Установка максимального значения ProgressBar (кол-во ссылок для скачивания)
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        prbProgress.Maximum = LinksCount;
                    }));

                    Downloader Downloader = new Downloader(this, Options);
                    // Задаём папку для сохранения текущего треда, обрамляя путь C:\folder —> "C:\folder"
                    Downloader.SavePath = Utils.AddQuoteMark(Thread.OutputDir);

                    if (await Downloader.DownloadList(Links))
                    {
                        if (Options.AutoRefresh)
                            Threads[CurrentThreadProcessing].Status = msgAutoRefresh;
                        else
                            Threads[CurrentThreadProcessing].Status = msgSuccessful;
                    }
                    else
                        Threads[i].Status = msgError;
                    //}
                    i++;
                    CurrentThreadProcessing++;
                }
                IsDownloading = false;

                if (!Options.AutoRefresh)
                    UnlockButtons();
            }
            else
            {
                btnAddThreadURL_Click(sender, e);
            }
        }

        private async Task<List<string>> GetLinks(Thread Thread)
        {
            // Список ссылок для закачки
            List<string> Links = new List<string>();
            // Определяем тип борды по ссылке
            Board Board = AnalyzerLinks.Do(Thread.Link);
            // Получаем список ссылок для закачки
            switch (Board)
            {
                case Board.Arhivach:
                    Arhivach Arhivach = new Arhivach();
                    Links = await Arhivach.GetLinksToDownload(Thread.Link);
                    break;
                case Board.Dvach:
                    Dvach Dvach = new Dvach();
                    Links = await Dvach.GetLinksToDownload(Thread.Link);
                    break;
            }
            LinksCount = Links.Count;

            return Links;
        }

        private void dspTimer_Tick(object sender, EventArgs e)
        {
            DownloadThreads(Threads, sender, new RoutedEventArgs());
        }

        private void btnAddThreadURL_Click(object sender, RoutedEventArgs e)
        {
            AddThreadURL AddThreadURL = new AddThreadURL(this);
            AddThreadURL.Owner = this;
            AddThreadURL.Show();
        }

        private void btnRemoveThreadURL_Click(object sender, RoutedEventArgs e)
        {
            // Удаляем выделенные треды из списка
            if (lstViewURLs.SelectedItems.Count > 0)
            {
                foreach (var eachItem in lstViewURLs.SelectedItems)
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

        private void AddThreadToList()
        {
            lstViewURLs.Items.Add(new Thread()
            {
                Link = Threads[Threads.Count - 1].Link,
                OutputDir = Threads[Threads.Count - 1].OutputDir,
                DownloadEntirePage = Threads[Threads.Count - 1].DownloadEntirePage,
                Progress = "0/0",
                Status = msgInQueue
            });
        }

        private void BlockButtons()
        {
            btnDownload.IsEnabled = false;
            btnAddThreadURL.IsEnabled = false;
            btnRemoveThreadURL.IsEnabled = false;
            if (!Options.AutoRefresh)
                chkAutoRefresh.IsEnabled = false;
        }

        private void UnlockButtons()
        {
            btnDownload.IsEnabled = true;
            btnAddThreadURL.IsEnabled = true;
            btnRemoveThreadURL.IsEnabled = true;
            if (!Options.AutoRefresh)
                chkAutoRefresh.IsEnabled = true;
        }


        private void ClearAllURLs()
        {
            CurrentThreadProcessing = 0;
            LinksCount = 0;
            Threads.Clear();
            lstViewURLs.Items.Clear();
        }

        private ProgressBar GetProgressBar(int index)
        {
            return Utils.FindVisualChildren<ProgressBar>(lstViewURLs).ToArray()[index];
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProcessHelper.KillProcessByName("aria2c");

            if (!(bool)chkAutoRefresh.IsChecked)
                Options.AutoRefresh = false;
            Options.AutoUpdateInterval = (int)numAutoUpdateTime.Value;

            Options.Save();
        }

        private void chkAutoRefresh_Checked(object sender, RoutedEventArgs e)
        {
            btnAddThreadURL.IsEnabled = false;
            btnDownload.IsEnabled = false;
            btnRemoveThreadURL.IsEnabled = false;
            Options.AutoRefresh = true;

            numAutoUpdateTime.IsEnabled = false;
            ChangeStatusOnSuccessful(msgAutoRefresh);

            ChangeStatus(msgAutoRefresh);

            ReStartTimer();
        }

        private void chkAutoRefresh_Unchecked(object sender, RoutedEventArgs e)
        {
            btnAddThreadURL.IsEnabled = true;
            btnDownload.IsEnabled = true;
            btnRemoveThreadURL.IsEnabled = true;
            Options.AutoRefresh = false;

            numAutoUpdateTime.IsEnabled = true;
            ChangeStatusOnSuccessful(msgSuccessful);
            dspTimer.Stop();
        }

        private void ChangeStatusOnSuccessful(string status)
        {
            ChangeStatus(msgSuccessful);
            dspTimer.Stop();
        }

        private void ChangeStatus(string status)
        {
            for (int i = 0; i < Threads.Count; i++)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    lstViewURLs.Items[i] = new Thread()
                    {
                        Link = Threads[i].Link,
                        OutputDir = Threads[i].OutputDir,
                        DownloadEntirePage = Threads[i].DownloadEntirePage,
                        Progress = Threads[i].ProgressBarVal + "/" + Threads[i].ProgressBarVal,
                        Status = status
                    };
                    // Находим нужный ProgressBar в колонке ListView
                    prbProgress = GetProgressBar(i);
                    prbProgress.Value = Threads[i].ProgressBarVal;

                }));
            }
        }

        private void btnChangeStyle_Click(object sender, RoutedEventArgs e)
        {
            AppTheme AppTheme = new AppTheme();
            AppTheme.Owner = this;
            AppTheme.Show();
        }

        private void numAutoUpdateTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<System.Nullable<double>> e)
        {
            Options.AutoUpdateInterval = (int)numAutoUpdateTime.Value;
            if (Options.AutoRefresh)
                ReStartTimer();

        }
    }
}
