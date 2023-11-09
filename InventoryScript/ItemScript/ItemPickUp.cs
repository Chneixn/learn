using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1.0f;
    public InventoryItemDate ItemDate;

    [Tooltip("允许使用碰撞捡取")]
    [SerializeField] private bool allowPickUpByTouch;
    private SphereCollider myCollider;

    [SerializeField] private ItemPickUpSaveData itemSaveData;
    public string id { get; private set; }

    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;

        myCollider = GetComponent<SphereCollider>();
        if (allowPickUpByTouch)
        {
            myCollider.isTrigger = true;
            myCollider.radius = PickUpRadius;
        }
        else myCollider.enabled = false;
        
    }

    private void Start()
    {
        // 初始化物品数据
        itemSaveData = new ItemPickUpSaveData(ItemDate, transform.position, transform.rotation);
        // 在存档数据中写入物品数据
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void LoadGame(SaveDate data)
    {
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        // 在存档数据移除activeItems标记
        if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadGame;
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;
        if (inventory.AddToInventory(ItemDate, 1))
        {
            // 在存档数据标记为collectedItems
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public InventoryItemDate ItemData;
    public Vector3 position;
    public Quaternion rotation;

    public ItemPickUpSaveData(InventoryItemDate _data, Vector3 _position, Quaternion _rotation)
    {
        this.ItemData = _data;
        this.position = _position;
        this.rotation = _rotation;
    }
}
