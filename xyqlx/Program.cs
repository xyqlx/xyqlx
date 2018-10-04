using System;
using System.Collections.Generic;
using System.IO;

namespace xyqlx
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> argsDict = new Dictionary<string, string>();
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
            CCodeType codeType = new CCodeType(args[0].Substring(1));
            if (!codeType.Succeed)
            {
                Console.WriteLine("Sorry we failed.");
            }
            else
            {
                codeType.createCodeFile(argsDict);
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
