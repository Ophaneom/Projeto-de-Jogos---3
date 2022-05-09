using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool occupied;
    public bool areStackable;
    public int quantity;
    public ItemStruct currentItem;

    public void SetInfos(bool _occupied, ItemStruct _currentItem, int _quantity)
    {
        occupied = _occupied;
        currentItem = _currentItem;
        areStackable = _currentItem.stackable;
        quantity = _quantity;
    }

    public void ResetInfos()
    {
        occupied = false;
        currentItem = null;
        areStackable = false;
        quantity = 0;
    }
}
