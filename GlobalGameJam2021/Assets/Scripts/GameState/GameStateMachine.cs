using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameStateMachine : MonoStateMachine {

    public static GameStateMachine Instance { get; private set; }

    [SerializeField] private CanvasGroup gameCanvasGroup;
    [SerializeField] private CanvasGroup gameEndCanvasGroup;
    [Space]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    protected override void Awake() {
        Instance = this;
        base.Awake();

        restartButton.onClick.AddListener(OnRestartButtonClicked);
        menuButton.onClick.AddListener(OnMenuButtonClicked);
    }

    private void OnDestroy() {
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        menuButton.onClick.RemoveListener(OnMenuButtonClicked);
    }

    public void ToggleGameCanvas(bool state, float transitionDuration) {
        ToggleCanvas(gameCanvasGroup, state, transitionDuration);
    }

    public void ToggleGameEndCanvas(bool state, float transitionDuration) {
        ToggleCanvas(gameEndCanvasGroup, state, transitionDuration);
    }

    private void ToggleCanvas(CanvasGroup canvasGroup, bool state, float transitionDuration) {
        canvasGroup.DOFade(state ? 1f : 0f, transitionDuration);
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    private void OnRestartButtonClicked() {
        GameManager.Instance.RestartLevel();
    }

    private void OnMenuButtonClicked() {
        PlayerAbilityManager.Instance.Deinitialize();
        GameManager.Instance.LoadMenu();
    }
}
