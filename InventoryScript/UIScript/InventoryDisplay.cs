using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemDate mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary; //���������UI�󶨣�key��UI�ű���value�ǲ�۽ű�

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem invToDisplay, int offest); //Ԥ����Ҫʵ�ֵĺ���

    /// <summary>
    /// ���¶Բ�۶�Ӧ��UI
    /// </summary>
    /// <param name="updateSlot"></param>
    protected virtual void UpdateSlot(InventorySlot updateSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            //key��UI�ű���value�ǲ�۽ű�
            if (slot.Value == updateSlot)
            {
                slot.Key.UpdateUISlot(updateSlot);
            }
        }
    }

    /// <summary>
    /// ʵ�ֲ�۽�������
    /// </summary>
    /// <param name="clickedUISlot"></param>
    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;
        //�������Ʒ�Ĳ��ʱ + �����û����Ʒ -> ������Ʒ
        if (clickedUISlot.AssignedInventorySlot.ItemDate != null && mouseInventoryItem.AssignedInventorySlot.ItemDate == null)
        {
            //��סshiftʱ������һ����Ʒ
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }

        //����ղ��ʱ + ���������Ʒ -> ����Ʒ������
        if (clickedUISlot.AssignedInventorySlot.ItemDate == null && mouseInventoryItem.AssignedInventorySlot.ItemDate != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        //����ۺ͵����۶�����Ʒʱ
        if (clickedUISlot.AssignedInventorySlot.ItemDate != null && mouseInventoryItem.AssignedInventorySlot.ItemDate != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemDate == mouseInventoryItem.AssignedInventorySlot.ItemDate;

            //��Ʒ��ͬʱ
            //�����Ʒ�� + �����Ʒ�� <= ��Ʒ�ѵ��� -> ���Ӳ����Ʒ������������
            if (isSameItem &&
                clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
                return;
            }
            //�����Ʒ�� + �����Ʒ�� < ��Ʒ�ѵ���
            else if (isSameItem &&
                !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                //�����Ʒ�� = MaxStackSize -> �л���Ʒ
                if (leftInStack < 1) { SwapSlots(clickedUISlot); return; }
                //�����Ʒ�� < MaxStackSize -> ����������Ʒ
                else
                {
                    int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    var tempItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemDate, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(tempItem);
                    return;
                }
            }
            //��Ʒ��ͬʱ -> �л���Ʒ
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    /// <summary>
    /// �л�UI��ۺ������
    /// </summary>
    /// <param name="clickedUISlot"></param>
    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        //���������
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemDate, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();

        //������ۻ�Ϊ������Ĳ��
        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        //���������Ĳ��
        clickedUISlot.ClearSlot();
        //��������Ĳ�ۻ�Ϊ����������
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
