using System;

namespace Entities
{
    [Serializable]
    public class Dictionary : IModel
    {
        public int Id { get; set; }

        public BaseWord BaseWord { get; set; }

        public BaseWord TranslatedWord { get; set; }
        
        public override string ToString()
        {
            return "Dictionary [ id = " + Id + ", baseWord = " + BaseWord + ", translatedWord = " + TranslatedWord + " ]";
        }
    }
}