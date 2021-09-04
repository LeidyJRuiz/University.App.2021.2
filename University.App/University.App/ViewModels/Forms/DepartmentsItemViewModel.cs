using System;
using University.App.Helpers;
using University.App.Views.Forms;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class DepartmentsItemViewModel : DepartmentDTO
    {
        #region Fields
        private ApiService _apiService;

        #endregion

        #region Commands
        public Command EditDepartmentsCommand { get; set; }
        public Command DeleteDepartmentsCommand { get; set; }



        #endregion


        #region Methods

        async void EditDepartments()
        {
            MainViewModel.GetInstance().EditDepartments = new EditDepartmentsViewModel(this);

            await Application.Current.MainPage.Navigation.PushAsync(new EditDepartmentsPage());

        }

        async void DeleteDepartments()
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

                var responseDTO = await _apiService.RequestAPI<DepartmentDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.DELETE_DEPARTMENTS + this.DepartmentID, null, ApiService.Method.Delete);

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

        #region Constructor
        public DepartmentsItemViewModel()
        {
            this._apiService = new ApiService();
            this.DeleteDepartmentsCommand = new Command(DeleteDepartments);
            this.EditDepartmentsCommand = new Command(EditDepartments);

        }
        #endregion

    }
}
