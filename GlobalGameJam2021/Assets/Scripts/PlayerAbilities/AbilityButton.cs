﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using DB.SimpleFramework.SimpleAudioManager;
using Random = UnityEngine.Random;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private const float LOCKED_DURATION = 2.5f;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [Space]
    [SerializeField] private Sprite moveSprite;
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite flareSprite;
    [Space]
    [SerializeField] private float hoverScaleIn = 1.2f;
    [SerializeField] private float hoverScaleInDuration = 0.2f;
    [SerializeField] private float hoverScaleOutDuration = 0.2f;
    [Space]
    [SerializeField] private float clickScaleStrength = -0.1f;
    [SerializeField] private float clickScaleDuration = 0.2f;
    [Space]
    [SerializeField] private GameObject cooldownOverlay;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private AudioClip clickAudio;

    private Button button;
    private PlayerAbilityManager.AbilityType type;
    private int cooldown;
    private float lockedTimer;

    private bool isOnCooldown => cooldown > 0;
    private bool isLocked => lockedTimer > 0;

    public void Initialize(PlayerAbilityManager.AbilityType type, Action onClick = null) {
        this.type = type;

        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick());
        button.onClick.AddListener(OnButtonClicked);

        cooldownOverlay.SetActive(false);
        cooldown = 0;

        switch (type) {
            case PlayerAbilityManager.AbilityType.Move: image.sprite = moveSprite; text.text = "Move"; break;
            case PlayerAbilityManager.AbilityType.Attack: image.sprite = attackSprite; text.text = "Attack"; break;
            case PlayerAbilityManager.AbilityType.Flare: image.sprite = flareSprite; text.text = "Flare"; break;
        }

        PlayerAbilityManager.Instance.OnAbilitySelected += OnAbilitySelectedHandler;
        GameStateMachine.Instance.GetState<RoundPlayState>().OnRoundIncreased += OnRoundIncreasedHandler;
    }

    public void Deinitialize() {
        if (button) {
            button.onClick.RemoveAllListeners();
        }
        PlayerAbilityManager.Instance.OnAbilitySelected -= OnAbilitySelectedHandler;
        GameStateMachine.Instance.GetState<RoundPlayState>().OnRoundIncreased -= OnRoundIncreasedHandler;
    }

    private void Update() {
        HandleLockedTimer();
        HandleButtonInteractability();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (isOnCooldown || isLocked) { return; }
        image.transform.DOKill(true);
        image.transform.DOScale(hoverScaleIn, hoverScaleInDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (isOnCooldown || isLocked) { return; }
        image.transform.DOKill(true);
        image.transform.DOScale(1f, hoverScaleOutDuration);
    }

    private void OnButtonClicked() {
        if (isOnCooldown || isLocked) { return; }
        image.transform.DOKill(true);
        image.transform.DOPunchScale(Vector3.one * clickScaleStrength, clickScaleDuration);
        SimpleAudioManager.Play2D(clickAudio, 0.5f, Random.Range(0.95f, 1.05f));
    }

    private void OnAbilitySelectedHandler(PlayerAbilityManager.AbilityType type) {
        if (type == PlayerAbilityManager.AbilityType.Flare && this.type == PlayerAbilityManager.AbilityType.Flare) {
            cooldown = BoardManager.Instance.currentLevel.flareCooldown;
            cooldownOverlay.SetActive(true);
            cooldownText.text = $"{cooldown}";
        }

        lockedTimer = LOCKED_DURATION;
    }

    private void OnRoundIncreasedHandler(int round) {
        if (isOnCooldown) {
            cooldown--;
            cooldownOverlay.SetActive(isOnCooldown);
            cooldownText.text = $"{cooldown}";
        }
    }

    private void HandleLockedTimer() {
        if (isLocked) {
            lockedTimer -= Time.deltaTime;
        }
    }

    private void HandleButtonInteractability() {
        if (!button) { return; }
        if (isOnCooldown || isLocked) {
            if (button.interactable) {
                button.interactable = false;
            }
        } else {
            if (!button.interactable) {
                button.interactable = true;
            }
        }
    }
}
