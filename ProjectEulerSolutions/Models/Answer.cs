using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectEulerSolutions.Models
{
    public class Answer
    {
        public Answer(int number, string mesg, long value, string controller)
        {
            Number = number;
            Message = mesg;
            Value = value;
            Controller = controller;
            StrValue = "";
        }

        public Answer(int number, string mesg, ulong value, string controller) : this(number, mesg, (long) value, controller)
        {
        }

        public Answer(int number, string mesg, string value, string controller) : this(number, mesg, 0, controller)
        {
            StrValue = value;
        }

        public int Number { get; set; }

        public string Message { get; set; }

        public long Value { get; set; }

        public string Controller { get; set; }

        public string StrValue { get; set; }
    }
}