using Sagrada.Dominio;
using Sagrada.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sagrada.ViewModels
{
    public class MainViewModel
    {
        public LoginViewModel Login { get; set; }

        public HomeViewModel Home { get; set; }
        
        public RegisterViewModel Register { get; set; }

        public MyProfileViewModel MyProfile { get; set; }

        public TurnoViewModel Turno { get; set; }

        public InformeViewModel Informe { get; set; }

        public PagoViewModel Pago { get; set; }

        public Usuario User { get; set; }

        public Medico Medico { get; set; }

        public TokenResponse Token { get; set; }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public ObservableCollection<PacientesMedicoItemViewModel> PacientesViejos { get; set; }

        public ObservableCollection<PacientesMedicoItemViewModel> PacientesNuevos { get; set; }

        public ObservableCollection<MisPacientesItemViewModel> PacientesAtendidos { get; set; }

        public ObservableCollection<PacientesMedicoItemViewModel> AllTurnos { get; set; }

        public ObservableCollection<PacientesMedicoItemViewModel> PacientesEnEspera { get; set; }

        private static MainViewModel instance;

        private MainViewModel() {
            instance = this;
            this.Login = new LoginViewModel();           
        }

        
        public static MainViewModel GetInstance() {

            if (instance == null) {
                instance = new MainViewModel();
            }

            return instance;
        }    
    }
}
