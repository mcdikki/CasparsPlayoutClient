using System;

namespace Bespoke.Common
{
    /// <summary>
    /// Helper class for random numbers.
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Random"/> object associated with this helper.
        /// </summary>
        public static Random Randmon
        {
            get
            {
                return sRandom;
            }
            set
            {
                Assert.ParamIsNotNull(value);
                sRandom = value;
            }
        }

        /// <summary>
        /// Initializes the static instance of the <see cref="RandomHelper"/> class.
        /// </summary>
        static RandomHelper()
        {
            sRandom = new Random();
        }

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="int.MaxValue"/>.</returns>
        public static int Next()
        {
            return sRandom.Next();
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
        /// <returns>A 32-bit signed integer greater than or equal to zero, and less than maxValue; that is, the range of return values ordinarily
        /// includes zero but not maxValue. However, if maxValue equals zero, maxValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">maxValue is less than zero.</exception>
        public static int Next(int maxValue)
        {
            return sRandom.Next(maxValue);
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes
        /// minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue is greater than maxValue.</exception>
        public static int Next(int minValue, int maxValue)
        {
            return sRandom.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a random number between 0.0f and 1.0f.
        /// </summary>
        /// <returns>A single-precision floating point number greater than or equal to 0.0f, and less than 1.0f.</returns>
        public static float NextFloat()
        {
            return (float)sRandom.NextDouble();
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <returns>A single-precision floating point numner greater than or equal to minValue and less than maxValue;
        /// that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue is greater than maxValue.</exception>
        public static float NextFloat(double minValue, double maxValue)
        {
            return (float)NextDouble(minValue, maxValue);
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        public static double NextDouble()
        {
            return sRandom.NextDouble();
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <returns>A double-precision floating point numner greater than or equal to minValue and less than maxValue;
        /// that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue is greater than maxValue.</exception>
        public static double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }

            return minValue + (sRandom.NextDouble() * (maxValue - minValue));
        }

        /// <summary>
        /// Returns a random boolean value.
        /// </summary>
        /// <returns>A random boolean value.</returns>
        public static bool NextBoolean()
        {
            return (sRandom.NextDouble() > 0.5);
        }

        private static Random sRandom;
    }
}
