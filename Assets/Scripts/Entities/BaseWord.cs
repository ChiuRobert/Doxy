using System;

namespace Doxy.Entities
{
    [Serializable]
    public class BaseWord : IModel
    {
        public int ID { get; set; }

        public string Word { get; set; }

        public Dialect Dialect { get; set; }
    }
}