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
            SerialPort targetport; // Holds our coms port
            Bitmap sourceimage; // Holds the source image
            Byte[] myimage = new Byte[65]; // Holds the transmit buffer
            Byte frameposition; // Keeps our position in the transmit buffer.
            Color pixelcolor; // Holds the current pixel color.
            int offset; // Keeps track of where we are in the image

            Console.WriteLine("Opening Image...");
            sourceimage = new Bitmap("source.png");

            Console.WriteLine("Opening port...");
            targetport = new SerialPort("COM8",115200);
            targetport.Open();
            // Dump a frame of 0s into the device to reset the receiver.
            targetport.Write(myimage, 0, 65);

            // Make sure we're starting fresh.
            offset=0;

            // Until we get a keypress...
            Console.WriteLine("Displaying... (Press any key to stop.)");
            while (!Console.KeyAvailable)
            {
                // *** Start building the frame ***

                // Reset our frame pointer
                frameposition = 0;

                // For each pixel in the frame (scanning in left to right, top to bottom order)
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0+offset; x < 8+offset; x++)
                    {
                        // Get the pixel in the source image...
                        pixelcolor = sourceimage.GetPixel(x,y);
                        // Compute the pixel value. Pixels have the following format:
                        // Bit 7: Always high (0 is used for end of frame indication.)
                        // Bit 5-6: Red
                        // Bit 2-4: Green (green gets 1 extra bit since humans are more sensitive to green.)
                        // Bit 0-1: Blue
                        myimage[frameposition] = (byte)(0x80 | (pixelcolor.R & 0xC0) >> 1 | (pixelcolor.G & 0xE0) >> 3 | (pixelcolor.B & 0xC0) >> 6);

                        // Move to the next position.
                        frameposition++;
                    }
                }

                // Write a 0 at the end to mark an end of frame
                myimage[frameposition] = 0;
                // Transmit the buffer on the bus.
                targetport.Write(myimage, 0, 65);

                // Move one pixel to the right
                offset=offset+1;
                // If we're at the right edge of the source image
                if (offset + 8 >= sourceimage.Width)
                {
                    // Move back to the beginning
                    offset = 0;
                    // And wait a second
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    // Otherwise wait 100ms
                    System.Threading.Thread.Sleep(100);
                }
            }

            // If the user pressed a key
            Console.WriteLine("Closing...");
            // Disconnect
            targetport.Close();
            // And dispose our resources.
            sourceimage.Dispose();
            targetport.Dispose();          
            Console.WriteLine("Closed.");
        }
    }
}
