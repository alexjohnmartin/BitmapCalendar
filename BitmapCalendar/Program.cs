using System;
using System.Drawing;

namespace BitmapCalendar
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ERROR - no outpuf file name specified");
                Console.WriteLine();
                Console.WriteLine("Usage: BitmapCalendarRenderer.exe outputfilename");
                Console.WriteLine();
                return; 
            }

            var renderer = new BitmapCalendarRenderer();
            var bitmap = renderer.DrawThisMonthsCalendar(600, 300, 14, Color.Red, Color.White); 
            bitmap.Save(args[0]);
            Console.WriteLine("DONE - image saved to " + args[0]);
        }
    }
}
