using Microsoft.Win32;
using System;
using System.Media;
using System.Windows;
using System.Windows.Threading;

namespace AlarmClock.Views
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindowView
    {
        private readonly MainWindowViewModel _viewModel;
        private DispatcherTimer _dispatcherTimer;
        private SoundPlayer _soundPlayer;

        public MainWindowView()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            UpdateCurrentTime();
            StartDispatcherTimer();
        }

        private void UpdateCurrentTime()
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            _viewModel.CurrentTime = now.ToString("T");

            if (_viewModel.WakeUpTime != null)
            {
                if (_viewModel.WakeUpTime >= now)
                {
                    TimeSpan duration = (TimeSpan)(_viewModel.WakeUpTime - now);
                    _viewModel.TimeToWakeUp = duration.ToString(@"hh\:mm\:ss");

                    if (_viewModel.WakeUpTime == now)
                        _soundPlayer.PlayLooping();
                }
            }
        }

        private void StartDispatcherTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += UpdateCurrentTimeTick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        private void UpdateCurrentTimeTick(object sender, object e) => UpdateCurrentTime();

        private void UpdateControlEnablingFromRadioButtons()
        {
            if (TimeToWakeUpRadioButton == null)
                return;

            if (TimeToWakeUpRadioButton.IsChecked == true)
            {
                TimeToWakeUpTextBox.IsEnabled = true;
                WakeUpTimeTextBox.IsEnabled = false;
            }
            else
            {
                TimeToWakeUpTextBox.IsEnabled = false;
                WakeUpTimeTextBox.IsEnabled = true;
            }
        }

        private void WakeUpTimeRadioButton_Checked(object sender, RoutedEventArgs e) => UpdateControlEnablingFromRadioButtons();

        private void TimeToWakeUpRadioButton_Checked(object sender, RoutedEventArgs e) => UpdateControlEnablingFromRadioButtons();

        private void StartAlarmClock()
        {
            if (string.IsNullOrEmpty(_viewModel.WakeUpSoundFilePath))
            {
                _ = MessageBox.Show("No sound file choosen", "Error", MessageBoxButton.OK);
                return;
            }

            if (TimeToWakeUpRadioButton.IsChecked == true)
            {
                try
                {
                    string[] timeParts = TimeToWakeUpTextBox.Text.Split(':');
                    int hour = int.Parse(timeParts[0]);
                    int min = int.Parse(timeParts[1]);

                    if (hour < 0 || hour > 23 || min < 0 || min > 59)
                        throw new InvalidOperationException();

                    DateTime now = DateTime.Now;
                    DateTime wakeUpTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

                    wakeUpTime = wakeUpTime.AddHours(hour);
                    wakeUpTime = wakeUpTime.AddMinutes(min);

                    _viewModel.WakeUpTime = wakeUpTime;
                }
                catch (Exception)
                {
                    _ = MessageBox.Show("Invalid time to wake-up - set between 0:0 and 23:59", "Error", MessageBoxButton.OK);
                    return;
                }
            }
            else
            {
                try
                {
                    string[] timeParts = WakeUpTimeTextBox.Text.Split(':');
                    int hour = int.Parse(timeParts[0]);
                    int min = int.Parse(timeParts[1]);
                    DateTime now = DateTime.Now;
                    DateTime wakeUpTime = new DateTime(now.Year, now.Month, now.Day, hour, min, 0);

                    if (wakeUpTime < now)
                        wakeUpTime = wakeUpTime.AddDays(1);

                    _viewModel.WakeUpTime = wakeUpTime;
                }
                catch (Exception)
                {
                    _ = MessageBox.Show("Invalid wake-up time - set between 0:0 and 23:59", "Error", MessageBoxButton.OK);
                    return;
                }
            }

            TimeToWakeUpRadioButton.IsEnabled = false;
            TimeToWakeUpTextBox.IsEnabled = false;
            WakeUpTimeRadioButton.IsEnabled = false;
            WakeUpTimeTextBox.IsEnabled = false;
            StartButton.IsEnabled = false;
            ChooseSoundFileButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e) => StartAlarmClock();

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _soundPlayer.Stop();
            StopButton.IsEnabled = false;
            _viewModel.WakeUpTime = null;
            _viewModel.TimeToWakeUp = string.Empty;
            TimeToWakeUpRadioButton.IsEnabled = true;
            TimeToWakeUpTextBox.IsEnabled = TimeToWakeUpRadioButton.IsChecked == true;
            WakeUpTimeRadioButton.IsEnabled = true;
            WakeUpTimeTextBox.IsEnabled = WakeUpTimeRadioButton.IsChecked == true;
            ChooseSoundFileButton.IsEnabled = true;
            StartButton.IsEnabled = true;
        }

        private void ChooseSoundFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "WAV files (*.wav)|*.wav"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                _viewModel.WakeUpSoundFilePath = openFileDialog.FileName;
                _viewModel.WakeUpSoundFileName = System.IO.Path.GetFileName(_viewModel.WakeUpSoundFilePath);
                _soundPlayer = new SoundPlayer(_viewModel.WakeUpSoundFilePath);
            }
        }

        private void ChooseSoundFileButton_Click(object sender, RoutedEventArgs e) => ChooseSoundFile();
    }
}