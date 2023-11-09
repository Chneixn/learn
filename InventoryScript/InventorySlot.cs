using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{
    /// <summary>
    /// 创建一个插槽
    /// </summary>
    /// <param name="source"></param>
    /// <param name="amount"></param>
    public InventorySlot(InventoryItemDate source, int amount)
    {
        itemDate = source;
        _itemID = itemDate.ID;
        stackSize = amount;
    }

    /// <summary>
    /// 创建一个空白插槽
    /// </summary>
    public InventorySlot()
    {
        ClearSlot();
    }

    /// <summary>
    /// 更新当前插槽内所有数据
    /// </summary>
    /// <param name="newDate"></param>
    /// <param name="amount"></param>
    public void UpdateInventorySlot(InventoryItemDate newDate, int amount)
    {
        itemDate = newDate;
        _itemID = itemDate.ID;
        stackSize = amount;
    }

    /// <summary>
    /// 判断当前插槽的剩余空间是否足够添加传入的数量，输出剩余空间的大小
    /// </summary>
    /// <param name="amoutnToAdd"></param>
    /// <param name="amoutnRemaining"></param>
    /// <returns></returns>
    public bool EnoughRoomLeftInStack(int amoutnToAdd, out int amoutnRemaining)
    {
        amoutnRemaining = ItemDate.MaxStackSize - stackSize;

        return EnoughRoomLeftInStack(amoutnToAdd);
    }

    /// <summary>
    /// 判断当前插槽的剩余空间是否足够添加传入的数量
    /// </summary>
    /// <param name="amoutnToAdd"></param>
    /// <returns></returns>
    public bool EnoughRoomLeftInStack(int amoutnToAdd)
    {
        if (stackSize + amoutnToAdd <= itemDate.MaxStackSize) { return true; }
        else return false;
    }

    /// <summary>
    /// 拆分插槽内物品数量（拆分一半）
    /// </summary>
    /// <param name="splitStack"></param>
    /// <returns></returns>
    public bool SplitStack(out InventorySlot splitStack)
    {
        if (StackSize <= 1) //插槽内物品数量不能拆分
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(stackSize / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(itemDate, halfStack);
        return true;
    }
}
