using GalaSoft.MvvmLight;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartToKinect.Frames {
    public abstract class KinectViewModelBase : ViewModelBase {

        public virtual void Initialize() {
            Sensor = KinectSensor.GetDefault();
            Sensor.IsAvailableChanged += Sensor_IsAvailableChanged;
        }

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e) {
            IsAvailable = Sensor.IsAvailable;
        }

        public virtual void Destroy() {
            Sensor.Close();
        }

        public KinectSensor Sensor {
            get;
            private set;
        }

        bool _IsAvailable;
        public bool IsAvailable {
            get { return _IsAvailable; }
            private set {
                _IsAvailable = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAudioCapable {
            get { return Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Audio); }
        }

        public bool IsExpressionsCapable {
            get { return Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Expressions); }
        }

        public bool IsFaceCapable {
            get { return Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Face); }
        }

        public bool IsGamechatCapable {
            get { return Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Gamechat); }
        }

        public bool IsNoneCapable {
            get { return Sensor.KinectCapabilities.HasFlag(KinectCapabilities.None); }
        }

        public bool IsVisionCapable {
            get { return Sensor.KinectCapabilities.HasFlag(KinectCapabilities.Vision); }
        }
    }
}
