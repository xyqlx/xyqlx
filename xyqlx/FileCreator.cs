using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyqlx
{
    class FileCreator
    {
        public static void Create(Dictionary<string, string> argsDict)
        {
            //有没有什么取代if的方法。。。
            //其实这应该是那个类的责任啊啊啊啊
            String path = CppGenerator.FilePos(argsDict);
            StreamWriter streamWriter = new StreamWriter(path, true);
            streamWriter.WriteLine(CppGenerator.Generate(argsDict));
            streamWriter.Close();
        }
    }
}
