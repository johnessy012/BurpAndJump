using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Block : MonoBehaviour {

    private PoolManager _pooler;
    private LevelCreator _levelCreator;
    private BlockManager _blockManager;

    private Rigidbody _rigidBody;

    private List<GameObject> _possibleItemsToCreate = new List<GameObject>();
    private const int maxItems = 3;
    private bool right, up, left, down;

    private GameObject _occupant;
    private bool _secondTimeCall = false;

    private void OnEnable()
    {
        _pooler = FindObjectOfType<PoolManager>();
        _levelCreator = FindObjectOfType<LevelCreator>();
        _blockManager = FindObjectOfType<BlockManager>();
        _levelCreator.NewBlockSetup();
        // This will get called all the time if it is on enable as we pool these items, start is only called once, so this lets us control when we create items
        if (_secondTimeCall)
        {
            if (_occupant == null)
            {
                CreateItem();
            }
        }
    }

    private void Start()
    {
        _secondTimeCall = true;
    }

    private void CreateItem()
    {
        if (!_blockManager.CanCreateItem)
        {
            _blockManager.ItemCreated();
            return;
        }

        for (int i = 0; i < _levelCreator.ObstaclesToCreate.Count; i++)
        {
            // Change difficulty to a static class or variable;
            if (_levelCreator.ObstaclesToCreate[i].ChanceOfSpawning < _levelCreator.DifficultyLevel)
            {
                _possibleItemsToCreate.Add(_levelCreator.ObstaclesToCreate[i].ItemToCreate);
            }
        }

        if (_possibleItemsToCreate.Count <= 0)
            return;

        for (int i = 0; i < _levelCreator.ObstaclesToCreate.Count; i++)
        {
            float chanceOfSpawning = _levelCreator.ObstaclesToCreate[i].ChanceOfSpawning;
            // There is a 50/50 chance of being created 
            float random = Random.Range(0, chanceOfSpawning + 10);
            // need to some how incorporate the difficlulty number in here 
            if (random > chanceOfSpawning)
            {
                GameObject go = Instantiate(_possibleItemsToCreate[Random.Range(0, _possibleItemsToCreate.Count)], transform.position, Quaternion.identity) as GameObject;
                go.transform.eulerAngles = ReturnCorrectRotationOfItem();
                go.transform.parent = transform;
                _blockManager.ItemCreated();
                _occupant = go;
                return;
            }
        }
    }

    private Vector3 ReturnCorrectRotationOfItem()
    { 
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0:
                {
                    return new Vector3(0, 0, 0);
                }
            case 1:
                {
                    return new Vector3(0, 0, 90);
                }
            case 2:
                {
                    return new Vector3(0, 0, 180);
                }
            case 3:
                {
                    return new Vector3(0, 0, 270);
                }
        }
        return Vector3.zero;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<Rigidbody>();
        }
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.mass = 0.5f;
        StartCoroutine(CR_WaitToPool());
    }

    private IEnumerator CR_WaitToPool()
    {
        _levelCreator.PooledBlocks();
        _levelCreator.currentActiveBlocks.Remove(gameObject);
        _levelCreator.currentActiveBlocks.TrimExcess();
        _pooler.PoolItem(gameObject);
        Destroy(_rigidBody);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.000f);
    }
}
