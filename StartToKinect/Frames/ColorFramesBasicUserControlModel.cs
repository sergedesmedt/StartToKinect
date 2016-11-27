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
    class ColorFramesBasicUserControlModel : ViewModelBase, IViewModelLifetime {

        private KinectSensor kinectSensor = null;
        private WriteableBitmap targetBitmap = null;

        private ColorFrameReader colorFrameReader = null;

        public void Initialize() {
            kinectSensor = KinectSensor.GetDefault();

            colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();
            colorFrameReader.FrameArrived += Reader_FrameArrived;

            var frameDescription = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            targetBitmap = ColorFrameConverter.GetBitmapFromFrameDescription(frameDescription);

            kinectSensor.Open();
        }

        public void Destroy() {
            colorFrameReader.FrameArrived -= Reader_FrameArrived;
            colorFrameReader.Dispose();
            kinectSensor.Close();
        }

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource {
            get {
                return targetBitmap;
            }
        }

        private void Reader_FrameArrived(object sender, ColorFrameArrivedEventArgs e) {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame()) {
                if (colorFrame != null) {
                    ColorFrameConverter.FillFromFrame(targetBitmap, colorFrame);
                }
            }
        }
    }
}
