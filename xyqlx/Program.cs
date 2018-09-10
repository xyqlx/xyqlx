using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyqlx
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("收到" + args.Length + "条命令");
            foreach(string i in args) {
                Console.WriteLine(i);
            }
            String path = @"C:\code\cpp\test\test.txt";
            //FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(path, true);
            streamWriter.WriteLine("That sounds good.");
            streamWriter.Close();
            Console.ReadKey();
        }
    }
}
