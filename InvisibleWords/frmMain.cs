using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvisibleWords
{
    public partial class frmMain : Form
    {
        readonly string filePath;

        readonly Random random;

        private Form formSettings { get; set; }

        private int showTime { get; set; }

        private int sleepTime { get; set; }

        private bool _isStop;

        private bool isStop
        {
            get
            {
                return _isStop;
            }
            set
            {
                _isStop = value;

                if (_isStop)
                {
                    clearAll();
                    stopToolStripMenuItem.Text = "Start";
                }
                else
                {
                    IsShowTime(true);
                    stopToolStripMenuItem.Text = "Stop";
                }
            }
        }

        private KeyValuePair<string, string> data { get; set; }

        public frmMain(string filePath)
        {
            InitializeComponent();

            this.filePath = filePath;

            random = new Random(DateTime.Now.Millisecond);

            init();
        }

        private void init()
        {
            clearAll();

            IsShowTime(true);
        }

        private void clearAll()
        {
            showTime = 0;
            sleepTime = 0;

            timerSleepTime.Stop();
            timerSleepTime.Enabled = false;

            timerShowTime.Stop();
            timerShowTime.Enabled = false;
        }

        private void showSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formSettings == null || formSettings.IsDisposed) formSettings = new frmSettings(filePath);

            formSettings.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timerShowTime_Tick(object sender, EventArgs e)
        {
            showTime++;

            if (showTime == 3) IsShowTime(false);
        }

        private void timerSleepTime_Tick(object sender, EventArgs e)
        {
            sleepTime++;

            if (sleepTime == 30) IsShowTime(true);
        }

        private void IsShowTime(bool value)
        {
            if (value)
            {
                if (Program.wordList != null && Program.wordList.Count != 0)
                {
                    data = Program.wordList[random.Next(0, Program.wordList.Count)];

                    lblKey.Text = data.Key;
                    lblValue.Text = data.Value;
                }

                panel.Visible = true;

                timerSleepTime.Stop();
                timerSleepTime.Enabled = false;

                timerShowTime.Start();
                timerShowTime.Enabled = true;

                sleepTime = 0;
            }
            else
            {
                panel.Visible = false;

                timerShowTime.Stop();
                timerShowTime.Enabled = false;

                timerSleepTime.Start();
                timerSleepTime.Enabled = true;

                showTime = 0;
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isStop)
            {
                isStop = false;
            }
            else
            {
                isStop = true;
            }
        }
    }
}
