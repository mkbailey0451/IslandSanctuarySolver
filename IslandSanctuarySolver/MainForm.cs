using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IslandSanctuarySolver
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The user-configurable number of workshops on the island.
        /// </summary>
        public int Workshops
        {
            get
            {
                return _workshops;
            }
            set
            {
                _workshops = value;
                workshopTextBox.Text = _workshops.ToString();
            }
        }
        int _workshops;
        
        public static Resource? GetResource(string name)
        {
            foreach(Resource resource in Program.ResourceValues)
            {
                if(resource.ToString() == name)
                {
                    return resource;
                }
            }
            return null;
        }

        /// <summary>
        /// User-entered stocks of items, read from Isleventory.txt.
        /// </summary>
        int[] Stocks = new int[Program.ResourceValues.Length];

        /// <summary>
        /// Stocks normalized to quantities used in the calculator - i.e., divided by 5. Separate because the existing stocks are still displayed to the user.
        /// </summary>
        int[] SecretStocks = new int[Program.ResourceValues.Length];

        private void Form1_Load(object sender, EventArgs e)
        {
            if(!File.Exists("Isleventory.txt"))
            {
                return;
            }

            //read in Isleventory data
            foreach(string line in File.ReadAllLines("Isleventory.txt"))
            {
                string[] lineData = line.Split(':');

                if(lineData.Length != 2)
                {
                    continue;
                }

                string key = lineData[0];
                if (!int.TryParse(lineData[1].Trim(), out int value))
                {
                    continue;
                }

                if (key.Equals("Workshops", StringComparison.CurrentCultureIgnoreCase))
                {
                    Workshops = value;
                }
                else
                {
                    Resource? resource = GetResource(key);

                    if(resource != null)
                    {
                        Stocks[(int)resource] = value;
                        SecretStocks[(int)resource] = value / 5;
                    }
                }
            }

            //ensure we have enough primes for later hashing
            Primes.GetPrime(Program.ResourceValues.Length + 20);

            //precaclulate all presets
            Preset.AllPresets = Preset.GetAllPresets(SecretStocks);

            UpdateStockTextBox();
        }

        private void UpdateStockTextBox()
        {
            textBoxStocks.Clear();

            string text = "Workshops: " + _workshops + Environment.NewLine + Environment.NewLine;

            foreach (Resource resource in Program.ResourceValues)
            {
                if(resource == Resource.Popoto || resource == Resource.Fleece)
                {
                    text += Environment.NewLine;
                }
                text += resource.ToString() + ": " + Stocks[(int)resource] + Environment.NewLine;
            }

            textBoxStocks.Text = text;
        }

        private void FindBestStrategy()
        {
            switch(_workshops)
            {
                case 1:
                    Strategy.FindBestStrategy1(this, SecretStocks);
                    break;
                case 2:
                    Strategy.FindBestStrategy2(this, SecretStocks);
                    break;
                case 3:
                    Strategy.FindBestStrategy3(this, SecretStocks);
                    break;
                case 4:
                    Strategy.FindBestStrategy4(this, SecretStocks);
                    break;
                default:
                    MessageBox.Show("Unsupported number of workshops.");
                    break;
            }
            EnableFindStratsButton();
        }

        public void EnableFindStratsButton()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(EnableFindStratsButton), new object[] { });
                return;
            }

            findStratsButton.Enabled = true;
        }

        public void UpdateStrategyTextBox(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStrategyTextBox), new object[] { text });
                return;
            }

            strategyTextBox.Text = text;
        }

        public void UpdateCandidateLabel(long candidates, int bestCandidates)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<long, int>(UpdateCandidateLabel), new object[] { candidates, bestCandidates });
                return;
            }
            
            stratProgressLabel.Text = "Finding best strategy." + Environment.NewLine
                + candidates + " candidates tried. " + Environment.NewLine 
                + bestCandidates + " candidates exceeded previous ones.";
        }

        private void findStratsButton_Click(object sender, EventArgs e)
        {
            Thread strategyThread = new Thread(FindBestStrategy)
            {
                Priority = ThreadPriority.BelowNormal
            };
            strategyThread.Start();
            findStratsButton.Enabled = false;
        }

        private void workshopTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(workshopTextBox.Text, out int val))
            {
                workshopTextBox.Text = _workshops.ToString();
                return;
            }
            _workshops = val;
        }
    }
}
