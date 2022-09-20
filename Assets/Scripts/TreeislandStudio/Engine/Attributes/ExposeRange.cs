using System;

namespace TreeislandStudio.Engine.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
// ReSharper disable once CheckNamespace
    public class ExposeRange : Attribute {
        /// <summary>
        /// Minim value for range
        /// </summary>
        public float Minimum = 0f;
        public float Maximum = 1f;

        /// <summary>
        /// Expose range for floats
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public ExposeRange(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Expose range for ints
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public ExposeRange(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }
    }
}