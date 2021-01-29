using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private string sceneToLoad = "Menu";

    protected override void Awake() {
        base.Awake();
        Initialize();
    }

    private void Initialize() {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(sceneToLoad);
    }
}
