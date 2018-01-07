using System;
using System.Collections.Generic;

namespace Predicting.NeuralNetworkClassifier
{
    public static class Operators
    {
        private static Random rng = new Random();  

        /*
        * ZIPPING 
        */

        public static IEnumerable<T> Zip<A, B, T>(this IEnumerable<A> seqA, IEnumerable<B> seqB, Func<A, B, T> func)
        {
            if (seqA == null) throw new ArgumentNullException("seqA");
            if (seqB == null) throw new ArgumentNullException("seqB");

            return Zip35Deferred(seqA, seqB, func);
        }

        private static IEnumerable<T> Zip35Deferred<A, B, T>(this IEnumerable<A> seqA, IEnumerable<B> seqB, Func<A, B, T> func)
        {
            using (var iteratorA = seqA.GetEnumerator())
            using (var iteratorB = seqB.GetEnumerator())
            {
                while (iteratorA.MoveNext() && iteratorB.MoveNext())
                {
                    yield return func(iteratorA.Current, iteratorB.Current);
                }
            }
        }


        /*
        * DOTPRODUCT / MATRIX ADDITION 
        */

        public static double[][] MultiplyMatrix(double[][] A, double[][] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double temp = 0;
            double[][] kHasil = new double[rA][];
            if (cA != rB)
            {
                Console.WriteLine("Matrix can't be multiplied!");
                return null;
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    kHasil[rA] = new double[cB];

                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i][k] * B[k][j];
                        }
                        kHasil[i][j] = temp;
                    }
                }
                return kHasil;
            }
        }

        public static double[][] SumMatrix(double[][] A, double[][] B)
        {
            if (A == null) A = new double[0][];
            if (B == null) B = new double[0][];
            double[][] union = new double[A.Length + B.Length][];
            
            for (int i = 0; i < A.Length; i++)
            {
                union[i] = A[i];
            }
            
            int j = 0;
            for (int i = A.Length; i < union.Length; i++)
            {
                union[i] = B[j++];
            }
            return union;
        }

        
        /*
         * LIST FUNCTIONS
         */

        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
        
        public 
    }
}