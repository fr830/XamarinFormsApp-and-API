using System.Windows.Input;
using Sagrada.Dominio;
using GalaSoft.MvvmLight.Command;
using Sagrada.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Sagrada.Services;
using Xamarin.Forms;

namespace Sagrada.ViewModels {
    

    public class RegisterViewModel : BaseViewModel {
        

        private ApiService apiService;
        

        
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        

        
        public ImageSource ImageSource {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }

        public bool IsEnabled {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public string FirstName {
            get;
            set;
        }

        public string LastName {
            get;
            set;
        }

        public string Email {
            get;
            set;
        }

        public string Telephone {
            get;
            set;
        }

        public int DNI {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }

        public string Confirm {
            get;
            set;
        }
       

        
        public RegisterViewModel() {
            this.apiService = new ApiService();

            this.IsEnabled = true;
            this.ImageSource = "no_image";
        }
        

      
        public ICommand RegisterCommand {
            get {
                return new RelayCommand(Register);
            }
        }

        private async void Register() {
            if (string.IsNullOrEmpty(this.FirstName)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un nombre",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.LastName)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un apellido",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Email)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un email",
                    "Aceptar");
                return;
            }

            if (!RegexUtilities.IsValidEmail(this.Email)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un email valido",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Telephone)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un telefono",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.DNI.ToString())) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un DNI",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Password)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar una contraseña",
                    "Aceptar");
                return;
            }

            if (this.Password.Length < 6) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar una contraseña valida",
                    "Aceptar");
                return;
            }

            

            if (string.IsNullOrEmpty(this.Confirm)) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un valor en confirmar contraseña",
                    "Aceptar");
                return;
            }

            if (this.Password != this.Confirm) {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Las contraseñas no coinciden",
                    "Aceptar");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess) {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    checkConnetion.Message,
                    "Aceptar");
                return;
            }

            byte[] imageArray = null;
            if (this.file != null) {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var user = new Usuario {
                Email = this.Email,
                Nombre = this.FirstName,
                Apellido = this.LastName,
                Telefono = this.Telephone,
                ImageArray = imageArray,
                IdTipoUsuario = 2,
                Password = this.Password,
                DNI = this.DNI,
                
                
            };

           // var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.Post(
                "http://sagradaapi.azurewebsites.net/",
                "/api",
                "/Usuarios",
                user);

            if (!response.IsSuccess) {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            this.IsRunning = false;
            this.IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                "Exito",
                "Usuario Registrado Exitosamente",
                "Aceptar");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public ICommand ChangeImageCommand {
            get {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage() {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported) {
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    "Elige una opcion",
                    "Cancelar",
                    null,
                    "Desde galeria",
                    "Desde camara");

                if (source == "Cancelar") {
                    this.file = null;
                    return;
                }

                if (source == "Desde camara") {
                    this.file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                } else {
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }
            } else {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null) {
                this.ImageSource = ImageSource.FromStream(() => {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
   
    }
}