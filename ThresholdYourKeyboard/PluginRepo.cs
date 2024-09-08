using ExtensionAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThresholdYourKeyboard {
    internal class PluginRepo {

        public List<ThresholdChecker> Plugins = new List<ThresholdChecker>();

        public void LoadPlugins() {
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] dllFiles = Directory.GetFiles(currentDirectory, "*.dll");

            foreach (string file in dllFiles) {
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (var type in assembly.GetTypes()) {
                    if (!type.IsClass || type.IsAbstract || !type.IsSubclassOf(typeof(ThresholdChecker))) { continue; }
                    var item = Activator.CreateInstance(type);
                    if (item == null) { continue; }
                    ThresholdChecker plugin = (ThresholdChecker)item;
                    if (plugin == null) { continue; }
                    Plugins.Add(plugin);
                }
            }
        }
    }
}
