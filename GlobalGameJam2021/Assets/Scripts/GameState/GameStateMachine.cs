using UnityEngine;
using DG.Tweening;

public class GameStateMachine : MonoStateMachine {

    public static GameStateMachine Instance { get; private set; }

    [SerializeField] private CanvasGroup gameCanvasGroup;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;

    protected override void Awake() {
        Instance = this;
        base.Awake();
    }

    public void ToggleGameCanvas(bool state, float transitionDuration) {
        ToggleCanvas(gameCanvasGroup, state, transitionDuration);
    }

    public void ToggleGameOverCanvas(bool state, float transitionDuration) {
        ToggleCanvas(gameOverCanvasGroup, state, transitionDuration);
    }

    private void ToggleCanvas(CanvasGroup canvasGroup, bool state, float transitionDuration) {
        canvasGroup.DOFade(state ? 1f : 0f, transitionDuration);
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }
}
