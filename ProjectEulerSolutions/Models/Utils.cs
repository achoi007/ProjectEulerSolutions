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
    }
}