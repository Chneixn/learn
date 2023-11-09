using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InventoryUIController : MonoBehaviour
{
    [FormerlySerializedAs("chestPanel")] public DynamicInventoryDisplay inventoryPanel;
    public DynamicInventoryDisplay playerBackpackPanel;

    private void Awake()
    {
        Tools.ISetActive(inventoryPanel.gameObject, false);
        Tools.ISetActive(playerBackpackPanel.gameObject, false);
    }

    private void OnEnable()
    {
        //InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
        PlayerInventoryHolder.OnChestInteraction += DisplayInventory;
    }

    private void OnDisable()
    {
        //InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
        PlayerInventoryHolder.OnChestInteraction -= DisplayInventory;
    }

    private void Update()
    {
        // ����escape�رտ�����
        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            Tools.ISetActive(inventoryPanel.gameObject, false);

        // ����escape�ر���ұ������
        if (playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            Tools.ISetActive(playerBackpackPanel.gameObject, false);
    }

    private void DisplayInventory(InventorySystem invToDisplay, int offest)
    {
        Tools.ISetActive(inventoryPanel.gameObject, true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay, offest);
    }

    private void DisplayPlayerInventory(InventorySystem invToDisplay, int offest)
    {
        Tools.ISetActive(playerBackpackPanel.gameObject, true);
        playerBackpackPanel.RefreshDynamicInventory(invToDisplay, offest);
    }
}
