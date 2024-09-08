using ExtensionAbstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThresholdYourKeyboard {
    internal class KeyStatsCollect {
        private const int VK_LCONTROL = 0xA2;
        private const int VK_RCONTROL = 0xA3;
        private const long fiveSec = 50000000;
        private Queue<long> LeftCtrlQ;
        private Queue<long> RightCtrlQ;
        private bool LeftStarted = false;
        private bool RightStarted = false;
        private CancellationTokenSource CancellationTokenSource;
        private readonly ThresholdChecker<double> ThresholdChecker;

        public double LeftPlayerScore;
        public double RightPlayerScore;

        internal Task CollectStats;

        [DllImport("user32.dll")]
        internal static extern short GetAsyncKeyState(int vKey);
        internal KeyStatsCollect(ThresholdChecker<double> thresholdChecker, CancellationTokenSource cts) {
            CancellationTokenSource = cts;
            CollectStats = new Task(Collect, cts.Token);
            LeftCtrlQ = new Queue<long>();
            RightCtrlQ = new Queue<long>();
            ThresholdChecker = thresholdChecker;
        }

        internal Task Start() {
            CollectStats.Start();
            return CollectStats;
        }
        public long startCheckingThreshold = 0;
        public long winAt = 0;

        private void Collect() {
            bool leftCtrl = false;
            bool rightCtrl = false;
            
            try {
                while (!CancellationTokenSource.IsCancellationRequested) {
                    if ((GetAsyncKeyState((int)ConsoleKey.Escape) & 0x8000) != 0) {
                        CancellationTokenSource.Cancel();
                        break; // Exit on Esc
                    } 
                    long currentTime = DateTime.Now.Ticks;
                    long remove = currentTime - fiveSec;
                    leftCtrl = IsControlPressed(LeftCtrlQ, VK_LCONTROL, leftCtrl, currentTime, remove);
                    rightCtrl = IsControlPressed(RightCtrlQ, VK_RCONTROL, rightCtrl, currentTime, remove);
                    LeftPlayerScore = LeftCtrlQ.Count() * 12;
                    RightPlayerScore = RightCtrlQ.Count() * 12;
                    if (leftCtrl) { LeftStarted = true; }
                    if (rightCtrl) { RightStarted = true; }
                    if (startCheckingThreshold == 0 && LeftStarted && RightStarted) {
                        startCheckingThreshold = currentTime + fiveSec; // After both Players started clicking their Ctrl's give them 5 sec before judging.
                    }
                    if (startCheckingThreshold < currentTime) {
                        bool res = ThresholdChecker.CheckThreshold(LeftPlayerScore, RightPlayerScore);
                        if (res && winAt == 0) {
                            winAt = currentTime + fiveSec;
                        } else if (!res) {
                            winAt = 0;
                        } else if (res && winAt > 0 && winAt < currentTime) { 
                            break; // Winner found
                        }
                    }
                    Thread.Sleep(10); // Add a small delay to reduce CPU usage
                }
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        private bool IsControlPressed(Queue<long> queue, int key, bool isCtrlPressed, long currentTime, long remove) {
            if ((GetAsyncKeyState(key) & 0x8000) != 0) { // Ctrl is pressed
                isCtrlPressed = true;
            } else { // Ctrl is released
                if (isCtrlPressed) { // and if previously pressed 
                    queue.Enqueue(currentTime);
                    isCtrlPressed = false;
                }
            }
            while (queue.Count > 0 && queue.Peek() < remove) {
                queue.Dequeue();
            }

            return isCtrlPressed;
        }
    }
}
