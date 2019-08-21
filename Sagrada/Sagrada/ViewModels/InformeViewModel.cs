using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using Sagrada.Dominio;
using Sagrada.Helpers;
using Sagrada.Models;
using Sagrada.Services;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sagrada.ViewModels
{
    public class InformeViewModel : BaseViewModel
    {
        private ApiService apiService;

        public ICommand PagosCommand { get { return new RelayCommand(Pagos); } }

        public ICommand ObrasSocialesCommand { get { return new RelayCommand(ObrasSociales); } }

        public ICommand TurnosCommand { get { return new RelayCommand(Turnos); } }        

        public ICommand PacientesEsperaCommand { get { return new RelayCommand(PacientesEspera); } }

        public ICommand PacientesAtendidosCommand { get { return new RelayCommand(PacientesAtendidos); } }        

        public ICommand PagosMedicoCommand { get { return new RelayCommand(PagosMedico); } }      

        public ICommand ObrasSocialesMedicoCommand { get { return new RelayCommand(ObrasSocialesMedico); } }


        public ICommand MedicosPeriodoCommand { get { return new RelayCommand(MedicosPeriodo); } }

        public ICommand AtendidosPeriodoCommand { get { return new RelayCommand(AtendidosPeriodo); } }
       
        public ICommand RecaudadoPeriodoCommand { get { return new RelayCommand(RecaudadoPeriodo); } }
        
        public ICommand RecaudadoMedicoCommand { get { return new RelayCommand(RecaudadoMedico); } }

        

        public InformeViewModel() {
            apiService = new ApiService();
        }

        private async void Pagos() {


            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetList<PagoResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/Pagoes",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!response.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var result = (List<PagoResponse>)response.Result;

            List<object> data = new List<object>();

            foreach(var item in result) {

                if(item.IdTarjeta == null) {
                    item.FormaDePago = "Efectivo";

                } else {
                    item.FormaDePago = "Tarjeta";
                }

                data.Add(new { FormaDePago = item.FormaDePago, Paciente = item.Usuario.FullName, Monto = item.Monto, Fecha = item.Fecha });
            }
           
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();          
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy") + "\n\n", font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Listado de pagos particulares, discriminado por modalidad de pago", new PdfStandardFont(PdfFontFamily.TimesRoman,15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("ListadoDePagosParticulares.pdf", "application/pdf", stream);
        }

        private async void ObrasSociales() {

           

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetList<ObraSocialUsuarioResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/ObraSocialUsuarios",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!response.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var result = (List<ObraSocialUsuarioResponse>)response.Result;

            List<object> data = new List<object>();

            foreach (var item in result) {

                data.Add(new { ObraSocial = item.ObraSocial.Nombre, Paciente = item.Usuario.FullName, NroDeAfiliado = item.NroAfiliado });
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy") + "\n\n", font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Listado de pacientes por obra social", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("ListadoDePacientesPorObraSocial.pdf", "application/pdf", stream);
        }

        private async void Turnos() {

            

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetList<TurnoResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/Turnoes",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!response.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var result = (List<TurnoResponse>)response.Result;

            List<object> data = new List<object>();

            foreach (var item in result) {

                data.Add(new { Fecha = item.Fecha, Paciente = item.Usuario.FullName, Medico = item.Medico.FullName });
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy") + "\n\n", font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Listado de turnos de pacientes", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("ListadoDeTurnosDePacientes.pdf", "application/pdf", stream);
        }

        private async void PacientesEspera() {

           

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetList<UsuarioEnEsperaResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/UsuarioEnEsperas",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!response.IsSuccess) {
               
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var result = (List<UsuarioEnEsperaResponse>)response.Result;

            List<object> data = new List<object>();

            foreach (var item in result) {

                data.Add(new { Fecha = item.Fecha, Paciente = item.Usuario.FullName, Medico = item.Medico.FullName });
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy") + "\n\n", font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Listado de espera de pacientes", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("ListadoDeEsperaDePacientes.pdf", "application/pdf", stream);
        }

        private async void PacientesAtendidos() {

            

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetList<UsuarioAtendidoResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/UsuarioAtendidoes",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!response.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var result = (List<UsuarioAtendidoResponse>)response.Result;

            List<object> data = new List<object>();

            foreach (var item in result) {

                data.Add(new { Fecha = item.Fecha, Paciente = item.Usuario.FullName, Medico = item.CobroMedico.Medico.FullName, Honorarios = item.CobroMedico.Honorarios });
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy") + "\n\n", font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Listado de pacientes atendidos", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("ListadoDePacientesAtendidos.pdf", "application/pdf", stream);
        }

        private async void PagosMedico() {

            

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetList<PagoResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/Pagoes",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!response.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var result = (List<PagoResponse>)response.Result;

            List<object> data = new List<object>();

            foreach (var item in result) {

                if (item.IdTarjeta == null) {
                    item.FormaDePago = "Efectivo";

                } else {
                    item.FormaDePago = "Tarjeta";
                }

                data.Add(new { FormaDePago = item.FormaDePago, Medico = item.Usuario.FullName, Paciente = item.Usuario.FullName, Monto = item.Monto, Fecha = item.Fecha });
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy") + "\n\n", font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Listado de pagos particulares, discriminado por modalidad de pago y medico", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("ListadoDePagosParticularesMedico.pdf", "application/pdf", stream);
        }

        private async void ObrasSocialesMedico() {

            

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
               
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetList<UsuarioAtendidoResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/UsuarioAtendidoes",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!response.IsSuccess) {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var result = (List<UsuarioAtendidoResponse>)response.Result;
            
            List<object> data = new List<object>();

            foreach (var item in result) {

                data.Add(new { Fecha = item.Fecha, Paciente = item.Usuario.FullName, Medico = item.CobroMedico.Medico.FullName, Honorarios = item.CobroMedico.Honorarios });
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy") + "\n\n", font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Listado de pacientes atendidos", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("ListadoDePacientesAtendidos.pdf", "application/pdf", stream);
        }

        private async void MedicosPeriodo() {

            int Dni;
            DateTime fechaInicio;
            DateTime fechaFin;

            var promptConfig = new PromptConfig();
            promptConfig.InputType = InputType.Number;
            promptConfig.IsCancellable = true;
            promptConfig.OkText = "Aceptar";
            promptConfig.CancelText = "Cancelar";
            promptConfig.Message = "Ingrese el DNI del paciente";
            var resultNombreCompleto = await UserDialogs.Instance.PromptAsync(promptConfig);

            if (resultNombreCompleto.Ok) {
                Dni = Convert.ToInt32(resultNombreCompleto.Text);
            } else {
                return;
            }

            var resultFechaInicio = await UserDialogs.Instance.DatePromptAsync(
                    "Selecciona la fecha de inicio",
                    DateTime.Now);

            if (resultFechaInicio.Ok) {
                fechaInicio = resultFechaInicio.SelectedDate;
            } else {
                return;
            }

            var resultFechaFin = await UserDialogs.Instance.DatePromptAsync(
                    "Selecciona la fecha de finalizacion",
                    DateTime.Now);

            if (resultFechaFin.Ok) {
                fechaFin = resultFechaFin.SelectedDate;
            } else {
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var busquedaUsuario = await this.apiService.Get<Usuario>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/Usuarios/GetUsuariosByDni",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken,
                        Dni);

            if (!busquedaUsuario.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaUsuario.Message,
                    "Aceptar");
                return;
            }

            var resultadoUsuario = (Usuario)busquedaUsuario.Result;

            var busquedaUsuarioAtendidos = await this.apiService.GetList<UsuarioAtendidoResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/UsuarioAtendidoes",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken,
                        resultadoUsuario.IdUsuario);

            if (!busquedaUsuarioAtendidos.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaUsuarioAtendidos.Message,
                    "Aceptar");
                return;
            }

            var resultadoUsuarioAtendidos = (List<UsuarioAtendidoResponse>)busquedaUsuarioAtendidos.Result;

            List<object> data = new List<object>();

            foreach(var item in resultadoUsuarioAtendidos) {

                var busquedaMedico = await this.apiService.Get<Medico>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/Medicos",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken,
                        item.CobroMedico.IdMedico);

                if (!busquedaMedico.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaMedico.Message,
                        "Aceptar");
                    return;
                }

                var resultadoMedico = (Medico)busquedaMedico.Result;

                data.Add(new { Medico = resultadoMedico.FullName, DNI = resultadoMedico.DNI, EMail = resultadoMedico.Email, Fecha = item.Fecha });
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy"), font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Medicos que atendieron a " + resultadoUsuario.FullName + " entre " + fechaInicio.ToString("dd MMM yyyy") + " y " + fechaFin.ToString("dd MMM yyyy") + "", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("MedicosQueAtendieronAUnPacienteEnUnPeriodoDado.pdf", "application/pdf", stream);

        }

        private async void AtendidosPeriodo() {

            string formaDePago;
            string obraSocial;
            DateTime fechaInicio;
            DateTime fechaFin;

            var promptConfig = new PromptConfig();
            promptConfig.InputType = InputType.Name;
            promptConfig.IsCancellable = true;
            promptConfig.OkText = "Aceptar";
            promptConfig.CancelText = "Cancelar";
            promptConfig.Message = "Ingrese Forma de pago del paciente (Efectivo o Tarjeta)";
            var resultFormaDePago = await UserDialogs.Instance.PromptAsync(promptConfig);

            if (resultFormaDePago.Ok) {
                formaDePago = resultFormaDePago.Text;
            } else {
                return;
            }

            var promptConfig2 = new PromptConfig();
            promptConfig2.InputType = InputType.Name;
            promptConfig2.IsCancellable = true;
            promptConfig2.OkText = "Aceptar";
            promptConfig2.CancelText = "Cancelar";
            promptConfig2.Message = "Ingrese la obra social del paciente";
            var resultObraSocial = await UserDialogs.Instance.PromptAsync(promptConfig2);

            if (resultObraSocial.Ok) {
                obraSocial = resultObraSocial.Text;
            } else {
                return;
            }

            var resultFechaInicio = await UserDialogs.Instance.DatePromptAsync(
                    "Selecciona la fecha de inicio",
                    DateTime.Now);

            if (resultFechaInicio.Ok) {
                fechaInicio = resultFechaInicio.SelectedDate;
            } else {
                return;
            }

            var resultFechaFin = await UserDialogs.Instance.DatePromptAsync(
                    "Selecciona la fecha de finalizacion",
                    DateTime.Now);

            if (resultFechaFin.Ok) {
                fechaFin = resultFechaFin.SelectedDate;
            } else {
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var busquedaUsuarioAtendidos = await this.apiService.GetList<UsuarioAtendidoResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/UsuarioAtendidoes",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!busquedaUsuarioAtendidos.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaUsuarioAtendidos.Message,
                    "Aceptar");
                return;
            }

            var resultadoUsuarioAtendidos = (List<UsuarioAtendidoResponse>)busquedaUsuarioAtendidos.Result;

            var busquedaObraSocial = await this.apiService.Get<ObraSocial>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/ObraSocials/GetObraSocialByNombre",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken,
                        obraSocial);

            if (!busquedaObraSocial.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaObraSocial.Message,
                    "Aceptar");
                return;
            }

            var resultadoObraSocial = (ObraSocial)busquedaObraSocial.Result;

            List<object> data = new List<object>();

            foreach (var item in resultadoUsuarioAtendidos) {

                var busquedaPago = await this.apiService.Post<Pago>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Pagoes/GetPagoByFecha",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken,
                            new { Fecha = DateTime.Now, IdUsuario = item.IdUsuario });

                if (!busquedaPago.IsSuccess) {

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        busquedaPago.Message,
                        "Aceptar");
                    return;
                }

                var resultadoPago = (Pago)busquedaPago.Result;

                if(formaDePago.ToLower() == "efectivo" && resultadoPago.IdTarjeta == null && item.IdObraSocial == resultadoObraSocial.IdObraSocial 
                    || formaDePago.ToLower() == "tarjeta" && resultadoPago.IdTarjeta != null && item.IdObraSocial == resultadoObraSocial.IdObraSocial) {

                    data.Add(new { Nombre = item.Usuario.FullName, DNI = item.Usuario.DNI, FormaDePago = formaDePago.ToLower(), ObraSocial = obraSocial.ToLower(), Fecha = item.Fecha });
                }               
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy"), font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Total de pacientes atendidos entre " + fechaInicio.ToString("dd MMM yyyy") + " y " + fechaFin.ToString("dd MMM yyyy") + ". Forma de pago: " + formaDePago + "  Obra social: " + obraSocial + "", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("TotalDePacientesAtendidosEnUnPeríodoDeTiempoDeterminado.pdf", "application/pdf", stream);


        }

        private async void RecaudadoPeriodo() {

            string formaDePago;
            DateTime fechaInicio;
            DateTime fechaFin;

            var promptConfig = new PromptConfig();
            promptConfig.InputType = InputType.Name;
            promptConfig.IsCancellable = true;
            promptConfig.OkText = "Aceptar";
            promptConfig.CancelText = "Cancelar";
            promptConfig.Message = "Ingrese Forma de pago del paciente (Efectivo o Tarjeta)";
            var resultFormaDePago = await UserDialogs.Instance.PromptAsync(promptConfig);

            if (resultFormaDePago.Ok) {
                formaDePago = resultFormaDePago.Text;
            } else {
                return;
            }

            var resultFechaInicio = await UserDialogs.Instance.DatePromptAsync(
                    "Selecciona la fecha de inicio",
                    DateTime.Now);

            if (resultFechaInicio.Ok) {
                fechaInicio = resultFechaInicio.SelectedDate;
            } else {
                return;
            }

            var resultFechaFin = await UserDialogs.Instance.DatePromptAsync(
                    "Selecciona la fecha de finalizacion",
                    DateTime.Now);

            if (resultFechaFin.Ok) {
                fechaFin = resultFechaFin.SelectedDate;
            } else {
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var busquedaPago = await this.apiService.GetList<PagoResponse>(
                            "http://sagradaapi.azurewebsites.net",
                            "/api",
                            "/Pagoes",
                            MainViewModel.GetInstance().Token.TokenType,
                            MainViewModel.GetInstance().Token.AccessToken);

            if (!busquedaPago.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaPago.Message,
                    "Aceptar");
                return;
            }

            var resultadoPago = (List<PagoResponse>)busquedaPago.Result;

            List<object> data = new List<object>();

            int totalPago = 0;
            foreach (var item in resultadoPago) {

                if(formaDePago.ToLower() == "efectivo" && item.IdTarjeta == null || formaDePago.ToLower() == "tarjeta" && item.IdTarjeta != null) {

                    data.Add(new { Nombre = item.Usuario.FullName, DNI = item.Usuario.DNI, Monto = item.Monto, Fecha = item.Fecha });
                    totalPago += item.Monto;
                }
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy"), font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Total recaudado entre " + fechaInicio.ToString("dd MMM yyyy") + " y " + fechaFin.ToString("dd MMM yyyy") + ". Forma de pago: " + formaDePago + "", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            graphics.DrawString("   TOTAL: " + totalPago + "", font, PdfBrushes.Black, new PointF(50, 100));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("TotalRecaudadoEnUnPeríodoDeTiempoDeterminado.pdf", "application/pdf", stream);

        }

        private async void RecaudadoMedico() {

            int DniMedico;
            DateTime fechaInicio;
            DateTime fechaFin;

            var promptConfig = new PromptConfig();
            promptConfig.InputType = InputType.Number;
            promptConfig.IsCancellable = true;
            promptConfig.OkText = "Aceptar";
            promptConfig.CancelText = "Cancelar";
            promptConfig.Message = "Ingrese el DNI del medico";
            var resultNombreCompleto = await UserDialogs.Instance.PromptAsync(promptConfig);

            if (resultNombreCompleto.Ok) {
                DniMedico = Convert.ToInt32(resultNombreCompleto.Text);
            } else {
                return;
            }

            var resultFechaInicio = await UserDialogs.Instance.DatePromptAsync(
                    "Selecciona la fecha de inicio",
                    DateTime.Now);

            if (resultFechaInicio.Ok) {
                fechaInicio = resultFechaInicio.SelectedDate;
            } else {
                return;
            }

            var resultFechaFin = await UserDialogs.Instance.DatePromptAsync(
                    "Selecciona la fecha de finalizacion",
                    DateTime.Now);

            if (resultFechaFin.Ok) {
                fechaFin = resultFechaFin.SelectedDate;
            } else {
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var busquedaMedico = await this.apiService.Get<Medico>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/Medicos/GetMedicoByDni",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken,
                        DniMedico);

            if (!busquedaMedico.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaMedico.Message,
                    "Aceptar");
                return;
            }

            var resultadoMedico = (Medico)busquedaMedico.Result;

            var busquedaUsuarioAtendidos = await this.apiService.GetList<UsuarioAtendidoResponse>(
                        "http://sagradaapi.azurewebsites.net",
                        "/api",
                        "/UsuarioAtendidoes",
                        MainViewModel.GetInstance().Token.TokenType,
                        MainViewModel.GetInstance().Token.AccessToken);

            if (!busquedaUsuarioAtendidos.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaUsuarioAtendidos.Message,
                    "Aceptar");
                return;
            }

            var resultadoUsuarioAtendidos = (List<UsuarioAtendidoResponse>)busquedaUsuarioAtendidos.Result;

            List<object> data = new List<object>();
            int totalPago = 0;

            foreach (var item in resultadoUsuarioAtendidos) {

                if(item.CobroMedico.IdMedico == resultadoMedico.IdMedico) {

                    data.Add(new { Paciente = item.Usuario.FullName, DNI = item.Usuario.DNI, ObraSocial = item.ObraSocial.Nombre, Monto = item.CobroMedico.Honorarios, Fecha = item.Fecha });
                    totalPago += item.CobroMedico.Honorarios;
                }
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString("   Sagrada Familia                                    " + DateTime.Now.ToString("dd MMM yyyy"), font, PdfBrushes.Black, new PointF(0, 0));
            graphics.DrawString("   Total recaudado entre " + fechaInicio.ToString("dd MMM yyyy") + " y " + fechaFin.ToString("dd MMM yyyy") + ". Medico: " + resultadoMedico.FullName + "", new PdfStandardFont(PdfFontFamily.TimesRoman, 15), PdfBrushes.Black, new PointF(0, 30));
            graphics.DrawString("   TOTAL: " + totalPago + "", font, PdfBrushes.Black, new PointF(50, 100));
            IEnumerable<object> dataTable = data;
            pdfGrid.DataSource = dataTable;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("TotalRecaudadoEnUnPeríodoDeTiempoDeterminado.pdf", "application/pdf", stream);
        }
    }
}
