using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartToKinect.Commands {
    class ShowColorFramesBasicSample : RelayCommand {
        public const string Message = "ShowColorFramesBasicSample";

        public ShowColorFramesBasicSample() : base(() => { Messenger.Default.Send(new NotificationMessage(Message)); }) {
        }
    }
}
