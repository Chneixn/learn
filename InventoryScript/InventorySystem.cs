using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Drawing;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => inventorySlots.Count;

    [SerializeField] private int _gold;
    public int Gold => _gold;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    /// <summary>
    /// ����һ������б���������Ĳ������
    /// </summary>
    /// <param name="size"></param>
    public InventorySystem(int size)
    {
        _gold = 0;

        CreateInventory(size);
    }

    public InventorySystem(int size, int gold)
    {
        _gold = gold;

        CreateInventory(size);
    }

    private void CreateInventory(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    /// <summary>
    /// �����Ʒ�������б�
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns>�Ƿ�ɹ�������</returns>
    public bool AddToInventory(InventoryItemDate itemToAdd, int amountToAdd)
    {
        if (ContainItem(itemToAdd, out List<InventorySlot> invSlot)) //�����Ʒ�Ƿ��Ѵ����б���
        {
            foreach (var slot in invSlot)
            {
                //��ѯÿ���洢����ͬ��Ʒ�Ĳ���Ƿ��пռ�洢
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot)) //��ȡ��һ�����õĲ��
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ��ѯ�б����Ƿ��д洢��ͬ��Ʒ�Ĳ�ۣ�����洢��ͬ��Ʒ��۵��б�
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="invSlot"></param>
    /// <returns></returns>
    public bool ContainItem(InventoryItemDate itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = InventorySlots.Where(i => i.ItemDate == itemToAdd).ToList();

        return invSlot == null ? false : true;
    }

    /// <summary>
    /// ������б����Ƿ��пղ�ۣ�����б��е�һ���ղ��
    /// </summary>
    /// <param name="freeSlot"></param>
    /// <returns></returns>
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemDate == null);
        return freeSlot == null ? false : true;
    }
}
