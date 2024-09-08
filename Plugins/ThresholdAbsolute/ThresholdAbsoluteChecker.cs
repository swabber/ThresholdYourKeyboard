using ExtensionAbstraction;

namespace ThresholdAbsolute {
    public class ThresholdAbsoluteChecker : ThresholdChecker {
        public ThresholdAbsoluteChecker() {
            FriendlyName = "Absolute amount threshold";
            UserInstructions = "Set Absolute value for number of clicks player need to dominate.";
        }

        public override bool SetThreshold(double threshold) {
            if (threshold < 0) { return false; }
            Threshold = threshold;
            return true;
        }

        // Returns true if delta is over the threshold
        public override bool CheckThreshold(double value1, double value2) {
            return Math.Abs(value2 - value1) > Threshold;
        }
    }
}
