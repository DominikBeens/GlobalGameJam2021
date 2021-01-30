using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private string menuScene = "Menu";
    [SerializeField] private string gameScene = "Game";

    private List<AsyncOperation> loadOperations = new List<AsyncOperation>();

    public Camera Camera;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() {
        yield return new WaitForSeconds(0.1f);
        yield return LoadScene(menuScene);
        yield return LoadGame();
    }

    private IEnumerator LoadGame() {
        yield return LoadScene(gameScene);
        Camera = Camera.main;
        BoardManager.Instance.BuildLevel(2); //Level needs to be chosen!!!11!
    }

    private IEnumerator LoadScene(string sceneName) {
        loadOperations.Clear();
        loadOperations.Add(SceneManager.LoadSceneAsync(sceneName));
        while (!IsDoneLoading()) {
            yield return null;
        }
    }

    private bool IsDoneLoading() {
        return loadOperations.All(x => x?.isDone ?? true);
    }
}
