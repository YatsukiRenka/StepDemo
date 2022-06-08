using System;
using System.Collections.Generic;

namespace StepDemo
{
    public static class Tools
    {
        public static double[] CalculateArray(double startValue, double stopValue, double step)
        {
            return CalculateArray(startValue, stopValue, step, 5);
        }

        public static double[] CalculateArray(double startValue, double stopValue, double step, int digits)
        {
            List<double> list = new List<double>();
            int num = 0;
            double num2 = 0.0;
            while (true)
            {
                num2 = (startValue + (double)num * step).ScientificNotationRound(digits);
                if (num2 * step >= stopValue * step)
                {
                    break;
                }

                list.Add(num2);
                num++;
            }

            list.Add(stopValue);
            return list.ToArray();
        }

        public static double ScientificNotationRound(this double value, int digits)
        {
            if (digits < 1)
            {
                throw new ArgumentOutOfRangeException("digits", digits, "digits cannot be smaller than 1.");
            }

            string s = value.ToString("e" + (digits - 1));
            return double.Parse(s);
        }
    }
}
