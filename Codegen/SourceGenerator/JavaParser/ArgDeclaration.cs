namespace JavaParser
{
    public class ArgDeclaration
    {
        public string ArgType { get; set; }
        public string ArgName { get; set; }

        public ArgDeclaration(string argType, string argName)
        {
            ArgName = argName;
            ArgType = argType;
        }
    }
}