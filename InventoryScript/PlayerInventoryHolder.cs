using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChange;

    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;

    public static UnityAction<InventorySystem, int> OnChestInteraction;

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
    }

    private void OnEnable()
    {
        ChestInventory.OnChestInteraction += DisplayPlayerInventoryOnChestInteraction;
    }

    private void OnDisable()
    {
        ChestInventory.OnChestInteraction -= DisplayPlayerInventoryOnChestInteraction;
    }

    protected override void LoadInventory(SaveDate data)
    {
        // 检查存档数据是否存在，存在则导入数据
        if (data.playerInventory.InvSystem != null)
        {
            this.primaryInventorySystem = data.playerInventory.InvSystem;
            OnPlayerInventoryChange?.Invoke();
        }
    }

    private void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
            OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offest);
    }

    public bool AddToInventory(InventoryItemDate data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }

    public void DisplayPlayerInventoryOnChestInteraction(InventorySystem invSystem, int _offest)
    {
        OnChestInteraction?.Invoke(invSystem, _offest);
        OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offest);
    }
}
