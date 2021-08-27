using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class StudentsViewModel:BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<StudentsDTO> _students;
        #endregion

        #region Properties

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }

        public ObservableCollection<StudentsDTO> Students
        {
            get { return this._students; }
            set { this.SetValue(ref this._students, value); }
        }


        #endregion
        #region Constructor
        public StudentsViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetStudents);
            this.RefreshCommand.Execute(null);
        }

        #endregion
        #region Methods
        async void GetStudents()
        {
            try
            {
                this.IsRefreshing = true;
                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Notificación", "No internet conecction", "Cancel");
                    return;
                }
                var responseDTO = await _apiService.RequestAPI<List<StudentsDTO>>(Endpoint.URL_BASE_UNIVERSITY_API, Endpoint.GET_STUDENTS, null, ApiService.Method.Get);

                this.Students = new ObservableCollection<StudentsDTO>((List<StudentsDTO>)responseDTO.Data);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");

            }
        }

        #endregion
        #region  Commands

        public Command RefreshCommand { get; set; }

        #endregion
    }
}
