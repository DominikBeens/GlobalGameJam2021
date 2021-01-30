using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class GameOverState : MonoState {

    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private CanvasGroup buttonCanvasGroup;
    [Space]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private Coroutine gameOverRoutine;
    private TextAnimator gameOverTextAnimator;

    public override void Initialize(MonoStateMachine stateMachine) {
        base.Initialize(stateMachine);
        gameOverTextAnimator = gameOverText.GetComponent<TextAnimator>();

        buttonCanvasGroup.alpha = 0f;
        buttonCanvasGroup.interactable = false;
        buttonCanvasGroup.blocksRaycasts = false;

        gameOverText.text = string.Empty;
    }

    public override void Enter(params object[] data) {
        gameOverRoutine = StartCoroutine(GameOverRoutine());
    }

    public override void Exit() {
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        menuButton.onClick.RemoveListener(OnMenuButtonClicked);
        if (gameOverRoutine != null) {
            StopCoroutine(gameOverRoutine);
        }
    }

    public override void Tick() { }

    private IEnumerator GameOverRoutine() {
        GameStateMachine.Instance.ToggleGameCanvas(false, 0.1f);
        yield return new WaitForSeconds(0.5f);
        GameStateMachine.Instance.ToggleGameOverCanvas(true, 1.5f);
        PlayerAbilityManager.Instance.Deinitialize();
        yield return new WaitForSeconds(1f);

        gameOverTextAnimator.ShowText("Game Over", () => {
            buttonCanvasGroup.DOFade(1f, 0.5f);
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
        });

        restartButton.onClick.AddListener(OnRestartButtonClicked);
        menuButton.onClick.AddListener(OnMenuButtonClicked);

        gameOverRoutine = null;
    }

    private void OnRestartButtonClicked() {
        GameManager.Instance.RestartLevel();
    }

    private void OnMenuButtonClicked() {
        GameManager.Instance.LoadMenu();
    }
}
