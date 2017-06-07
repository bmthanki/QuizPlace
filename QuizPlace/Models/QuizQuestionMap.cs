using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Models
{
    public class QuizQuestionMap 
    {
        public int quizid { get; set; }
        public int questionid { get; set; }

        public static string conndb = ConfigurationManager.ConnectionStrings["conndb"].ConnectionString;

        public void AddQuestions(List<Models.QuizQuestionMap> listmap)
        {
            var _sql = "insert into [dbo].[Quiz_Question_Map] values(@quizid,@questionid)";

            using (var cn = new SqlConnection(conndb))
            {

                var cmd = new SqlCommand(_sql, cn);
                
                foreach (QuizQuestionMap map in listmap)
                {
                    cn.Open();
                    cmd.Parameters.AddWithValue("@quizid", map.quizid);
                    cmd.Parameters.AddWithValue("@questionid", map.questionid);
                    SqlDataReader nwReader = cmd.ExecuteReader();
                    cn.Close();
                    cmd.Parameters.Clear();
                }
                
            }
        }
    }
}