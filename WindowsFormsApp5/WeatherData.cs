using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp5.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static WindowsFormsApp5.SettingForm;

namespace WindowsFormsApp5
{
    public partial class WeatherData : Form
    {
        private WeatherService _weatherService;
        private int _panelIndex = 0;
        private int _totalCities = 0;
        private WeatherSettings _settings;

        public WeatherData(WeatherSettings _weathersettings)
        {
            InitializeComponent();
            InitializeFormSize();
            this._settings = _settings;
            this._weatherService = new WeatherService(_weathersettings);

            if (_settings != null && _settings._cities != null && _settings._refreshTime != null)
            {
                _totalCities = _settings._cities.Count;
                timer1.Interval = (int)(_settings._refreshTime) * 1000;
            }

            RefreshWeatherInfo();

            timer1.Start();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            _panelIndex++;
            if (_panelIndex >= _totalCities)
            {
                _panelIndex = 0;
            }

            RefreshWeatherInfo();
        }

        private async void RefreshWeatherInfo()
        {
            if (_settings != null && _settings._cities != null && _settings._cities.Any())
            {
                string city = _settings._cities.ElementAt(_panelIndex);
                string weatherData = await _weatherService.GetWeatherData(city);

                WeatherInfo weatherInfo = ExtractWeatherData(weatherData);

                UpdateWeatherDisplay(city, weatherInfo.temperature, weatherInfo.humidity, weatherInfo.pressure);
            }
        }

        private WeatherInfo ExtractWeatherData(string weatherData)
        {
            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(weatherData);
            double temperature = json.main.temp;
            double humidity = json.main.humidity;
            double pressure = json.main.pressure;
            return new WeatherInfo(temperature, humidity, pressure);
        }

        private void UpdateWeatherDisplay(string city, double temperature, double humidity, double pressure)
        {
            label1.Text = city;
            lblHumidity.Text = String.Format(MyStrings.HumidityText, humidity);
            lblAtmosphere.Text = String.Format(MyStrings.AtmoshphereText, pressure);
            lblTemperature.Text = String.Format(MyStrings.TemperatureText, temperature);
        }

        private async void DisplaySettingForm()
        {
            SettingForm settingsForm = new SettingForm(_settings);

            settingsForm.Show(this);

            if (settingsForm._settings != null)
            {
                _weatherService = new WeatherService(settingsForm._settings);
                _settings = settingsForm._settings;

                if (_settings._cities != null && _settings._refreshTime != null)
                {
                    _totalCities = _settings._cities.Count;

                    timer1.Interval = (int)(_settings._refreshTime) * 1000;
                }

                RefreshWeatherInfo();
                timer1.Start();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            DisplaySettingForm();
        }

        private void InitializeFormSize()
        {
            this.Width = 700;
            this.Height = 550;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
        }

        private void WeatherData_Load(object sender, EventArgs e)
        {
            DisplaySettingForm();
        }
    }
}









