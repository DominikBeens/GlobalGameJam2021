using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class GameEndState : MonoState {

    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private CanvasGroup buttonCanvasGroup;
    [Space]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [Space]
    [SerializeField] private Material wonResultTextMaterial;
    [SerializeField] private Material lostResultTextMaterial;

    private Coroutine endRoutine;
    private TextAnimator resultTextAnimator;
    private bool won;

    public override void Initialize(MonoStateMachine stateMachine) {
        base.Initialize(stateMachine);
        resultTextAnimator = resultText.GetComponent<TextAnimator>();

        buttonCanvasGroup.alpha = 0f;
        buttonCanvasGroup.interactable = false;
        buttonCanvasGroup.blocksRaycasts = false;

        resultText.text = string.Empty;
    }

    public override void Enter(params object[] data) {
        won = (bool)data[0];
        endRoutine = StartCoroutine(EndRoutine());
    }

    public override void Exit() {
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        menuButton.onClick.RemoveListener(OnMenuButtonClicked);
        if (endRoutine != null) {
            StopCoroutine(endRoutine);
        }
    }

    public override void Tick() { }

    private IEnumerator EndRoutine() {
        GameStateMachine.Instance.ToggleGameCanvas(false, 0.1f);
        yield return new WaitForSeconds(0.5f);
        GameStateMachine.Instance.ToggleGameEndCanvas(true, 1.5f);
        PlayerAbilityManager.Instance.Deinitialize();
        yield return new WaitForSeconds(1f);

        string text = won ? "Victory" : "Game Over";
        resultText.fontSharedMaterial = won ? wonResultTextMaterial : lostResultTextMaterial;
        resultTextAnimator.ShowText(text, () => {
            buttonCanvasGroup.DOFade(1f, 0.5f);
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
        });

        restartButton.onClick.AddListener(OnRestartButtonClicked);
        menuButton.onClick.AddListener(OnMenuButtonClicked);

        restartButton.gameObject.SetActive(!won);

        endRoutine = null;
    }

    private void OnRestartButtonClicked() {
        GameManager.Instance.RestartLevel();
    }

    private void OnMenuButtonClicked() {
        GameManager.Instance.LoadMenu();
    }
}
