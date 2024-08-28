using System;

namespace SimpleInventory.Inventory
{
    public class Slot 
    {
        public delegate void ItemChangedDelegate(IItem newItem, int newAmount); 
   
        public event ItemChangedDelegate ItemChangedEvent;
        public event Action<int> AmountChangedEvent;
        public event Action DisposeEvent;

        public IItem Item { get; private set; } = null;
        public int ItemsAmount { get; private set; } = 0;

        public Slot(IItem item, int amount)
        {
            this.Item = item;
            this.ItemsAmount = amount;
        }

        public void SetItem(IItem item, int amount)
        {
            this.Item = item;
            this.ItemsAmount = amount;

            ItemChangedEvent?.Invoke(item, amount);
        }

        public void AddAmount(int amountToAdd)
        {
            this.ItemsAmount += amountToAdd;

            AmountChangedEvent?.Invoke(ItemsAmount);
        }

        public void Dispose()
        {
            DisposeEvent?.Invoke();
        }
    }
}
