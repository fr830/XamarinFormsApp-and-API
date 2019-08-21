using GalaSoft.MvvmLight.Command;
using Sagrada.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sagrada.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private string nombreString;

        public string NombreString {

            get { return this.nombreString; }
            set { SetValue(ref this.nombreString, value); }
        }
        //dsf
        public HomeViewModel() {

            nombreString = "BIENVENIDO/A, " + MainViewModel.GetInstance().User.FullName;
            
        }
    }
}
