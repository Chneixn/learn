using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot_UI : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private ShopSlot _assignedItemSlot;

    [SerializeField] private Button _addItemToCartButton;
    [SerializeField] private Button _reomveItemFromCartButton;

    public ShopKeeperDisplay ParentDisplay { get; private set; }
    public float MarkUp { get; private set; }

    private void Awake()
    {
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;
        _itemName.text = string.Empty;
        _itemCount.text = string.Empty;

        _addItemToCartButton?.onClick.AddListener(AddItemToCart);
        _reomveItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    public void Init(ShopSlot slot, float markUp)
    {
        _assignedItemSlot = slot;
        MarkUp = markUp;
        UpdataUISlot();
    }

    private void UpdataUISlot()
    {
        if (_assignedItemSlot.ItemDate != null)
        {
            _itemSprite.sprite = _assignedItemSlot.ItemDate.Icon;
            _itemSprite.color = Color.white;
            _itemName.text = $"{_assignedItemSlot.ItemDate.DisplayName} - {_assignedItemSlot.ItemDate.Value}";
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.preserveAspect = true;
            _itemSprite.color = Color.clear;
            _itemName.text = string.Empty;
            _itemCount.text = string.Empty;
        }

    }

    private void AddItemToCart()
    {

    }

    private void RemoveItemFromCart()
    {

    }
}
