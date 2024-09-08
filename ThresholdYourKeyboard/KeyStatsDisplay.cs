using ExtensionAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThresholdYourKeyboard {
    internal class KeyStatsDisplay {
        private KeyStatsCollect Stats;
        private const long OneSec = 10000000;
        private Task DisplayStats;
        private Task? CollectStats;
        private CancellationTokenSource CancellationTokenSource;
        public KeyStatsDisplay(KeyStatsCollect stats, CancellationTokenSource cts) {
            DisplayStats = new Task(Display, cts.Token);
            CancellationTokenSource = cts;
            Stats = stats;
        }

        internal Task Start() {
            DisplayStats.Start();
            CollectStats = Stats.Start();
            return DisplayStats;
        }

        private void Display() {
            try {
                Console.Clear();
                Console.WriteLine("Press 'Esc' to exit.");
                Console.WriteLine();
                Console.WriteLine("{0,5} {1,30}", "Player on Left Ctrl", "Player on Right Ctrl");
                while (!CancellationTokenSource.IsCancellationRequested) {
                    Console.SetCursorPosition(0, 4);
                    long timeNow = DateTime.Now.Ticks;
                    if (Stats.startCheckingThreshold == 0) {
                        Console.WriteLine(string.Format("{0, -40}", "Both Players my start."));
                    } else if (Stats.startCheckingThreshold > 0 && Stats.startCheckingThreshold > timeNow) {
                        Console.WriteLine(string.Format("{0, -40}", "Competition phase in " + (Stats.startCheckingThreshold - timeNow) / OneSec));
                    } else if (Stats.startCheckingThreshold < timeNow && Stats.winAt == 0) {
                        Console.WriteLine(string.Format("{0, -40}", "Keep going!"));
                    } else if (Stats.startCheckingThreshold < timeNow && Stats.winAt > 0) {
                        string potentialWinner = (Stats.LeftPlayerScore > Stats.RightPlayerScore) ? "Left" : "Right";
                        Console.WriteLine(string.Format("{0, -40}", $"{potentialWinner} domination in {(Stats.winAt - timeNow) / OneSec}"));
                    } else { Console.WriteLine(); }
                    Console.WriteLine();


                    Console.WriteLine(string.Format("Clicks/min: {0,4}\t\tClicks/min: {1,4}", Stats.LeftPlayerScore, Stats.RightPlayerScore));
                    if (CollectStats != null && CollectStats.IsCompleted) { break; }
                    Thread.Sleep(200);
                }

                Console.Clear();
                if (CollectStats != null && CollectStats.IsCompletedSuccessfully && !CancellationTokenSource.IsCancellationRequested) {
                    if (Stats.LeftPlayerScore > Stats.RightPlayerScore) {
                        Console.WriteLine($"Left Player Win. {Stats.LeftPlayerScore} vs {Stats.RightPlayerScore} Right");
                    } else {
                        Console.WriteLine($"Right Player Win. {Stats.RightPlayerScore} vs {Stats.LeftPlayerScore} Left");
                    }
                } else {
                    return;
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

    }
}
