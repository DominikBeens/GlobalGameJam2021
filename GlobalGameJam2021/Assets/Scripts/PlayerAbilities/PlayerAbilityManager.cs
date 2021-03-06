﻿using UnityEngine;
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
        canvasGroup.blocksRaycasts = false;
    }

    private void OnDestroy() {
        Deinitialize();
    }

    private void Update() {
        HandleTileSelection();
    }

    public void Initialize() {
        ToggleCanvas(true, 0.2f);

        //flareButton.gameObject.SetActive(canUseFlare);

        moveButton.Initialize(AbilityType.Move, OnMoveButtonClicked);
        attackButton.Initialize(AbilityType.Attack, OnAttackButtonClicked);
        flareButton.Initialize(AbilityType.Flare, OnFlareButtonClicked);
    }

    public void Deinitialize() {
        ToggleCanvas(false, 0.1f);

        moveButton.Deinitialize();
        attackButton.Deinitialize();
        flareButton.Deinitialize();
    }

    private void ToggleCanvas(bool state, float duration) {
        canvasGroup.DOFade(state ? 1f : 0f, duration);
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public void ToggleButtonInteractability(bool state) {
        buttonCanvasGroup.DOFade(state ? 1f : 0.3f, 0.1f);
        buttonCanvasGroup.interactable = state;
        buttonCanvasGroup.blocksRaycasts = state;
    }

    private void OnMoveButtonClicked() {
        BoardManager.Instance.DeSelectSpots();
        BoardManager.Instance.SelectPlayerMoveSpots();
        tileSelectionMode = AbilityType.Move;
    }

    private void OnAttackButtonClicked() {
        BoardManager.Instance.DeSelectSpots();
        BoardManager.Instance.SelectPlayerAttackSpots();
        tileSelectionMode = AbilityType.Attack;
    }

    private void OnFlareButtonClicked() {
        BoardManager.Instance.DeSelectSpots();
        BoardManager.Instance.SelectAllSpots();
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
                        BoardManager.Instance.PlayerAttack(selectedTile);
                        break;
                    case AbilityType.Flare:
                        BoardManager.Instance.UseFlare(selectedTile);
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
