using System;

namespace Doxy.Entities
{
    [Serializable]
    public class Dictionary : IModel
    {
        public int ID { get; set; }

        public BaseWord BaseWord { get; set; }

        public BaseWord TranslatedWord { get; set; }
    }
}