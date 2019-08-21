using GalaSoft.MvvmLight.Command;
using Sagrada.Dominio;
using Sagrada.Models;
using Sagrada.Services;
using Sagrada.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sagrada.ViewModels
{
    public class MenuItemViewModel
    {
        private ApiService apiService;

        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
     
        public ICommand NavigateCommand { get { return new RelayCommand(Navigate); } }

        public MenuItemViewModel() {

            apiService = new ApiService();
        }

        private async void Navigate() {
            App.Master.IsPresented = false;

            if (this.PageName == "LoginPage") {
                // Settings.IsRemembered = "false";
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = null;
                mainViewModel.User = null;
                mainViewModel.Menus = null;
                Application.Current.MainPage = new NavigationPage(
                    new LoginPage());
            } else if (this.PageName == "MyProfilePage") {
                MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                await App.Navigator.PushAsync(new MyProfilePage());

            } else if (this.PageName == "TurnoPage") {
                MainViewModel.GetInstance().Turno = new TurnoViewModel();
                await App.Navigator.PushAsync(new TurnoPage());
                MainViewModel.GetInstance().Turno.IsEnabledBoton = false;

            } else if (this.PageName == "InformePage") {
                MainViewModel.GetInstance().Informe = new InformeViewModel();
                await App.Navigator.PushAsync(new InformePage());

            } else if (this.PageName == "PagoPage") {
                MainViewModel.GetInstance().Pago = new PagoViewModel();
                await App.Navigator.PushAsync(new PagoPage());

            } else if (this.PageName == "PacientesMedicoPage") { 

                var connection = await this.apiService.CheckConnection();

                if (!connection.IsSuccess) {
                    
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        connection.Message,
                        "Aceptar");
                    return;
                }

                var busquedaTurnosViejos = await this.apiService.PostList<TurnoResponse>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes/GetTurnosViejos",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            new { Fecha = DateTime.Now, IdMedico = MainViewModel.GetInstance().Medico.IdMedico });

                if (!busquedaTurnosViejos.IsSuccess) {
                    
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaTurnosViejos.Message,
                        "Aceptar");
                    return;
                }

                List<TurnoResponse> resultadoTurnosViejos = (List<TurnoResponse>)busquedaTurnosViejos.Result;            

                MainViewModel.GetInstance().PacientesViejos = new ObservableCollection<PacientesMedicoItemViewModel>();

                foreach(TurnoResponse item in resultadoTurnosViejos) {

                    var obj = new PacientesMedicoItemViewModel {

                        NombreCompleto = item.Usuario.FullName,
                        ObraSocialString = item.ObraSocial.Nombre,
                        EspecialidadString = item.Especialidad.Nombre,
                        FechaString = item.Fecha.ToString("dddd, dd MMMM yyyy HH:mm"),
                        Especialidad = item.Especialidad,
                        Fecha = item.Fecha,
                        IdEspecialidad = item.IdEspecialidad,
                        IdMedico = item.IdMedico,
                        IdObraSocial = item.IdObraSocial,
                        IdTurno = item.IdTurno,
                        IdUsuario = item.IdUsuario,
                        Medico = item.Medico,
                        ObraSocial = item.ObraSocial,
                        Usuario = item.Usuario,

                    };

                    if (obj.ObraSocial == null) {
                        obj.ObraSocialString = "Particular";
                    } else {
                        obj.ObraSocialString = item.ObraSocial.Nombre;
                    }

                    MainViewModel.GetInstance().PacientesViejos.Add(obj);
                }

                //-------------------------
                var busquedaTurnosNuevos = await this.apiService.PostList<TurnoResponse>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes/GetTurnosNuevos",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            new { Fecha = DateTime.Now, IdMedico = MainViewModel.GetInstance().Medico.IdMedico });

                if (!busquedaTurnosNuevos.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaTurnosNuevos.Message,
                        "Aceptar");
                    return;
                }

                List<TurnoResponse> resultadoTurnosNuevos = (List<TurnoResponse>)busquedaTurnosNuevos.Result;

                MainViewModel.GetInstance().PacientesNuevos = new ObservableCollection<PacientesMedicoItemViewModel>();

                foreach (TurnoResponse item in resultadoTurnosNuevos) {

                    var obj = new PacientesMedicoItemViewModel {

                        NombreCompleto = item.Usuario.FullName,
                        EspecialidadString = item.Especialidad.Nombre,
                        FechaString = item.Fecha.ToString("dddd, dd MMMM yyyy HH:mm"),
                        Especialidad = item.Especialidad,
                        Fecha = item.Fecha,
                        IdEspecialidad = item.IdEspecialidad,
                        IdMedico = item.IdMedico,
                        IdObraSocial = item.IdObraSocial,
                        IdTurno = item.IdTurno,
                        IdUsuario = item.IdUsuario,
                        Medico = item.Medico,
                        ObraSocial = item.ObraSocial,
                        Usuario = item.Usuario,

                    };

                    if (obj.ObraSocial == null) {
                        obj.ObraSocialString = "Particular";
                    } else {
                        obj.ObraSocialString = item.ObraSocial.Nombre;
                    }

                    MainViewModel.GetInstance().PacientesNuevos.Add(obj);
                }

                await App.Navigator.PushAsync(new PacientesMedicoPage());

            }else if(this.PageName == "MisPacientesPage") {

                var connection = await this.apiService.CheckConnection();

                if (!connection.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        connection.Message,
                        "Aceptar");
                    return;
                }

                var busquedaPacientesAtendidos = await this.apiService.GetList<UsuarioAtendidoResponse>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/UsuarioAtendidoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken);

                if (!busquedaPacientesAtendidos.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaPacientesAtendidos.Message,
                        "Aceptar");
                    return;
                }

                var resultadoPacientesAtendidos = (List<UsuarioAtendidoResponse>) busquedaPacientesAtendidos.Result;

                MainViewModel.GetInstance().PacientesAtendidos = new ObservableCollection<MisPacientesItemViewModel>();

                foreach(var item in resultadoPacientesAtendidos) {

                    if (item.CobroMedico.IdMedico == MainViewModel.GetInstance().Medico.IdMedico) {

                        var obj = new MisPacientesItemViewModel {

                            NombreCompleto = item.Usuario.FullName,
                            FechaString = item.Fecha.ToString("dddd, dd MMMM yyyy HH:mm"),
                            HistoriaClinica = item.HistoriaClinica,
                            IdUsuario = item.IdUsuario,
                            ObraSocial = item.ObraSocial
                        };

                        if (obj.ObraSocial == null) {
                            obj.ObraSocialString = "Particular";
                        } else {
                            obj.ObraSocialString = item.ObraSocial.Nombre;
                        }

                        MainViewModel.GetInstance().PacientesAtendidos.Add(obj);
                    }
                }

                await App.Navigator.PushAsync(new MisPacientesPage());

            }else if(this.PageName == "PacientesSecretariaPage") {

                var connection = await this.apiService.CheckConnection();

                if (!connection.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        connection.Message,
                        "Aceptar");
                    return;
                }

                var busquedaTurnos = await this.apiService.GetList<TurnoResponse>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken);

                if (!busquedaTurnos.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaTurnos.Message,
                        "Aceptar");
                    return;
                }

                var resultadoTurnos = (List<TurnoResponse>)busquedaTurnos.Result;

                MainViewModel.GetInstance().AllTurnos = new ObservableCollection<PacientesMedicoItemViewModel>();

                foreach (var item in resultadoTurnos) {

                    var obj = new PacientesMedicoItemViewModel {

                        NombreCompleto = item.Usuario.FullName,
                        EspecialidadString = item.Especialidad.Nombre,
                        FechaString = item.Fecha.ToString("dddd, dd MMMM yyyy HH:mm"),
                        Especialidad = item.Especialidad,
                        Fecha = item.Fecha,
                        IdEspecialidad = item.IdEspecialidad,
                        IdMedico = item.IdMedico,
                        IdObraSocial = item.IdObraSocial,
                        IdTurno = item.IdTurno,
                        IdUsuario = item.IdUsuario,
                        Medico = item.Medico,
                        ObraSocial = item.ObraSocial,
                        Usuario = item.Usuario,
                        MedicoString = item.Medico.FullName,

                    };

                    if(obj.ObraSocial == null) {
                        obj.ObraSocialString = "Particular";
                    } else {
                        obj.ObraSocialString = item.ObraSocial.Nombre;
                    }

                    MainViewModel.GetInstance().AllTurnos.Add(obj);
                }

                //--------------------------------------------

                var busquedaPacientesEnEspera = await this.apiService.GetList<UsuarioEnEsperaResponse>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/UsuarioEnEsperas",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken);

                if (!busquedaPacientesEnEspera.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaPacientesEnEspera.Message,
                        "Aceptar");
                    return;
                }

                var resultadoPacientesEnEspera = (List<UsuarioEnEsperaResponse>)busquedaPacientesEnEspera.Result;

                MainViewModel.GetInstance().PacientesEnEspera = new ObservableCollection<PacientesMedicoItemViewModel>();

                foreach (var item in resultadoPacientesEnEspera) {

                    var obj = new PacientesMedicoItemViewModel {

                        NombreCompleto = item.Usuario.FullName,
                        EspecialidadString = item.Especialidad.Nombre,
                        FechaString = item.Fecha.ToString("dddd, dd MMMM yyyy HH:mm"),
                        Especialidad = item.Especialidad,
                        Fecha = item.Fecha,
                        IdEspecialidad = item.IdEspecialidad,
                        IdMedico = item.IdMedico,
                        IdObraSocial = item.IdObraSocial,
                        IdUsuario = item.IdUsuario,
                        Medico = item.Medico,
                        ObraSocial = item.ObraSocial,
                        Usuario = item.Usuario,
                        MedicoString = item.Medico.FullName,
                        IdUsuarioEnEspera = item.IdUsuarioEnEspera,

                    };

                    if (obj.ObraSocial == null) {
                        obj.ObraSocialString = "Particular";
                    } else {
                        obj.ObraSocialString = item.ObraSocial.Nombre;
                    }

                    MainViewModel.GetInstance().PacientesEnEspera.Add(obj);
                }

                await App.Navigator.PushAsync(new PacientesSecretariaPage());

            } else if (this.PageName == "AyudaPage") {


                
            }
        }
    }
}
