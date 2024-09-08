using ExtensionAbstraction;

namespace ThresholdRelative {
    public class ThresholdRelativeChecker : ThresholdChecker {
        public ThresholdRelativeChecker() {
            FriendlyName = "Relative % threshold";
            UserInstructions = "Set % value from 1 to 99 for player to dominate.";
        }

        public override bool SetThreshold(double threshold) {
            if (threshold >= 1 && threshold <= 99) {
                Threshold = threshold / 100;
                return true;
            }
            return false;
        }

        public override bool CheckThreshold(double value1, double value2) {
            double mn = Math.Min(value1, value2);
            double difference = Math.Abs(value2 - value1);
            return (difference / mn) > Threshold;
        }
    }
}
