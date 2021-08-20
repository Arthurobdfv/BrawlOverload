using BrawlServer.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static Dictionary<int, GameObject> Entities = new Dictionary<int, GameObject>();

    public static Dictionary<int, GameObject> Prefabs = new Dictionary<int, GameObject>();

    public List<InteractibleGameObject> LocalPrefabs = new List<InteractibleGameObject>();

    private void Awake()
    {
        RegisterPrefabs();
    }

    public static void SpawnObject(BrawlEntityType type, Vector3 pos, Quaternion rot,int objId)
    {
        GameObject objectToSpawn;
        if (Prefabs.TryGetValue((int)type, out objectToSpawn))
        {
            Entities.Add(objId, Instantiate(objectToSpawn, pos, rot));
        }
        else Debug.Log("Object not found on prefabs list");
    }

    public static void DestroyObject(int objId)
    {
        GameObject objToDestroy;
        if (Entities.TryGetValue(objId, out objToDestroy))
        {
            Entities.Remove(objId);
            Destroy(objToDestroy);
        }
        else Debug.Log("Object instance not found on entity list");
    }

    void RegisterPrefabs()
    {
        foreach (var local in LocalPrefabs)
        {
            Prefabs.Add((int)local.EntityType, local.gameObject);
        }
    }
}
