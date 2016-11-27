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
    class InfraredFramesBasicUserControlModel : ViewModelBase, IViewModelLifetime {

        private KinectSensor kinectSensor = null;
        private WriteableBitmap targetBitmap = null;

        private InfraredFrameReader infraredFrameReader = null;
        private InfraredFrameConverter converter = new InfraredFrameConverter();

        public void Initialize() {
            kinectSensor = KinectSensor.GetDefault();

            infraredFrameReader = kinectSensor.InfraredFrameSource.OpenReader();
            infraredFrameReader.FrameArrived += Reader_FrameArrived;

            var frameDescription = kinectSensor.InfraredFrameSource.FrameDescription;
            targetBitmap = converter.GetBitmapFromFrameDescription(frameDescription);

            kinectSensor.Open();
        }

        public void Destroy() {
            infraredFrameReader.FrameArrived -= Reader_FrameArrived;
            infraredFrameReader.Dispose();
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

        private void Reader_FrameArrived(object sender, InfraredFrameArrivedEventArgs e) {
            using (InfraredFrame infraredFrame = e.FrameReference.AcquireFrame()) {
                if (infraredFrame != null) {
                    converter.FillFromFrame(targetBitmap, infraredFrame);
                }
            }
        }
    }
}
