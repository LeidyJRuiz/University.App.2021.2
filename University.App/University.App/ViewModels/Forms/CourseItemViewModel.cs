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
    public class CourseItemViewModel : CourseDTO
    {
        #region Fields
        private ApiService _apiService;

        #endregion

        #region Methods

        async void EditCourse()
        {
            MainViewModel.GetInstance().EditCourse = new EditCourseViewModel(this);

            await Application.Current.MainPage.Navigation.PushAsync(new EditCoursePage());

        }

        async void DeleteCourse()
        { 

            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Notificación", "Delete Confirm","Yes",
                    "No" );

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
        Endpoint.DELETE_COURSES + this.CourseID, null, ApiService.Method.Delete);

                if (responseDTO.Code< 200 || responseDTO.Code> 299)
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
public Command EditCourseCommand { get; set; }
        public Command DeleteCourseCommand { get; set; }



        #endregion

        #region Constructor
        public CourseItemViewModel()
        {
            this._apiService = new ApiService();
            this.DeleteCourseCommand = new Command(DeleteCourse);
            this.EditCourseCommand = new Command(EditCourse);

        }
        #endregion
    }
}
