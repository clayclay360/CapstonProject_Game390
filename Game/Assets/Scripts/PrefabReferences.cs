using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabReferences : MonoBehaviour
{
    //List from editor of respawnable prefabs
    //ONLY put RESPAWNABLE items in the list please
    [SerializeField] public GameObject[] itemPrefabs; 

    public GameObject FindPrefab(Item item)
    {
        //Check the list of prefabs against the item in the player's hand. If the hand item is on the list,
        //send it back to the trash can script for respawning.
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            if (itemPrefabs[i].name == item.gameObject.name || itemPrefabs[i].name + "(Clone)" == item.gameObject.name)
            {
                return itemPrefabs[i];
            }
        }
        return null; //Return null if item isn't on respawnable list, trashcan spawns nothing.
    }
}
