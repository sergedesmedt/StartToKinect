using GalaSoft.MvvmLight.Messaging;
using StartToKinect.Frames;
using StartToKinect.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartToKinect {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        private void NotificationMessageReceived(NotificationMessage msg) {
            if (msg.Notification == ShowColorFramesBasicSampleCommand.Message) {
                this.SampleHolder.Content = new ColorFramesBasicUserControl();
            }
            if (msg.Notification == ShowDepthFrameBasicSampleCommand.Message) {
                this.SampleHolder.Content = new DepthFramesBasicUserControl();
            }
            if (msg.Notification == ShowInfraredFrameBasicSampleCommand.Message) {
                this.SampleHolder.Content = new InfraredFramesBasicUserControl();
            }
            if (msg.Notification == ShowBodyFramesBasicSampleCommand.Message) {
                this.SampleHolder.Content = new BodyFramesBasicUserControl();
            }
        }
    }
}
