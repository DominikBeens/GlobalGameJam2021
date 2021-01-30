using UnityEngine;
using DG.Tweening;

public class GameStateMachine : MonoStateMachine {

    public static GameStateMachine Instance { get; private set; }

    [SerializeField] private CanvasGroup gameCanvasGroup;
    [SerializeField] private CanvasGroup gameEndCanvasGroup;

    protected override void Awake() {
        Instance = this;
        base.Awake();
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
}
