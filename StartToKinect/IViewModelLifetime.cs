using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartToKinect {
    public interface IViewModelLifetime {
        void Initialize();
        void Destroy();
    }
}
