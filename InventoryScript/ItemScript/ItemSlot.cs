using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [NonSerialized] protected InventoryItemDate itemDate; // �������Ʒ����
    [SerializeField] protected int _itemID = -1;
    [SerializeField] protected int stackSize; // �������Ʒ��ʵ������

    public InventoryItemDate ItemDate => itemDate;
    public int StackSize => stackSize;

    public void ClearSlot()
    {
        itemDate = null;
        _itemID = -1;
        stackSize = -1;
    }

    /// <summary>
    /// ���ʵ�ǰ�������Ʒ���봫��Ĳ����Ʒ�Ƿ���ͬ
    /// ��ͬ�����ӵ�ǰ�����Ʒ����
    /// ��ͬ������ǰ��ۻ�Ϊ������
    /// </summary>
    /// <param name="invSlot">����Ĳ��</param>
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
    /// ���ӵ�ǰ�����Ʒ����  ÿ��+1
    /// </summary>
    /// <param name="amount"></param>
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    /// <summary>
    /// ���ٵ�ǰ�����Ʒ����  ÿ��-1
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
