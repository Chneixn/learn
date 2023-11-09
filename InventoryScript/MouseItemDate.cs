using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class MouseItemDate : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;

    [SerializeField] private Transform _dropTranfrom;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;
        ItemCount.text = string.Empty;

        if (_dropTranfrom == null) Debug.LogWarning("ObjectGrabPos not found!");
    }

    /// <summary>
    /// ����������е�����
    /// </summary>
    /// <param name="invSlot"></param>
    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }

    /// <summary>
    /// ˢ��������е�����
    /// </summary>
    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemDate.Icon;
        if (AssignedInventorySlot.StackSize > 1) ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    private void Update()
    {
        // TODO: ֧�ֿ���������

        if (AssignedInventorySlot.ItemDate != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                // ������Ʒ����
                if (AssignedInventorySlot.ItemDate.ItemPrefab != null)
                    ObjectPoolManager.SpawnObject(AssignedInventorySlot.ItemDate.ItemPrefab, _dropTranfrom.position, Quaternion.identity);

                if (AssignedInventorySlot.StackSize > 1)
                {
                    AssignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }

                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = string.Empty;
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    /// <summary>
    /// �ж�����Ƿ���UI����
    /// </summary>
    /// <returns></returns>
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDateCurrentPosition = new PointerEventData(EventSystem.current);
        eventDateCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDateCurrentPosition, results);
        return results.Count > 0;
    }
}
