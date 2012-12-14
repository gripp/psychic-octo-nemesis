using System;
using CS8803AGA.PsychSim;
namespace CS8803AGA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
           // testProgram.test();
            //TestHarness.WorldBuilder.test();
            using (Engine game = new Engine())
            {
                game.Run();
            }
        }
    }
}

