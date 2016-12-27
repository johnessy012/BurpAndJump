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

    public Blocks[] blockTypes;

    [SerializeField]
    private List<GameObject> _possibleBlocksToCreate;

    public GameObject nextBlock;

    private void Awake()
    {
        _pooler = FindObjectOfType<PoolManager>();
    }

    private void Start()
    {
        parent = GameObject.Find("LevelParent").transform;
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
            nextBlock = _pooler.chosenBlock;
            nextBlock.transform.position = transform.position += new Vector3(0, 0, 1);
            nextBlock.transform.SetParent(parent);
            CreateLevel = false;
            Debug.LogWarning("We are using a recycled block");
            return;
        }
        Debug.Log("We are creating a new block");
        GameObject newBlock = Instantiate(ReturnBlock(), transform.position += new Vector3(0, 0, 1), Quaternion.identity);
        Debug.LogError("Instantiating.....");
        newBlock.transform.SetParent(parent);
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

    public void NewBlockSetup()
    {
        currentBlocks++;
    }

    public void PooledBlocks()
    {
        currentBlocks--;
    }


}
