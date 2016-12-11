using StartToKinect.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StartToKinect {
    public class MainWindowViewModel {

        public ICommand ShowColorFramesBasics { get; private set; } = new ShowColorFramesBasicSampleCommand();

        public ICommand ShowDepthFramesBasics { get; private set; } = new ShowDepthFrameBasicSampleCommand();

        public ICommand ShowInfrafredFramesBasics { get; private set; } = new ShowInfraredFrameBasicSampleCommand();

        public ICommand ShowBodyFramesBasics { get; private set; } = new ShowBodyFramesBasicSampleCommand();
    }
}
