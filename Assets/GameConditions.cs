using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConditions : MonoBehaviour {

    public Player _player;
    public CameraMovement _camera;
    private LevelCreator _levelCreator;

    private List<Block> _blocksInScene = new List<Block>();

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _camera = FindObjectOfType<CameraMovement>();
        _levelCreator = FindObjectOfType<LevelCreator>();
    }

    public void GameOver()
    {
        foreach (var go in FindObjectsOfType<Block>())
        {
            _blocksInScene.Add(go);
            if (!go.gameObject.GetComponent<Rigidbody>())
                go.gameObject.AddComponent<Rigidbody>();

        }

        _camera.Speed = 0;
        _player.Speed = 0;

        float lastKnownDifficulty = PlayerPrefs.GetFloat("BestScore");
        Debug.Log("Last Score is " + lastKnownDifficulty);
        if (lastKnownDifficulty < _levelCreator.DifficultyLevel)
        {
            PlayerPrefs.SetFloat("BestScore", _levelCreator.DifficultyLevel);
            Debug.Log("Saved the score");
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void HalfSpeedPowerUp()
    {
        float origSpeed = _camera.Speed;
        _camera.Speed = _camera.Speed / 2;

        float origPlayerspeed = _player.Speed;
        _player.Speed = _player.Speed / 2;

        StartCoroutine(CR_WaitToDefault(origSpeed, origPlayerspeed));
    }

    public IEnumerator CR_WaitToDefault(float origCamSpeed, float origPlayerSpeed)
    {
        yield return new WaitForSeconds(10);
        _player.Speed = origPlayerSpeed;
        _camera.Speed = origCamSpeed;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            HalfSpeedPowerUp();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RestartGame();
        }
    }
}
