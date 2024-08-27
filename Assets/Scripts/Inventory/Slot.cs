using System;

namespace SimpleInventory.Inventory
{
    public class Slot 
    {
        //TODO: We could use delegates instead of this
        public event Action<IItem, int> ItemChangedEvent;
        public event Action<int> AmountChangedEvent;
        public event Action DisposeEvent;

        public IItem Item { get; private set; } = null;
        public int ItemsAmount { get; private set; } = 0;

        public Slot(IItem item, int amount)
        {
            this.Item = item;
            this.ItemsAmount = amount;
        }

        public bool IsEmpty()
        {
            return Item == null && ItemsAmount == 0;
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

        public void Clean()
        {
            this.Item = null;
            this.ItemsAmount = 0;

            ItemChangedEvent?.Invoke(null, 0);
        }

        public void Dispose()
        {
            DisposeEvent?.Invoke();
        }
    }
}
