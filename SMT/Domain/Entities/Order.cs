using System;
using System.Collections.Generic;

namespace SMT.Domain.Entities
{
    public class Order : Entity
    {
        public string Name { get; set; }
        public string Description { get;  set; }
        public DateTime OrderDate { get; set; }

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