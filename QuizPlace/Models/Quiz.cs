using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Mvc;

namespace Models
{
    public class Quiz
    {

        public int id { get; set; }
        [Required(ErrorMessage = "Please enter Quiz Name")]
        [Display(Name = "Quiz Name")]
        public string name { get; set; }
        [Required(ErrorMessage = "Please enter Description")]
        [Display(Name = "Description")]
        public string descriptions { get; set; }
        [Required(ErrorMessage = "Please select Shuffle Question")]
        [Display(Name = "Shuffle Question")]
        public Nullable<bool> shuffle { get; set; }
        [Required(ErrorMessage = "Please enter Show Answers")]
        [Display(Name = "Show Answers")]
        public Nullable<bool> see { get; set; }
        [Required(ErrorMessage = "Please enter Multiple Attempts Allowed")]
        [Display(Name = "Multiple Attempts Allowed")]
        public Nullable<bool> multiple_attempts { get; set; }
        [Required(ErrorMessage = "Please enter Due Date")]
        [Display(Name = "Due Date")]
        [Remote("ValidateDateEqualOrGreater","Quiz", HttpMethod = "POST",ErrorMessage = "Date isn't equal or greater than current date.")]
        public Nullable<System.DateTime> due_date { get; set; }
        [Required(ErrorMessage = "Please enter Time for Quiz")]
        [Display(Name = "Time for Quiz")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please Enter Numbers only")]
        public int time { get; set; }
        [Required(ErrorMessage = "Please enter Points Per Question")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please Enter Numbers only")]
        [Display(Name = "Points Per Question")]
        public int point_per_question { get; set; }
        [Required(ErrorMessage = "Please enter Question")]
        [RegularExpression("^[0-9,]*$", ErrorMessage = "Please Enter Comma Separated Values like 1,2,3,4,10")]
        [Display(Name = "Question(Enter in Comma Separated Values")]
        public string questions { get; set; }
        public List<Question> question { get; set; }

        public static string conndb = ConfigurationManager.ConnectionStrings["conndb"].ConnectionString;

        public int AddQuiz(Models.Quiz quiz)
        {
            var _sql = "INSERT INTO [dbo].[System_Quiz]([name],[descriptions],[shuffle],[see],[multiple_attempts],[due_date],[time],[point_per_question]) VALUES (@Name,@Descriptions,@Shuffle,@See,@Multiple,@Due_Date,@Time,@Points) SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using (var cn = new SqlConnection(conndb))
            {
                
                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@Name", quiz.name);
                cmd.Parameters.AddWithValue("@Descriptions", quiz.descriptions);
                cmd.Parameters.AddWithValue("@Shuffle", quiz.shuffle);
                cmd.Parameters.AddWithValue("@See", quiz.see);
                cmd.Parameters.AddWithValue("@Multiple", quiz.multiple_attempts);
                cmd.Parameters.AddWithValue("@Due_Date", quiz.due_date);
                cmd.Parameters.AddWithValue("@Time", quiz.time); 
                cmd.Parameters.AddWithValue("@Points", quiz.point_per_question);
                cn.Open();
                int output =(int)cmd.ExecuteScalar();
                cn.Close();
                return output;
            }
        }
    }
}

