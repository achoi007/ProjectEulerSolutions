using Facet.Combinatorics;
using ProjectEulerSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProjectEulerSolutions.Controllers
{
    public class SetOneController : Controller
    {
        private string Name = "SetOne";

        // Helper function used to display question
        private ViewResult ViewQuestion(int questionNum, string mesg)
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

        // Helper function used to display answer
        private ViewResult ViewAnswer(int questionNum, string mesg, string value)
        {
            return View("Answer", new Answer(questionNum, mesg, value, Name));
        }


        public ActionResult Index()
        {
            int[] questions = new int[] { 7, 10, 27, 35, 41, 47, 49, 50 };
            return View(questions);
        }

        public ActionResult Problem7()
        {
            return ViewQuestion(7, "What is the n-th prime number?");
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
            return ViewQuestion(10, "What is the sum of all primes below n?");
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
            return ViewQuestion(27, "Quadratic primes with abs(coefficients) < n?");
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
            return ViewQuestion(35, "Circular primes below n?");
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

        public ActionResult Problem41()
        {
            return ViewQuestion(41, "Largest pandigital prime with n-digit?");
        }

        [HttpPost]
        public ActionResult Problem41(uint n)
        {
            if (n > 9)
            {
                n = 9;
            }

            // Set up prime calculator
            var cal = new PrimeCalculator();

            // Keep trying until we run out of digits.
            while (n > 0)
            {
                List<int> digits = Enumerable.Range(1, (int)n).ToList();
                var permutations = new Permutations<int>(digits);

                var allPrimes = permutations.Select(numList => numList.MakeNumber())
                    .Where(num => cal.IsPrimeAutoExpand(num)).ToList();

                // If there is at least 1 prime with n-digit, find the maximum amongst these primes and 
                // that's the answer
                if (allPrimes.Count > 0)
                {
                    ulong max = allPrimes.Max();
                    return ViewAnswer(41, "The maximum " + n + "-digit pandigital prime is", max);
                }

                // Otherwise, reduce number of digits and try again.
                --n;
            }

            return ViewAnswer(41, "No n-digit pandigital prime found.", 0);
        }

        public ActionResult Problem47()
        {
            return ViewQuestion(47, "First N consecutive integers to have N distinct prime factors");
        }

        [HttpPost]
        public ActionResult Problem47(uint n)
        {
            var cal = new PrimeCalculator();
            uint cnt = 0;
            ulong potentialLeft = 0;

            for (ulong i = 1; cnt < n; i++)
            {
                if (i.GetDistinctPrimeFactors(cal).Count() == n)
                {
                    if (cnt == 0)
                    {
                        potentialLeft = i;
                    }
                    ++cnt;
                }
                else
                {
                    cnt = 0;
                }
            }

            return ViewAnswer(47, "Earliest consecutive integers with " + n + " prime factors starts at", potentialLeft);
        }

        public ActionResult Problem49()
        {
            return ViewQuestion(49, "N 4-digit prime numbers which are permutation of each other");
        }

        [HttpPost]
        public ActionResult Problem49(uint n)
        {
            var cal = new PrimeCalculator();
            cal.ExtendToMinimumGT(9999);    // We know that the prime numbers will be 4-digit, as stated in problem

            // Algorithm:
            // 1) Find all prime numbers between 1000 to 9999, group them by permutations
            // 2) Skip if number of permutations in group is < n
            // 3) Find the all arithmetic sequences within group with len >= n
            //    - Not all prime numbers with same permutation will be part of the arithemetic sequence, for example,
            //      for 1487 -> 4817 -> 8147, 1847 won't particpate.

            // Steps 1
            var prime_grps = from p in cal.Primes
                             where p >= 1000 && p <= 9999
                             group p by p.GetSortedDigits() into g
                             select g;

            // Step 2
            prime_grps = prime_grps.Where(g => g.Count() >= n);

            // Step 3
            var matched_series = prime_grps.SelectMany(g => Utils.FindArithemeticSeries(g, n));

            // Stringify solution.  Each member of series separates by a comma.  Series separates by semicolon
            var sb = new StringBuilder();
            foreach (var series in matched_series)
            {
                foreach (var member in series)
                {
                    sb.Append(member);
                    sb.Append(',');
                }
                if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);    // Remove last comma
                sb.Append(";");
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);        // Remove last semicolon

            return ViewAnswer(49, "4-digit primes which are permutations of each other", sb.ToString());
        }

        public ActionResult Problem50()
        {
            return ViewQuestion(50, "Longest consecutive prime sum for prime below N");
        }

        [HttpPost]
        public ActionResult Problem50(ulong n)
        {
            var cal = new PrimeCalculator(40000);
            cal.ExtendToMinimumGT(n);

            // Cumsums.. cumsum[i] = sum of primes[0 .. i]
            var primes = cal.Primes;
            var cumsums = primes.CumSum().ToArray();

            // Calc sum from [fromPos, toPos]
            Func<int, int, ulong> calcSum = (fromPos, toPos) =>
            {
                int before = fromPos - 1;
                if (before >= 0)
                {
                    return cumsums[toPos] - cumsums[before];
                }
                else
                {
                    return cumsums[toPos];
                }
            };

            // Find end position for range to examine.
            int endPos;
            for (endPos = primes.Length - 1; endPos >= 0 && primes[endPos] > n; endPos--) ;

            // Loop through all possible values for [start, end] to find the longest range whose
            // sum is still a prime
            int maxLen = 0, maxStart = 0;
            ulong maxNum = 0;

            for (int end = 0; end <= endPos; end++)
            {
                for (int start = 0; start < end; start++)
                {
                    int len = end - start + 1;
                    if (len < maxLen)
                    {
                        continue;
                    }

                    ulong sum = calcSum(start, end);
                    if (sum > n)
                    {
                        continue;
                    }

                    if (!cal.IsPrimeAutoExpand(sum))
                    {
                        continue;
                    }

                    maxLen = len;
                    maxNum = sum;
                    maxStart = start;
                }
            }

            int maxEnd = maxStart + maxLen - 1;
            ulong maxValue = calcSum(maxStart, maxEnd);
            var s = string.Format("{0} + .. + {1} = {2} len = {3}",
                primes[maxStart], primes[maxEnd], maxValue, maxEnd, maxLen);
            return ViewAnswer(50, s, maxValue);
       }
    }
}
