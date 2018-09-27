using System;
using System.Collections.Generic;
using System.IO;

namespace xyqlx
{
    class Program
    {
        private static Dictionary<string, string> argsDict = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            string lastArg = "";
            //这里没有考虑args可能出现的其它情形，甚至可能报错（
            if (args.Length == 0) return;
            foreach (string i in args)
            {
                if (i[0] == '-')
                {
                    argsDict.Add(i.Substring(1), "");
                    lastArg = i.Substring(1);
                }
                else
                    argsDict[lastArg] = i;
            }
            //Console.WriteLine(CppGenerator.Generate(argsDict));
            //String path = @"C:\code\cpp\test\test.txt";
            //StreamWriter streamWriter = new StreamWriter(path, true);
            //streamWriter.WriteLine("That sounds good.");
            //streamWriter.Close();
            Console.ReadKey();
        }
    }
}
