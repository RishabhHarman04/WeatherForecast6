using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApp5.Properties;


namespace WindowsFormsApp5
{
    public partial class SettingForm : Form
    {
        public WeatherSettings _settings;

        public SettingForm(WeatherSettings _weathersettings)
        {
            InitializeComponent();
            InitializeFormSize();
            InitializeSettings(_weathersettings);
        }

        private void InitializeSettings(WeatherSettings _weathersettings)
        {
            if (_weathersettings != null)
            {
                _settings = _weathersettings;
            }
            else
            {
                _settings = new WeatherSettings();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateCitySelection())
            {
                return;
            }

            GetSettings();
            this.Close();
        }

        private WeatherSettings GetSettings()
        {
            _settings._cities = new List<string>();
            foreach (string item in checkedListBoxCities.CheckedItems)
            {
                _settings._cities.Add(item.ToString());
            }

            _settings._refreshTime = (int)numericUpDownTime.Value;
            return _settings;
        }

        private bool ValidateCitySelection()
        {
            if (checkedListBoxCities.CheckedItems.Count == 0)
            {
                MessageBox.Show(MyStrings.ValidationMessage);
                return false;
            }

            return true;
        }

        private void InitializeCities()
        {
            checkedListBoxCities.Items.Add(MyStrings.City1);
            checkedListBoxCities.Items.Add(MyStrings.City2);
            checkedListBoxCities.Items.Add(MyStrings.City3);
            checkedListBoxCities.Items.Add(MyStrings.City4);
            checkedListBoxCities.Items.Add(MyStrings.City5);
            checkedListBoxCities.CheckOnClick = true;

            ShowLastUserSelectionCity();
        }

        private void ShowLastUserSelectionCity()
        {
            if (_settings != null && _settings._cities != null)
            {
                foreach (string city in _settings._cities)
                {
                    int index = checkedListBoxCities.Items.IndexOf(city);
                    if (index >= 0)
                    {
                        checkedListBoxCities.SetItemChecked(index, true);
                    }
                }
            }
        }

        private void InitializeRefreshTime()
        {
            numericUpDownTime.Minimum = 5;
            numericUpDownTime.Maximum = 15;
            numericUpDownTime.Value = 5;
            numericUpDownTime.Increment = 5;

            ShowLastUserSelectionRefreshTime();
        }

        private void ShowLastUserSelectionRefreshTime()
        {
            if (_settings != null && _settings._refreshTime != null && _settings._refreshTime >= numericUpDownTime.Minimum && _settings._refreshTime <= numericUpDownTime.Maximum)
            {
                numericUpDownTime.Value = _settings._refreshTime;
            }
        }

        private void SettingForm_Load_1(object sender, EventArgs e)
        {
            InitializeCities();
            InitializeRefreshTime();
        }

        private void InitializeFormSize()
        {
            this.Width = 460;
            this.Height = 400;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
        }
    }
}





