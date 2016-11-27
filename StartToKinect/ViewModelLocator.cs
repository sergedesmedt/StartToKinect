using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using StartToKinect.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartToKinect {
    class ViewModelLocator {
        static ViewModelLocator() {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ColorFramesBasicUserControlModel>();
        }

        public MainWindowViewModel MainWindowViewModel {
            get {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        public ColorFramesBasicUserControlModel ColorFramesBasicUserControlModel {
            get {
                return ServiceLocator.Current.GetInstance<ColorFramesBasicUserControlModel>();
            }
        }
    }
}
