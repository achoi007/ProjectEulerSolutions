using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectEulerSolutions.Models
{
    public class Question
    {
        public Question(string q, int number, string controller)
        {
            Query = q;
            Number = number;
            Controller = controller;
        }

        public string Query { get; set; }

        public int Number { get; set; }

        public string Controller { get; set; }
    }
}