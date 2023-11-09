using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;
    [SerializeField] private ShopSystem _shopSystem;

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private void Awake()
    {
        _shopSystem = new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedGold,
            _shopItemsHeld.BuyMarkUp, _shopItemsHeld.SellMarkUp);

        foreach (ShopIventoryItem item in _shopItemsHeld.Items)
        {
            //Debug.Log($"{item.ItemData.DisplayName}: {item.Amount}");
            _shopSystem.AddToShop(item.ItemData, item.Amount);
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
            interactSuccessful = true;
        }
        else
        {
            interactSuccessful = false;
            Debug.LogError("PlayerInv not found!");
        }
    }

    public void EndInteraction()
    {

    }

}
