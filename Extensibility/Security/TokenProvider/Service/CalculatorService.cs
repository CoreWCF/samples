// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.TokenProvider
{
    // Service class which implements the service contract interface.
    // Added code to write output to the console window
    public class CalculatorService : ICalculatorService
    {
        static void DisplayIdentityInformation()
        {
            Console.WriteLine("\t\tSecurity context identity  :  {0}", ServiceSecurityContext.Current.PrimaryIdentity.Name);
        }

        public double Add(double n1, double n2)
        {
            DisplayIdentityInformation();
            double result = n1 + n2;
            Console.WriteLine("Received Add({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Subtract(double n1, double n2)
        {
            DisplayIdentityInformation();
            double result = n1 - n2;
            Console.WriteLine("Received Subtract({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Multiply(double n1, double n2)
        {
            DisplayIdentityInformation();
            double result = n1 * n2;
            Console.WriteLine("Received Multiply({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Divide(double n1, double n2)
        {
            DisplayIdentityInformation();
            double result = n1 / n2;
            Console.WriteLine("Received Divide({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }
    }
}
