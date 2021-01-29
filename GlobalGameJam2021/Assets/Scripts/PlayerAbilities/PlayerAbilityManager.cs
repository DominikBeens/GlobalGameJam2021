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
    [SerializeField] private LayerMask tileLayerMask;

    private AbilityType tileSelectionMode;
    private Tile selectedTile;

    public enum AbilityType { Undefined, Move, Attack, Flare }

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;

        moveButton.Initialize(AbilityType.Move, OnMoveButtonClicked);
        attackButton.Initialize(AbilityType.Attack, OnAttackButtonClicked);
        flareButton.Initialize(AbilityType.Flare, OnFlareButtonClicked);

        Initialize(true);
    }

    private void OnDestroy() {
        moveButton.Deinitialize();
        attackButton.Deinitialize();
        flareButton.Deinitialize();
    }

    private void Update() {
        HandleTileSelection();
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
        tileSelectionMode = AbilityType.Move;
    }

    private void OnAttackButtonClicked() {
        tileSelectionMode = AbilityType.Attack;
    }

    private void OnFlareButtonClicked() {
        tileSelectionMode = AbilityType.Flare;
    }

    private void HandleTileSelection() {
        if (tileSelectionMode == AbilityType.Undefined) {
            if (selectedTile) {
                
                selectedTile = null;
            }
        }
    }
}
