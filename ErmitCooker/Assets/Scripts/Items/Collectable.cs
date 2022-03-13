using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Collectable", order = 1)]
public class Collectable : ScriptableObject
{
    public Sprite drop;
    public int quantity;
}
