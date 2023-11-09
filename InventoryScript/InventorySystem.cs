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
    /// 创建一个插槽列表，输入所需的插槽数量
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
    /// 添加物品进入插槽列表
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns>是否成功进入插槽</returns>
    public bool AddToInventory(InventoryItemDate itemToAdd, int amountToAdd)
    {
        if (ContainItem(itemToAdd, out List<InventorySlot> invSlot)) //检查物品是否已存在列表中
        {
            foreach (var slot in invSlot)
            {
                //轮询每个存储有相同物品的插槽是否有空间存储
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot)) //获取第一个可用的插槽
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 查询列表中是否有存储相同物品的插槽，输出存储相同物品插槽的列表
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
    /// 检查插槽列表中是否有空插槽，输出列表中第一个空插槽
    /// </summary>
    /// <param name="freeSlot"></param>
    /// <returns></returns>
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemDate == null);
        return freeSlot == null ? false : true;
    }
}
