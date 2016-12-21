using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StartToKinect.Frames {
    public abstract class KinectViewModelBase : ViewModelBase {

        public KinectViewModelBase() {
            InitializeCommand = new RelayCommand(() => { this.Initialize(); });
        }

        public ICommand InitializeCommand {
            get;
            private set;
        }

        public virtual void Initialize() {
            Sensor = KinectSensor.GetDefault();

            UniqueId = Sensor.UniqueKinectId;
            IsAudioCapable = Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Audio);
            IsExpressionsCapable = Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Expressions);
            IsFaceCapable = Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Face);
            IsGamechatCapable = Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Gamechat);
            IsNoneCapable = Sensor.KinectCapabilities.HasFlag(KinectCapabilities.None);
            IsVisionCapable = Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Vision);

            Sensor.IsAvailableChanged += Sensor_IsAvailableChanged;
        }

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e) {
            IsAvailable = Sensor.IsAvailable;
        }

        public virtual void Destroy() {
            Sensor?.Close();
        }

        public KinectSensor Sensor {
            get;
            private set;
        }

        string _uniqueId;
        public string UniqueId {
            get { return _uniqueId; }
            private set {
                _uniqueId = value;
                RaisePropertyChanged();
            }
        }

        bool _IsAvailable;
        public bool IsAvailable {
            get { return _IsAvailable; }
            private set {
                _IsAvailable = value;
                RaisePropertyChanged();
            }
        }

        private bool _isAudioCapable;
        public bool IsAudioCapable {
            get { return _isAudioCapable; }
            private set {
                _isAudioCapable = value;
                RaisePropertyChanged();
            }
        }

        private bool _isExpressionsCapable;
        public bool IsExpressionsCapable {
            get { return _isExpressionsCapable; }
            private set {
                _isExpressionsCapable = value;
                RaisePropertyChanged();
            }
        }

        private bool _isFaceCapable;
        public bool IsFaceCapable {
            get { return _isFaceCapable; }
            private set {
                _isFaceCapable = value;
                RaisePropertyChanged();
            }
        }

        private bool _isGamechatCapable;
        public bool IsGamechatCapable {
            get { return _isGamechatCapable; }
            private set {
                _isGamechatCapable = value;
                RaisePropertyChanged();
            }
        }

        private bool _isNoneCapable;
        public bool IsNoneCapable {
            get { return _isNoneCapable; }
            private set {
                _isNoneCapable = value;
                RaisePropertyChanged();
            }
        }

        private bool _isVisionCapable;
        public bool IsVisionCapable {
            get { return _isVisionCapable; }
            private set {
                _isVisionCapable = value;
                RaisePropertyChanged();
            }
        }
    }
}
