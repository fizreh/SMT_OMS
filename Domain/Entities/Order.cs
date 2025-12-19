using System;
using System.Collections.Generic;

namespace SMT.Domain.Entities
{
    public class Order : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime OrderDate { get; private set; }

        private readonly List<OrderBoard> _orderBoards = new();
        public IReadOnlyCollection<OrderBoard> OrderBoards => _orderBoards.AsReadOnly();

        private Order() { }

        public Order(string name, string description, DateTime orderDate)
        {
            Name = name;
            Description = description;
            OrderDate = orderDate;
        }
    }
}