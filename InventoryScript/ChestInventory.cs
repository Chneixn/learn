using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public static UnityAction<InventorySystem, int> OnChestInteraction;

    [SerializeField] private InventorySaveData chestSaveData;
    private string id;

    protected override void Awake()
    {
        base.Awake();

        //绑定当前id
        id = GetComponent<UniqueID>().ID;
        //订阅OnLoadGame动作
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        // 初始化箱子数据
        chestSaveData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);
        // 在存档数据中写入箱子数据
        SaveGameManager.data.chestDictionary.Add(id, chestSaveData);
    }

    protected override void LoadInventory(SaveDate data)
    {
        // 检查存档数据是否存在，存在则导入数据
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.primaryInventorySystem = chestData.InvSystem;
            this.transform.position = chestData.Position;
            this.transform.rotation = chestData.Rotation;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        //OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem, 0);
        OnChestInteraction?.Invoke(primaryInventorySystem, 0);
        interactSuccessful = true;
    }

    public void EndInteraction()
    {
        
    }
}
