using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionAbstraction {
    public abstract class ThresholdChecker<T> {
        public T Threshold { get; protected set; }
        public string FriendlyName { get; protected set; }
        public string UserInstructions { get; protected set; }
        protected ThresholdChecker(){
            FriendlyName = "Title of your custom ThresholdChecker";
            UserInstructions = "input proper value for current ThresholdChecker implementation";
        }

        public abstract bool SetThreshold(T value);

        public abstract bool CheckThreshold(T value1, T value2);
    }
}
