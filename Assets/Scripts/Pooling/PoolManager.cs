using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private Transform parent;

    [System.Serializable]
    public class PooledBlock
    {
        public string name;
        public List<GameObject> objects = new List<GameObject>();
    }

    public List<PooledBlock> pooledBlocks;

    private void Start()
    {
        parent = transform;
    }

    public void PoolItem(GameObject go)
    {
        for (int i = 0; i < pooledBlocks.Count; i++)
        {
            if (pooledBlocks[i].name.Contains(go.name))
            {
                pooledBlocks[i].objects.Add(go);
                return;
            }
        }

        PooledBlock newBlockType = new PooledBlock();
        pooledBlocks.Add(newBlockType);
        newBlockType.name = go.name;
        newBlockType.objects.Add(go);

        go.transform.position = new Vector3(1000, 1000, 1000);
        go.transform.parent = parent;
    }

    public void ReturnPooledItemByName()
    {

    }

    public GameObject HasBlockAvailableForUse(GameObject go)
    {
        for (int i = 0; i < pooledBlocks.Count; i++)
        {
            if (pooledBlocks[i].name.Contains(go.name))
            {
                return pooledBlocks[i].objects[0];
            }
        }
        return null;
    }
	
}
