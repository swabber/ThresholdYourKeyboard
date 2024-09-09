using ExtensionAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThresholdYourKeyboardTest {
    [TestClass]
    public class ThresholdIntervalCoverageCheckerTest {

        [TestMethod]
        public void CheckThresholdTest() {
            ThresholdChecker<List<(int, int)>> thresholdChecker = new ThresholdIntervalCoverageChecker();
            thresholdChecker.SetThreshold(new List<(int, int)> { (1, 4), (5, 8) });

            var list1 = new List<(int, int)> { (-3, 0), (1, 3), (3, 6), (6, 9), (10, 20)};
            var list2 = new List<(int, int)> { (-3, 20) };
            Assert.IsTrue(thresholdChecker.CheckThreshold(list1, list2));

            list1 = new List<(int, int)> { (-3, 0), (1, 3), (3, 6), (7, 9), (10, 20) };
            Assert.IsFalse(thresholdChecker.CheckThreshold(list1, list2));
        }
    }
}
