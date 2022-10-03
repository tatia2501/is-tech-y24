using System.Collections.Generic;

namespace JavaParser
{
    public class EntityDeclaration
    {
        public EntityDeclaration(string entityName, List<ArgDeclaration> fields)
        {
            EntityName = entityName;
            Fields = fields;
        }

        public string EntityName { get; set; }
        public List<ArgDeclaration> Fields { get; set; }
        
    }
}