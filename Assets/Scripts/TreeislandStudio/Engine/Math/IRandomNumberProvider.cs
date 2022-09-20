namespace TreeislandStudio.Engine.Math {
    public interface IRandomNumberProvider {

        /// <summary>
        /// Initializes the random number generator state with a seed.
        /// </summary>
        /// <param name="seed"></param>
        void InitState(int seed);

        /// <summary>
        /// Returns a random float number between and minimum [inclusive] and maximum [inclusive].
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        float Range(float minimum, float maximum);

        /// <summary>
        /// Returns a random integer number between and minimum [inclusive] and maximum [inclusive].
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        int Range(int minimum, int maximum);
    }
}