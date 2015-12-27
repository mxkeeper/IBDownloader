using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IBDownloader
{
    public static class Utils
    {
        #region FindVisualChildren<T>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion

        #region AddQuoteMark(string input)
        public static string AddQuoteMark(string input)
        {
            var sb = new StringBuilder(input);
            sb.Insert(0, '"');
            sb.Insert(sb.Length, '"');
            return sb.ToString();
        }
        #endregion

        public static string HTTPtoHTTPS(string input)
        {
            return input.Replace("http","https");
        }
    }
}
