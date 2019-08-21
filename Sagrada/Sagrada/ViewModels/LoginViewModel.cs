using GalaSoft.MvvmLight.Command;
using Sagrada.Dominio;
using Sagrada.Models;
using Sagrada.Services;
using Sagrada.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sagrada.ViewModels {

    public class LoginViewModel : BaseViewModel {

        private ApiService apiService;

        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        public string Email {

            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password {

            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunning {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled {

            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRemembered { get; set; }

        public ICommand LoginCommand { get { return new RelayCommand(Login); } }

        public ICommand RegisterCommand { get { return new RelayCommand(Register); } }



        public LoginViewModel() {

            this.apiService = new ApiService();
            this.IsRemembered = true;
            this.IsEnabled = true;


            //this.Email = "maria@hotmail.com";
            //this.Password = "123456";
        }

        private async void Login() {



            if (string.IsNullOrEmpty(this.Email)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar un correo",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Password)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar una contraseña",
                    "Aceptar");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            // var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var token = await this.apiService.GetToken(
                "http://sagradaapi.azurewebsites.net/",
                this.Email,
                this.Password);

            if (token == null) {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Algo salió mal, intente más tarde",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken)) {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    token.ErrorDescription,
                    "Aceptar");
                this.Password = string.Empty;
                return;
            }

            var user = await this.apiService.GetUserByEmail(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/Usuarios/GetUserByEmail",
                token.TokenType,
                token.AccessToken,
                this.Email);

            /*  var userLocal = Converter.ToUserLocal(user);
              userLocal.Password = this.Password; */

            var mainViewModel = MainViewModel.GetInstance();

            mainViewModel.Token = token;
            mainViewModel.User = user;

            if (MainViewModel.GetInstance().User.IdTipoUsuario == 3) {

                var busquedaMedico = await this.apiService.Get<Medico>(
                    "http://sagradaapi.azurewebsites.net",
                    "/api",
                    "/Medicos/GetMedicoByDni",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken,
                    user.DNI);

                if (!busquedaMedico.IsSuccess) {

                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaMedico.Message,
                        "Aceptar");
                    return;
                }

                MainViewModel.GetInstance().Medico = (Medico)busquedaMedico.Result;

            }

                /*  if (this.IsRemembered) {
                      Settings.IsRemembered = "true";
                  } else {
                      Settings.IsRemembered = "false";
                  }

                  this.dataService.DeleteAllAndInsert(userLocal);
                  this.dataService.DeleteAllAndInsert(token);
                  */


            MainViewModel.GetInstance().Menus = new ObservableCollection<MenuItemViewModel>();

            /*   MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                   Icon = "ic_settings.png",
                   PageName = "MyProfilePage",
                   Title = "Mi Perfil",
               }); */

            if (MainViewModel.GetInstance().User.IdTipoUsuario == 1) {

                MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                    Icon = "ic_control_point.png",
                    PageName = "TurnoPage",
                    Title = "Solicitar un turno",
                });

                MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                    Icon = "ic_monetization_on.png",
                    PageName = "PagoPage",
                    Title = "Registrar pago",
                });

                MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                    Icon = "ic_list_alt.png",
                    PageName = "PacientesSecretariaPage",
                    Title = "Todos los turnos",
                });

                MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                    Icon = "ic_insert_chart.png",
                    PageName = "InformePage",
                    Title = "Informes",
                });
            }
            

            if(MainViewModel.GetInstance().User.IdTipoUsuario == 3) {

                MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                    Icon = "ic_people.png",
                    PageName = "MisPacientesPage",
                    Title = "Mis Pacientes",
                });

                MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                    Icon = "ic_list_alt.png",
                    PageName = "PacientesMedicoPage",
                    Title = "Mis Consultas",
                });
            }

            MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                Icon = "ic_insert_chart.png",
                PageName = "AyudaPage",
                Title = "Ayuda",
            });

            MainViewModel.GetInstance().Menus.Add(new MenuItemViewModel {
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage",
                Title = "Cerrar sesión",
            });

            mainViewModel.Home = new HomeViewModel();
            Application.Current.MainPage = new MasterPage();

            this.IsRunning = false;
            this.IsEnabled = true;

            this.Email = string.Empty;
            this.Password = string.Empty;

        }

        private async void Register() {

            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
    }
}
