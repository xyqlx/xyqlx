using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyqlx
{
    static class CppGenerator
    {
        public static string Generate(Dictionary<string,string> argsDict)
        {
            return AnnotationGenerator.Generate(argsDict);
        }

    }
}
