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
    public class DepthFrameConverter {

        private const int MapDepthToByte = 8000 / 256;
        private byte[] depthPixels = null;

        public WriteableBitmap GetBitmapFromFrameDescription(FrameDescription frameDescription) {
            this.depthPixels = new byte[frameDescription.Width * frameDescription.Height];

            return new WriteableBitmap(
                frameDescription.Width,
                frameDescription.Height, 96.0, 96.0,
                PixelFormats.Gray8, null
                );
        }

        public string FillFromFrame(WriteableBitmap bitmap, DepthFrame depthFrame) {
            FrameDescription frameDescription = depthFrame.FrameDescription;
            bool depthFrameProcessed = false;

            using (Microsoft.Kinect.KinectBuffer depthBuffer = depthFrame.LockImageBuffer()) {
                // verify data and write the color data to the display bitmap
                if (((frameDescription.Width * frameDescription.Height) == (depthBuffer.Size / frameDescription.BytesPerPixel)) &&
                    (frameDescription.Width == bitmap.PixelWidth) && (frameDescription.Height == bitmap.PixelHeight)) {
                    // Note: In order to see the full range of depth (including the less reliable far field depth)
                    // we are setting maxDepth to the extreme potential depth threshold
                    ushort maxDepth = ushort.MaxValue;

                    // If you wish to filter by reliable depth distance, uncomment the following line:
                    //// maxDepth = depthFrame.DepthMaxReliableDistance

                    this.ProcessDepthFrameData(frameDescription, depthBuffer.UnderlyingBuffer, depthBuffer.Size, depthFrame.DepthMinReliableDistance, maxDepth);
                    depthFrameProcessed = true;
                }
            }

            if (depthFrameProcessed) {
                RenderDepthPixels(bitmap);
            }

            return "";
        }

        private unsafe void ProcessDepthFrameData(FrameDescription frameDescription, IntPtr depthFrameData, uint depthFrameDataSize, ushort minDepth, ushort maxDepth) {
            // depth frame data is a 16 bit value
            ushort* frameData = (ushort*)depthFrameData;

            // convert depth to a visual representation
            for (int i = 0; i < (int)(depthFrameDataSize / frameDescription.BytesPerPixel); ++i) {
                // Get the depth for this pixel
                ushort depth = frameData[i];

                // To convert to a byte, we're mapping the depth value to the byte range.
                // Values outside the reliable depth range are mapped to 0 (black).
                this.depthPixels[i] = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / MapDepthToByte) : 0);
            }
        }

        /// <summary>
        /// Renders color pixels into the writeableBitmap.
        /// </summary>
        private void RenderDepthPixels(WriteableBitmap bitmap) {
            bitmap.WritePixels(
                new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight),
                this.depthPixels,
                bitmap.PixelWidth,
                0);
        }
    }
}
