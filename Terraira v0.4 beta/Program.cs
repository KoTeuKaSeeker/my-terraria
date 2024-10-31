using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;
using System.Diagnostics;

namespace Terraira_v0._4_beta
{
    class Program
    {

        public static RenderWindow win;
        public static Terraria terraria;
        public static Random rand = new Random();
        public static ChunkThread chunkThread;
        private static float k = 0.5625f;
        public static float time_coff = 0.5f;

        static void Main(string[] args)
        { 
            
            win = new RenderWindow(new VideoMode(1000, (uint)(1000 * k)), "Teraria v0.4beta");
            win.SetVerticalSyncEnabled(false);
            chunkThread = new ChunkThread();
            chunkThread.start();
            win.Closed += Win_Close;
            win.Resized += Win_Resized;
            Content.loadAllTextures();
            Registery.registery();
            terraria = new Terraria();

            Stopwatch stopwatch = new Stopwatch();

            while (win.IsOpen)
            {
                stopwatch.Restart();
                stopwatch.Start();
                win.DispatchEvents();
                win.Clear(new Color(166, 255, 246));

                win.Draw(terraria);

                win.Display();
                stopwatch.Stop();

                DeltaTime.deltaTime = stopwatch.ElapsedMilliseconds;
                DeltaTime.deltaTime *= time_coff;
            }

        }

        private static void Win_Resized(object sender, SizeEventArgs e)
        {
            win.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        private static void Win_Close(object sender, EventArgs e)
        {
            terraria.map.activeWorld.saveActiveChunks();
            DeltaTime.deltaTimeThread.Abort();
            chunkThread.close();
            win.Close();
        }
    }
}
