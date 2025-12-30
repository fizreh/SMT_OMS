using System.Collections.Generic;

namespace SMT.Domain.Entities
{
    public class Component : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private readonly List<BoardComponent> _boardComponents = new();
        public IReadOnlyCollection<BoardComponent> BoardComponents => _boardComponents.AsReadOnly();

        private Component() { } // For ORM

        public Component(string name, string description)
        {
            Name = name;
            Description = description;
        }

        // Optional: Add/remove board association method later
    }
}
