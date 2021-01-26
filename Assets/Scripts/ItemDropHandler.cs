using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ondrop enabled");
        RectTransform rectTransform = transform as RectTransform;
        Equipment item;
        if(RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
        {
            item = eventData.pointerDrag.GetComponent<Equipment>();
            item.transform.parent.SetSiblingIndex(
                    item.GetComponent<ItemDragHandler>().startSlotSiblingIndex);
            //play sound here;
            Debug.Log("Trashed " + item.displayName);
            item.gameObject.SetActive(false);
        }
    }
}
