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

        //�󶨵�ǰid
        id = GetComponent<UniqueID>().ID;
        //����OnLoadGame����
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        // ��ʼ����������
        chestSaveData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);
        // �ڴ浵������д����������
        SaveGameManager.data.chestDictionary.Add(id, chestSaveData);
    }

    protected override void LoadInventory(SaveDate data)
    {
        // ���浵�����Ƿ���ڣ�������������
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
