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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sagrada.ViewModels
{
    public class MisPacientesItemViewModel
    {
        private ApiService apiService;

        public string NombreCompleto { get; set; }

        public string ObraSocialString { get; set; }

        public string FechaString { get; set; }       

        public int IdUsuario { get; set; }

        public string HistoriaClinica { get; set; }

        public ObraSocial ObraSocial { get; set; }


        public ICommand GenerarHistoriaClinicaCommand { get { return new RelayCommand(GenerarHistoriaClinica); } }

        
        public MisPacientesItemViewModel() {

            apiService = new ApiService();
        }

        private async void GenerarHistoriaClinica() {

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
                             MainViewModel.GetInstance().Token.AccessToken,
                             this.IdUsuario);

            if (!busquedaPacientesAtendidos.IsSuccess) {

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    busquedaPacientesAtendidos.Message,
                    "Aceptar");
                return;
            }

            var resultadoPacientesAtendidos = (List<UsuarioAtendidoResponse>)busquedaPacientesAtendidos.Result;

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            graphics.DrawString("   HISTORIA CLINICA DE " + this.NombreCompleto.ToUpper(), new PdfStandardFont(PdfFontFamily.Helvetica, 20), PdfBrushes.DarkBlue, new PointF(0, 0));

            int i = 0;
            foreach (var item in resultadoPacientesAtendidos) {

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

                var resultadoBusquedaMedico = (Medico)busquedaMedico.Result;

                
                i += 40;
                graphics.DrawString("Atendido por " + resultadoBusquedaMedico.FullName + ", el dia " + item.Fecha.ToString("dddd, dd MMMM yyyy HH:mm") + "\n\n", font, PdfBrushes.Black, new PointF(0, i));
                i += 40;
                graphics.DrawString("Observaciones: " + item.HistoriaClinica, new PdfStandardFont(PdfFontFamily.TimesRoman, 8), PdfBrushes.Black, new PointF(0, i));
                i += 40;
            }

            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 60));
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("HistoriaClinica_" + this.NombreCompleto + ".pdf", "application/pdf", stream);
        }
    }
}
