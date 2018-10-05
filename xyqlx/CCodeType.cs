using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using System.IO;
using System.Diagnostics;

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
        private string author;
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
            public class CIncludes
            {
                public class CInclude
                {
                    private string call;
                    private string text;
                    private string asText;
                    private string appText;

                    public CInclude(string call,string text,string astext,string apptext)
                    {
                        this.call = call;
                        this.text = text;
                        this.asText = astext;
                        this.appText = apptext;
                    }
                    
                    public string Call { get => call; set => call = value; }
                    public string Text { get => text; set => text = value; }
                    public string AsText { get => asText; set => asText = value; }
                    public string AppText { get => appText; set => appText = value; }
                }
                public void AddInclude(string callCommand,string text,string asText,string apptext)
                {
                    includesContent.Add(callCommand, new CInclude(callCommand, text, asText, apptext));
                }
                private Dictionary<string,CInclude> includesContent = new Dictionary<string, CInclude>();

                public Dictionary<string,CInclude> IncludesContent { get => includesContent; set => includesContent = value; }
            }
            private string Head(CIncludes.CInclude include)
            {
                if (include.Text == "") return "";
                else return beforeInclude + include.Text + afterInclude + "\n";
            }
            private string Tail(CIncludes.CInclude include)
            {
                if (include.Text == "") return "";
                else return include.AppText;
            }
            public string GetHead(string cmd)
            {
                if (includes.IncludesContent.ContainsKey(cmd))
                {
                    return Head(Includes.IncludesContent[cmd]);
                }
                else return beforeInclude + cmd + afterInclude+"\n";
            }
            public string GetTail(string cmd)
            {
                if (includes.IncludesContent.ContainsKey(cmd))
                {
                    return Tail(Includes.IncludesContent[cmd]);
                }
                else return "";
            }
            private CIncludes includes = new CIncludes();

            public string BeforeInclude { get => beforeInclude; set => beforeInclude = value; }
            public string AfterInclude { get => afterInclude; set => afterInclude = value; }
            public CIncludes Includes { get => includes; set => includes = value; }
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
            author = root.SelectSingleNode("/xyqlx/info/author/text()").Value;
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
            foreach(XmlNode i in node.SelectNodes(".//import/includes/include"))
            {
                string callString="", textString="", asTextString="", appTextString="";
                if (i.SelectSingleNode(".//call/text()") != null) callString = i.SelectSingleNode(".//call/text()").Value;
                if (i.SelectSingleNode(".//text/text()") != null) textString = i.SelectSingleNode(".//text/text()").Value;
                if (i.SelectSingleNode(".//asText/text()") != null) asTextString = i.SelectSingleNode(".//asText/text()").Value;
                if (i.SelectSingleNode(".//appText/text()") != null) appTextString = i.SelectSingleNode(".//appText/text()").Value;
                import.Includes.AddInclude(callString, textString, asTextString, appTextString);
            }
        }
        //进行必要的转义
        public string ReplaceAllSymbol(string toReplace,string orderStrVer="")
        {
            if (toReplace.IndexOf("%FILE%") != -1)
                return toReplace.Replace("%FILE%", "C:\\code\\" + Language + "\\" + typeName + "\\" + callCommand + orderStrVer + "." + suffix);
            else if (toReplace.IndexOf("%FILENAMENOEXTENSION%")!=-1)
                return toReplace.Replace("%FILENAMENOEXTENSION%", callCommand + orderStrVer);
            else
                return toReplace;
        }
        public void createCodeFile(Dictionary<string,string> argsDict)
        {
            //表示代码在当前类型的顺序
            int order = 0;
            //xml对象
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\xy\\code.xml");
            //xml根
            XmlElement root = doc.DocumentElement;
            //xml根节点
            XmlNode nodeXY = root.SelectSingleNode("/xy");
            //类型节点
            XmlNode node = root.SelectSingleNode("/xy/" + language + "/" + TypeName);
            //是否创建类型节点
            if (node == null)
            {
                //创建已有代码数为0的类型节点
                XmlNode nodeCntText = doc.CreateTextNode("0");
                XmlElement nodeCnt = doc.CreateElement("cnt");
                XmlElement nodeType = doc.CreateElement(TypeName);
                nodeCnt.AppendChild(nodeCntText);
                nodeType.AppendChild(nodeCnt);
                //查找相应父语言节点
                XmlNode nodeLanguage = root.SelectSingleNode("/xy/" + language);
                //如果找不到就创建一个
                if (null == nodeLanguage)
                {
                    XmlElement tempNodeLanguage = doc.CreateElement(language);
                    nodeXY.AppendChild(tempNodeLanguage);
                    //再次查找，这一步需要保证一定查到
                    nodeLanguage = root.SelectSingleNode("/xy/" + language);
                }
                //类型添加完成，node转向类型节点
                nodeLanguage.AppendChild(nodeType);
                node = root.SelectSingleNode("/xy/" + language + "/" + TypeName);
            }   //顺序等于类型中已有代码数+1
            else order = int.Parse(node.SelectSingleNode(".//cnt/text()").Value);
            ++order;
            //求字符串格式的order
            string orderStrVer = order.ToString("D3");
            //设置type的已有节点数
            node.SelectSingleNode(".//cnt").InnerText = order.ToString();
            
            //开始创建代码文件
            string defaultPath = "C:\\code\\"+Language+"\\"+typeName;
            
            if (false == System.IO.Directory.Exists(defaultPath))
            {
                System.IO.Directory.CreateDirectory(defaultPath);
            }
            if (CreatePath.IndexOf("%FOLDER%") != -1)
            {
                defaultPath += "\\" + callCommand + orderStrVer;
                System.IO.Directory.CreateDirectory(defaultPath);
            }
            if (CreatePath.IndexOf("%FILE%")!=-1)
            {
                string codeFilePath = defaultPath + "\\" + callCommand + orderStrVer + "." + suffix;
                StreamWriter sw = new StreamWriter(codeFilePath, true);
                //以下是打印注释
                sw.Write(GetFirstLine());
                sw.Write(GetBeforeLine());
                sw.WriteLine("Filename    : " + CallCommand + orderStrVer + "." + suffix);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Category    : " + argsDict["c"]);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Description : " + argsDict["d"]);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Author      : " + author);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Date        : " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
                sw.Write(GetBeforeLine());
                sw.WriteLine("Principle   : " + argsDict["p"]);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Addition    : " + argsDict["a"]);
                sw.Write(GetBeforeLine());
                sw.WriteLine("Source      : " + argsDict["s"]);
                sw.Write(GetLastLine());
                //这里是打印import
                string includesString = argsDict["i"];
                string[] includesStringArray = includesString.Split(';');
                foreach(string i in includesStringArray)
                {
                    sw.Write(import.GetHead(i));
                }
                foreach (string i in includesStringArray)
                {
                    sw.Write(ReplaceAllSymbol(import.GetTail(i),orderStrVer));
                }
                sw.Write(context);
                sw.Close();
                defaultPath = codeFilePath;
            }
            //创建code及其节点(放后面是因为可以引用生成时的数据）
            XmlElement nodeCode = doc.CreateElement("code");
            XmlElement nodeID = doc.CreateElement("id");
            nodeID.InnerText = order.ToString();
            nodeCode.AppendChild(nodeID);
            XmlElement nodeFileName = doc.CreateElement("filename");
            nodeFileName.InnerText = callCommand + orderStrVer + "." + suffix;
            nodeCode.AppendChild(nodeFileName);
            XmlElement nodePath = doc.CreateElement("path");
            nodePath.InnerText = defaultPath;
            nodeCode.AppendChild(nodePath);
            XmlElement nodeCategory = doc.CreateElement("category");
            nodeCategory.InnerText = argsDict["c"];
            nodeCode.AppendChild(nodeCategory);
            XmlElement nodeDescription = doc.CreateElement("description");
            nodeDescription.InnerText = argsDict["d"];
            nodeCode.AppendChild(nodeDescription);
            XmlElement nodeDate = doc.CreateElement("date");
            nodeDate.InnerText = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            nodeCode.AppendChild(nodeDate);
            XmlElement nodePrinciple = doc.CreateElement("principle");
            nodePrinciple.InnerText = argsDict["p"];
            nodeCode.AppendChild(nodePrinciple);
            XmlElement nodeAddition = doc.CreateElement("addition");
            nodeAddition.InnerText = argsDict["a"];
            nodeCode.AppendChild(nodeAddition);
            XmlElement nodeSource = doc.CreateElement("source");
            nodeSource.InnerText = argsDict["s"];
            nodeCode.AppendChild(nodeSource);
            node.AppendChild(nodeCode);
            doc.Save("C:\\xy\\code.xml");
            //打开代码文件
            if (argsDict.ContainsKey("o"))
            {
                string realOpenStyle = ReplaceAllSymbol(openStyle,orderStrVer);
                
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = "cmd";
                myProcess.StartInfo.UseShellExecute = false; ;
                myProcess.StartInfo.RedirectStandardInput = true;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.RedirectStandardError = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.StandardInput.WriteLine(realOpenStyle + "&exit");
                //myProcess.StandardInput.WriteLine("exit");
                myProcess.StandardInput.AutoFlush = true;
                myProcess.WaitForExit();
                myProcess.Close();
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
