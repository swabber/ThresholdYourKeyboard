using ExtensionAbstraction;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThresholdYourKeyboard {
    internal class UserInterface {
        internal enum State { SelectThresholdType, SetThresholdValue, Play, Exit }
        private readonly PluginRepo PluginRepo;
        private readonly Dictionary<State, Action> StateActions = new();
        private ThresholdChecker? CurrentThresholdChecker;
        internal State CurrentState { get; private set; }

        internal UserInterface(PluginRepo pluginRepo) {
            PluginRepo = pluginRepo;
            CurrentState = State.SelectThresholdType;
            StateActions.Add(State.SelectThresholdType, ShowThresholdMenu);
            StateActions.Add(State.SetThresholdValue, ShowSetThresholdValue);
            StateActions.Add(State.Play, ShowGamePlay);
        }

        internal void Interact() {
            while (CurrentState != State.Exit) {
                StateActions[CurrentState].Invoke();
            }
        }
        internal void ShowThresholdMenu() {
            Console.Clear();
            Console.WriteLine("Choose Threshold Method:");
            for (int i = 0; i < PluginRepo.Plugins.Count; i++) {
                Console.WriteLine($"{i + 1}) {PluginRepo.Plugins[i].FriendlyName}");
            }
            Console.WriteLine($"{PluginRepo.Plugins.Count + 1}) Exit");
            Console.Write("\r\nSelect an option: ");

            int option = 0;
            while (option < 1 || option > PluginRepo.Plugins.Count + 1) {
                var userInput = Console.ReadLine();
                if (userInput == null || !int.TryParse(userInput, out option)) {
                    Console.WriteLine("Enter valid option");
                    continue;
                }
            }
            if (option == PluginRepo.Plugins.Count + 1) {
                CurrentState = State.Exit;
                return;
            }
            if (option > 0 && option <= PluginRepo.Plugins.Count) {
                CurrentThresholdChecker = PluginRepo.Plugins[option - 1];
                CurrentState = State.SetThresholdValue;
            }
        }
        internal void ShowSetThresholdValue() {
            if (CurrentThresholdChecker == null) {
                CurrentState = State.SelectThresholdType;
                return;
            }
            Console.Clear();
            Console.WriteLine("Set Threshold Value");
            Console.WriteLine(CurrentThresholdChecker.UserInstructions);
            bool invalideValue = true;
            while (invalideValue) {
                var userInput = Console.ReadLine();
                if (userInput == null || !double.TryParse(userInput, out double value) || !CurrentThresholdChecker.SetThreshold(value)) {
                    Console.WriteLine("Enter valid value");
                    continue;
                }
                invalideValue = false;
                CurrentState = State.Play;
            }

        }
        internal void ShowGamePlay() {
            if (CurrentThresholdChecker == null) {
                CurrentState = State.SelectThresholdType;
                return;
            }
            if (CurrentState != State.Play) { return; }
            var cts = new CancellationTokenSource();
            var collect = new KeyStatsCollect(CurrentThresholdChecker, cts);
            var display = new KeyStatsDisplay(collect, cts);
            //Task.WaitAny(collect.Start(), display.Start());
            display.Start().Wait();
            CurrentState = State.SelectThresholdType;
        }
    }
}
