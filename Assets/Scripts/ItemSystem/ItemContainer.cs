using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemContainer : IItemContainer
{
    private ItemSlot[] itemSlots;

    public event Action OnItemsUpdated;

    public ItemContainer(int size)
    {
        itemSlots = new ItemSlot[size];
    }

    public ItemSlot GetSlotByIndex(int slotIndex) => itemSlots[slotIndex];

    public ItemSlot AddItem(ItemSlot itemSlot)
    {
        for (var i = 0; i < itemSlots.Length; ++i)
        {
            if (itemSlots[i].item != itemSlot.item) continue;

            var putAmount = Math.Min(itemSlot.item.MaxStack - itemSlots[i].quantity, itemSlot.quantity);
            itemSlots[i].quantity += putAmount;
            OnItemsUpdated?.Invoke();
            itemSlot.quantity -= putAmount;
            if (itemSlot.quantity == 0) return itemSlot; 
        }

        for (var i = 0; i < itemSlots.Length; ++i)
        {
            if (itemSlots[i].item != null) continue;
            
            var putAmount = Math.Min(itemSlot.item.MaxStack, itemSlot.quantity);
            itemSlots[i] = new ItemSlot(itemSlot.item, putAmount);
            OnItemsUpdated?.Invoke();
            itemSlot.quantity -= putAmount;
            if (itemSlot.quantity == 0) return itemSlot;
        }

        
        OnItemsUpdated?.Invoke();
        return itemSlot;
    }

    public void RemoveItem(ItemSlot itemSlot)
    {
        if (itemSlot.item == null) return;

        for (var i = 0; i < itemSlots.Length; ++i)
        {
            if (itemSlots[i].item != itemSlot.item) continue;

            if (itemSlots[i].quantity < itemSlot.quantity)
            {
                itemSlot.quantity -= itemSlots[i].quantity;
                itemSlots[i] = ItemSlot.Empty;
            } else
            {
                itemSlots[i].quantity -= itemSlot.quantity;
                if (itemSlots[i].quantity == 0)
                {
                    itemSlots[i] = ItemSlot.Empty;
                }
                break;
            }
        }
        
        OnItemsUpdated?.Invoke();
    }

    public void RemoveAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= itemSlots.Length) return;

        itemSlots[slotIndex] = ItemSlot.Empty;
        OnItemsUpdated?.Invoke();
    }

    public void Swap(int indexA, int indexB)
    {
        if (indexA < 0 || indexB < 0 || indexA >= itemSlots.Length || indexB >= itemSlots.Length) return;

        var slotA = itemSlots[indexA];
        var slotB = itemSlots[indexB];

        if (ItemSlot.Equals(slotA, slotB)) return;

        if (slotB.item != null)
        {
            if (slotA.item == slotB.item)
            {
                int availableSpace = slotB.item.MaxStack - slotB.quantity;
                
                if (slotA.quantity <= availableSpace)
                {
                    itemSlots[indexB].quantity += slotA.quantity;
                    itemSlots[indexA] = ItemSlot.Empty;
                    
                    OnItemsUpdated?.Invoke();
                    return;
                }
            }
        }

        itemSlots[indexA] = slotB;
        itemSlots[indexB] = slotA;
        OnItemsUpdated?.Invoke();
    }

    public bool HasItem(InventoryItem item)
    {
        return itemSlots.Where(s => s.quantity > 0).Any(s => s.item == item);
    }

    public int GetTotalQuantity(InventoryItem item)
    {
        return itemSlots.Where(s => s.item == item).Select(s => s.quantity).Sum();
    }
}
