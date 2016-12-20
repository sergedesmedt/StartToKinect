using GalaSoft.MvvmLight;
using Microsoft.Kinect;
using StartToKinect.Frames.FrameConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartToKinect.Frames {
    class DepthFramesBasicUserControlModel : KinectViewModelBase {

        private WriteableBitmap targetBitmap = null;

        private DepthFrameReader depthFrameReader = null;
        private DepthFrameConverter converter = new DepthFrameConverter();

        public void Initialize() {
            base.Initialize();

            depthFrameReader = Sensor.DepthFrameSource.OpenReader();
            depthFrameReader.FrameArrived += Reader_FrameArrived;

            var frameDescription = Sensor.DepthFrameSource.FrameDescription;
            targetBitmap = converter.GetBitmapFromFrameDescription(frameDescription);

            Sensor.Open();
        }

        public void Destroy() {
            depthFrameReader.FrameArrived -= Reader_FrameArrived;
            depthFrameReader.Dispose();

            base.Destroy();
        }

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource {
            get {
                return targetBitmap;
            }
        }

        private void Reader_FrameArrived(object sender, DepthFrameArrivedEventArgs e) {
            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame()) {
                if (depthFrame != null) {
                    converter.FillFromFrame(targetBitmap, depthFrame);
                }
            }
        }
    }
}
