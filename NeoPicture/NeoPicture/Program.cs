using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;



namespace NeoPicture
{
    class Program
    {
        static void Main(string[] args)
        {
            MatrixDisplayEngine mydisplay;
            System.Random R = new System.Random();
            string msg;

            Console.WriteLine("Opening port...");
            mydisplay = new MatrixDisplayEngine("COM8", 115200, 70);

            Console.WriteLine("Displaying... (Press any key to stop.)");
            mydisplay.DrawText("\x03\x03\x03 ", false);
            mydisplay.Start();
            while (!Console.KeyAvailable)
            {
                if (mydisplay.IsQueueEmpty())
                {
                    // Pick one of the 5 messages.
                    switch (R.Next(10))
                    {
                        case 1:
                            msg = "\x03\x03\x03Love is in the air\x03\x03\x03 ";
                            break;
                        case 2:
                            mydisplay.DrawImage("source.png", false);
                            mydisplay.DrawText("  ", false);
                            msg = null;
                            break;
                        case 3:
                            msg = "(\x03w\x03)/~~ ";
                            break;
                        case 4:
                            msg = "THE JOYOUS SPRINGTIME OF YOUTH! ";
                            break;
                        default:
                            msg = "\x03Have a happy Valentine's day!\x03 ";
                            break;
                    }

                    // If we have text
                    if (msg != null)
                    {
                        // Display it.
                        mydisplay.DrawText(msg, false);
                        Console.WriteLine("Displaying: " + msg);
                    }
                    else
                    {
                        Console.WriteLine("Displaying Picture.");
                    }
                }
                // Sleep to keep from spinlocking the system
                System.Threading.Thread.Sleep(500);
            }
            // Consume our stop key.
            Console.ReadKey();
            
            // Shutdown the display
            mydisplay.Stop();
            // If the user pressed a key
            Console.WriteLine("Closing...");
            // And dispose our resources.
            mydisplay.Dispose();
            Console.WriteLine("Closed.");

        }
    }
}
