using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour {

    public bool CreateLevel = true;

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

    [System.Serializable]
    public class Items
    {
        public string ItemName;
        public GameObject ItemToCreate;
        public float ChanceOfSpawning;
    }


    private int currentZ;
    private Vector3 newPos;
    public List<Blocks> blockTypes;


    public List<GameObject> currentActiveBlocks;
    public List<Items> ObstaclesToCreate;

    [SerializeField]
    private List<GameObject> _possibleBlocksToCreate = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _possibleItemsToCreate;

    public GameObject nextBlock;
    public GameObject nextItem;

    private void Awake()
    {
        _pooler = FindObjectOfType<PoolManager>();
    }

    private void Start()
    {
        parent = GameObject.Find("LevelParent").transform;
    }

    // This could be made Generic
    private void ReturnBlocksWithinChanceRange()
    {
        if (_possibleBlocksToCreate.Count > 0)
        {
            _possibleBlocksToCreate.Clear();
        }

        for (int i = 0; i < blockTypes.Count; i++)
        {
            if (blockTypes[i].ChanceOfSpawning <= DifficultyLevel)
            {
                _possibleBlocksToCreate.Add(blockTypes[i].Block);
            }
        }
    }
    // Return a block from our list and add it to our possible blocks to create 
    private GameObject ReturnBlock()
    {
        ReturnBlocksWithinChanceRange();
        if (_possibleBlocksToCreate.Count > 0)
        {
            return _possibleBlocksToCreate[Random.Range(0, _possibleBlocksToCreate.Count)];
        }
        else
            return null;
    }
    // Create or use a block from the pooling system.
    private void CreateBlock()
    {
        // Get a rendom block from our list of blocks
        nextBlock = ReturnBlock();
        // Grab the next pos
        newPos = new Vector3(newPos.x, newPos.y, currentZ);
        currentZ++;
        // If we have a block in our pooler then ue that block instead;
        if (_pooler.HasBlockAvailableForUse(nextBlock))
        {
            nextBlock = _pooler.chosenBlock;
            nextBlock.transform.position = newPos;
            nextBlock.transform.SetParent(parent);
            nextBlock.transform.eulerAngles = Vector3.zero;
            nextBlock.SetActive(true);
            currentActiveBlocks.Add(nextBlock);
            return;
        }
        // Ok we did not have a block to use so we will create one;
        GameObject newBlock = Instantiate(nextBlock, newPos, Quaternion.identity);
        newBlock.transform.eulerAngles = parent.eulerAngles;
        currentActiveBlocks.Add(newBlock);
        newBlock.transform.SetParent(parent);
    }

    public void NewBlockSetup()
    {
        currentBlocks++;
    }

    public void PooledBlocks()
    {
        currentBlocks--;
    }

    public void IncreaseDifficulty(float difficulty)
    {
        DifficultyLevel = difficulty;
    }

    private void Update()
    {
        if (CreateLevel)
        {
            if (currentBlocks < maxBlocks)
            {
                // Debug.Log("We are creating a new block" + currentBlocks);
                //BUG This is not stopping at the max of 25 
                CreateBlock();
            }
            else
            {
                // Debug.Log("We have reached our limit");
            }
        }
    }
}


