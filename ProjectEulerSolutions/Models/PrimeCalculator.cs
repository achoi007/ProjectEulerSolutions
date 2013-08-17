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

        public void ExtendToSquareRoot(ulong value)
        {
            if (LastPrime * LastPrime > value)
            {
                return;
            }

            value = (ulong)Math.Ceiling(Math.Sqrt(value));
            ExtendToMinimumGT(value);
        }

        public ulong[] Primes
        {
            get { return primes_; }
        }

        public ulong LastPrime
        {
            get { return primes_[len_ - 1]; }
        }

        public bool IsPrimeInRange(ulong n)
        {
            return Array.BinarySearch(primes_, 0, (int)len_, n) >= 0;
        }

        public bool IsPrimeAutoExpand(ulong n)
        {
            ExtendToSquareRoot(n);
            return IsPrime(n);
        }

        private void Resize(uint capacity)
        {
            ulong[] newprimes = new ulong[capacity];
            Array.Copy(primes_, newprimes, len_);
            primes_ = newprimes;
        }

        private bool IsPrime(ulong n)
        {
            for (int i = 1; i < primes_.Length; i++)
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