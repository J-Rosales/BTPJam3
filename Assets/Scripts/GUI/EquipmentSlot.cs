using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentSlot : MonoBehaviour
{
    public bool dragLocked;
    [SerializeField] private GameObject padlock;
    [HideInInspector] public UnityEvent onUnlock = new UnityEvent();

    private void Awake()
    {
        onUnlock.AddListener(Unlock);
    }

    public void Unlock()
    {
        dragLocked = false;
        padlock.SetActive(false);
    }
}
