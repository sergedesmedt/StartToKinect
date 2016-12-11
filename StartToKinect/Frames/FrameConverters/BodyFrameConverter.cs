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
    public class BodyFrameConverter {

        private const double HandSize = 30;
        private const double JointThickness = 3;
        private const double ClipBoundsThickness = 10;
        private const float InferredZPositionClamp = 0.1f;

        private readonly Brush handClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));
        private readonly Brush handOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));
        private readonly Brush handLassoBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        private int displayWidth;
        private int displayHeight;

        private CoordinateMapper coordinateMapper = null;
        private List<Pen> bodyColors;

        private Body[] bodies = null;

        public BodyFrameConverter() {
            this.coordinateMapper = KinectSensor.GetDefault().CoordinateMapper;

            this.bodyColors = new List<Pen>();

            this.bodyColors.Add(new Pen(Brushes.Red, 6));
            this.bodyColors.Add(new Pen(Brushes.Orange, 6));
            this.bodyColors.Add(new Pen(Brushes.Green, 6));
            this.bodyColors.Add(new Pen(Brushes.Blue, 6));
            this.bodyColors.Add(new Pen(Brushes.Indigo, 6));
            this.bodyColors.Add(new Pen(Brushes.Violet, 6));
        }

        public DrawingGroup GetBitmapFromFrameDescription(FrameDescription frameDescription) {
            displayWidth = frameDescription.Width;
            displayHeight = frameDescription.Height;

            var drawingGroup = new DrawingGroup();
            drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, displayWidth, displayHeight));

            return drawingGroup;
        }

        public string FillFromFrame(DrawingGroup drawingGroup, BodyFrame bodyFrame) {
            if (this.bodies == null) {
                this.bodies = new Body[bodyFrame.BodyCount];
            }

            // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
            // As long as those body objects are not disposed and not set to null in the array,
            // those body objects will be re-used.
            bodyFrame.GetAndRefreshBodyData(this.bodies);

            using (DrawingContext dc = drawingGroup.Open()) {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));

                int penIndex = 0;
                foreach (Body body in this.bodies) {
                    Pen drawPen = this.bodyColors[penIndex++];

                    if (body.IsTracked) {
                        this.DrawClippedEdges(body, dc);

                        IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                        // convert the joint points to depth (display) space
                        Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                        foreach (JointType jointType in joints.Keys) {
                            // sometimes the depth(Z) of an inferred joint may show as negative
                            // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                            CameraSpacePoint position = joints[jointType].Position;
                            if (position.Z < 0) {
                                position.Z = InferredZPositionClamp;
                            }

                            DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                            jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
                        }

                        this.DrawBody(joints, jointPoints, dc, drawPen);

                        this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                        this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                    }
                }

                //// prevent drawing outside of our render area
                //this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));
            }

            return "";
        }

        /// <summary>
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="drawingPen">specifies color to draw a specific body</param>
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen) {
            // Draw the bones
            //foreach (var bone in this.bones) {
            //    this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
            //}

            // Draw the joints
            foreach (JointType jointType in joints.Keys) {
                Brush drawBrush = null;

                TrackingState trackingState = joints[jointType].TrackingState;

                if (trackingState == TrackingState.Tracked) {
                    drawBrush = this.trackedJointBrush;
                }
                else if (trackingState == TrackingState.Inferred) {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null) {
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        /// </summary>
        /// <param name="handState">state of the hand</param>
        /// <param name="handPosition">position of the hand</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawHand(HandState handState, Point handPosition, DrawingContext drawingContext) {
            switch (handState) {
                case HandState.Closed:
                    drawingContext.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Open:
                    drawingContext.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
                    break;
            }
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping body data
        /// </summary>
        /// <param name="body">body to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawClippedEdges(Body body, DrawingContext drawingContext) {
            FrameEdges clippedEdges = body.ClippedEdges;

            if (clippedEdges.HasFlag(FrameEdges.Bottom)) {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, this.displayHeight - ClipBoundsThickness, this.displayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Top)) {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, this.displayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Left)) {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, this.displayHeight));
            }

            if (clippedEdges.HasFlag(FrameEdges.Right)) {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(this.displayWidth - ClipBoundsThickness, 0, ClipBoundsThickness, this.displayHeight));
            }
        }
    }
}
