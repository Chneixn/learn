using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{
    /// <summary>
    /// ����һ�����
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
    /// ����һ���հײ��
    /// </summary>
    public InventorySlot()
    {
        ClearSlot();
    }

    /// <summary>
    /// ���µ�ǰ�������������
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
    /// �жϵ�ǰ��۵�ʣ��ռ��Ƿ��㹻��Ӵ�������������ʣ��ռ�Ĵ�С
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
    /// �жϵ�ǰ��۵�ʣ��ռ��Ƿ��㹻��Ӵ��������
    /// </summary>
    /// <param name="amoutnToAdd"></param>
    /// <returns></returns>
    public bool EnoughRoomLeftInStack(int amoutnToAdd)
    {
        if (stackSize + amoutnToAdd <= itemDate.MaxStackSize) { return true; }
        else return false;
    }

    /// <summary>
    /// ��ֲ������Ʒ���������һ�룩
    /// </summary>
    /// <param name="splitStack"></param>
    /// <returns></returns>
    public bool SplitStack(out InventorySlot splitStack)
    {
        if (StackSize <= 1) //�������Ʒ�������ܲ��
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
