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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Forms = System.Windows.Forms;

namespace IBDownloader
{
    /// <summary>
    /// Interaction logic for AddThreadURL.xaml
    /// </summary>
    public partial class AddThreadURL : MetroWindow
    {
        private MainWindow _MainWindow;
        private Thread Thread = new Thread();
        //private static readonly object _object = new object();
        public AddThreadURL(MainWindow MainWindow)
        {
            InitializeComponent();
            _MainWindow = MainWindow;
        }

        private void btnAddThread_Click(object sender, RoutedEventArgs e)
        {
            if (AddThread())
                this.Close();
        }

        private bool AddThread()
        {
            SetOutputDir();

            if (ValidateURL() && IsSetOutputDir())
            {
                Thread.Link = txtURL.Text;

                if ((bool)chkFullThread.IsChecked)
                    Thread.DownloadEntirePage = "X";
                else
                    Thread.DownloadEntirePage = string.Empty;

                List<Thread> Threads = _MainWindow.Threads;
                Threads.Add(Thread);
                _MainWindow.Threads = Threads;
                return true;
            }
            else
            {
                MessageBox.Show("Проверьте поля ввода", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void btnOpenOutputDir_Click(object sender, RoutedEventArgs e)
        {
            if (!OpenFolderDialog())
            {
                MessageBox.Show("Не выбрана папка", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool ValidateURL()
        {
            string uriName = txtURL.Text;
            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private bool OpenFolderDialog()
        {
            using (Forms.FolderBrowserDialog fbd = new Forms.FolderBrowserDialog())
            {
                fbd.Description = "Выберите папку для сохранения";
                fbd.SelectedPath = _MainWindow.Options.LastFolder;
                if (fbd.ShowDialog() == Forms.DialogResult.OK)
                {
                    txtOutputDir.Text = fbd.SelectedPath;
                    _MainWindow.Options.LastFolder = fbd.SelectedPath;
                    return true;
                }
                else return false;
            }
        }


        private void SetOutputDir()
        {
            if (!String.IsNullOrEmpty(txtOutputDir.Text))
            {
                Thread.OutputDir = txtOutputDir.Text;
            }
        }

        private bool IsSetOutputDir()
        {
            if (String.IsNullOrEmpty(Thread.OutputDir) && String.IsNullOrEmpty(txtOutputDir.Text))
                return OpenFolderDialog();
            return true;
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            _MainWindow.Activate();
        }

        private void chkFullThread_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkFullThread_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
