using System;

namespace Entities
{
    [Serializable]
    public class Language : IModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public override string ToString()
        {
            return "Language [ id = " + Id + ", name = " + Name + " ]";
        }
    }
}