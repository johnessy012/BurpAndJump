using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour {

    private PoolManager _pooler;

    [SerializeField]
    public float DifficultyLevel;
    [SerializeField]
    private int maxBlocks;
    [SerializeField]
    private int currentBlocks;
    private Transform parent;

    [System.Serializable]
    public class Blocks
    {
        public GameObject Block;
        public float ChanceOfSpawning;
    }

    public Blocks[] blockTypes;

    [SerializeField]
    private List<GameObject> _possibleBlocksToCreate;

    public GameObject nextBlock;


    private void Start()
    {
        parent = GameObject.Find("LevelParent").transform;
        StartCoroutine(CR_CreateBlocks());
    }

    private void ReturnBlocksWithinChanceRange()
    {
        if (_possibleBlocksToCreate.Count > 0)
        {
            _possibleBlocksToCreate.Clear();
        }

        for (int i = 0; i < blockTypes.Length; i++)
        {
            if (blockTypes[i].ChanceOfSpawning < DifficultyLevel)
            {
                _possibleBlocksToCreate.Add(blockTypes[i].Block);
            }
        }
    }

    private GameObject ReturnBlock()
    {
        ReturnBlocksWithinChanceRange();
        return _possibleBlocksToCreate[Random.Range(0, _possibleBlocksToCreate.Count)];
    }

    private void HasBlockAvailable()
    {

    }

    private void CreateBlock()
    {
        nextBlock = ReturnBlock();
        if (_pooler.HasBlockAvailableForUse(nextBlock))
        {
            nextBlock = _pooler.ReturnRandomPooledItem();
        }
        GameObject newBlock = Instantiate(ReturnBlock(), transform.position += new Vector3(0, 0, 1), Quaternion.identity);
        newBlock.transform.SetParent(parent);
    }

    private IEnumerator CR_CreateBlocks()
    {
        while (true)
        {
            if (currentBlocks <= maxBlocks)
            {
                Debug.Log("Create a new bblock : " + currentBlocks + " :: " + maxBlocks);
                // BUG : This is getting called all the time. 
                CreateBlock();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void NewBlockSetup()
    {
        currentBlocks++;
    }

    public void PooledBlocks()
    {
        currentBlocks--;
    }


}
