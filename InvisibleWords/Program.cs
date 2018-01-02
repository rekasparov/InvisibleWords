using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace InvisibleWords
{
    static class Program
    {
        public static List<KeyValuePair<string, string>> wordList { get; set; }

        private static string data { get; set; }

        static string filePath { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            for (int index = 0; index < args.Length; index++)
            {
                if (args[index] == "-p")
                {
                    filePath = args[index + 1];

                    break;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Dosya yolu için -p parametresi gereklidir!", "Invisible Words", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            ReadDataFromFile();

            Application.Run(new frmMain(filePath));
        }

        public static void ReadDataFromFile()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            else
            {
                StreamReader streamReader = new StreamReader(filePath);

                data = string.Empty;

                while (!streamReader.EndOfStream)
                {
                    data += streamReader.ReadLineAsync().Result;
                }

                streamReader.Close();
                streamReader.Dispose();

                if (!string.IsNullOrEmpty(data))
                {
                    try
                    {
                        Program.wordList = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(data);
                    }
                    catch
                    {
                        MessageBox.Show("Veri içeriği doğru değil veya okunamadı!", "Invisible Works", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
