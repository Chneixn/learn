using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem primaryInventorySystem;
    [SerializeField] protected int offest = 10;
    
    public int Offest => offest;

    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested; //Inv System to Display, amount to offest display by

    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory;

        primaryInventorySystem = new InventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveDate saveData);

}

[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem InvSystem;
    public Vector3 Position;
    public Quaternion Rotation;

    public InventorySaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation)
    {
        this.InvSystem = _invSystem;
        this.Position = _position;
        this.Rotation = _rotation;
    }

    public InventorySaveData(InventorySystem _invSystem)
    {
        this.InvSystem = _invSystem;
        this.Position = Vector3.zero;
        this.Rotation = Quaternion.identity;
    }
}
