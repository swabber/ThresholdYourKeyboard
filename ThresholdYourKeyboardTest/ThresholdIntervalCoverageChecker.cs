using ExtensionAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ThresholdYourKeyboardTest {
    /// <summary>
    /// This class test different type ThresholdChecker scalability.
    /// Data is sorted intervals.
    /// Class checks if 2 intervals fully intersect at Threshold interval
    /// </summary>
    internal class ThresholdIntervalCoverageChecker : ThresholdChecker<List<(int, int)>> {
        public override bool CheckThreshold(List<(int, int)> value1, List<(int, int)> value2) {
            return FullyCovered(value1) && FullyCovered(value2);
        }

        public override bool SetThreshold(List<(int, int)> value) {
            if (value == null) { return false; }
            Threshold = value;
            return true;
        }

        private bool FullyCovered(List<(int, int)> value) {
            int j = 0;
            foreach ((int, int) item in Threshold) {
                int start = item.Item1;
                int end = item.Item2;
                while (j < value.Count) {
                    if (value[j].Item2 <= start) {
                        j++;
                    } else if (start < value[j].Item2 && value[j].Item2 < end) {
                        if (value[j].Item1 > start) {
                            return false;
                        } else {
                            start = value[j].Item2;
                            j++;
                        }
                    } else if (end < value[j].Item2) {
                        if (value[j].Item1 <= start) {
                            break;
                        } else {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
