using System;
using System.ComponentModel;

namespace AlarmClock.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _currentTime;
        private string _timeToWakeUp;
        private string _wakeUpSoundFileName;
        private string _wakeUpSoundFilePath;
        private DateTime? _wakeUpTime;

        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentTime"));
            }
        }

        public string TimeToWakeUp
        {
            get => _timeToWakeUp;
            set
            {
                _timeToWakeUp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeToWakeUp"));
            }
        }

        public string WakeUpSoundFileName
        {
            get => _wakeUpSoundFileName;
            set
            {
                _wakeUpSoundFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WakeUpSoundFileName"));
            }
        }
        public string WakeUpSoundFilePath
        {
            get => _wakeUpSoundFilePath;
            set
            {
                _wakeUpSoundFilePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WakeUpSoundFilePath"));
            }
        }

        public DateTime? WakeUpTime
        {
            get => _wakeUpTime;
            set
            {
                _wakeUpTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WakeUpTime"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
    }
}