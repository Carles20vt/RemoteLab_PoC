using TreeislandStudio.Engine.Math;
using UnityEngine;

namespace TreeislandStudio.Engine.Attributes
{
    public class MinMaxRangeAttribute : PropertyAttribute {
        public float minLimit, maxLimit;

        public MinMaxRangeAttribute(float minLimit, float maxLimit) {
            this.minLimit = minLimit;
            this.maxLimit = maxLimit;
        }
    }

    [System.Serializable]
    public class MinMaxRange {
        public float rangeStart;
        public float rangeEnd;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rangeStart"></param>
        /// <param name="rangeEnd"></param>
        public MinMaxRange(float rangeStart, float rangeEnd) {
            this.rangeStart = rangeStart;
            this.rangeEnd   = rangeEnd;
        }

        /// <summary>
        /// Return a random value between rangeStart and rangeEnd
        /// </summary>
        /// <param name="randomGenerator"></param>
        /// <returns></returns>
        public float GetOne(IRandomNumberProvider randomGenerator) {
            return randomGenerator.Range(rangeStart, rangeEnd);
        }

        /// <summary>
        /// Is the value en in range ?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool In(float value) {
            return value >= rangeStart && value <= rangeEnd;
        }
    }
}