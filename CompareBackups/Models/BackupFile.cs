using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace CompareBackups.Models
{
    public class BackupFile : INotifyPropertyChanged
    {
        private const int SHA1_CHAR_COUNT = 40;

        private string _name;
        private string _path;

        /// <summary>
        /// Gets or sets the backup file's name.
        /// </summary>
        public string Name
        {
            get => _name; set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the backup file's full file path.
        /// </summary>
        public string Path
        {
            get => _path; set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }

        /// <summary>
        /// Gets the collection of keys and values, representing the SHA-1 Hash and File Name, respectively.
        /// </summary>
        public Dictionary<string, string> Contents { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Read each line in the text file and splits the context into their SHA-1 Hash and file name.
        /// </summary>
        public void SplitHashAndFileName()
        {
            Contents = new Dictionary<string, string>();

            using (StreamReader sr = File.OpenText(Path))
            {
                string s = string.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    string[] subs = s.Split(null, 2);
                    if (subs.Length > 1 && subs[0].Length == SHA1_CHAR_COUNT)
                    {
                        if (!Contents.ContainsKey(subs[0]))
                        {
                            Contents.Add(subs[0], subs[1]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears the object properties.
        /// </summary>
        public void Clear()
        {
            Name = string.Empty;
            Path = string.Empty;
            Contents = new Dictionary<string, string>();
        }
    }
}
