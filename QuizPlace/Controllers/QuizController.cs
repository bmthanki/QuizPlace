using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuizPlace.Controllers
{
    public class QuizController : Controller
    {
        // GET: Quiz


        public static Models.Question question= new Question();

        public ActionResult AddQuiz(Models.Quiz quiz)
        {
            if (Request.IsAuthenticated)
            {
                List<Question> questionList = this.ListOfQuestions();
                quiz.question = new List<Question>(questionList);
                ModelState.Clear();
                return View(quiz);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private List<Question> ListOfQuestions()
        {
            List<Question> questionList = question.GetQuestions();
            return questionList;
        }
        public ActionResult AddQuestions(Models.Question question)
        {
            if (Request.IsAuthenticated)
            {
                List<Question> questionList = question.GetQuestions();
                return View(questionList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        

        [HttpPost]
        public JsonResult InsertData(Models.Question inStudent)
        {
            String result = "0";

            if(String.IsNullOrEmpty(inStudent.title)|| String.IsNullOrEmpty(inStudent.option1)|| String.IsNullOrEmpty(inStudent.option2))
            {
                result = "3";
            }
            else if ((String.IsNullOrEmpty(inStudent.option3) || String.IsNullOrEmpty(inStudent.option4)) && (inStudent.answer==3 || inStudent.answer == 4))
            {
                result = "4";
            }
            else if (inStudent.option1.Equals(inStudent.option2, StringComparison.InvariantCultureIgnoreCase)
               || inStudent.option1.Equals(inStudent.option3, StringComparison.InvariantCultureIgnoreCase)
               || inStudent.option1.Equals(inStudent.option4, StringComparison.InvariantCultureIgnoreCase)
               || inStudent.option2.Equals(inStudent.option3, StringComparison.InvariantCultureIgnoreCase)
               || inStudent.option2.Equals(inStudent.option4, StringComparison.InvariantCultureIgnoreCase)
               || inStudent.option3.Equals(inStudent.option4, StringComparison.InvariantCultureIgnoreCase))
            {
                result = "2";
            }
            else if (inStudent.answer > 4 || inStudent.answer < 1)
            {
                result = "5";
            }
            else
            {
                inStudent.AddQuestions(inStudent);
                result = "1";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveData(Models.Question student)
        {
            int index = 0;
            String result = String.Empty;
            if (String.IsNullOrEmpty(student.title) || String.IsNullOrEmpty(student.option1) || String.IsNullOrEmpty(student.option2) )
            {
                result = "3";
            }
            else if ((String.IsNullOrEmpty(student.option3) || String.IsNullOrEmpty(student.option4)) && (student.answer == 3 || student.answer == 4))
            {
                result = "4";
            }
            else if (student.option1.Equals(student.option2, StringComparison.InvariantCultureIgnoreCase) 
                || student.option1.Equals(student.option3, StringComparison.InvariantCultureIgnoreCase) 
                || student.option1.Equals(student.option4, StringComparison.InvariantCultureIgnoreCase) 
                || student.option2.Equals(student.option3, StringComparison.InvariantCultureIgnoreCase) 
                || student.option2.Equals(student.option4, StringComparison.InvariantCultureIgnoreCase) 
                || student.option3.Equals(student.option4, StringComparison.InvariantCultureIgnoreCase))
            {
                result = "2";
            }
            
            else if (student.answer > 4 || student.answer < 1)
            {
                result = "5";
            }
            else if(index >= 0)
            {
                student.UpdateQuestion(student);
                result = "1";
            }
            

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertQuizs(Models.Quiz quiz)
        {

            int[] ia = quiz.questions.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
           if(quiz.questions.EndsWith(","))
            {
                quiz.questions = quiz.questions.Substring(0, quiz.questions.Length - 1);
            }
            var distinctArray = ia.Distinct().ToArray();
            List<QuizQuestionMap> map = new List<QuizQuestionMap>();
            
            if (true)
            {
                int QuizId=quiz.AddQuiz(quiz);
                QuizQuestionMap qq = null;
                foreach (int a in distinctArray)
                {
                    qq = new QuizQuestionMap();
                    qq.questionid=a;
                    qq.quizid=QuizId;
                    map.Add(qq);
                }
                qq.AddQuestions(map);
                TempData["UserMessage"] = new { CssClassName = "alert-sucess", Title = "Success!", Message = "Operation Done." };
                return RedirectToAction("Success","Home");
            }
            else
            {
                TempData["UserMessage"] = new { CssClassName = "alert-error", Title = "Error!", Message = "Operation Failed." };
                return RedirectToAction("Success", "Home");
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ValidateDateEqualOrGreater([Bind(Prefix = "due_date")]DateTime Date)
        {
            // validate your date here and return True if validated
            if (Date >= DateTime.Now)
            {
                return Json(true);
            }
            return Json(false);
        }
    }
}

