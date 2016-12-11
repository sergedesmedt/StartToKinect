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

        IViewModelLifetime currentViewModel;

        static ViewModelLocator() {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ColorFramesBasicUserControlModel>();
            SimpleIoc.Default.Register<DepthFramesBasicUserControlModel>();
            SimpleIoc.Default.Register<InfraredFramesBasicUserControlModel>();
            SimpleIoc.Default.Register<BodyFramesBasicUserControlModel>();
        }

        public MainWindowViewModel MainWindowViewModel {
            get {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        public ColorFramesBasicUserControlModel ColorFramesBasicUserControlModel {
            get {
                if (currentViewModel != null) {
                    currentViewModel.Destroy();
                }
                var vm = ServiceLocator.Current.GetInstance<ColorFramesBasicUserControlModel>();
                vm.Initialize();
                currentViewModel = vm;
                return vm;
            }
        }

        public DepthFramesBasicUserControlModel DepthFramesBasicUserControlModel {
            get {
                if (currentViewModel != null) {
                    currentViewModel.Destroy();
                }
                var vm = ServiceLocator.Current.GetInstance<DepthFramesBasicUserControlModel>();
                vm.Initialize();
                currentViewModel = vm;
                return vm;
            }
        }

        public InfraredFramesBasicUserControlModel InfraredFramesBasicUserControlModel {
            get {
                if (currentViewModel != null) {
                    currentViewModel.Destroy();
                }
                var vm = ServiceLocator.Current.GetInstance<InfraredFramesBasicUserControlModel>();
                vm.Initialize();
                currentViewModel = vm;
                return vm;
            }
        }

        public BodyFramesBasicUserControlModel BodyFramesBasicUserControlModel {
            get {
                if (currentViewModel != null) {
                    currentViewModel.Destroy();
                }
                var vm = ServiceLocator.Current.GetInstance<BodyFramesBasicUserControlModel>();
                vm.Initialize();
                currentViewModel = vm;
                return vm;
            }
        }
    }
}
