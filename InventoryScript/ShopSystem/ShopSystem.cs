using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> _shopInventory;
    [SerializeField] private int _availableGold;
    [SerializeField] private float _buyMarkUp;
    [SerializeField] private float _sellMarkUp;

    public List<ShopSlot> ShopInventory => _shopInventory;
    public int AvailableGold => _availableGold;
    public float BuyMarkUp => _buyMarkUp;
    public float SellMarkUp => _sellMarkUp;

    public ShopSystem(int size,int gold, float buyMarkUp,float sellMarkUp)
    {
        _availableGold = gold;
        _buyMarkUp = buyMarkUp;
        _sellMarkUp = sellMarkUp;

        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        _shopInventory = new List<ShopSlot>();

        for (int i = 0; i < size; i++)
        {
            _shopInventory.Add(new ShopSlot());
        }
    }

    public void AddToShop(InventoryItemDate data, int amount)
    {
        if (ContainItem(data, out ShopSlot shopSlot))
        {
            shopSlot.AddToStack(amount);
        }

        ShopSlot freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }

    private ShopSlot GetFreeSlot()
    {
        ShopSlot freeSlot = _shopInventory.FirstOrDefault(i => i.ItemDate == null);

        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            _shopInventory.Add(freeSlot);
        }

        return freeSlot;
    }


    /// <summary>
    /// 判断商店库存中是否有相同物品，输出商店物品插槽
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="shopSlot"></param>
    /// <returns></returns>
    public bool ContainItem(InventoryItemDate itemToAdd, out ShopSlot shopSlot)
    {
        shopSlot = _shopInventory.Find(i => i.ItemDate == itemToAdd);

        return shopSlot != null;
    }
}
