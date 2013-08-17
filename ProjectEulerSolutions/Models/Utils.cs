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
    }
}