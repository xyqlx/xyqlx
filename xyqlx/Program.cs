using System;
using System.Collections.Generic;
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
            Console.ReadKey();
        }
    }
}
