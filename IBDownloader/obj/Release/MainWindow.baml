   M S B A M L   `   `   `  ���� G  CIBDownloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:IBDownloaderIBDownloader  J FMahApps.Metro, Version=1.1.3.234, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls     IBDownloader.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:IBDownloader     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     -   (  "MahApps.Metro.Controls.MetroWindow     IsMaxRestoreButtonEnabled  .   � Title$ ImageBoard Downloader��$��340.812��$��764.637�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ CanMinimize=�.��� .+��� -    ��Name$ lstViewURLs��$
��Left=�$	��202��$��	23,91,0,0q�$	��Top=�$	��714��$��1q� � 	Resources    DownloadEntirePage 
 Status  ProgressBar �Name � Value �� Path	 �� ElementName
 �� StringFormat��%T  &	        &	 :     &	 h     �� .R��� $��Center=����� *DownloadEntirePage+�� .R��� $��Center=����� *Status+�� .R�� .+�� $ prbProgress��$��0q� �� *ProgressBarVal+���� $ Width�$	 ProgressCell������ $ Height�$	 ProgressCell���� $��Center=����� *
Progress+$	
 {0}�� �� View �� P��� $	��URL�� �� Width$	 210�� �� DisplayMemberBinding �� *Link+�� $��
Папка��$	 240�� �� *	OutputDir+�� $��W��$ 30�� �� CellTemplate# [  �� $��Прогресс��$	 100��# [ �� $��Статус��$	 130��# [ �� -    ��Name$ btnAddThreadURL�� � ToolTip$3 -Добавить тред на закачку��$
��Left=�$��	23,44,0,0q�$	��Top=�$��42��$��42��.���� -    ��Name$ imgAddThreadURL��$"��Resources/1445442450_add.png���� -   $ btnRemoveThreadURL��$4 .Удалить выделенные треды��$
��Left=�$��	70,44,0,0q�$	��Top=�$��42��$��42��.���� -   $ imgRemoveThreadURL��$%��Resources/1445442458_delete.png���� -   $ btnDownload��$! Начать закачку��$
��Left=�$��
122,44,0,0q�$	��Top=�$��42��$��42��.���� .+��� -   $ imgDownload��$5��/Resources/1448648981_icon-92-inbox-download.png����  �� Orientation$ 
Horizontal=�$��409,7,308,225q�.+��� #��["��� -	    ��Name$ chkAutoRefresh��$/��)Автообновление каждые��$��Center=�$��410,11,135,263q�$	��168��*  $MahApps.Metro.Controls.NumericUpDown  -
     Name$ numAutoUpdateTime��   TextAlignment$ Center=�   Minimum$ 1Q�   Maximum$
 1440Q�   Interval$ 1Q�   Value$ 5X�$��601,17,84,268q�$��0q� J� RenderTransformOrigin$ 
0.57,0.536&��� -    ��Name$ lblAutoUpdateTime��$��мин��$
��Left=�$��
675,15,0,0q�$	��Top=��� -   $ btnChangeStyle��$��оформление��$
��Left=�$��
513,55,0,0q�$	��Top=�$	��136��$��19��$��10.667(�$
��Bold"�