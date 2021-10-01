using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class InventoryItem : ScriptableObject
{
    [Header("Item data")]
    [SerializeField] private new string name = "New inventory item";
    [SerializeField] private Sprite icon = null;
    [SerializeField] [Min(1)] private int maxStack = 1; 

    public string Name => name;
    public Sprite Icon => icon;
    public int MaxStack => maxStack;

    public string GetInfoDisplayText()
    {
        var sb = new StringBuilder();
        sb.Append(Name).AppendLine();
        sb.Append("Max stack: ").Append(MaxStack).AppendLine();
        return sb.ToString();
    }
}
