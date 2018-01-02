using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace InvisibleWords
{
    public partial class frmSettings : Form
    {
        readonly string filePath;

        private DataTable dataTable { get; set; }

        private DataRow dataRow { get; set; }

        public frmSettings(string filePath)
        {
            InitializeComponent();

            this.filePath = filePath;

            init();

            fillData();
        }

        private void init()
        {
            Program.wordList = new List<KeyValuePair<string, string>>();

            dataTable = new DataTable();

            dataTable.Columns.Add("Key", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));
        }

        private void fillData()
        {

            Program.ReadDataFromFile();

            dataTable.Clear();

            if (Program.wordList != null)
            {
                foreach (var word in Program.wordList)
                {
                    addNewRow(word.Key, word.Value);
                }
            }

            dataGridView.DataSource = dataTable;

            dataGridView.Rows[dataGridView.Rows.Count - 1].Selected = true;
        }

        private void saveData()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            else
            {
                File.WriteAllText(filePath, string.Empty);
            }

            StreamWriter streamWriter = new StreamWriter(filePath, true, Encoding.UTF8);

            Program.wordList.Clear();

            foreach (DataRow item in dataTable.Rows)
            {
                if (string.IsNullOrEmpty(item["Key"].ToString()) || string.IsNullOrEmpty(item["Value"].ToString())) continue;

                Program.wordList.Add(new KeyValuePair<string, string>(item["Key"].ToString(), item["Value"].ToString()));
            }

            streamWriter.WriteAsync(JsonConvert.SerializeObject(Program.wordList));

            streamWriter.Close();
            streamWriter.Dispose();

            fillData();
        }

        private void addNewRow(string key, string value)
        {
            dataRow = dataTable.NewRow();

            dataRow["Key"] = key;
            dataRow["Value"] = value;

            dataTable.Rows.Add(dataRow);
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N) addNewRow(string.Empty, string.Empty);

            else if (e.Control && e.KeyCode == Keys.S) saveData();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addNewRow(string.Empty, string.Empty);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
