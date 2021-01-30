using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour {

    [SerializeField] private CanvasGroup buttonCanvasGroup;
    [SerializeField] private CanvasGroup levelsCanvasGroup;
    [Space]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    [Space]
    [SerializeField] private Transform levelSelectionPanel;
    [SerializeField] private Transform levelSelectionButtonHolder;
    [SerializeField] private LevelSelectButton levelSelectButtonPrefab;
    [SerializeField] private float levelButtonsMoveDuration = 1f;

    private Vector3 defaultLevelSelectionButtonsPosition;

    private void Awake() {
        buttonCanvasGroup.alpha = 0f;
        levelsCanvasGroup.alpha = 0f;

        ToggleCanvasGroup(buttonCanvasGroup, true, 0.3f);
        ToggleCanvasGroup(levelsCanvasGroup, false, 0f);

        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);

        foreach (Level level in BoardManager.Instance.Level) {
            LevelSelectButton button = Instantiate(levelSelectButtonPrefab, levelSelectionButtonHolder);
            button.Initialize(level, () => OnLevelSelectionButtonClicked(level));
        }

        defaultLevelSelectionButtonsPosition = levelSelectionButtonHolder.localPosition;
    }

    private void OnDestroy() {
        playButton.onClick.RemoveListener(OnPlayButtonClicked);
        quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        backButton.onClick.RemoveListener(OnBackButtonClicked);
    }

    private void OnPlayButtonClicked() {
        ToggleCanvasGroup(buttonCanvasGroup, false, 0.1f);
        ToggleCanvasGroup(levelsCanvasGroup, true, 0.1f);
        ToggleLevelSelectionButtons(true);
    }

    private void OnQuitButtonClicked() {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying) {
            UnityEditor.EditorApplication.ExitPlaymode();
        }
#elif !UNITY_EDITOR
        Application.Quit();
#endif
    }

    private void OnBackButtonClicked() {
        ToggleCanvasGroup(buttonCanvasGroup, true, 0.1f);
        ToggleCanvasGroup(levelsCanvasGroup, false, 0.1f);
        ToggleLevelSelectionButtons(false);
    }

    private void OnLevelSelectionButtonClicked(Level level) {
        GameManager.Instance.LoadLevel(level);
    }

    private void ToggleCanvasGroup(CanvasGroup canvasGroup, bool state, float duration) {
        canvasGroup.DOKill();
        canvasGroup.DOFade(state ? 1f : 0f, duration);
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    private void ToggleLevelSelectionButtons(bool state) {
        float offset = 200;
        Vector3 targetPosition = defaultLevelSelectionButtonsPosition;
        Vector3 offsetPosition = defaultLevelSelectionButtonsPosition + Vector3.right * offset;

        levelSelectionButtonHolder.DOKill();
        levelSelectionButtonHolder.localPosition = state ? offsetPosition : targetPosition;
        levelSelectionButtonHolder.DOLocalMove(state ? targetPosition : offsetPosition, levelButtonsMoveDuration).SetEase(Ease.OutCubic);
    }
}
