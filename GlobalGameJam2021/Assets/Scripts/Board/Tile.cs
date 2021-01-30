using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour {

    public Entity myEntity;

    [SerializeField] private Transform rotationTransform;
    [SerializeField] private Transform hoverTransform;
    [SerializeField] private Transform highlightTransform;
    [SerializeField] private Transform randomVisualHolder;
    [SerializeField] private MeshRenderer tileBaseRenderer;

    [Header("Flip")]
    [SerializeField] private float flipDuration = 0.3f;

    [Header("Hover")]
    [SerializeField] private float hoverInDuration = 0.2f;
    [SerializeField] private float hoverOutDuration = 0.1f;

    [Header("Highlight")]
    [SerializeField] private float highlightInDuration = 0.2f;
    [SerializeField] private float highlightOutDuration = 0.1f;

    private bool isFlipped = false;
    private MaterialPropertyBlock tileBasePropertyBlock;
    private Tween highlightTween;

    private void Awake() {
        foreach (Transform t in randomVisualHolder) {
            t.gameObject.SetActive(false);
        }
        randomVisualHolder.GetChild(Random.Range(0, randomVisualHolder.childCount)).gameObject.SetActive(true);

        tileBasePropertyBlock = new MaterialPropertyBlock();
        tileBaseRenderer.SetPropertyBlock(tileBasePropertyBlock);
    }

    public void FlipTile() {
        isFlipped = !isFlipped;

        Vector3 rotation = new Vector3(0, 0, gameObject.transform.localEulerAngles.x - 180);
        rotationTransform.DOKill(true);
        rotationTransform.DOLocalRotate(rotation, flipDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack);
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
        highlightTransform.DOKill();
        highlightTransform.DOLocalMoveY(0.1f, highlightInDuration);
        TweenTileBaseEmission(Color.red, highlightInDuration);
    }

    public void UnHighlight() {
        highlightTransform.DOKill();
        highlightTransform.DOLocalMoveY(0f, highlightOutDuration);
        TweenTileBaseEmission(Color.green, highlightOutDuration);
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
