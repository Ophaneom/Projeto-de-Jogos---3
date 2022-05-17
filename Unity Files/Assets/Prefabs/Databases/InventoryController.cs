using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    public int actualSlot;
    public ItemStruct actualItem;
    [SerializeField] private GameObject[] uiSlot;

    private Item itemComponent;
    public bool optionsState;

    private bool areMovingItem;
    private ItemStruct movingItem;
    private GameObject movingFromSlot;
    private int quantityMoving;

    public Color selectedSlot = Color.red;
    public Color deselectedSlot = Color.white;

    private void Awake()
    {
        itemComponent = this.GetComponent<Item>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8)) SelectSlot(7);
        else if (Input.GetKeyDown(KeyCode.Alpha9)) SelectSlot(8);
        else if (Input.GetKeyDown(KeyCode.Alpha0)) SelectSlot(9);

        if (Input.GetKeyDown(KeyCode.F)) AddItemAuto(0, 1);
        if (Input.GetKeyDown(KeyCode.G)) AddItemAuto(1, 1);
        if (Input.GetKeyDown(KeyCode.H)) AddItemAuto(2, 1);
        if (Input.GetKeyDown(KeyCode.J)) AddItemAuto(3, 1);
        if (Input.GetKeyDown(KeyCode.K)) AddItemAuto(4, 1);
        if (Input.GetKeyDown(KeyCode.L)) AddItemAuto(5, 1);
    }

    private void SelectSlot(int _slot)
    {
        actualSlot = _slot;
        for (int i = 0; i < uiSlot.Length; i++)
        {
            if (i != _slot) uiSlot[i].GetComponent<Image>().color = deselectedSlot;
            else uiSlot[i].GetComponent<Image>().color = selectedSlot;
        }
        actualItem = uiSlot[_slot].GetComponent<Slot>().currentItem;
    }

    public ItemStruct FindItemInDatabase(int _id)
    {
        foreach(ItemStruct _item in itemComponent.existentItems)
        {
            if (_item.id == _id) return _item;
        }

        return null;
    }

    public void AddItemAuto(int _id, int _quantity)
    {
        ItemStruct _item = FindItemInDatabase(_id);
        if (_item == null) return;

        foreach(GameObject _slotRef in uiSlot)
        {
            Slot _slot = _slotRef.GetComponent<Slot>();

            if (_slot.currentItem == _item && _slot.areStackable)
            {
                if (_slot.quantity < 99)
                {
                    _slot.quantity += _quantity;

                    TextMeshProUGUI itemQuantity = _slot.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
                    itemQuantity.text = _slot.quantity.ToString();
                    return;
                }
            }
        }

        for (var i = 0; i < uiSlot.Length; i++)
        {
            Slot _slot = uiSlot[i].GetComponent<Slot>();

            if (!_slot.occupied)
            {
                GameObject itemSprite = uiSlot[i].transform.Find("ItemSprite").gameObject;
                itemSprite.SetActive(true);
                itemSprite.GetComponent<Image>().sprite = _item.uiSprite;

                GameObject itemQuantity = uiSlot[i].transform.Find("Quantity").gameObject;
                if (_item.stackable)
                {
                    _slot.quantity = _quantity;
                    itemQuantity.SetActive(true);
                    itemQuantity.GetComponent<TextMeshProUGUI>().text = _slot.quantity.ToString();
                }

                _slot.SetInfos(true, _item, _quantity);
                return;
            }
        }

        return;
    }

    public void AddItemToSlot(int _id, int _quantity, int _wantedSlot, bool forced)
    {
        ItemStruct _item = FindItemInDatabase(_id);
        if (_item == null) return;

        Slot _slot = uiSlot[_wantedSlot].GetComponent<Slot>();

        if (!_slot.occupied || forced)
        {
            _slot.SetInfos(true, _item, _quantity);

            GameObject itemSprite = uiSlot[_wantedSlot].transform.Find("ItemSprite").gameObject;
            itemSprite.SetActive(true);
            itemSprite.GetComponent<Image>().sprite = _item.uiSprite;

            GameObject itemQuantity = uiSlot[_wantedSlot].transform.Find("Quantity").gameObject;
            if (_item.stackable)
            {
                _slot.quantity = _quantity;
                itemQuantity.SetActive(true);
                itemQuantity.GetComponent<TextMeshProUGUI>().text = _slot.quantity.ToString();
            }
        }
    }

    public void RemoveItemFromSlot(int _slotId)
    {
        ResetInventorySlot(uiSlot[_slotId]);
    }

    public void RemoveItemFromSlotUI(GameObject _slotRef)
    {
        ResetInventorySlot(_slotRef);
        ToggleOptions(_slotRef);
    }

    public void ToggleOptions(GameObject _slotRef)
    {
        Slot _slot = _slotRef.GetComponent<Slot>();

        switch (optionsState)
        {
            case false:
                if (_slot.occupied && !areMovingItem)
                {
                    if (_slot.currentItem.usableSettings.isUsable) _slotRef.transform.Find("SlotOptions/Use").gameObject.SetActive(true);
                    if (_slot.currentItem.foodSettings.isEatable) _slotRef.transform.Find("SlotOptions/Eat").gameObject.SetActive(true);
                    if (_slot.currentItem.drinkSettings.isDrinkable) _slotRef.transform.Find("SlotOptions/Drink").gameObject.SetActive(true);
                    if (_slot.currentItem.equipmentSettings.isEquipable) _slotRef.transform.Find("SlotOptions/Equip").gameObject.SetActive(true);
                    _slotRef.transform.Find("SlotOptions").gameObject.SetActive(true);

                    optionsState = true;
                }
                break;

            case true:
                ResetOptions(_slotRef);
                break;
        }
    }

    public void BeginMoveToSlot(GameObject _slotRef)
    {
        if (!areMovingItem)
        {
            Slot _slot = _slotRef.GetComponent<Slot>();

            movingFromSlot = _slotRef;

            movingItem = _slot.currentItem;
            quantityMoving = _slot.quantity;
            areMovingItem = true;

            ResetInventorySlot(_slotRef);
            ResetOptions(_slotRef);
        }
    }

    public void EndMoveToSlot(GameObject _slotRef)
    {
        if (areMovingItem)
        {
            Slot _slot = _slotRef.GetComponent<Slot>();

            if (_slot.occupied)
            {
                Slot _previousSlot = movingFromSlot.GetComponent<Slot>();
                _previousSlot = _slot;
                AddItemToSlot(_slotRef.GetComponent<Slot>().currentItem.id, _slotRef.GetComponent<Slot>().quantity, System.Array.IndexOf(uiSlot, movingFromSlot), true);

                _slot.currentItem = movingItem;
                AddItemToSlot(movingItem.id, quantityMoving, System.Array.IndexOf(uiSlot, _slotRef), true);
                areMovingItem = false;
            }
            else
            {
                _slot.currentItem = movingItem;
                AddItemToSlot(movingItem.id, quantityMoving, System.Array.IndexOf(uiSlot, _slotRef), false);
                areMovingItem = false;
            }

            SelectSlot(actualSlot);
        }
    }

    public void CancelMoveToSlot()
    {
        EndMoveToSlot(movingFromSlot);
    }

    public void ResetInventorySlot(GameObject _slotRef)
    {
        Slot _slot = _slotRef.GetComponent<Slot>();

        GameObject itemSprite = _slotRef.transform.Find("ItemSprite").gameObject;
        itemSprite.SetActive(false);

        GameObject itemQuantity = _slotRef.transform.Find("Quantity").gameObject;
        itemQuantity.SetActive(false);

        _slot.ResetInfos();
    }

    public void ResetOptions(GameObject _slotRef)
    {
        _slotRef.transform.Find("SlotOptions").gameObject.SetActive(false);
        _slotRef.transform.Find("SlotOptions/Use").gameObject.SetActive(false);
        _slotRef.transform.Find("SlotOptions/Eat").gameObject.SetActive(false);
        _slotRef.transform.Find("SlotOptions/Drink").gameObject.SetActive(false);
        _slotRef.transform.Find("SlotOptions/Equip").gameObject.SetActive(false);
        optionsState = false;
    }

    public void Equiptem(GameObject _slotRef)
    {
        RemoveItemFromSlotUI(_slotRef);
    }

    public void EatItem(GameObject _slotRef)
    {
        RemoveItemFromSlotUI(_slotRef);
    }

    public void DrinkItem(GameObject _slotRef)
    {
        RemoveItemFromSlotUI(_slotRef);
    }

    public void UseItem(GameObject _slotRef)
    {
        RemoveItemFromSlotUI(_slotRef);
    }

    public void DropItem(GameObject _slotRef)
    {
        RemoveItemFromSlotUI(_slotRef);
    }
}
