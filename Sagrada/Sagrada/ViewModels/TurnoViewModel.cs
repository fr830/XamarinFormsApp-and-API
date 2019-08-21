using GalaSoft.MvvmLight.Command;
using Sagrada.Dominio;
using Sagrada.Helpers;
using Sagrada.Models;
using Sagrada.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sagrada.ViewModels
{
    public class TurnoViewModel : BaseViewModel {

        private ApiService apiService;
        private List<EspecialidadResponse> especialidadResponses;
        private List<MedicoResponse> medicoResponses;
        private DateTime fecha;
        private bool isRunning;
        private bool isEnabledMedico;
        private bool isEnabledFecha;
        private bool isEnabledBoton;
        private bool isEnabledTrabajaObraSocial;
        private bool isEnabledTrabajaFecha;
        private bool isEnabledEspecialidad;
        private bool isEnabledNombreValidacion;
        private bool isEnabledApellidoValidacion;
        private bool isEnabledDniValidacion;
        private bool isEnabledEmailValidacion;
        private EspecialidadResponse especialidadSeleccionada;
        private MedicoResponse medicoSeleccionado;
        private List<int> hora;
        private List<string> minuto;
        private int horaSeleccionada;
        private string minutoSeleccionado;       
        private bool isEnabledRegistro;
        private string pacienteEncontrado;
        private bool isEnabledPaciente;
        private string montoString;
        private string trabajaObraSocial;
        private string trabajaFecha;
        private int monto;
        private int idUsuario;
        private DateTime fechaDeNacimiento;       
        private bool isEnabledObraSocial;        
        private List<ObraSocialResponse> prepaga;
        private ObraSocialResponse prepagaSeleccionada;
        private bool isEnabledRegistroObraSocial;
        private List<ObraSocialResponse> obraSocialResponses;
        private ObraSocialResponse obraSocialSeleccionada;
        private int busquedaDNI;
        private string nombre;
        private string apellido;
        private int dni;
        private string email;
        public string Telefono { get; set; }             
        public int NroAfiliado { get; set; }
        public DateTime FechaMaxima { get; set; }

        public string Nombre {

            get {
                return nombre;
            }

            set {
                nombre = value;
                IsEnabledNombreValidacion = string.IsNullOrEmpty(this.nombre);

            }
        }

        public string Apellido {

            get {
                return apellido;
            }

            set {
                apellido = value;
                IsEnabledApellidoValidacion = string.IsNullOrEmpty(this.apellido);

            }
        }

        public int DNI {

            get { return this.dni; }
            set {

                SetValue(ref this.dni, value);
                if (isEnabledDniValidacion) {
                    IsEnabledDniValidacion = dni == 0 ? true : false;
                }
            }
            
        }

        public string Email {

            get {
                return email;
            }

            set {
                email = value;
                IsEnabledEmailValidacion = string.IsNullOrEmpty(this.email);

            }
        }

        public List<EspecialidadResponse> EspecialidadResponses {

            get { return this.especialidadResponses; }
            set { SetValue(ref this.especialidadResponses, value); }
        }

        public List<MedicoResponse> MedicoResponses {

            get { return this.medicoResponses; }
            set { SetValue(ref this.medicoResponses, value); }
        }

        

        public bool IsRunning {

            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public string MontoString {

            get { return this.montoString; }
            set { SetValue(ref this.montoString, value); }
        }

        public int Monto {

            get { return this.monto; }
            set { SetValue(ref this.monto, value); }
        }

        public bool IsEnabledMedico {

            get { return this.isEnabledMedico; }
            set { SetValue(ref this.isEnabledMedico, value); }
        }

        public bool IsEnabledFecha {

            get { return this.isEnabledFecha; }
            set { SetValue(ref this.isEnabledFecha, value); }
        }

        public bool IsEnabledTrabajaObraSocial {

            get { return this.isEnabledTrabajaObraSocial; }
            set { SetValue(ref this.isEnabledTrabajaObraSocial, value); }
        }

        public bool IsEnabledTrabajaFecha {

            get { return this.isEnabledTrabajaFecha; }
            set { SetValue(ref this.isEnabledTrabajaFecha, value); }
        }

        public bool IsEnabledBoton {

            get { return this.isEnabledBoton; }
            set { SetValue(ref this.isEnabledBoton, value); }
        }

        public bool IsEnabledNombreValidacion {

            get { return this.isEnabledNombreValidacion; }
            set { SetValue(ref this.isEnabledNombreValidacion, value); }
        }

        public bool IsEnabledApellidoValidacion {

            get { return this.isEnabledApellidoValidacion; }
            set { SetValue(ref this.isEnabledApellidoValidacion, value); }
        }

        public bool IsEnabledDniValidacion {

            get { return this.isEnabledDniValidacion; }
            set { SetValue(ref this.isEnabledDniValidacion, value); }
        }

        public bool IsEnabledEmailValidacion {

            get { return this.isEnabledEmailValidacion; }
            set { SetValue(ref this.isEnabledEmailValidacion, value); }
        }

        public EspecialidadResponse EspecialidadSeleccionada {

            get { return this.especialidadSeleccionada; }

            set {

                SetValue(ref this.especialidadSeleccionada, value);
                this.IsEnabledMedico = false;
                this.IsEnabledFecha = false;
                this.IsEnabledBoton = false;
                this.IsEnabledTrabajaFecha = false;
                this.IsEnabledTrabajaObraSocial = false;
                this.CargarMedicos();

                
            }
        }

        public MedicoResponse MedicoSeleccionado {

            get { return this.medicoSeleccionado; }

            set {

                SetValue(ref this.medicoSeleccionado, value);
                this.IsEnabledFecha = true;               
                this.IsEnabledTrabajaFecha = true;
                this.IsEnabledTrabajaObraSocial = true;
                this.IsEnabledObraSocial = true;
                

                if (medicoSeleccionado != null && especialidadSeleccionada != null) {
                    this.IsEnabledBoton = true;
                    this.CargarHonorarios();
                    this.CargarTrabajaObraSocial();
                    this.CargarTrabajaFecha();
                }
            }
        }

        public DateTime Fecha {

            get { return this.fecha; }

            set {

                SetValue(ref this.fecha, value);

                if((fecha.Date - DateTime.Now.Date).TotalDays < 0) {
                    Fecha = DateTime.Now;
                } 

                if((fecha.Date - DateTime.Now.Date).TotalDays > 7) {
                    Fecha = DateTime.Now.AddDays(7);
                }

                
            }
        }

        public List<int> Hora {

            get { return this.hora; }
            set { SetValue(ref this.hora, value); }
        }

        public List<string> Minuto {

            get { return this.minuto; }
            set { SetValue(ref this.minuto, value); }
        }

        public int HoraSeleccionada {

            get { return this.horaSeleccionada; }
            set {

                SetValue(ref this.horaSeleccionada, value);
                Minuto = new List<string>();

                if(horaSeleccionada == 20) {
                    Minuto.Add("00");
                    Minuto.Add("15");
                } else {
                    Minuto.Add("00");
                    Minuto.Add("15");
                    Minuto.Add("30");
                    Minuto.Add("45");
                }

                MinutoSeleccionado = Minuto[0];
            }
        }

        public string MinutoSeleccionado {

            get { return this.minutoSeleccionado; }
            set { SetValue(ref this.minutoSeleccionado, value); }
        }

        public string TrabajaObraSocial {

            get { return this.trabajaObraSocial; }
            set { SetValue(ref this.trabajaObraSocial, value); }
        }

        public string TrabajaFecha {

            get { return this.trabajaFecha; }
            set { SetValue(ref this.trabajaFecha, value); }
        }

        public bool IsEnabledRegistro {

            get { return this.isEnabledRegistro; }
            set {

                SetValue(ref this.isEnabledRegistro, value);

                if (isEnabledRegistro) {
                    IsEnabledNombreValidacion = string.IsNullOrEmpty(this.nombre);
                    IsEnabledApellidoValidacion = string.IsNullOrEmpty(this.apellido);
                    IsEnabledDniValidacion = dni == 0 ? true : false;
                    IsEnabledEmailValidacion = string.IsNullOrEmpty(this.email);
                }
            }
        }

        public bool IsEnabledPaciente {

            get { return this.isEnabledPaciente; }
            set { SetValue(ref this.isEnabledPaciente, value); }
        }

        public bool IsEnabledEspecialidad {

            get { return this.isEnabledEspecialidad; }
            set { SetValue(ref this.isEnabledEspecialidad, value); }
        }

        public string PacienteEncontrado {

            get { return this.pacienteEncontrado; }
            set { SetValue(ref this.pacienteEncontrado, value); }
        }

        public bool IsEnabledObraSocial {

            get { return this.isEnabledObraSocial; }
            set {
                SetValue(ref this.isEnabledObraSocial, value);
            }
        }

        public ObraSocialResponse PrepagaSeleccionada {

            get { return this.prepagaSeleccionada; }
            set {

                SetValue(ref this.prepagaSeleccionada, value);

                if (prepagaSeleccionada != null) {

                    if (prepagaSeleccionada.Nombre == "Nueva obra social") {
                        IsEnabledRegistroObraSocial = true;
                        CargarObraSocial();

                    } else {
                        IsEnabledRegistroObraSocial = false;
                    }
                }
            }
        }

        public List<ObraSocialResponse> Prepaga {

            get { return this.prepaga; }
            set { SetValue(ref this.prepaga, value); }
        }

        public bool IsEnabledRegistroObraSocial {

            get { return this.isEnabledRegistroObraSocial; }
            set { SetValue(ref this.isEnabledRegistroObraSocial, value); }
        }

        

        public DateTime FechaDeNacimiento {

            get { return this.fechaDeNacimiento; }
            set {

                SetValue(ref this.fechaDeNacimiento, value);

                if ((fechaDeNacimiento.Date - DateTime.Now.Date).TotalDays > 0) {
                    FechaDeNacimiento = DateTime.Now;
                }
            }
        }

        public List<ObraSocialResponse> ObraSocialResponses {

            get { return this.obraSocialResponses; }
            set { SetValue(ref this.obraSocialResponses, value); }
        }

        public ObraSocialResponse ObraSocialSeleccionada {

            get { return this.obraSocialSeleccionada; }
            set { SetValue(ref this.obraSocialSeleccionada, value); }
        }

        public int BusquedaDNI {

            get { return this.busquedaDNI; }
            set {

                SetValue(ref this.busquedaDNI, value);

                if (this.isEnabledObraSocial) {

                    this.IsEnabledBoton = false;
                    this.IsEnabledFecha = false;
                    this.IsEnabledMedico = false;
                    this.IsEnabledObraSocial = false;
                    this.IsEnabledPaciente = false;
                    this.IsEnabledRegistro = false;
                    this.IsEnabledRegistroObraSocial = false;
                    this.IsEnabledTrabajaFecha = false;
                    this.IsEnabledTrabajaObraSocial = false;
                    this.IsEnabledEspecialidad = false;
                }
            }
        }


        public ICommand BuscarPacienteCommand { get { return new RelayCommand(BuscarPaciente); } }

        

        public ICommand SolicitarTurnoCommand { get { return new RelayCommand(SolicitarTurno); } }

        public TurnoViewModel() { //Constructor

            apiService = new ApiService();
            Hora = new List<int> {
                8,
                9,
                10,
                11,
                16,
                17,
                18,
                19,
                20
            };

            FechaMaxima = DateTime.Now;
            dni = 0;
            FechaDeNacimiento = DateTime.Now;
            fecha = DateTime.Now;
            HoraSeleccionada = 8;
            this.CargarEspecialidades();
        }

        private async void CargarEspecialidades() {

            this.IsRunning = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetList<EspecialidadResponse>(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/Especialidads",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken);

            if (!response.IsSuccess) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            this.EspecialidadResponses = (List<EspecialidadResponse>)response.Result;


            this.IsRunning = false;
        }

        private async void CargarMedicos() {

            this.IsRunning = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }


            var busquedaCobroMedico = await this.apiService.GetList<CobroMedicoResponse>(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/CobroMedicoes",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                EspecialidadSeleccionada.IdEspecialidad);

            
            if (!busquedaCobroMedico.IsSuccess) {
                this.IsRunning = false;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaCobroMedico.Message,
                    "Aceptar");
                return;
            }



            List<CobroMedicoResponse> resultadoCobroMedico = (List<CobroMedicoResponse>) busquedaCobroMedico.Result; 
            
            if(resultadoCobroMedico.Count == 0) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Disculpe, por el momento no disponemos de un medico con esa especialidad",
                    "Aceptar");
                return;
            }
            List<MedicoResponse> resultadoMedico = new List<MedicoResponse>();
            Response busquedaMedico;
               
            foreach (var item in resultadoCobroMedico) {

                busquedaMedico = await this.apiService.Get<MedicoResponse>(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/Medicos",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                item.IdMedico);

                if (!busquedaMedico.IsSuccess) {
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaMedico.Message,
                        "Aceptar");
                    return;
                }

                resultadoMedico.Add((MedicoResponse)busquedaMedico.Result);
            }

           

            this.MedicoResponses = resultadoMedico;


            this.IsRunning = false;
            this.IsEnabledMedico = true;

        }

        private async void SolicitarTurno() {

            this.IsRunning = true;

            if (string.IsNullOrEmpty(this.Nombre)) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un nombre",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Apellido)) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un apellido",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Email)) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un email",
                    "Aceptar");
                return;
            }

            if (!RegexUtilities.IsValidEmail(this.Email)) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un email valido",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Telefono)) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un telefono",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.DNI.ToString())) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un DNI",
                    "Aceptar");
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var busquedaHorarios = await this.apiService.GetList<HorarioResponse>(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/Horarios",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                MedicoSeleccionado.IdMedico);

            List<HorarioResponse> resultadoHorarios = (List<HorarioResponse>)busquedaHorarios.Result;

            string diaSeleccionado = "vacio";
            switch (fecha.DayOfWeek) {

                case DayOfWeek.Monday:
                    diaSeleccionado = "lunes";
                    break;

                case DayOfWeek.Tuesday:
                    diaSeleccionado = "martes";
                    break;

                case DayOfWeek.Wednesday:
                    diaSeleccionado = "miercoles";
                    break;

                case DayOfWeek.Thursday:
                    diaSeleccionado = "jueves";
                    break;

                case DayOfWeek.Friday:
                    diaSeleccionado = "viernes";
                    break;

                case DayOfWeek.Saturday:
                    diaSeleccionado = "sabado";
                    break;

                case DayOfWeek.Sunday:
                    diaSeleccionado = "domingo";
                    break;

            }

            string mensajeError = "El medico " + medicoSeleccionado.FullName + " solo trabaja los ";

            foreach(var item in resultadoHorarios) {

                mensajeError += item.Dia + " de " + item.HoraInicio + "hs a " + item.HoraFin + "hs, " ;

                if(diaSeleccionado == item.Dia.ToLower() && horaSeleccionada >= item.HoraInicio && horaSeleccionada < item.HoraFin) {

                    int idObraSocialBusqueda = 0;

                    if(prepagaSeleccionada.Nombre == "Nueva obra social") {
                        idObraSocialBusqueda = obraSocialSeleccionada.IdObraSocial;
                    }else if(prepagaSeleccionada.Nombre != "Particular" && prepagaSeleccionada.Nombre != "Nueva obra social") {
                        idObraSocialBusqueda = prepagaSeleccionada.IdObraSocial;
                    }

                    if (prepagaSeleccionada.Nombre != "Particular") {

                        var busquedaObraSocialMedico = await this.apiService.GetWith2Parameters<ObraSocialMedico>(
                                "http://sagradaapi.azurewebsites.net",
                                "/api",
                                "/ObraSocialMedicoes/GetObraSocialByMedicoAndObraSocial",
                                MainViewModel.GetInstance().Token.TokenType,
                                MainViewModel.GetInstance().Token.AccessToken,
                                medicoSeleccionado.IdMedico,
                                idObraSocialBusqueda);

                        if (!busquedaObraSocialMedico.IsSuccess) {

                            this.IsRunning = false;
                            await Application.Current.MainPage.DisplayAlert(
                                "Error",
                                "El medico " + medicoSeleccionado.FullName + " no trabaja con esa obra social",
                                "Aceptar");
                            return;
                        }
                    }

                    DateTime fechaTurno = this.fecha + new TimeSpan(horaSeleccionada, Convert.ToInt32(minutoSeleccionado), 0);

                    var busquedaTurno = await this.apiService.Post<Turno>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes/GetTurnoByFecha",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            new { Fecha = fechaTurno, IdMedico = medicoSeleccionado.IdMedico });

                    if (!busquedaTurno.IsSuccess) {

                        if (IsEnabledRegistro) {

                            Usuario usuario = new Usuario {

                                Nombre = this.Nombre,
                                Apellido = this.Apellido,
                                DNI = this.DNI,
                                Email = this.Email,
                                Telefono = this.Telefono,
                                IdTipoUsuario = 2,
                                Password = "123456",
                                FechaDeNacimiento = this.FechaDeNacimiento,

                            };

                            var postUsuario = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Usuarios",
                            usuario);

                            if (!postUsuario.IsSuccess) {
                                this.IsRunning = false;

                                await Application.Current.MainPage.DisplayAlert(
                                    "Error",
                                    "El paciente ya se encuentra registrado",
                                    "Aceptar");
                                return;
                            }

                            var busquedaUsuario = await this.apiService.Get<Usuario>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Usuarios",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            usuario.DNI);

                            if (!busquedaUsuario.IsSuccess) {
                                this.IsRunning = false;
                                await Application.Current.MainPage.DisplayAlert(
                                    "Error",
                                    busquedaUsuario.Message,
                                    "Aceptar");
                                return;
                            }

                            var resultadoUsuario = (Usuario)busquedaUsuario.Result;
                            this.idUsuario = resultadoUsuario.IdUsuario;

                        }



                        Turno turno = new Turno {

                            IdUsuario = this.idUsuario,
                            IdMedico = MedicoSeleccionado.IdMedico,
                            Fecha = fechaTurno,
                            IdEspecialidad = especialidadSeleccionada.IdEspecialidad,
                            
                        };

                        if (PrepagaSeleccionada.Nombre == "Particular") {
                            turno.IdObraSocial = null;

                        } else if (PrepagaSeleccionada.Nombre == "Nueva obra social") {

                            ObraSocialUsuario obraSocialUsuario = new ObraSocialUsuario {
                                IdObraSocial = obraSocialSeleccionada.IdObraSocial,
                                IdUsuario = this.idUsuario,
                                NroAfiliado = this.NroAfiliado,
                            };

                            var postObraSocial = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/ObraSocialUsuarios",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            obraSocialUsuario);

                            if (!postObraSocial.IsSuccess) {

                                this.IsRunning = false;
                                await Application.Current.MainPage.DisplayAlert(
                                    "Error",
                                    postObraSocial.Message,
                                    "Aceptar");
                                return;
                            }

                            turno.IdObraSocial = obraSocialSeleccionada.IdObraSocial;

                        }else {

                            turno.IdObraSocial = prepagaSeleccionada.IdObraSocial;
                        }

                        var response = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Turnoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            turno);

                        if (!response.IsSuccess) {

                            this.IsRunning = false;
                            await Application.Current.MainPage.DisplayAlert(
                                "Error",
                                response.Message,
                                "Aceptar");
                            return;
                        }

                        this.IsRunning = false;

                        await Application.Current.MainPage.DisplayAlert(
                            "Exito",
                            @"Turno solicitado exitosamente. Para efectuar el pago, dirijase a la pestaña ""Registrar pago""",
                            "Aceptar");
                        await App.Navigator.PopAsync();
                        return;
                    }

                    this.IsRunning = false;
                    bool resultadoMensaje = await Application.Current.MainPage.DisplayAlert(
                       "Importante",
                       "El turno seleccionado ya esta ocupado, ¿Desea agregarlo a la lista de espera?",
                       "Si",
                       "No");


                    if (resultadoMensaje) {

                        if (isEnabledRegistro) {

                            this.IsRunning = true;
                            Usuario usuario = new Usuario {

                                Nombre = this.Nombre,
                                Apellido = this.Apellido,
                                DNI = this.DNI,
                                Email = this.Email,
                                Telefono = this.Telefono,
                                IdTipoUsuario = 2,
                                Password = "123456",
                                FechaDeNacimiento = this.FechaDeNacimiento,
                                

                            };

                            var postUsuario = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Usuarios",
                            usuario);

                            if (!postUsuario.IsSuccess) {
                                this.IsRunning = false;

                                await Application.Current.MainPage.DisplayAlert(
                                    "Error",
                                    "El paciente ya se encuentra registrado",
                                    "Aceptar");
                                return;
                            }

                            var busquedaUsuario = await this.apiService.Get<Usuario>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Usuarios",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            usuario.DNI);

                            if (!busquedaUsuario.IsSuccess) {
                                this.IsRunning = false;
                                await Application.Current.MainPage.DisplayAlert(
                                    "Error",
                                    busquedaUsuario.Message,
                                    "Aceptar");
                                return;
                            }

                            var resultadoUsuario = (Usuario)busquedaUsuario.Result;
                            this.idUsuario = resultadoUsuario.IdUsuario;
                        }

                        UsuarioEnEspera usuarioEnEspera = new UsuarioEnEspera {

                            IdUsuario = this.idUsuario,
                            IdMedico = MedicoSeleccionado.IdMedico,
                            Fecha = fechaTurno,
                            IdEspecialidad = especialidadSeleccionada.IdEspecialidad,
                            
                        };

                        if (PrepagaSeleccionada.Nombre == "Particular") {
                            usuarioEnEspera.IdObraSocial = null;

                        } else if (PrepagaSeleccionada.Nombre == "Nueva obra social") {

                            ObraSocialUsuario obraSocialUsuario = new ObraSocialUsuario {
                                IdObraSocial = obraSocialSeleccionada.IdObraSocial,
                                IdUsuario = this.idUsuario,
                                NroAfiliado = this.NroAfiliado,
                                
                            };

                            var postObraSocial = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/ObraSocialUsuarios",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            obraSocialUsuario);

                            if (!postObraSocial.IsSuccess) {

                                this.IsRunning = false;
                                await Application.Current.MainPage.DisplayAlert(
                                    "Error",
                                    postObraSocial.Message,
                                    "Aceptar");
                                return;
                            }

                            usuarioEnEspera.IdObraSocial = obraSocialSeleccionada.IdObraSocial;

                        } else {

                            usuarioEnEspera.IdObraSocial = prepagaSeleccionada.IdObraSocial;
                        }

                        var postTurnoEnEspera = await this.apiService.Post(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/UsuarioEnEsperas",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            usuarioEnEspera);

                        if (!postTurnoEnEspera.IsSuccess) {

                            this.IsRunning = false;
                            await Application.Current.MainPage.DisplayAlert(
                                "Error",
                                postTurnoEnEspera.Message,
                                "Aceptar");
                            return;
                        }

                        this.IsRunning = false;

                        await Application.Current.MainPage.DisplayAlert(
                            "Exito",
                            @"Paciente Registrado en la lista de espera registrado exitosamente. Para efectuar el pago, dirijase a la pestaña ""Registrar pago""",
                            "Aceptar");
                        await App.Navigator.PopAsync();
                        return;

                    } else {

                        return;
                    }
                }                                    
            }

            IsRunning = false;
            await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        mensajeError,
                        "Aceptar");            
        }

        private async void CargarHonorarios() {

            this.IsRunning = true;
            var busquedaCobroMedico = await this.apiService.GetWith2Parameters<CobroMedicoResponse>(
                "http://sagradaapi.azurewebsites.net",
                "/api",
                "/CobroMedicoes/GetHonorario",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                medicoSeleccionado.IdMedico,
                especialidadSeleccionada.IdEspecialidad);

            if (!busquedaCobroMedico.IsSuccess) {

                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaCobroMedico.Message,
                    "Aceptar");
                return;
            }

            CobroMedicoResponse resultadoCobroMedico = (CobroMedicoResponse) busquedaCobroMedico.Result;
            Monto = resultadoCobroMedico.Honorarios;
            MontoString = "El monto total a pagar es de: " + resultadoCobroMedico.Honorarios;
            this.IsRunning = false;
        }

        private async void BuscarPaciente() {

            this.IsRunning = true;

            
            
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.Get<Usuario>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/Usuarios",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken,
                        BusquedaDNI);

            if (!response.IsSuccess) {               

                
                List<ObraSocialResponse> auxPrepaga1 = new List<ObraSocialResponse> {

                new ObraSocialResponse {
                    IdObraSocial = -1,
                    Nombre = "Particular",
                },

                new ObraSocialResponse {
                    IdObraSocial = 0,
                    Nombre = "Nueva obra social",
                },
                };

                this.Prepaga = auxPrepaga1;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El paciente no se encuentra registrado en la base de datos",
                    "Aceptar");

                this.IsRunning = false;
                this.IsEnabledRegistro = true;
                this.IsEnabledPaciente = false;
                this.PacienteEncontrado = null;
                this.IsEnabledEspecialidad = true;

                return;
            }

            var result = (Usuario)response.Result;

            

            var busquedaUsuarioObraSocial = await this.apiService.GetList<ObraSocialUsuarioResponse>(
            "http://sagradaapi.azurewebsites.net",
            "/api",
            "/ObraSocialUsuarios",
            MainViewModel.GetInstance().Token.TokenType,
            MainViewModel.GetInstance().Token.AccessToken,
            result.IdUsuario);

            if (!busquedaUsuarioObraSocial.IsSuccess) {

                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaUsuarioObraSocial.Message,
                    "Aceptar");
                return;
            }

            List<ObraSocialUsuarioResponse> obraSocialUsuarioResponses = (List<ObraSocialUsuarioResponse>)busquedaUsuarioObraSocial.Result;

            List<ObraSocialResponse> auxPrepaga = new List<ObraSocialResponse> {

                new ObraSocialResponse {
                    IdObraSocial = -1,
                    Nombre = "Particular",
                },

                new ObraSocialResponse {
                    IdObraSocial = 0,
                    Nombre = "Nueva obra social",
                },
                };

            var busquedaObraSocial = await this.apiService.GetList<ObraSocialResponse>(
            "http://sagradaapi.azurewebsites.net",
            "/api",
            "/ObraSocials",
            MainViewModel.GetInstance().Token.TokenType,
            MainViewModel.GetInstance().Token.AccessToken);

            if (!busquedaObraSocial.IsSuccess) {

                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaObraSocial.Message,
                    "Aceptar");
                return;
            }

            List<ObraSocialResponse> resultObraSocial = (List<ObraSocialResponse>)busquedaObraSocial.Result;

            foreach (var usuario in obraSocialUsuarioResponses) {

                foreach(var obraSocial in resultObraSocial) {

                    if(usuario.IdObraSocial == obraSocial.IdObraSocial) {

                        auxPrepaga.Add(new ObraSocialResponse {

                            IdObraSocial = obraSocial.IdObraSocial,
                            Nombre = obraSocial.Nombre,
                        });
                    }
                }
            }

            
            this.Prepaga = auxPrepaga;
            
            this.idUsuario = result.IdUsuario;
            this.Nombre = result.Nombre;
            this.Apellido = result.Apellido;
            this.DNI = result.DNI;
            this.Email = result.Email;
            this.Telefono = result.Telefono;

            this.IsEnabledPaciente = true;
            this.IsEnabledRegistro = false;
            this.IsEnabledEspecialidad = true;

            this.PacienteEncontrado = "El paciente es " + result.FullName + " . Sus datos se autocompletaran";

            await Application.Current.MainPage.DisplayAlert(
                    "Exito",
                    "El paciente " + result.FullName + " se encuentra registrado en la base de datos",
                    "Aceptar");

            this.IsRunning = false;

        }

        private async void CargarObraSocial() {

            this.IsRunning = true;
            var busquedaObraSocial = await this.apiService.GetList<ObraSocialResponse>(
               "http://sagradaapi.azurewebsites.net",
               "/api",
               "/ObraSocials",
               MainViewModel.GetInstance().Token.TokenType,
               MainViewModel.GetInstance().Token.AccessToken);

            if (!busquedaObraSocial.IsSuccess) {

                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaObraSocial.Message,
                    "Aceptar");
                return;
            }

            ObraSocialResponses = (List<ObraSocialResponse>)busquedaObraSocial.Result;
            this.IsRunning = false;
        }

        private async void CargarTrabajaObraSocial() {

            this.IsRunning = true;
            var busquedaObraSocialMedico = await this.apiService.GetList<ObraSocialMedicoResponse>(
               "http://sagradaapi.azurewebsites.net",
               "/api",
               "/ObraSocialMedicoes/GetObraSocialByMedico",
               MainViewModel.GetInstance().Token.TokenType,
               MainViewModel.GetInstance().Token.AccessToken,
               medicoSeleccionado.IdMedico);

            if (!busquedaObraSocialMedico.IsSuccess) {

                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaObraSocialMedico.Message,
                    "Aceptar");
                return;
            }

            var resultadoObraSocialMedico = (List<ObraSocialMedicoResponse>)busquedaObraSocialMedico.Result;

            TrabajaObraSocial = "El medico " + medicoSeleccionado.FullName + " trabaja con las siguientes obras sociales: ";

            foreach(var item in resultadoObraSocialMedico) {

                TrabajaObraSocial += item.ObraSocial.Nombre + ", ";
            }

            this.IsRunning = false;
        }

        private async void CargarTrabajaFecha() {

            this.IsRunning = true;
            var busquedaHorario = await this.apiService.GetList<HorarioResponse>(
               "http://sagradaapi.azurewebsites.net",
               "/api",
               "/Horarios",
               MainViewModel.GetInstance().Token.TokenType,
               MainViewModel.GetInstance().Token.AccessToken,
               medicoSeleccionado.IdMedico);

            if (!busquedaHorario.IsSuccess) {

                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaHorario.Message,
                    "Aceptar");
                return;
            }

            var resultadoHorario = (List<HorarioResponse>)busquedaHorario.Result;

            TrabajaFecha = "El medico " + medicoSeleccionado.FullName + " trabaja en los siguientes horarios: ";

            foreach (var item in resultadoHorario) {

                TrabajaFecha += item.Dia + ", de " + item.HoraInicio + " a " + item.HoraFin + ". ";
            }

            this.IsRunning = false;
        }

    }
}
