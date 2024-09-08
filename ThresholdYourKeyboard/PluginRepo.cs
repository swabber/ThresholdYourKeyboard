using ExtensionAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThresholdYourKeyboard {
    internal class PluginRepo<T> where T : struct {

        public List<ThresholdChecker<T>> Plugins = new List<ThresholdChecker<T>>();

        public void LoadPlugins() {
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] dllFiles = Directory.GetFiles(currentDirectory, "*.dll");

            foreach (string file in dllFiles) {
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (var type in assembly.GetTypes()) {
                    if (!type.IsClass || type.IsAbstract || type.BaseType == null || !type.BaseType.IsGenericType || type.BaseType.GetGenericTypeDefinition() != typeof(ThresholdChecker<>)) { continue; }
                    var item = Activator.CreateInstance(type);
                    if (item == null) { continue; }
                    ThresholdChecker<T> plugin = (ThresholdChecker<T>)item;
                    if (plugin == null) { continue; }
                    Plugins.Add(plugin);
                }
            }
        }
    }
}
