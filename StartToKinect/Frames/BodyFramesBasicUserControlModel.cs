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
    class BodyFramesBasicUserControlModel : ViewModelBase, IViewModelLifetime {

        private KinectSensor kinectSensor = null;
        private DrawingGroup drawingGroup = null;
        private DrawingImage targetBitmap = null;

        private BodyFrameReader bodyFrameReader = null;
        private BodyFrameConverter converter = new BodyFrameConverter();

        public void Initialize() {
            kinectSensor = KinectSensor.GetDefault();
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += Reader_FrameArrived;

            var frameDescription = kinectSensor.DepthFrameSource.FrameDescription;
            drawingGroup = converter.GetBitmapFromFrameDescription(frameDescription);
            targetBitmap = new DrawingImage(drawingGroup);

            kinectSensor.Open();
        }

        public void Destroy() {
            bodyFrameReader.FrameArrived -= Reader_FrameArrived;
            bodyFrameReader.Dispose();
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
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e) {
            using(var bodyFrame = e.FrameReference.AcquireFrame()) {
                if(bodyFrame != null) {
                    converter.FillFromFrame(drawingGroup, bodyFrame);
                }
            }
        }

    }
}
