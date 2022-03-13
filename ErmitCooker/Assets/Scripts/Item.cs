using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public string itemName;
    public Sprite sprite;
    public GameObject item;
    public int amount = 0;
}
