using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Interactable
{
    public ItemSlot[] slot = new ItemSlot[2];
    [Header("Item ID")]
    public int itemID;

    //public List<ItemSlot> slots = new List<ItemSlot>();

    public override int Interact()
    {
        return itemID;
    }

   public class ItemSlot
    {
        //public int ID = -1;
        //public ItemSlot item;
        public ItemSlot()
        {
            //ID = -1;
            //item = null;
        }

        public ItemSlot(int _id, ItemSlot _item, int _amount)
        {
            //ID = _id;
            //item = _item;
        }
        public void UpdateSlot(int _id, ItemSlot _item, int _amount)
        {
            //ID = _id;
            //item = _item;
        }
    }
}
