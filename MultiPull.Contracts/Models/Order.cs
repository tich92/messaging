using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiPull.Contracts.Models
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Header { get; private set; }
        public double TotalPrice { get; private set; }
        public int TotalCount { get; private set; }

        public ICollection<OrderItem> Items { get; set; }

        public OrderState State { get; private set; }

        public DateTime CreatedOn { get; private set; }
        public DateTime ModifiedOn { get; private set; }

        #region Logic

        public void AddItem(int count, double price)
        {
            Items.Add(new OrderItem()
            {
                Id = Guid.NewGuid(),
                Count = count,
                Price = price,
                ProductId = Guid.NewGuid()
            });

            CalculateOrder();
        }

        public void RemoveItem(Guid itemId)
        {
            var item = Items.FirstOrDefault(o => o.Id == itemId);

            if(item == null)
                return;

            Items.Remove(item);

            CalculateOrder();
        }

        private void CalculateOrder()
        {
            TotalPrice = Items.Sum(i => i.Price * i.Count);
            TotalCount = Items.Sum(i => i.Count);
        }

        public void Initialize()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            Items = new List<OrderItem>();

            State = OrderState.Draft;
            Id = id;
            CustomerId = customerId;

            Header = $"{customerId}-{id}";

            CreatedOn = DateTime.UtcNow;
        }

        public void Agree()
        {
            State = OrderState.Agree;
            ModifiedOn = DateTime.UtcNow;
        }

        public void Cancel()
        {
            State = OrderState.Canceled;
            ModifiedOn = DateTime.UtcNow;
        }

        #endregion
    }

    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }

    public enum OrderState
    {
        Draft,
        Agree,
        Canceled
    }
}
