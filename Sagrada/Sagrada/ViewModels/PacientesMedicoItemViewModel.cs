using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using Sagrada.Dominio;
using Sagrada.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sagrada.ViewModels
{
    public class PacientesMedicoItemViewModel
    {
        private ApiService apiService;

        public string NombreCompleto { get; set; }

        public string ObraSocialString { get; set; }

        public string MedicoString { get; set; }

        public string EspecialidadString { get; set; }

        public string FechaString { get; set; }

        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        public int IdMedico { get; set; }

        public Medico Medico { get; set; }

        public int? IdObraSocial { get; set; }

        public ObraSocial ObraSocial { get; set; }

        public int IdEspecialidad { get; set; }

        public Especialidad Especialidad { get; set; }

        public DateTime Fecha { get; set; }

        public int IdTurno { get; set; }

        public int IdUsuarioEnEspera { get; set; }


        public ICommand RegistrarCommand { get { return new RelayCommand(Registrar); } }

        public ICommand EliminarCommand { get { return new RelayCommand(Eliminar); } }

        public ICommand AgregarCommand { get { return new RelayCommand(Agregar); } }


        public PacientesMedicoItemViewModel() {

            apiService = new ApiService();
        }

        private async void Registrar() {

            string historiaClinica;

            var promptConfig = new PromptConfig();
            promptConfig.InputType = InputType.Name;
            promptConfig.OkText = "Agregar";
            promptConfig.IsCancellable = true;
            promptConfig.Message = "Escribe la historia clinica";
            var result = await UserDialogs.Instance.PromptAsync(promptConfig);

            if (result.Ok) {
                historiaClinica = result.Text;

                var connection = await this.apiService.CheckConnection();

                if (!connection.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        connection.Message,
                        "Aceptar");
                    return;
                }

                var busquedaCobroMedico = await this.apiService.GetWith2Parameters<CobroMedico>(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/CobroMedicoes/GetHonorario",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                this.IdMedico,
                this.IdEspecialidad);

                if (!busquedaCobroMedico.IsSuccess) {


                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaCobroMedico.Message,
                        "Aceptar");
                    return;
                }

                CobroMedico resultadoCobroMedico = (CobroMedico)busquedaCobroMedico.Result;

                UsuarioAtendido usuarioAtendido = new UsuarioAtendido {
                    IdUsuario = this.IdUsuario,
                    IdObraSocial = this.IdObraSocial,
                    Fecha = this.Fecha,
                    HistoriaClinica = historiaClinica,
                    IdCobroMedico = resultadoCobroMedico.IdCobroMedico,
                    
                };

                var postUsuarioAtendido = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/UsuarioAtendidoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            usuarioAtendido);

                if (!postUsuarioAtendido.IsSuccess) {


                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        postUsuarioAtendido.Message,
                        "Aceptar");
                    return;
                }

                var eliminarTurno = await this.apiService.Delete(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            this.IdTurno);

                if (!eliminarTurno.IsSuccess) {


                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        eliminarTurno.Message,
                        "Aceptar");
                    return;
                }

                MainViewModel.GetInstance().PacientesViejos.Remove(MainViewModel.GetInstance().PacientesViejos.Where(p => p.IdTurno == this.IdTurno).SingleOrDefault());

                await Application.Current.MainPage.DisplayAlert(
                        "Exito",
                        "El paciente se ha registrado correctamente",
                        "Aceptar");
            }
        }

        private async void Eliminar() {

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var eliminarTurno = await this.apiService.Delete(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            this.IdTurno);

            if (!eliminarTurno.IsSuccess) {


                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    eliminarTurno.Message,
                    "Aceptar");
                return;
            }

            MainViewModel.GetInstance().AllTurnos.Remove(MainViewModel.GetInstance().AllTurnos.Where(p => p.IdTurno == this.IdTurno).SingleOrDefault());

            var busquedaUsuarioEnEspera = await this.apiService.Post<UsuarioEnEspera>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/UsuarioEnEsperas/GetTurnoByFecha",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            new { Fecha = this.Fecha, IdMedico = this.IdMedico });

            if (!busquedaUsuarioEnEspera.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Exito",
                    "El turno se ha eliminado correctamente",
                    "Aceptar");
                return;
            }

            UsuarioEnEspera resultadoUsuarioEnEspera = (UsuarioEnEspera)busquedaUsuarioEnEspera.Result;

            bool resultadoMensaje = await Application.Current.MainPage.DisplayAlert(
                    "Exito",
                    "El turno se ha eliminado correctamente. Existe un turno en espera en la misma fecha, ¿desea agregarlo?",
                    "Si",
                    "No");

            if (resultadoMensaje) {

                Turno turno = new Turno {

                    IdUsuario = resultadoUsuarioEnEspera.IdUsuario,
                    IdMedico = resultadoUsuarioEnEspera.IdMedico,
                    IdObraSocial = resultadoUsuarioEnEspera.IdObraSocial,
                    IdEspecialidad = resultadoUsuarioEnEspera.IdEspecialidad,
                    Fecha = resultadoUsuarioEnEspera.Fecha,
                    
                };

                var postTurnoEnEspera = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            turno);

                if (!postTurnoEnEspera.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        postTurnoEnEspera.Message,
                        "Aceptar");
                    return;
                }

                var eliminarTurnoEnEspera = await this.apiService.Delete(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/UsuarioEnEsperas",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            resultadoUsuarioEnEspera.IdUsuarioEnEspera);

                if (!eliminarTurnoEnEspera.IsSuccess) {


                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        eliminarTurnoEnEspera.Message,
                        "Aceptar");
                    return;
                }

                var busquedaTurno = await this.apiService.Post<Turno>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes/GetTurnoByFecha",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            new { Fecha = this.Fecha, IdMedico = this.IdMedico });

                if (!busquedaTurno.IsSuccess) {


                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaTurno.Message,
                        "Aceptar");
                    return;
                }

                Turno resultadoBusquedaTurno = (Turno)busquedaTurno.Result;

                PacientesMedicoItemViewModel turnoParaAgregar = MainViewModel.GetInstance().PacientesEnEspera.Where(p => p.Fecha == this.Fecha && p.IdMedico == this.IdMedico).SingleOrDefault();
                turnoParaAgregar.IdTurno = resultadoBusquedaTurno.IdTurno;

                MainViewModel.GetInstance().AllTurnos.Add(turnoParaAgregar);
                MainViewModel.GetInstance().PacientesEnEspera.Remove(MainViewModel.GetInstance().PacientesEnEspera.Where(p => p.Fecha == this.Fecha && p.IdMedico == this.IdMedico).SingleOrDefault());


                await Application.Current.MainPage.DisplayAlert(
                    "Exito",
                    "El turno se ha registrado correctamente",
                    "Aceptar");
            }

        }

        private async void Agregar() {

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var busquedaTurno = await this.apiService.Post<Turno>(
                    "http://sagradaapi.azurewebsites.net",
                    "/api",
                    "/Turnoes/GetTurnoByFecha",
                    MainViewModel.GetInstance().Token.TokenType,
                    MainViewModel.GetInstance().Token.AccessToken,
                    new { Fecha = this.Fecha, IdMedico = this.IdMedico });

            if (!busquedaTurno.IsSuccess) {

                bool resultadoMensaje = await Application.Current.MainPage.DisplayAlert(
                    "Exito",
                    "El turno se encuentra disponible. ¿Esta seguro que desea agregarlo?",
                    "Si",
                    "No");

                if (resultadoMensaje) {

                    Turno turno = new Turno {

                        IdUsuario = this.IdUsuario,
                        IdMedico = this.IdMedico,
                        IdObraSocial = this.IdObraSocial,
                        IdEspecialidad = this.IdEspecialidad,
                        Fecha = this.Fecha,

                    };

                    var postTurno = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            turno);

                    if (!postTurno.IsSuccess) {

                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            postTurno.Message,
                            "Aceptar");
                        return;
                    }

                    var eliminarTurnoEnEspera = await this.apiService.Delete(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/UsuarioEnEsperas",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            this.IdUsuarioEnEspera);

                    if (!eliminarTurnoEnEspera.IsSuccess) {


                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            eliminarTurnoEnEspera.Message,
                            "Aceptar");
                        return;
                    }

                    var busquedaTurnoByFecha = await this.apiService.Post<Turno>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes/GetTurnoByFecha",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            new { Fecha = this.Fecha, IdMedico = this.IdMedico });

                    if (!busquedaTurnoByFecha.IsSuccess) {


                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            busquedaTurnoByFecha.Message,
                            "Aceptar");
                        return;
                    }

                    Turno resultadoBusquedaTurno = (Turno)busquedaTurnoByFecha.Result;

                    PacientesMedicoItemViewModel turnoParaAgregar = MainViewModel.GetInstance().PacientesEnEspera.Where(p => p.Fecha == this.Fecha && p.IdMedico == this.IdMedico).SingleOrDefault();
                    turnoParaAgregar.IdTurno = resultadoBusquedaTurno.IdTurno;

                    MainViewModel.GetInstance().AllTurnos.Add(turnoParaAgregar);
                    MainViewModel.GetInstance().PacientesEnEspera.Remove(MainViewModel.GetInstance().PacientesEnEspera.Where(p => p.Fecha == this.Fecha && p.IdMedico == this.IdMedico).SingleOrDefault());

                    await Application.Current.MainPage.DisplayAlert(
                        "Exito",
                        "El turno se ha registrado correctamente",
                        "Aceptar");
                    return;

                } else {

                    return;
                }
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El turno se encuentra ocupado",
                    "Aceptar");
        }
    }
}
