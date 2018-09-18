using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyqlx
{
    static class CppGenerator
    {
        //还是改成数据成员吧。。。没法扩展了
        public static string FilePos(Dictionary<string,string> argsDict)
        {
            if (argsDict["cp"] == "t")
                return "C:\\code\\cpp\\test\\cpt001.cpp";
            else
                return "C:\\code\\cpp\\test\\cpt001.cpp";
        }
        public static string Generate(Dictionary<string,string> argsDict)
        {
            string[] headList = argsDict["i"].Split(';');
            return AnnotationGenerator.Generate(argsDict) + HeadIncludes(headList) 
                + (argsDict.ContainsKey("nousing")?"":UsingNames(headList))
                + BeforeMain(headList) + MainFunction();
        }
        public static string HeadIncludes(string[] headList) {
            string res = "";
            
            foreach(string i in headList)
            {
                res += "#include<" + i + ">\n";
            }
            return res;
        }
        public static string UsingNames(string[] headList) {
            string res = "";
            foreach (string i in headList) {
                if (i.Equals("iostream")) {
                    res += "using std::cin;\nusing std::cout;\nusing std::endl;\n";
                }
                //因为暂时不知道命名空间的详细信息，暂时采用此方案，可随时改进
                if (i.Equals("fstream"))
                {
                    res += "using std::ifstream;\nusing std::ofstream;\n";
                }
                if (i.Equals("vector"))
                {
                    res += "using std::vector;\n";
                }
                if (i.Equals("string")) {
                    res += "using std::string;\n";
                }
            }
            return res;
        }
        public static string BeforeMain(string[] headList)
        {
            string res = "";
            foreach (string i in headList)
            {
                if (i.Equals("fstream"))
                {
                    res += "ifstream fin(\"cpt002.in\");\n";
                    res += "ofstream fout(\"cpt002.out\");\n";
                    //关于此项目的相关信息。。。打算弄成类似单例模式这种的。。
                    //。这样也不用瞎传参了
                }
                //是否还需要其它选项。。。不明，暂且
            }
            return res;
        }
        public static string MainFunction()
        {
            //咱就按照一直以来用的格式写了吧。。。带参数的一般也不会需要吧。。
            return "int main()\n{\n\t\n\treturn 0;\n}";
        }
    }   
}
