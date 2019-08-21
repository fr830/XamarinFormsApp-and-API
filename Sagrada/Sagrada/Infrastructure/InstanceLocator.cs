using Sagrada.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sagrada.Infrastructure {

    public class InstanceLocator {

        public MainViewModel Main { get; set; }

        public InstanceLocator() {

            this.Main = MainViewModel.GetInstance();
        }
    }
}
