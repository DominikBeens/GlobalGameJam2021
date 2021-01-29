using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private string sceneToLoad = "Menu";

    [SerializeField] private string gameScene = "Game";

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneToLoad);
        yield return new WaitForSeconds(0.1f);
        yield return LoadGame();
    }

    private IEnumerator LoadGame() {
        yield return null;
        SceneManager.LoadScene(gameScene);
        yield return new WaitForSeconds(0.1f);
        BoardManager.Instance.BuildLevel(10, 10, new List<EntityPlacement>());
    }
}
