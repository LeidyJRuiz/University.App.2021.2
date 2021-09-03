using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.App.Views.Forms;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class OfficesItemViewModel : OfficeDTO
    {
        #region Fields
        private ApiService _apiService;

        #endregion

        #region Methods

        async void EditOffice()
        {
            MainViewModel.GetInstance().EditOffice = new EditOfficeViewModel(this);

            await Application.Current.MainPage.Navigation.PushAsync(new EditOfficePage());

        }

        async void DeleteOffice()
        {

            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Notificación", "Delete Confirm", "Yes",
                    "No");

                if (!answer)
                    return;

                var connection = await _apiService.CheckConnection();
                if (!connection)
                {

                    await Application.Current.MainPage.DisplayAlert("Notificación", "No internet conecction", "Cancel");
                    return;
                }


                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<CourseDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.DELETE_OFFICES + this.InstructorID, null, ApiService.Method.Delete);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                await Application.Current.MainPage.DisplayAlert("Notificación", message, "Cancel");


            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");

            }
        }

        #endregion
        #region Commands
        public Command EditOfficesCommand { get; set; }
        public Command DeleteOfficesCommand { get; set; }



        #endregion
        #region Constructor
        public OfficesItemViewModel()
        {
            this._apiService = new ApiService();
            this.DeleteOfficesCommand = new Command(DeleteOffice);
            this.EditOfficesCommand = new Command(EditOffice);

        }
        #endregion
    }
}
