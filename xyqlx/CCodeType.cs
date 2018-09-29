using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using System.IO;

namespace xyqlx
{
    class CCodeType
    {
        private bool succeed;
        private string language;
        private string typeName;
        private string callCommand;
        private string createPath;
        private string suffix;
        private class CAnno
        {
            private string firstLine;
            private string beforeLine;
            private string lastLine;

            public string FirstLine { get => firstLine; set => firstLine = value; }
            public string BeforeLine { get => beforeLine; set => beforeLine = value; }
            public string LastLine { get => lastLine; set => lastLine = value; }

        }
        private CAnno annotation = new CAnno();
        private class CImport
        {
            private string beforeInclude;
            private string afterInclude;
            private class CIncludes
            {
                private class CInclude
                {
                    private string call;
                    private string text;
                    private string asText;
                    private string appText;

                    public string Call { get => call; set => call = value; }
                    public string Text { get => text; set => text = value; }
                    public string AsText { get => asText; set => asText = value; }
                    public string AppText { get => appText; set => appText = value; }
                }
                private List<CInclude> includesContent;

                private List<CInclude> IncludesContent { get => includesContent; set => includesContent = value; }
            }
            private CIncludes includes;

            public string BeforeInclude { get => beforeInclude; set => beforeInclude = value; }
            public string AfterInclude { get => afterInclude; set => afterInclude = value; }
            private CIncludes Includes { get => includes; set => includes = value; }
        }
        private CImport import=new CImport();
        private string context;
        private string openStyle;

        public CCodeType(string typeCommand)
        {
            CallCommand = typeCommand;
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\xy\\xyqlx.xml");
            XmlElement root = doc.DocumentElement;
            XmlNode node = root.SelectSingleNode("/xyqlx/languages/language/types/type[call/text()='"+callCommand+"']");
            if(node == null)
            {
                succeed = false;
                return;
            }
            succeed = true;
            XmlNode langNode = node.ParentNode.ParentNode;
            typeName = node.SelectSingleNode(".//name/text()").Value;
            SetAnnotation(node.SelectSingleNode(".//annotation/firstLine/text()").Value,
                node.SelectSingleNode(".//annotation/beforeLine/text()").Value,
                node.SelectSingleNode(".//annotation/lastLine/text()").Value);
            context = node.SelectSingleNode(".//context/text/text()").Value;
            createPath = node.SelectSingleNode(".//path/text()").Value;
            SetImport(node.SelectSingleNode(".//import/beforeInclude/text()").Value,
                node.SelectSingleNode(".//import/afterInclude/text()").Value);
            language = langNode.SelectSingleNode(".//name/text()").Value;
            openStyle = node.SelectSingleNode(".//openStyle/text()").Value;
            suffix = langNode.SelectSingleNode(".//suffix/text()").Value;
        }
        public void createCodeFile(Dictionary<string,string> argsDict)
        {
            string defaultPath = "C:\\code\\"+Language+"\\"+typeName;
            string orderStrVer = "011";
            if (false == System.IO.Directory.Exists(defaultPath))
            {
                System.IO.Directory.CreateDirectory(defaultPath);
            }
            if (CreatePath.IndexOf("%FOLDER%") != -1)
            {
                defaultPath += "\\" + callCommand + orderStrVer;
                System.IO.Directory.CreateDirectory(defaultPath);
            }
            if (CreatePath == "%FILE%")
            {
                string codeFilePath = defaultPath + "\\" + callCommand + orderStrVer + "." + suffix;
                File.Create(codeFilePath);
                StreamWriter sw = new StreamWriter(codeFilePath, true);
                sw.Write(GetFirstLine());
                sw.Write(GetBeforeLine());
                sw.WriteLine("Filename : " + CallCommand + orderStrVer + "." + suffix);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Category : " + argsDict["c"]);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Description : " + argsDict["d"]);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Author : " + "暂时先这样。。");
                sw.Write(GetBeforeLine());
                sw.WriteLine("Date : " + "同上");
                sw.Write(GetBeforeLine());
                sw.WriteLine("Principle: " + argsDict["p"]);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Addition : " + argsDict["a"]);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Source : " + argsDict["s"]);
                sw.Write(GetLastLine());
                //暂时先这样吧。。。
                sw.Close();
            }
        }
        public string Language { get => language; set => language = value; }
        public string TypeName { get => typeName; set => typeName = value; }
        public string CallCommand { get => callCommand; set => callCommand = value; }
        public string CreatePath { get => createPath; set => createPath = value; }

        public string GetFirstLine()
        {
            return annotation.FirstLine;
        }

        public string GetBeforeLine()
        {
            return annotation.BeforeLine;
        }

        public string GetLastLine()
        {
            return annotation.LastLine;
        }

        public void SetAnnotation(string firstLine, string beforeLine, string lastLine)
        {
            annotation.LastLine = lastLine;
            annotation.BeforeLine = beforeLine;
            annotation.FirstLine = firstLine;
        }

        public string GetBeforeInclude()
        {
            return import.BeforeInclude;
        }

        public string GetAfterInclude()
        {
            return import.AfterInclude;
        }

        public void SetImport(string beforeInclude, string afterInclude)
        {
            import.BeforeInclude = beforeInclude;
            import.AfterInclude = afterInclude;
        }

        public string Context { get => context; set => context = value; }
        public string OpenStyle { get => openStyle; set => openStyle = value; }
        public bool Succeed => succeed;

        public string Suffix { get => suffix; set => suffix = value; }
    }
}
