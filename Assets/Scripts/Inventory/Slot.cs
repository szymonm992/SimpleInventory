namespace SimpleInventory.Inventory
{
    public class Slot 
    {
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
        }

        public void AddAmount(int amountToAdd)
        {
            this.ItemsAmount += amountToAdd;
        }

        public void Clean()
        {
            this.Item = null;
            this.ItemsAmount = 0;
        }
    }
}
