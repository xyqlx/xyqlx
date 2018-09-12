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
            string res = "/**-----------------------------------------\n";
            res += "--Filename    : cp" + argsDict["cp"] + "002" + "\n";
            res += "--Category    : " + argsDict["c"] + "\n";
            res += "--Description : " + argsDict["d"] + "\n";
            res += "--Author      : " + "xyqlx" + "(mxxyqlx@qq.com)" + "\n";
            res += "--Date        : " + DateTime.Now.ToString() + "\n";
            res += "--Principle   : " + argsDict["p"] + "\n";
            res += "--Addition    : " + argsDict["a"] + "\n";
            res += "--Source      : " + argsDict["s"] + "\n";
            res += "-------------------------------------------*/\n";
            return res;
        }
    }
}
