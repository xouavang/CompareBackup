using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CompareBackups
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Models.BackupFile OldBackupFile { get; set; } = new Models.BackupFile();
        public Models.BackupFile NewBackupFile { get; set; } = new Models.BackupFile();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ChooseFile_Button_Click(object sender, RoutedEventArgs e)
        {
            var backupFile = ((Button)sender).Tag as Models.BackupFile;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                backupFile.Name = openFileDialog.SafeFileName;
                backupFile.Path = openFileDialog.FileName;
                backupFile.SplitHashAndFileName();
            }
        }

        private void Compare_Button_Click(object sender, RoutedEventArgs e)
        {
            var oldNotInNew = new Dictionary<string, string>();

            foreach (var item in OldBackupFile.Contents)
            {
                if (!NewBackupFile.Contents.Remove(item.Key))
                {
                    oldNotInNew.Add(item.Key, item.Value);
                }
            }

            SaveToFile("OldNotInNew.txt", oldNotInNew);
            SaveToFile("NewNotInOld.txt", NewBackupFile.Contents);

            MessageBox.Show("Compare Complete");

            ClearSelectedFiles();
        }

        /// <summary>
        /// Clears the selected files from the user interface.
        /// </summary>
        private void ClearSelectedFiles()
        {
            OldBackupFile.Clear();
            NewBackupFile.Clear();
        }

        /// <summary>
        /// Saves the <see cref="Dictionary{TKey, TValue}"/> to a text file.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="dict">The key value pair to be written to file</param>
        private void SaveToFile(string fileName, Dictionary<string, string> dict)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var entry in dict)
                {
                    writer.WriteLine("{0} {1}", entry.Key, entry.Value);
                }
            }
        }
    }
}
