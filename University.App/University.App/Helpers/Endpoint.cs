namespace University.App.Helpers
{
    public class Endpoint
    {
        public static string URL_BASE_UNIVERSITY_API { get; set; } = "https://university-api.azurewebsites.net/";

        #region Courses
        public static string GET_COURSES { get; set; } = "api/Courses/GetCourses/";
        public static string POST_COURSES { get; set; } = "api/Courses/";
        public static string PUT_COURSES { get; set; } = "api/Courses/";
        public static string DELETE_COURSES { get; set; } = "api/Courses/";

        #endregion

        #region Students
        public static string GET_STUDENTS { get; set; } = "api/Students/GetStudents/";
        public static string POST_STUDENTS { get; set; } = "api/Students/";
        public static string PUT_STUDENTS { get; set; } = "api/Students/";
        public static string DELETE_STUDENTS { get; set; } = "api/Students/";
        #endregion

        #region Instructors

        public static string GET_INSTRUCTORS { get; set; } = "api/Instructors/GetInstructors/";
        public static string POST_INSTRUCTORS { get; set; } = "api/Instructors/";
        public static string DELETE_INSTRUCTORS { get; set; } = "api/Instructors/";
        public static string PUT_INSTRUCTORS { get; set; } = "api/Instructors/";

        #endregion

        #region Office
        public static string GET_OFFICES { get; set; } = "api/OfficeAssignments";
        public static string POST_OFFICES { get; set; } = "api/OfficeAssignments/";
        public static string PUT_OFFICES { get; set; } = "api/OfficeAssignments/";
        public static string DELETE_OFFICES { get; set; } = "api/OfficeAssignments/";

        #endregion

        #region  Departments

        public static string GET_DEPARTMENTS { get; set; } = "api/Departments";
        public static string POST_DEPARTMENTS { get; set; } = "api/Departments/";
        public static string PUT_DEPARTMENTS { get; set; } = "api/Departments/";
        public static string DELETE_DEPARTMENTS { get; set; } = "api/Departments/";

        #endregion

    }
}
