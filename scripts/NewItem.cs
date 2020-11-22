using System;
using UnityEngine;

public enum ItemType{
    Text,
    Video
}
///<summary>
/// Custom class for the items scattered across the Coventry map
///</summary>
[Serializable]
public class NewItem : MonoBehaviour
{
    public string itemName;

    public ItemType itemType;

    public string fileName;
}
