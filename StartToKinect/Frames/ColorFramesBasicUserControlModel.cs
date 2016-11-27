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
        private ColorFrameReader colorFrameReader = null;
        private WriteableBitmap colorBitmap = null;

        public ColorFramesBasicUserControlModel() {
            kinectSensor = KinectSensor.GetDefault();
        }

        public void Initialize() {
            colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();
            colorFrameReader.FrameArrived += Reader_FrameArrived;

            var frameDescription = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            colorBitmap = ColorFrameConverter.GetBitmapFromFrameDescription(frameDescription);

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
                return colorBitmap;
            }
        }

        private void Reader_FrameArrived(object sender, ColorFrameArrivedEventArgs e) {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame()) {
                if (colorFrame != null) {
                    ColorFrameConverter.FillFromFrame(colorBitmap, colorFrame);
                }
            }
        }
    }
}
