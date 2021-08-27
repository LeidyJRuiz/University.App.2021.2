﻿namespace University.App.Helpers
{
    public class Endpoint
    {
        public static string URL_BASE_UNIVERSITY_API { get; set; } = "https://university-api.azurewebsites.net/";
        public static string GET_COURSES { get; set; } = "api/Courses/GetCourses/";
        public static string GET_STUDENTS { get; set; } = "api/Students/GetStudents/";

        public static string POST_COURSES { get; set; } = "api/Courses";

    }
}
