using UnityEngine;

namespace TreeislandStudio.Engine.Math {

    /// <summary>
    /// Random number's provider
    /// </summary>
    public class RandomNumberProvider : IRandomNumberProvider {
        #region public methods

        /// <inheritdoc />
        /// <summary>
        /// Initializes the random number generator state with a seed.
        /// </summary>
        /// <param name="seed"></param>
        public void InitState(int seed) {
            Random.InitState(seed);
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns a random float number between and minimum [inclusive] and maximum [inclusive].
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public float Range(float minimum, float maximum) {
            return Random.Range(minimum, maximum);
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns a random integer number between and minimum [inclusive] and maximum [inclusive].
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public int Range(int minimum, int maximum) {
            return Random.Range(minimum, maximum);
        }

        #endregion
    }
}
