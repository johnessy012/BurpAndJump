using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : MonoBehaviour {

    private GameConditions _gameConditions;

    [SerializeField]
    private Transform _levelHolder;

    [SerializeField]
    private LevelCreator _levelCreator;

    [SerializeField]
    private const float delayIncrement = 0.025f;

    public float Speed;

    private void Start()
    {
        _levelHolder = GameObject.Find("LevelParent").transform;
        _gameConditions = FindObjectOfType<GameConditions>();
    }

    public void RotateLeft()
    {
        float delay = 0;
        foreach (var go in _levelCreator.currentActiveBlocks)
        {
            go.transform.DOComplete();
            go.transform.DOLocalRotate(Vector3.forward * 90, 1, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack).SetDelay(delay).SetAutoKill(false);
            delay += delayIncrement;
        }
    }

    public void RotateRight()
    {
        float delay = 0;
        foreach (var go in _levelCreator.currentActiveBlocks)
        {
            go.transform.DOComplete();
            go.transform.DOLocalRotate(-Vector3.forward * 90, 1, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack).SetDelay(delay).SetAutoKill(false);
            delay += delayIncrement;
        }
    }

    public void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.L))
        {
            RotateLeft();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateRight();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dead")
            _gameConditions.GameOver();
    }

}
