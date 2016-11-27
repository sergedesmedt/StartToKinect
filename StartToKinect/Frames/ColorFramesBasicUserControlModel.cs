using GalaSoft.MvvmLight;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartToKinect.Frames {
    class ColorFramesBasicUserControlModel : ViewModelBase {

        private KinectSensor kinectSensor = null;
        private ColorFrameReader colorFrameReader = null;
        private WriteableBitmap colorBitmap = null;

        public ColorFramesBasicUserControlModel() {
            this.kinectSensor = KinectSensor.GetDefault();

            // open the reader for the color frames
            this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

            // wire handler for frame arrival
            this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;

            // create the colorFrameDescription from the ColorFrameSource using Bgra format
            FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

            // create the bitmap to display
            this.colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

            // set IsAvailableChanged event notifier
            //this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

        }

        string frameDescriptionInfo;
        public string FrameDescriptionInfo {
            get { return frameDescriptionInfo; }
            set {
                frameDescriptionInfo = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource {
            get {
                return this.colorBitmap;
            }
        }

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e) {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame()) {
                if (colorFrame != null) {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    FrameDescriptionInfo = $"{colorFrameDescription.Width}, {colorFrameDescription.Height}, {colorFrameDescription.DiagonalFieldOfView}, {colorFrameDescription.HorizontalFieldOfView}, {colorFrameDescription.VerticalFieldOfView}";

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer()) {
                        this.colorBitmap.Lock();

                        // verify data and write the new color frame data to the display bitmap
                        if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) && (colorFrameDescription.Height == this.colorBitmap.PixelHeight)) {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.colorBitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            this.colorBitmap.AddDirtyRect(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));
                        }

                        this.colorBitmap.Unlock();
                    }
                }
            }
        }
    }
}
