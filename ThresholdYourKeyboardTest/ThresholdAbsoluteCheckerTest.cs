using ExtensionAbstraction;
using ThresholdAbsolute;

namespace ThresholdYourKeyboardTest {
    [TestClass]
    public class ThresholdAbsoluteCheckerTest {
        [TestMethod]
        public void SetThresholdTest() {
            ThresholdChecker thresholdChecker = new ThresholdAbsoluteChecker();
            thresholdChecker.SetThreshold(20);
            Assert.AreEqual(20, thresholdChecker.Threshold);

            thresholdChecker.SetThreshold(40);
            Assert.AreEqual(40, thresholdChecker.Threshold);
        }

        [TestMethod]
        public void CheckThresholdTest() {
            ThresholdChecker thresholdChecker = new ThresholdAbsoluteChecker();
            thresholdChecker.SetThreshold(20);

            Assert.IsTrue(thresholdChecker.CheckThreshold(80, 101));
            Assert.IsFalse(thresholdChecker.CheckThreshold(80, 99));
            Assert.IsFalse(thresholdChecker.CheckThreshold(80, 61));
            Assert.IsTrue(thresholdChecker.CheckThreshold(80, 59));
        }
    }
}