using System.Collections.Generic;

namespace JavaParser
{
    public class MethodDeclaration
    {
        public string MethodName { get; set; }
        public string ReturnType { get; set; }
        public List<ArgDeclaration> ArgList { get; set; }
        public string Url { get; set; }
        public string HttpMethodName { get; set; }

        public MethodDeclaration(string methodName, string returnType, List<ArgDeclaration> argList, string url, string httpMethodName)
        {
            MethodName = methodName;
            ReturnType = returnType;
            ArgList = argList;
            Url = url;
            HttpMethodName = httpMethodName;
        }
    }
}