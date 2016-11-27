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
    class InfraredFrameConverter {

        private const float InfraredSourceValueMaximum = (float)ushort.MaxValue;

        /// <summary>
        /// The value by which the infrared source data will be scaled
        /// </summary>
        private const float InfraredSourceScale = 0.75f;

        /// <summary>
        /// Smallest value to display when the infrared data is normalized
        /// </summary>
        private const float InfraredOutputValueMinimum = 0.01f;

        /// <summary>
        /// Largest value to display when the infrared data is normalized
        /// </summary>
        private const float InfraredOutputValueMaximum = 1.0f;


        public WriteableBitmap GetBitmapFromFrameDescription(FrameDescription frameDescription) {
            return new WriteableBitmap(
                frameDescription.Width, frameDescription.Height, 96.0, 96.0, PixelFormats.Gray32Float, null);
        }

        public string FillFromFrame(WriteableBitmap bitmap, InfraredFrame infraredFrame) {
            FrameDescription frameDescription = infraredFrame.FrameDescription;

            using (Microsoft.Kinect.KinectBuffer infraredBuffer = infraredFrame.LockImageBuffer()) {
                // verify data and write the new infrared frame data to the display bitmap
                if (((frameDescription.Width * frameDescription.Height) == (infraredBuffer.Size / frameDescription.BytesPerPixel)) &&
                    (frameDescription.Width == bitmap.PixelWidth) && (frameDescription.Height == bitmap.PixelHeight)) {
                    this.ProcessInfraredFrameData(bitmap, frameDescription, infraredBuffer.UnderlyingBuffer, infraredBuffer.Size);
                }
            }

            return "";
        }

        /// <summary>
        /// Directly accesses the underlying image buffer of the InfraredFrame to 
        /// create a displayable bitmap.
        /// This function requires the /unsafe compiler option as we make use of direct
        /// access to the native memory pointed to by the infraredFrameData pointer.
        /// </summary>
        /// <param name="infraredFrameData">Pointer to the InfraredFrame image data</param>
        /// <param name="infraredFrameDataSize">Size of the InfraredFrame image data</param>
        private unsafe void ProcessInfraredFrameData(WriteableBitmap bitmap, FrameDescription frameDescription, IntPtr infraredFrameData, uint infraredFrameDataSize) {
            // infrared frame data is a 16 bit value
            ushort* frameData = (ushort*)infraredFrameData;

            // lock the target bitmap
            bitmap.Lock();

            // get the pointer to the bitmap's back buffer
            float* backBuffer = (float*)bitmap.BackBuffer;

            // process the infrared data
            for (int i = 0; i < (int)(infraredFrameDataSize / frameDescription.BytesPerPixel); ++i) {
                // since we are displaying the image as a normalized grey scale image, we need to convert from
                // the ushort data (as provided by the InfraredFrame) to a value from [InfraredOutputValueMinimum, InfraredOutputValueMaximum]
                backBuffer[i] = Math.Min(InfraredOutputValueMaximum, (((float)frameData[i] / InfraredSourceValueMaximum * InfraredSourceScale) * (1.0f - InfraredOutputValueMinimum)) + InfraredOutputValueMinimum);
            }

            // mark the entire bitmap as needing to be drawn
            bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

            // unlock the bitmap
            bitmap.Unlock();
        }
    }
}
