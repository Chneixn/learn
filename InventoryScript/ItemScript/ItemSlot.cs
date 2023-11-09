using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [NonSerialized] protected InventoryItemDate itemDate; // 插槽内物品数据
    [SerializeField] protected int _itemID = -1;
    [SerializeField] protected int stackSize; // 插槽内物品的实际数量

    public InventoryItemDate ItemDate => itemDate;
    public int StackSize => stackSize;

    public void ClearSlot()
    {
        itemDate = null;
        _itemID = -1;
        stackSize = -1;
    }

    /// <summary>
    /// 访问当前插槽内物品并与传入的插槽物品是否相同
    /// 相同：增加当前插槽物品数量
    /// 不同：将当前插槽换为传入插槽
    /// </summary>
    /// <param name="invSlot">传入的插槽</param>
    public void AssignItem(InventorySlot invSlot)
    {
        if (itemDate == invSlot.itemDate) AddToStack(invSlot.stackSize);
        else
        {
            itemDate = invSlot.itemDate;
            _itemID = itemDate.ID;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }

    public void AssignItem(InventoryItemDate data, int amount)
    {
        if (itemDate == data) AddToStack(amount);
        else
        {
            itemDate = data;
            _itemID = data.ID;
            stackSize = 0;
            AddToStack(amount);
        }
    }

    /// <summary>
    /// 增加当前插槽物品数量  每次+1
    /// </summary>
    /// <param name="amount"></param>
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    /// <summary>
    /// 减少当前插槽物品数量  每次-1
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public void OnAfterDeserialize()
    {
        if (_itemID == -1) return;

        var db = Resources.Load<Database>("Database");
        itemDate = db.GetItem(_itemID);
    }

    public void OnBeforeSerialize()
    {
        
    }
}
