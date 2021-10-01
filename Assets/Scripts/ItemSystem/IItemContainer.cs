public interface IItemContainer
{
    ItemSlot GetSlotByIndex(int slotIndex);

    ItemSlot AddItem(ItemSlot itemSlot);
    void RemoveItem(ItemSlot item);
    void RemoveAt(int slotIndex);
    void Swap(int indexA, int indexB);

    bool HasItem(InventoryItem item);
    int GetTotalQuantity(InventoryItem item);
}
