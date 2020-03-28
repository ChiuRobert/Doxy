using System;

namespace Doxy.Entities
{
    [Serializable]
    public class Dialect : IModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public Language Language { get; set; }
    }
}