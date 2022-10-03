using System;
using System.Collections.Generic;

namespace JavaParser
{
    public class Parser
    {
        public static List<MethodDeclaration> ControllerParser(string path)
        {
            var methods = new List<MethodDeclaration>();
            string url = "";
            string httpMethodName = "";
            bool flagMapping = false;
            foreach (string line in System.IO.File.ReadLines(path))
            {
                if (flagMapping)
                {
                    string[] words = line.Split(' ');
                    var returnType = words[5];
                    var methodName = words[6].Substring(0, words[6].LastIndexOf('('));
                    List<ArgDeclaration> argList = new List<ArgDeclaration>();
                    for (int i = words.Length - 3; i > 5; i-=2)
                    {
                        string argType = words[i].Substring(words[i].LastIndexOf('(') + 1, words[i].Length - words[i].LastIndexOf('(') - 1);
                        string argName = words[i+1].Trim( new Char[] { ')', ',' } );
                        var args = new ArgDeclaration(argType, argName);
                        argList.Add(args);
                    }

                    var methodDeclaration = new MethodDeclaration(methodName, returnType, argList, url, httpMethodName);
                    methods.Add(methodDeclaration);

                }
                
                if (line.Contains("Mapping("))
                {
                    flagMapping = true;
                    string[] words = line.Split(' ');
                    url = words[6].Trim( new Char[] { '"', ',' } );
                    if (words[4][1] == 'G')
                    {
                        httpMethodName = "get";
                    }
                    if (words[4][1] == 'P')
                    {
                        httpMethodName = "post";
                    }
                }
                else
                {
                    flagMapping = false;
                }
            }

            // foreach (var met in methods)
            // {
            //     Console.Write("returnType:  ");
            //     Console.WriteLine(met.ReturnType);
            //     Console.Write("methodName:  ");
            //     Console.WriteLine(met.MethodName);
            //     Console.Write("url:  ");
            //     Console.WriteLine(met.Url);
            //     Console.Write("httpMethodName:  ");
            //     Console.WriteLine(met.HttpMethodName);
            //     foreach (var arg in met.ArgList)
            //     {
            //         Console.Write("argType:  ");
            //         Console.WriteLine(arg.ArgType);
            //         Console.Write("argName:  ");
            //         Console.WriteLine(arg.ArgName);
            //     }
            //     Console.WriteLine(" ");
            // }
            return methods;
        }

        public static EntityDeclaration EntityParser(string path)
        {
            string entityName = "";
            List<ArgDeclaration> fields = new List<ArgDeclaration>();
            bool flag = false;
            foreach (string line in System.IO.File.ReadLines(path))
            {
                if (line.Contains("class"))
                {
                    string[] words = line.Split(' ');
                    entityName = words[2];
                }

                if (flag)
                {
                    string[] words = line.Split(' ');
                    var args = new ArgDeclaration(words[5], words[6].Trim(';'));
                    fields.Add(args);
                }

                if (line.Contains("Column"))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            var entityDeclaration = new EntityDeclaration(entityName, fields);
            // Console.Write("entityName:  ");
            // Console.WriteLine(entityDeclaration.EntityName);
            //  Console.WriteLine("args:  ");
            //  foreach (var arg in entityDeclaration.Fields)
            //  {
            //      Console.Write(arg.ArgType);
            //      Console.Write(" ");
            //      Console.WriteLine(arg.ArgName);
            //  }
             return entityDeclaration;
        }
    }
}