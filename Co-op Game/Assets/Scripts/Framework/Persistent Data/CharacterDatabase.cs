using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;

    public GameObject GetByIndex(int index)
    {
        if (index < 0 || index >= characters.Count)
        {
            Debug.LogError("Index out of range");
            return null;
        }

        return characters[index];
    }
}
