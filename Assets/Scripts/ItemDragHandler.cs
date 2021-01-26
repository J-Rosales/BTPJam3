using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    EquipmentSlot slot;
    Vector3 startLocalPosition;
    Image itemIcon;
    [HideInInspector] public int startSlotSiblingIndex;
    void Start()
    {
        slot = transform.parent.GetComponent<EquipmentSlot>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(slot.dragLocked)
            return;

        Debug.Log("OnDrag: " + eventData.pointerDrag.name);
        itemIcon = eventData.pointerDrag.GetComponent<Image>();
        itemIcon.raycastTarget = false;
        startSlotSiblingIndex = transform.parent.GetSiblingIndex();
        transform.parent.SetAsLastSibling();

        startLocalPosition = transform.localPosition;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(slot.dragLocked)
            return;
        
        transform.parent.SetSiblingIndex(startSlotSiblingIndex);
        transform.localPosition = startLocalPosition;
    }
}
