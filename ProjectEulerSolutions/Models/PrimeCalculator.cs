using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectEulerSolutions.Models
{
    public class PrimeCalculator
    {
        private ulong[] primes_;
        private uint len_;

        public PrimeCalculator(uint initialCapacity = 1000)
        {
            initialCapacity = Math.Max(initialCapacity, 2);
            primes_ = new ulong[initialCapacity];
            primes_[0] = 2;
            primes_[1] = 3;
            len_ = 2;
        }   
 
        /// <summary>
        /// Fill calculator with at least len prime numbers.
        /// </summary>
        /// <param name="len"></param>
        public void ExtendToLength(uint len)
        {
            if (len > primes_.Length)
            {
                Resize(len);
            }

            for (ulong i = LastPrime + 2; len_ < len; i += 2)
            {
                while (!IsPrime(i))
                {
                    i += 2;
                }
                primes_[len_] = i;
                ++len_;
            }
        }

        /// <summary>
        /// Fill calculator so that largest prime number is the next prime number greater than or equal to value.
        /// </summary>
        /// <param name="value"></param>
        public void ExtendToMinimumGT(ulong value)
        {
            while (LastPrime <= value)
            {
                if (len_ == primes_.Length)
                {
                    Resize((uint)(len_ * 2));
                }

                ulong i;
                for (i = LastPrime + 2; !IsPrime(i); i += 2) ;

                primes_[len_] = i;
                ++len_;
            }
        }

        /// <summary>
        /// Fill calculator so that largest prime number is at least larger than square root of value.
        /// </summary>
        /// <param name="value"></param>
        public void ExtendToSquareRoot(ulong value)
        {
            if (LastPrime * LastPrime > value)
            {
                return;
            }

            value = (ulong)Math.Ceiling(Math.Sqrt(value));
            ExtendToMinimumGT(value);
        }

        /// <summary>
        /// Get all calculated prime numbers
        /// </summary>
        public ulong[] Primes
        {
            get
            {
                if (primes_.Length == len_)
                {
                    return primes_;
                }
                else
                {
                    ulong[] reduced = new ulong[len_];
                    Array.Copy(primes_, reduced, len_);
                    return reduced;
                }
            }
        }

        /// <summary>
        /// Get the largest prime number calculated so far.
        /// </summary>
        public ulong LastPrime
        {
            get { return primes_[len_ - 1]; }
        }

        /// <summary>
        /// Check if n is a prime number using fast binary search.  The precondition is that N must be <= largest prime number
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool IsPrimeInRange(ulong n)
        {
            return Array.BinarySearch(primes_, 0, (int)len_, n) >= 0;
        }

        /// <summary>
        /// Check if n is a prime number by either using fast binary search or automatic compute all primes up to square root of N.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool IsPrimeAutoExpand(ulong n)
        {
            if (LastPrime >= n)
            {
                return IsPrimeInRange(n);
            }
            else
            {
                ExtendToSquareRoot(n);
                return IsPrime(n);
            }
        }

        private void Resize(uint capacity)
        {
            ulong[] newprimes = new ulong[capacity];
            Array.Copy(primes_, newprimes, len_);
            primes_ = newprimes;
        }

        private bool IsPrime(ulong n)
        {
            for (int i = 0; i < primes_.Length; i++)
            {
                ulong p = primes_[i];

                if (p * p > n)
                {
                    return true;
                }
                else if (n % p == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}