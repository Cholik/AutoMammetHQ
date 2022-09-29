using System.IO;
using AutoMammetHQ.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace AutoMammetHQ
{
    public sealed class Plugin : IDalamudPlugin
    {
        private const string CommandName = "/mammethq";

        public string Name => "AutoMammet (HQ)";

        internal DalamudPluginInterface PluginInterface { get; init; }

        private CommandManager CommandManager { get; init; }

        public Configuration Configuration { get; init; }

        public WindowSystem WindowSystem = new("AutoMammet (HQ)");

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            WindowSystem.AddWindow(new MainWindow(this));

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open up the exporting UI/Interface."
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            var window = WindowSystem.GetWindow("AutoMammet (HQ)");

            if (window != null)
            {
                window.IsOpen = true;
            }
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }
    }
}
