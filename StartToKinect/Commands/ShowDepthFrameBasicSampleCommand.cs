﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartToKinect.Commands {
    class ShowDepthFrameBasicSampleCommand : RelayCommand {
        public const string Message = "ShowDepthFrameBasicSample";

        public ShowDepthFrameBasicSampleCommand() : base(() => { Messenger.Default.Send(new NotificationMessage(Message)); }) {
        }
    }
}
