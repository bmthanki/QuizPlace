using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Models
{
    public partial class Question
    {
        public int ID { get; set; }
        public String title { get; set; }
        public String option1 { get; set; }
        public String option2 { get; set; }
        public String option3 { get; set; }
        public String option4 { get; set; }
        public int answer { get; set; }

        public static string conndb = ConfigurationManager.ConnectionStrings["conndb"].ConnectionString;

        public List<Models.Question> GetQuestions()
        {
            using (var cn = new SqlConnection("Data Source=DESKTOP-B923A70\\SQLEXPRESS;Initial Catalog=quizdb;Integrated Security=True"))
            {
                string _sql = @"SELECT [id],[title],[option1],[option2],[option3],[option4],[answer] FROM [dbo].[System_Questions_Answers] ";
                var cmd = new SqlCommand(_sql, cn);
                cn.Open();
                SqlDataReader nwReader = cmd.ExecuteReader();
                List<Models.Question> questionList = new List<Models.Question>();
                Question question = null;
                while (nwReader.Read())
                {
                    question = new Question();
                    question.ID = (int)nwReader["id"];
                    question.title = (String)nwReader["title"];
                    question.option1 = (String)nwReader["option1"];
                    question.option2 = (String)nwReader["option2"];
                    if(nwReader["option3"]!= DBNull.Value)
                    {
                        question.option3 = (String)nwReader["option3"];
                    }
                    if (nwReader["option4"] != DBNull.Value)
                    {
                        question.option4 = (String)nwReader["option4"];
                    }
                    question.answer = (int)nwReader["answer"];
                    questionList.Add(question);
                }
                nwReader.Close();
                cn.Close();
                return questionList;
            }
        }

        public Question GetId(Models.Question inStudent)
        {
            using (var cn = new SqlConnection("Data Source=DESKTOP-B923A70\\SQLEXPRESS;Initial Catalog=quizdb;Integrated Security=True"))
            {
                var _sql = @"SELECT [id] FROM [dbo].[System_Questions_Answers] where [title]=@title";
                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@title", inStudent.title);
                cn.Open();
                SqlDataReader nwReader = cmd.ExecuteReader();
                while (nwReader.Read())
                {
                    inStudent.ID = (int)nwReader["id"];
                }
                nwReader.Close();
                cn.Close();
                return inStudent;
            }
        }
        public void AddQuestions(Models.Question inStudent)
        {
            var _sql = "insert into [dbo].[System_Questions_Answers] values(@Title,@Option1,@Option2,@Option3,@Option4,@Answer,@Last)";

            using (var cn = new SqlConnection("Data Source=DESKTOP-B923A70\\SQLEXPRESS;Initial Catalog=quizdb;Integrated Security=True"))
            {

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@Title", inStudent.title);
                cmd.Parameters.AddWithValue("@Option1", inStudent.option1);
                cmd.Parameters.AddWithValue("@Option2", inStudent.option2);
                if(inStudent.option3!=null && !inStudent.option3.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@Option3", inStudent.option3);
                }
                else {

                    cmd.Parameters.AddWithValue("@Option3", DBNull.Value);
                }
                if (inStudent.option4 != null && !inStudent.option4.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@Option4", inStudent.option4);
                }
                else
                {

                    cmd.Parameters.AddWithValue("@Option4", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@Answer", inStudent.answer);
                cmd.Parameters.AddWithValue("@Last", DateTime.Now);
                cn.Open();
                SqlDataReader nwReader = cmd.ExecuteReader();
                cn.Close();
            }
        }

        public void UpdateQuestion(Question student)
        {

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-B923A70\\SQLEXPRESS;Initial Catalog=quizdb;Integrated Security=True"))
            {

                String _sql = "EXEC [dbo].[updateQuestions] @id,@title,@option1,@option2,@option3,@option4,@answer";
                var cmd = new SqlCommand(_sql, con);
                student = this.GetId(student);
                cmd.Parameters.AddWithValue("@id", student.ID);
                cmd.Parameters.AddWithValue("@title", student.title);
                cmd.Parameters.AddWithValue("@option1", student.option1);
                cmd.Parameters.AddWithValue("@option2", student.option2);
                if (student.option3 != null && !student.option3.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@option3", student.option3);
                }
                else
                {

                    cmd.Parameters.AddWithValue("@option3", DBNull.Value);
                }
                if (student.option4 != null && !student.option4.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@option4", student.option4);
                }
                else
                {

                    cmd.Parameters.AddWithValue("@option4", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@answer", student.answer);
                con.Open();
                cmd.ExecuteReader();
                con.Close();

            }
            }

        }
    }




