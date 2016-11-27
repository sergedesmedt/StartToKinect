using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartToKinect.Frames.FrameConverters {
    public class ColorFrameConverter {
        public static WriteableBitmap GetBitmapFromFrameDescription(FrameDescription frameDescription) {
            return new WriteableBitmap(
                frameDescription.Width,
                frameDescription.Height, 96.0, 96.0,
                PixelFormats.Bgr32, null
                );
        }

        public static string FillFromFrame(WriteableBitmap bitmap, ColorFrame colorFrame) {
            FrameDescription colorFrameDescription = colorFrame.FrameDescription;

            var result = $"{colorFrameDescription.Width}, {colorFrameDescription.Height}, {colorFrameDescription.DiagonalFieldOfView}, {colorFrameDescription.HorizontalFieldOfView}, {colorFrameDescription.VerticalFieldOfView}";

            using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer()) {
                bitmap.Lock();

                // verify data and write the new color frame data to the display bitmap
                if ((colorFrameDescription.Width == bitmap.PixelWidth)
                    && (colorFrameDescription.Height == bitmap.PixelHeight)) {
                    colorFrame.CopyConvertedFrameDataToIntPtr(
                        bitmap.BackBuffer,
                        (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                        ColorImageFormat.Bgra);

                    bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                }

                bitmap.Unlock();
            }

            return result;
        }
    }
}
