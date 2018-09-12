using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyqlx
{
    static class AnnotationGenerator
    {
        public static string Generate(Dictionary<string,string> argsDict) {
            string res = "/**\n";
            res += " *Filename: cp" + argsDict["cp"] + "002" + "\n";
            res += " *Category: " + argsDict["ct"] + "\n";
            res += " *Description: " + argsDict["dc"] + "\n";
            res += " *Author: " + "xyqlx" + "(mxxyqlx@qq.com)" + "\n";
            res += " *Date: " + DateTime.Now.ToString() + "\n";
            res += " *Principle: " + argsDict["rp"] + "\n";
            res += " *Addition: " + argsDict["ad"] + "\n";
            res += " *Source: " + argsDict["sc"] + "\n";
            res += " */\n";
            return res;
        }
    }
}
