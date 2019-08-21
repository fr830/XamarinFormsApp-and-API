using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Sagrada.Helpers;
using Sagrada.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sagrada.ViewModels
{
    public class MyProfileViewModel : BaseViewModel {
    /*    #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Properties
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

        public UserLocal User {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MyProfileViewModel() {
            this.apiService = new ApiService();
            //this.dataService = new DataService();

            this.User = MainViewModel.GetInstance().User;
            this.ImageSource = this.User.ImageFullPath;
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand {
            get {
                return new RelayCommand(ChangePassword);
            }
        }

        private async void ChangePassword() {
            MainViewModel.GetInstance().ChangePassword = new ChangePasswordViewModel();
            await App.Navigator.PushAsync(new ChangePasswordPage());
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
                    Languages.SourceImageQuestion,
                    Languages.Cancel,
                    null,
                    Languages.FromGallery,
                    Languages.FromCamera);

                if (source == Languages.Cancel) {
                    this.file = null;
                    return;
                }

                if (source == Languages.FromCamera) {
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

        public ICommand SaveCommand {
            get {
                return new RelayCommand(Save);
            }
        }

        private async void Save() {
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

            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess) {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnetion.Message,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (this.file != null) {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var userDomain = Converter.ToUserDomain(this.User, imageArray);
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.Put(
                apiSecurity,
                "/api",
                "/Users",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                userDomain);

            if (!response.IsSuccess) {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var userApi = await this.apiService.GetUserByEmail(
                apiSecurity,
                "/api",
                "/Users/GetUserByEmail",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                this.User.Email);
            var userLocal = Converter.ToUserLocal(userApi);

            MainViewModel.GetInstance().User = userLocal;
            this.dataService.Update(userLocal);

            this.IsRunning = false;
            this.IsEnabled = true;

            await App.Navigator.PopAsync();
        }
        #endregion */
    }
}
