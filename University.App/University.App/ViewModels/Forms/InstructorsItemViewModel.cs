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
    public class InstructorsItemViewModel : InstructorDTO
    {
        #region Fields
        private ApiService _apiService;

        #endregion

        #region Methods

        async void EditInstructors()
        {
            MainViewModel.GetInstance().EditInstructors = new EditInstructorsViewModel(this);

            await Application.Current.MainPage.Navigation.PushAsync(new EditInstructorsPage());

        }

        async void DeleteInstructors()
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

                var responseDTO = await _apiService.RequestAPI<InstructorDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.DELETE_INSTRUCTORS + this.ID, null, ApiService.Method.Delete);

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
        public Command EditInstructorsCommand { get; set; }
        public Command DeleteInstructorsCommand { get; set; }



        #endregion
        #region Constructor
        public InstructorsItemViewModel()
        {
            this._apiService = new ApiService();
            this.DeleteInstructorsCommand = new Command(DeleteInstructors);
            this.EditInstructorsCommand = new Command(EditInstructors);

        }
        #endregion
    }
}
