using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class Tile : MonoBehaviour {

    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform rotationTransform;
    [SerializeField] private Transform hoverTransform;
    [SerializeField] private Transform highlightTransform;
    [SerializeField] private Transform randomVisualHolder;
    [SerializeField] private Transform entityHolder;
    [SerializeField] private Transform playerIndicator;
    [Space]
    [SerializeField] private MeshRenderer tileBaseRenderer;
    [Space]
    [SerializeField, ColorUsage(true, true)] private Color defaultEmissionColor;
    [SerializeField, ColorUsage(true, true)] private Color highlightEmissionColor;
    [SerializeField, ColorUsage(true, true)] private Color lethalEmissionColor;

    [Header("Flip")]
    [SerializeField] private float flipDuration = 0.3f;

    [Header("Hover")]
    [SerializeField] private float hoverInDuration = 0.2f;
    [SerializeField] private float hoverOutDuration = 0.1f;

    [Header("Highlight")]
    [SerializeField] private float highlightInDuration = 0.2f;
    [SerializeField] private float highlightOutDuration = 0.1f;

    [Header("Entity Land")]
    [SerializeField] private float landDuration = 0.2f;
    [SerializeField] private float landYStrength = 0.25f;

    private bool isFlipped = false;
    private MaterialPropertyBlock tileBasePropertyBlock;
    private Tween highlightTween;
    private Coroutine flashRoutine;

    public bool IsHighlighted { get; private set; }
    public Entity Entity { get; private set; }

    public Transform EntityHolder => entityHolder;

    private void Awake() {
        foreach (Transform t in randomVisualHolder) {
            t.gameObject.SetActive(false);
        }
        Transform visual = randomVisualHolder.GetChild(Random.Range(0, randomVisualHolder.childCount));
        visual.gameObject.SetActive(true);
        int rotation = Random.Range(0, 4) * 90;
        visual.localEulerAngles = new Vector3(0, rotation, 0);

        tileBasePropertyBlock = new MaterialPropertyBlock();
        tileBaseRenderer.SetPropertyBlock(tileBasePropertyBlock);

        playerIndicator.localScale = Vector3.zero;
    }

    public void AddEntity(Entity entity) {
        Entity = entity;
        if (entity is PlayerEntity) {
            playerIndicator.DOScale(1f, 0.1f);
        }
    }

    public void RemoveEntity() {
        if (Entity && Entity is PlayerEntity) {
            playerIndicator.DOScale(0f, 0.1f);
        }
        Entity = null;
    }

    public void FlipTile(int flipType = 0) {
        if (flipType == 1 && isFlipped == false) {
            return;
        } else if (flipType == 2 && isFlipped == true) {
            return;
        }

        isFlipped = !isFlipped;

        Vector3 rotation = new Vector3(0, 0, gameObject.transform.localEulerAngles.x - 180);
        rotationTransform.DOKill(true);
        rotationTransform.DOLocalRotate(rotation, flipDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack);

        EntityHolder.DOScaleY(isFlipped ? 0f : 1f, flipDuration / 6).SetDelay(isFlipped ? flipDuration / 4 : 0f);
    }

    public void Hover() {
        hoverTransform.DOKill();
        hoverTransform.DOLocalMoveY(0.1f, hoverInDuration);
    }

    public void UnHover() {
        hoverTransform.DOKill();
        hoverTransform.DOLocalMoveY(0f, hoverOutDuration);
    }

    public void Highlight() {
        IsHighlighted = true;
        highlightTransform.DOKill();
        highlightTransform.DOLocalMoveY(0.1f, highlightInDuration);
        TweenTileBaseEmission(highlightEmissionColor, highlightInDuration);
    }

    public void UnHighlight() {
        IsHighlighted = false;
        highlightTransform.DOKill();
        highlightTransform.DOLocalMoveY(0f, highlightOutDuration);
        TweenTileBaseEmission(defaultEmissionColor, highlightOutDuration);
    }

    public void FlashLethal() {
        Flash(lethalEmissionColor, 0.2f, 0.8f);
    }

    private void Flash(Color color, float duration, float pause, float delay = 0f) {
        if (flashRoutine != null) {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine(color, duration, pause, delay));
    }

    private IEnumerator FlashRoutine(Color color, float duration, float pause, float delay) {
        if (delay > 0) {
            yield return new WaitForSeconds(delay);
        }
        TweenTileBaseEmission(color, duration);
        yield return new WaitForSeconds(duration + pause);
        TweenTileBaseEmission(defaultEmissionColor, duration);
        yield return new WaitForSeconds(duration);
        flashRoutine = null;
    }

    public void BounceArea() {
        BounceDown(1f);
        List<Tile> sideTiles = new List<Tile>() {
            BoardManager.Instance.GetTile(Entity.North.position),
            BoardManager.Instance.GetTile(Entity.East.position),
            BoardManager.Instance.GetTile(Entity.South.position),
            BoardManager.Instance.GetTile(Entity.West.position),
        };
        List<Tile> cornerTiles = new List<Tile>() {
            BoardManager.Instance.GetTile(Entity.NorthEast.position),
            BoardManager.Instance.GetTile(Entity.NorthWest.position),
            BoardManager.Instance.GetTile(Entity.SouthEast.position),
            BoardManager.Instance.GetTile(Entity.SouthWest.position),
        };
        foreach (Tile tile in sideTiles) {
            if (!tile) { continue; }
            tile.BounceDown(0.2f, 0.1f);
        }
        foreach (Tile tile in cornerTiles) {
            if (!tile) { continue; }
            tile.BounceDown(-0.05f, 0.05f);
        }
    }

    public void BounceDown(float strength = 1f, float delay = 0f) {
        rootTransform.DOKill(true);
        rootTransform.DOPunchPosition(Vector3.down * (landYStrength * strength), landDuration).SetEase(Ease.OutQuint).SetDelay(delay);
    }

    private void TweenTileBaseEmission(Color color, float duration) {
        tileBaseRenderer.GetPropertyBlock(tileBasePropertyBlock);
        Color c = tileBasePropertyBlock.GetColor("_EmissionColor");
        highlightTween?.Kill();
        highlightTween = DOTween.To(() => c, x => c = x, color, duration).OnUpdate(() => {
            tileBasePropertyBlock.SetColor("_EmissionColor", c);
            tileBaseRenderer.SetPropertyBlock(tileBasePropertyBlock);
        });
    }
}
