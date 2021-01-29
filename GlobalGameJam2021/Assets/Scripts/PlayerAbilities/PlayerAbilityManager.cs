using UnityEngine;
using DG.Tweening;

public class PlayerAbilityManager : Singleton<PlayerAbilityManager> {

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup buttonCanvasGroup;
    [Space]
    [SerializeField] private AbilityButton moveButton;
    [SerializeField] private AbilityButton attackButton;
    [SerializeField] private AbilityButton flareButton;
    [Space]
    [SerializeField] private Sprite moveSprite;
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite flareSprite;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;

        moveButton.Initialize(moveSprite, OnMoveButtonClicked);
        attackButton.Initialize(attackSprite, OnAttackButtonClicked);
        flareButton.Initialize(flareSprite, OnFlareButtonClicked);
    }

    private void OnDestroy() {
        moveButton.Deinitialize();
        attackButton.Deinitialize();
        flareButton.Deinitialize();
    }

    public void Initialize(bool canUseFlare) {
        canvasGroup.DOFade(1f, 0.2f);
        canvasGroup.interactable = true;
        flareButton.gameObject.SetActive(canUseFlare);
    }

    public void Deinitialize() {
        canvasGroup.interactable = false;
        canvasGroup.DOFade(0f, 0.1f);
    }

    public void ToggleButtonInteractability(bool state) {
        buttonCanvasGroup.DOFade(state ? 1f : 0.8f, 0.1f);
        buttonCanvasGroup.interactable = state;
    }

    private void OnMoveButtonClicked() {

    }

    private void OnAttackButtonClicked() {

    }

    private void OnFlareButtonClicked() {

    }
}
