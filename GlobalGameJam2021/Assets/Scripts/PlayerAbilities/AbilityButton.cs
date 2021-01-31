using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

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

    private Button button;
    private PlayerAbilityManager.AbilityType type;
    private int cooldown;

    private bool isOnCooldown => cooldown > 0;

    public void Initialize(PlayerAbilityManager.AbilityType type, Action onClick = null) {
        this.type = type;

        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick());
        button.onClick.AddListener(OnButtonClicked);
        button.interactable = true;

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
        button.onClick.RemoveAllListeners();
        PlayerAbilityManager.Instance.OnAbilitySelected -= OnAbilitySelectedHandler;
        GameStateMachine.Instance.GetState<RoundPlayState>().OnRoundIncreased -= OnRoundIncreasedHandler;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (isOnCooldown) { return; }
        image.transform.DOKill(true);
        image.transform.DOScale(hoverScaleIn, hoverScaleInDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (isOnCooldown) { return; }
        image.transform.DOKill(true);
        image.transform.DOScale(1f, hoverScaleOutDuration);
    }

    private void OnButtonClicked() {
        if (isOnCooldown) { return; }
        image.transform.DOKill(true);
        image.transform.DOPunchScale(Vector3.one * clickScaleStrength, clickScaleDuration);
    }

    private void OnAbilitySelectedHandler(PlayerAbilityManager.AbilityType type) {
        if (type == PlayerAbilityManager.AbilityType.Flare && this.type == PlayerAbilityManager.AbilityType.Flare) {
            cooldown = BoardManager.Instance.currentLevel.flareCooldown;
            cooldownOverlay.SetActive(true);
            cooldownText.text = $"{cooldown}";
            button.interactable = false;
        }
    }

    private void OnRoundIncreasedHandler() {
        if (isOnCooldown) {
            cooldown--;
            cooldownOverlay.SetActive(isOnCooldown);
            cooldownText.text = $"{cooldown}";
            button.interactable = !isOnCooldown;
        }
    }
}
