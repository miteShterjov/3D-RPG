using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectPrefab;
        static bool hasSpawned = false;
    
        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObjects();
            hasSpawned = true;
        }

        // Spawns persistent objects that should not be destroyed on scene load
        // This method is called once at the start of the game
        private void SpawnPersistentObjects()
        {
            if (persistentObjectPrefab == null)
            {
                Debug.LogError("Persistent object prefab is not assigned.");
                return;
            }

            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
            Debug.Log("Persistent object spawned: " + persistentObject.name);
        }
    }
}
