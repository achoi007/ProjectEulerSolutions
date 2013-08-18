using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectEulerSolutions.Models
{
    public static class Utils
    {
        public static ulong Sum(this IEnumerable<ulong> nums, ulong seed = 0)
        {
            ulong sum = seed;
            foreach (var num in nums)
            {
                sum += num;
            }
            return sum;
        }

        public static ulong PowerOfTen(uint power)
        {
            ulong tens = 1;
            while (power > 0)
            {
                tens *= 10;
                power--;
            }
            return tens;
        }

        public static uint NumOfDigits(ulong num)
        {
            uint n = 1;
            while (num > 10)
            {
                ++n;
                num /= 10;
            }
            return n;
        }

        public static uint LeftMostDigit(ulong num)
        {
            while (num > 10)
            {
                num /= 10;    
            }
            return (uint) num;
        }

        public static ulong MakeNumber(this IEnumerable<int> digits)
        {
            return (ulong)(digits.Aggregate(0L, (s, n) => s * 10 + n, s => s));
        }

        public static IEnumerable<ulong> GetPrimeFactors(this ulong num, PrimeCalculator cal)
        {
            cal.ExtendToMinimumGT(num / 2);     // so that cal.Primes is populated with all prime factors needed

            if (cal.IsPrimeAutoExpand(num))     // Don't need to consider num which is a prime.
            {
                yield break;
            }

            foreach (var prime in cal.Primes)
            {
                while (num % prime == 0)
                {
                    yield return prime;
                    num /= prime;
                }

                if (num == 1)
                {
                    break;
                }
            }
        }

        public static IEnumerable<ulong> GetDistinctPrimeFactors(this ulong num, PrimeCalculator cal)
        {
            return num.GetPrimeFactors(cal).Distinct();
        }
    }
}