using Bit0.Utils.Windows.Info.Ui;
using System;

namespace Bit0.Utils.Windows
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "WinInfo";
            var info = new Tui();
            Console.Title = Console.Title +  ": " + info.SystemName;
            Console.WriteLine(info.GetInfo());
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
