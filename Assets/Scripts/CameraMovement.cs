using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float Speed;

    private Vector3 _startingPosition;
    private LevelCreator _levelCreator;
    private ScoreSystem _scoreSystem;

    private void Start()
    {
        _startingPosition = transform.position;
        _levelCreator = FindObjectOfType<LevelCreator>();
        _scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    private void Update()
    {
        transform.position += Vector3.forward * Speed * Time.deltaTime;
        _levelCreator.IncreaseDifficulty(Vector3.Distance(transform.position, _startingPosition));
        _scoreSystem.UpdateCurrentScore(_levelCreator.DifficultyLevel);
    }
}
