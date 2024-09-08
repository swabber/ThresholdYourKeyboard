namespace ThresholdYourKeyboard {
    internal class Program {
        static void Main(string[] args) {
            var pluginRepo = new PluginRepo();
            var userInterface = new UserInterface(pluginRepo);

            pluginRepo.LoadPlugins();
            userInterface.Interact();
        }
    }
}
