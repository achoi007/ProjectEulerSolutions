using ProjectEulerSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectEulerSolutions.Controllers
{
    public class SetOneController : Controller
    {
        private string Name = "SetOne";

        // Helper function used to display question
        private ViewResult ViewQuestion(string mesg, int questionNum)
        {
            return View("Question", new Question(mesg, questionNum, Name));
        }

        // Helper function used to display answer
        private ViewResult ViewAnswer(int questionNum, string mesg, ulong value)
        {
            return View("Answer", new Answer(questionNum, mesg, value, Name));
        }

        // Helper function used to display answer
        private ViewResult ViewAnswer(int questionNum, string mesg, long value)
        {
            return View("Answer", new Answer(questionNum, mesg, value, Name));
        }


        public ActionResult Index()
        {
            int[] questions = new int[] { 7, 10, 27, 35, };
            return View(questions);
        }

        public ActionResult Problem7()
        {
            return ViewQuestion("What is the n-th prime number?", 7);
        }

        [HttpPost]
        public ActionResult Problem7(uint n)
        {
            var cal = new PrimeCalculator(n);
            cal.ExtendToLength(n);
            return ViewAnswer(7, "The " + n + "-th prime number is", cal.LastPrime);
        }

        public ActionResult Problem10()
        {
            return ViewQuestion("What is the sum of all primes below n?", 10);
        }

        [HttpPost]
        public ActionResult Problem10(ulong n)
        {
            var cal = new PrimeCalculator((uint)n / 3);
            cal.ExtendToMinimumGT(n);
            var sum = cal.Primes.Sum() - cal.LastPrime;
            return ViewAnswer(10, "The sum of all primes below " + n + " is", sum);
        }

        public ActionResult Problem27()
        {
            return ViewQuestion("Quadratic primes with abs(coefficients) < n?", 27);
        }

        [HttpPost]
        public ActionResult Problem27(uint n)
        {
            ulong max = 2 * n * n + n;  // because n^2 + n*n + n is divisible by n
            var cal = new PrimeCalculator((uint)max / 3);

            int max_a = 0, max_b = 0, max_i = 0;
            int minus_n = (-1 * (int)n) + 1;
            for (int a = minus_n; a < n; a++)
            {
                for (int b = minus_n; b < n; b++)
                {
                    int i;
                    for (i = 0; i < n; i++)
                    {
                        long p = i * i + a * i + b;
                        if (p <= 0)
                        {
                            break;
                        }

                        if (!cal.IsPrimeAutoExpand((ulong) p))
                        {
                            break;
                        }
                    }

                    if (i > max_i)
                    {
                        max_i = i;
                        max_a = a;
                        max_b = b;
                    }
                }
            }

            string s = string.Format("a = {0} b = {1} n = 0..{2} a*b = ", max_a, max_b, max_i);
            return ViewAnswer(27, s, max_a * max_b);
        }

        public ActionResult Problem35()
        {
            return ViewQuestion("Circular primes below n?", 35);
        }

        [HttpPost]
        public ActionResult Problem35(ulong n)
        {
            var cal = new PrimeCalculator((uint) n / 3);
            cal.ExtendToMinimumGT(n);

            Func<ulong, bool> isCircular = (i) =>
            {
                ulong c = i;
                uint numDigits = Utils.NumOfDigits(c);
                ulong factor = Utils.PowerOfTen(numDigits - 1);
                do
                {
                    ulong lower = (c / 10);
                    ulong higher = (c % 10) * factor;   // Move rightmost digit to leftmost position
                    c = higher + lower;

                    if (!cal.IsPrimeInRange(c))
                    {
                        return false;
                    }
                } 
                while (c != i);

                return true;
            };

            int cnt = cal.Primes.Count(isCircular);
            return ViewAnswer(35, "The number of circular primes below " + n + " is", (uint)cnt);
        }
    }
}
