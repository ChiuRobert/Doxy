using System;

namespace Entities
{
    [Serializable]
    public class BaseWord : IModel
    {
        public int Id { get; set; }

        public string Word { get; set; }

        public Dialect Dialect { get; set; }
        
        public override string ToString()
        {
            return "BaseWord [ id = " + Id + ", word = " + Word + ", dialect = " + Dialect + " ]";
        }
    }
}