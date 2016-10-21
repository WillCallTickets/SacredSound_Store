using System;

namespace Utils
{
    public class WctMath
    {
        public static double RoundDown(double number, int decimalPlaces)
        {
            return Math.Floor(number * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
        }
    }
}
