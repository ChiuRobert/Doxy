using System;

namespace Entities
{
    [Serializable]
    public class Dialect : IModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Language Language { get; set; }
        
        public override string ToString()
        {
            return "Dialect [ id = " + Id + ", name = " + Name + ", language = " + Language + " ]";
        }
    }
}