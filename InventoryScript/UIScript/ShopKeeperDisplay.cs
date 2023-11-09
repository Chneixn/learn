using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlot_UI _shopSlotPrefab;
    [SerializeField] private ShoppingCartItem _shoppingCartItemPrefab;

    [SerializeField] private Button _buyTab;
    [SerializeField] private Button _sellTab;

    [Header("Shopping Cart")]
    [SerializeField] private TextMeshProUGUI _basketTotalText;
    [SerializeField] private TextMeshProUGUI _playerGoldText;
    [SerializeField] private TextMeshProUGUI _shopGoldText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;

    [Header("Item Preview Section")]
    [SerializeField] private Image _itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI _itemPreviewName;
    [SerializeField] private TextMeshProUGUI _itemPreviewDescription;

    [Header("Panel")]
    [SerializeField] private GameObject _itemListContentPanel;
    [SerializeField] private GameObject _shoppingCartContentPanel;

    //private int _basketTotalValue;

    private ShopSystem _shopSystem;
    private PlayerInventoryHolder _playerInventoryHolder;

    private Dictionary<InventoryItemDate, int> _shoppingCart = new();
    private Dictionary<InventoryItemDate, ShopSlot_UI> _shoppingCartUI = new();

    public void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {
        _shopSystem = shopSystem;
        _playerInventoryHolder = playerInventory;

        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        ClearShopSlots();

        _basketTotalText.enabled = false;
        Tools.ISetActive(_buyButton.gameObject, false);
        //_basketTotalValue = 0;
        _playerGoldText.text = $"玩家金额：{_playerInventoryHolder.PrimaryInventorySystem.Gold}";
        //_playerGoldText.text = $"Player Gold: {_playerInventoryHolder.PrimaryInventorySystem.Gold}";
        _shopGoldText.text = $"商家金额：{_shopSystem.AvailableGold}";
        //_shopGoldText.text = $"Shop Gold: {_playerInventoryHolder.PrimaryInventorySystem.Gold}";

        DisplayShopInventory();
    }

    private void ClearShopSlots()
    {
        _shoppingCart = new Dictionary<InventoryItemDate, int>();
        _shoppingCartUI = new Dictionary<InventoryItemDate, ShopSlot_UI>();

        foreach (var item in _itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        foreach (var item in _shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void DisplayShopInventory()
    {
        foreach (ShopSlot item in _shopSystem.ShopInventory)
        {
            if (item.ItemDate == null) continue;
            ShopSlot_UI shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Init(item, _shopSystem.BuyMarkUp);
        }
    }

    private void DisPlayerInventory()
    {

    }
}
