using UnityEngine;
using DG.Tweening;
using System;

public class PlayerAbilityManager : Singleton<PlayerAbilityManager> {

    public event Action<AbilityType> OnAbilitySelected = delegate { };

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
        buttonCanvasGroup.DOFade(state ? 1f : 0.5f, 0.1f);
        buttonCanvasGroup.interactable = state;
        buttonCanvasGroup.blocksRaycasts = state;
    }

    private void OnMoveButtonClicked() {
        BoardManager.Instance.SelectPlayerMoveSpots();
        tileSelectionMode = AbilityType.Move;
    }

    private void OnAttackButtonClicked() {
        BoardManager.Instance.DeSelectMoveSpots();
        tileSelectionMode = AbilityType.Attack;
    }

    private void OnFlareButtonClicked() {
        BoardManager.Instance.DeSelectMoveSpots();
        tileSelectionMode = AbilityType.Flare;
    }

    private void HandleTileSelection() {
        if (tileSelectionMode == AbilityType.Undefined) {
            DeselectTile();
            return;
        }

        if (Physics.Raycast(GameManager.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, tileLayerMask)) {
            Tile tile = hit.transform.GetComponent<Tile>();
            if (tile && tile.IsHighlighted) {
                SelectTile(tile);
            } else {
                DeselectTile();
            }
        } else {
            DeselectTile();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (selectedTile) {
                switch (tileSelectionMode) {
                    case AbilityType.Move:
                        BoardManager.Instance.MovePlayer(selectedTile);
                        break;
                    case AbilityType.Attack:
                        //BoardManager.Instance.Attack(selectedTile);
                        break;
                    case AbilityType.Flare:
                        //BoardManager.Instance.UseFlare(selectedTile);
                        break;
                }
                OnAbilitySelected(tileSelectionMode);
                tileSelectionMode = AbilityType.Undefined;
                DeselectTile();
            }
        }
    }

    private void SelectTile(Tile tile) {
        if (selectedTile == tile) { return; }
        DeselectTile();
        selectedTile = tile;
        selectedTile.Hover();
    }

    private void DeselectTile() {
        if (!selectedTile) { return; }
        selectedTile.UnHover();
        selectedTile = null;
    }
}
