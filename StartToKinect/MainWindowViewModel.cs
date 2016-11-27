using StartToKinect.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StartToKinect {
    public class MainWindowViewModel {

        public string WhayToDo { get; private set; } = "Wat te doen";

        public ICommand ShowColorFramesBasics { get; private set; } = new ShowColorFramesBasicSample();
    }
}
