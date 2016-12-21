using GalaSoft.MvvmLight;
using Microsoft.Kinect;
using StartToKinect.Frames.FrameConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartToKinect.Frames {
    class ColorFramesBasicUserControlModel : KinectViewModelBase {

        private KinectCapabilities kinectCapabilities;
        private WriteableBitmap targetBitmap = null;

        private ColorFrameReader colorFrameReader = null;

        public override void Initialize() {
            base.Initialize();

            colorFrameReader = Sensor.ColorFrameSource.OpenReader();
            colorFrameReader.FrameArrived += Reader_FrameArrived;

            var frameDescription = Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            targetBitmap = ColorFrameConverter.GetBitmapFromFrameDescription(frameDescription);
            RaisePropertyChanged("ImageSource");

            Sensor.Open();
        }

        public void Destroy() {
            colorFrameReader.FrameArrived -= Reader_FrameArrived;
            colorFrameReader.Dispose();

            base.Destroy();
        }

        public ImageSource ImageSource {
            get { return targetBitmap; }
        }

        string _colorFrameRelativeTime;
        public string ColorFrameRelativeTime {
            get { return _colorFrameRelativeTime; }
            set {
                _colorFrameRelativeTime = value;
                RaisePropertyChanged();
            }
        }

        private void Reader_FrameArrived(object sender, ColorFrameArrivedEventArgs e) {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame()) {
                if (colorFrame != null) {
                    ColorFrameRelativeTime = colorFrame.RelativeTime.ToString();
                    ColorFrameConverter.FillFromFrame(targetBitmap, colorFrame);
                }
            }
        }
    }
}
