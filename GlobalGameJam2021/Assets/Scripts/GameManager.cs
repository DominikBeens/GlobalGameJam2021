using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private string menuScene = "Menu";
    [SerializeField] private string gameScene = "Game";

    private List<AsyncOperation> loadOperations = new List<AsyncOperation>();
    private int currentLevel;

    public Camera Camera;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() {
        yield return new WaitForSeconds(0.1f);
        yield return LoadMenuRoutine();
        yield return LoadLevelRoutine(2);
    }

    private IEnumerator LoadMenuRoutine() {
        yield return LoadSceneRoutine(menuScene);
    }

    private IEnumerator LoadLevelRoutine(int level) { //Level needs to be chosen!!!11!
        currentLevel = level;
        yield return LoadSceneRoutine(gameScene);
        Camera = Camera.main;
        BoardManager.Instance.BuildLevel(level);
    }

    private IEnumerator LoadSceneRoutine(string sceneName) {
        loadOperations.Clear();
        loadOperations.Add(SceneManager.LoadSceneAsync(sceneName));
        while (!IsDoneLoading()) {
            yield return null;
        }
    }

    public void RestartLevel() {
        StartCoroutine(LoadLevelRoutine(currentLevel));
    }

    public void LoadMenu() {
        StartCoroutine(LoadMenuRoutine());
    }

    private bool IsDoneLoading() {
        return loadOperations.All(x => x?.isDone ?? true);
    }
}
