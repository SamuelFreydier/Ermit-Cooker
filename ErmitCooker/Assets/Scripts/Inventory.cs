using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory;

    public void AddItem(Item itemToAdd) {

        Item result = inventory.Find(item => item.name == itemToAdd.name);

        if(result != null) {
            itemToAdd.amount = 1;
            inventory.Add(itemToAdd);
        }
        else {
            result.amount++;
        }
        
    }

    public void RemoveItem(Item itemToRemove) {

        Item result = inventory.Find(item => item.name == itemToRemove.name);

        if(result.amount == 1) {
            inventory.Remove(result);
        }
        else {
            result.amount--;
        }
        
    }
}
