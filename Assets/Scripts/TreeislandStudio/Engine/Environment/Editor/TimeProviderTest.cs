using NUnit.Framework;

namespace TreeislandStudio.Engine.Environment.Editor
{
    /*
    public class TimeProviderTest {
        /// <summary>
        /// By default a time provider just return the real time
        /// </summary>
        [Test]
        public void ByDefaultTimeProviderReturnsRealTime()
        {
            var time = 0f;
            
            // ReSharper disable once AccessToModifiedClosure
            var timeProvider = new TimeProvider(() => time);
            
            Assert.AreEqual(0, timeProvider.Time);
            time = 3;
            Assert.AreEqual(3, timeProvider.Time);
            time = 666;
            Assert.AreEqual(666, timeProvider.Time);
        }
        
        /// <summary>
        /// When time scale is zero, time does not pass
        /// </summary>
        [Test]
        public void WhenTimeScaleZeroTimeDoesNotPass()
        {
            var time = 0f;
            
            // ReSharper disable once AccessToModifiedClosure
            var timeProvider = new TimeProvider(() => time);
            
            Assert.AreEqual(0, timeProvider.Time);
            time = 3;
            Assert.AreEqual(3, timeProvider.Time);
            time = 666;
            Assert.AreEqual(666, timeProvider.Time);
            timeProvider.TimeScale = 0;
            time = 667;
            Assert.AreEqual(666, timeProvider.Time);
            time = 668;
            Assert.AreEqual(666, timeProvider.Time);
        }
        
        /// <summary>
        /// When time scale is restored to one, time continues pass
        /// </summary>
        [Test]
        public void WhenTimeScaleRestoreToOneTimeContinues()
        {
            var time = 0f;
            
            // ReSharper disable once AccessToModifiedClosure
            var timeProvider = new TimeProvider(() => time);
            time = 666;
            Assert.AreEqual(666, timeProvider.Time);
            timeProvider.TimeScale = 0;
            time = 667;
            Assert.AreEqual(666, timeProvider.Time);
            timeProvider.TimeScale = 1;
            time = 668;
            Assert.AreEqual(667, timeProvider.Time);
        }
    }
    */
}