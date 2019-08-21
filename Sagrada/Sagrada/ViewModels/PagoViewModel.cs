using GalaSoft.MvvmLight.Command;
using Sagrada.Dominio;
using Sagrada.Models;
using Sagrada.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sagrada.ViewModels
{
    public class PagoViewModel : BaseViewModel
    {
        private ApiService apiService;
        private bool isEnabledTarjeta;
        private bool isEnabledPago;
        private bool isEnabledDatos;
        private bool isEnabledFormaDePago;
        private BancoResponse bancoSeleccionado;
        private List<BancoResponse> bancoResponses;
        private string formaDePagoSeleccionada;
        private string montoString;
        private int monto;
        private List<UsuarioResponse> pacientes;
        private UsuarioResponse pacienteSeleccionado;
        private List<TurnoResponse> turnos;       
        private TurnoResponse turnoSeleccionado;
        private string datosString;

        public List<string> FormaDePago { get; set; }
        public int CodOperacion { get; set; }

        public string FormaDePagoSeleccionada {

            get {
                return formaDePagoSeleccionada;
            }

            set {
                formaDePagoSeleccionada = value;

                IsEnabledPago = true;
                if (formaDePagoSeleccionada == "Tarjeta") {

                    IsEnabledTarjeta = true;
                    this.CargarBancos();
                } else {
                    IsEnabledTarjeta = false;
                }
            }
        }

        public bool IsEnabledTarjeta {

            get { return this.isEnabledTarjeta; }
            set { SetValue(ref this.isEnabledTarjeta, value); }
        }

        public bool IsEnabledPago {

            get { return this.isEnabledPago; }
            set { SetValue(ref this.isEnabledPago, value); }
        }

        public bool IsEnabledDatos {

            get { return this.isEnabledDatos; }
            set { SetValue(ref this.isEnabledDatos, value); }
        }

        public bool IsEnabledFormaDePago {

            get { return this.isEnabledFormaDePago; }
            set { SetValue(ref this.isEnabledFormaDePago, value); }
        }

        public string DatosString {

            get { return this.datosString; }
            set { SetValue(ref this.datosString, value); }
        }


        public string MontoString {

            get { return this.montoString; }
            set { SetValue(ref this.montoString, value); }
        }

        public int Monto {

            get { return this.monto; }
            set { SetValue(ref this.monto, value); }
        }


        public List<BancoResponse> BancoResponses {

            get { return this.bancoResponses; }
            set { SetValue(ref this.bancoResponses, value); }
        }

        public BancoResponse BancoSeleccionado {

            get { return this.bancoSeleccionado; }
            set { SetValue(ref this.bancoSeleccionado, value); }
        }

        public List<UsuarioResponse> Pacientes {

            get { return this.pacientes; }
            set { SetValue(ref this.pacientes, value); }
        }

        public UsuarioResponse PacienteSeleccionado {

            get { return this.pacienteSeleccionado; }
            set {

                SetValue(ref this.pacienteSeleccionado, value);
                IsEnabledFormaDePago = false;
                IsEnabledPago = false;
                IsEnabledTarjeta = false;
                IsEnabledDatos = false;
                if (pacienteSeleccionado != null) {
                    this.CargarTurnos();
                }
            }
        }

        public List<TurnoResponse> Turnos {

            get { return this.turnos; }
            set { SetValue(ref this.turnos, value); }
        }

        public TurnoResponse TurnoSeleccionado {

            get { return this.turnoSeleccionado; }
            set {

                SetValue(ref this.turnoSeleccionado, value);

                if (turnoSeleccionado != null) {
                    if (turnoSeleccionado.IdObraSocial != null) {
                        DatosString = "El paciente " + turnoSeleccionado.Usuario.FullName + " eligió abonar con la obra social " + turnoSeleccionado.ObraSocial.Nombre + ". Sus datos serán autocompletados.";
                        this.CargarHonorarios();
                        IsEnabledPago = true;
                        IsEnabledDatos = true;
                        IsEnabledTarjeta = false;

                    } else {
                        IsEnabledPago = false;
                        IsEnabledFormaDePago = true;
                        IsEnabledDatos = false;
                    }
                }
            }
        }

        public ICommand RegistrarPagoCommand { get { return new RelayCommand(RegistrarPago); } }

        

        public PagoViewModel() {

            apiService = new ApiService();

            FormaDePago = new List<string> {
                "Efectivo",
                "Tarjeta",
            };

            this.CargarPacientes();
        }

        private async void RegistrarPago() {

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            Pago pago = new Pago {
                IdUsuario = turnoSeleccionado.Usuario.IdUsuario,
                Monto = this.Monto,
                IdMedico = turnoSeleccionado.Medico.IdMedico,
                Fecha = turnoSeleccionado.Fecha,
                

            };

            if (isEnabledTarjeta) {

                Tarjeta tarjeta = new Tarjeta {

                    IdBanco = bancoSeleccionado.IdBanco,
                    CodOperacion = this.CodOperacion,
                };

                var postTarjeta = await this.apiService.Post(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/Tarjetas",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                tarjeta);

                if (!postTarjeta.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        postTarjeta.Message,
                        "Aceptar");
                    return;
                }

                var busquedaTarjeta = await this.apiService.Get<Tarjeta>(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/Tarjetas",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                tarjeta.CodOperacion);

                if (!busquedaTarjeta.IsSuccess) {
 
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaTarjeta.Message,
                        "Aceptar");
                    return;
                }

                Tarjeta tarjetaResponse = (Tarjeta)busquedaTarjeta.Result;

                pago.IdTarjeta = tarjetaResponse.IdTarjeta;

            } else {

                pago.IdTarjeta = null;
            }

            var response2 = await this.apiService.Post(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/Pagoes",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                pago);

            if (!response2.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response2.Message,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                           "Exito",
                           "Pago realizado exitosamente",
                           "Aceptar");

            await App.Navigator.PopAsync();
            return;
        }

        private async void CargarBancos() {

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {
           
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var busquedaBancos = await this.apiService.GetList<BancoResponse>(
               "http://sagradaapi.azurewebsites.net",
               "/api",
               "/Bancoes",
               MainViewModel.GetInstance().Token.TokenType,
               MainViewModel.GetInstance().Token.AccessToken);

            if (!busquedaBancos.IsSuccess) {
      
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaBancos.Message,
                    "Aceptar");
                return;
            }

            BancoResponses = (List<BancoResponse>)busquedaBancos.Result;
            
        }

        private async void CargarPacientes() {

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {
               
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var busquedaUsuarios = await this.apiService.GetList<UsuarioResponse>(
               "http://sagradaapi.azurewebsites.net",
               "/api",
               "/Usuarios",
               MainViewModel.GetInstance().Token.TokenType,
               MainViewModel.GetInstance().Token.AccessToken);

            if (!busquedaUsuarios.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaUsuarios.Message,
                    "Aceptar");
                return;
            }

            Pacientes = (List<UsuarioResponse>)busquedaUsuarios.Result;

        }

        private async void CargarTurnos() {

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
               "/Turnoes/GetTurnoByUsuario",
               MainViewModel.GetInstance().Token.TokenType,
               MainViewModel.GetInstance().Token.AccessToken,
               pacienteSeleccionado.IdUsuario);

            if (!busquedaTurnos.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaTurnos.Message,
                    "Aceptar");
                return;
            }

            Turnos = (List<TurnoResponse>)busquedaTurnos.Result;

            if(turnos.Count == 0) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El Paciente " + pacienteSeleccionado.FullName + " no tiene ningún turno asignado",
                    "Aceptar");
            }

        }

        private async void CargarHonorarios() {

            var busquedaCobroMedico = await this.apiService.GetWith2Parameters<CobroMedicoResponse>(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/CobroMedicoes/GetHonorario",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                turnoSeleccionado.Medico.IdMedico,
                turnoSeleccionado.Especialidad.IdEspecialidad);

            if (!busquedaCobroMedico.IsSuccess) {

                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaCobroMedico.Message,
                    "Aceptar");
                return;
            }

            CobroMedicoResponse resultadoCobroMedico = (CobroMedicoResponse)busquedaCobroMedico.Result;
            Monto = resultadoCobroMedico.Honorarios;
            MontoString = "El monto total a pagar es de: " + resultadoCobroMedico.Honorarios;
            
        }

    }
}
