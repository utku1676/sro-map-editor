using System;
using System.Runtime.InteropServices;

namespace SilkroadMapEditor
{
    static class Program
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Window window;
        public static SilkroadMapEditor game;
        [STAThread] 
        static void Main(string[] args)
        {
#if DEBUG
            AllocConsole();
#endif
            window = new Window();
            window.Show();
            game = new SilkroadMapEditor(window.renderPanel);        
            game.Run();            
        }
    }
}

