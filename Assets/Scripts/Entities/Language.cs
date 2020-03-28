using System;

namespace Doxy.Entities
{
    [Serializable]
    public class Language : IModel
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}