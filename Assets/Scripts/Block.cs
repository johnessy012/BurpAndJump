using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour {

    private PoolManager _pooler;
    private LevelCreator _levelCreator;

    private Rigidbody _rigidBody;

    private void OnEnable()
    {
        _pooler = FindObjectOfType<PoolManager>();
        _levelCreator = FindObjectOfType<LevelCreator>();
        _levelCreator.NewBlockSetup();
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
        yield return new WaitForSeconds(3);
        Destroy(_rigidBody);
        _pooler.PoolItem(gameObject);
    }
}
