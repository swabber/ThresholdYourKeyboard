using ExtensionAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThresholdRelative;

namespace ThresholdYourKeyboardTest {
    [TestClass]
    public class ThresholdRelativeCheckerTest {
        [TestMethod]
        public void SetThresholdTest() {
            ThresholdChecker thresholdChecker = new ThresholdRelativeChecker();
            Assert.IsFalse(thresholdChecker.SetThreshold(0.6));
            Assert.IsFalse(thresholdChecker.SetThreshold(101));
            Assert.IsTrue(thresholdChecker.SetThreshold(10));
            Assert.AreEqual(0.1, thresholdChecker.Threshold);
        }

        [TestMethod]
        public void CheckThresholdTest() {
            ThresholdChecker thresholdChecker = new ThresholdRelativeChecker();
            thresholdChecker.SetThreshold(10);

            Assert.IsFalse(thresholdChecker.CheckThreshold(100, 110));
            Assert.IsTrue(thresholdChecker.CheckThreshold(100, 110.5));
            Assert.IsFalse(thresholdChecker.CheckThreshold(110, 100));
        }
    }
}
