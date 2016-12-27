using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private Transform parent;
    public GameObject chosenBlock;

    [System.Serializable]
    public class PooledBlock
    {
        public string name;
        public List<GameObject> objects = new List<GameObject>();
    }

    public List<PooledBlock> pooledBlocks;

    private void Start()
    {
        // Set the parent for the pooled block to be this object
        parent = transform;
    }

    public void PoolItem(GameObject go)
    {

        go.transform.position = new Vector3(1000, 1000, 1000);
        go.transform.parent = parent;

        // Go through every block and see if we already have one
        for (int i = 0; i < pooledBlocks.Count; i++)
        {
            // If we do have one then we need to add it to the objects field within the found object. 
            if (pooledBlocks[i].name.Contains(go.name))
            {
                pooledBlocks[i].objects.Add(go);
                return;
            }
        }

        // Else we need to create a new item in the list and populate it. 
        PooledBlock newBlockType = new PooledBlock();
        pooledBlocks.Add(newBlockType);
        newBlockType.name = go.name;
        newBlockType.objects.Add(go);
    }

    public GameObject HasBlockAvailableForUse(GameObject go)
    {
        for (int i = 0; i < pooledBlocks.Count; i++)
        {
            if (pooledBlocks[i].name.Contains(go.name))
            {
                Debug.Log("We have found a block that we can use.");
                if (pooledBlocks[i].objects.Count > 0)
                {
                    Debug.Log("We are going to recycle a block");
                    chosenBlock = pooledBlocks[i].objects[0];
                    pooledBlocks[i].objects.Remove(chosenBlock);
                    return chosenBlock;
                }
            }
        }
        return null;
    }
	
}
