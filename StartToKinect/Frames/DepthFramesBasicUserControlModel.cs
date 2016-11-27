﻿using GalaSoft.MvvmLight;
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
    class DepthFramesBasicUserControlModel : ViewModelBase, IViewModelLifetime {

        private KinectSensor kinectSensor = null;
        private WriteableBitmap targetBitmap = null;

        private DepthFrameReader depthFrameReader = null;
        private DepthFrameConverter converter = new DepthFrameConverter();

        public void Initialize() {
            kinectSensor = KinectSensor.GetDefault();
            depthFrameReader = kinectSensor.DepthFrameSource.OpenReader();
            depthFrameReader.FrameArrived += Reader_FrameArrived;

            var frameDescription = kinectSensor.DepthFrameSource.FrameDescription;
            targetBitmap = converter.GetBitmapFromFrameDescription(frameDescription);

            kinectSensor.Open();
        }

        public void Destroy() {
            depthFrameReader.FrameArrived -= Reader_FrameArrived;
            depthFrameReader.Dispose();
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

        private void Reader_FrameArrived(object sender, DepthFrameArrivedEventArgs e) {
            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame()) {
                if (depthFrame != null) {
                    converter.FillFromFrame(targetBitmap, depthFrame);
                }
            }
        }
    }
}
