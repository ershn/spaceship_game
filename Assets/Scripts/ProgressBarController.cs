using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Image BarMask;

    float _originalBarMaskWidth;

    void Awake()
    {
        _originalBarMaskWidth = BarMask.rectTransform.rect.width;
    }

    void Start()
    {
        SetProgress(0f);
    }

    public void SetProgress(float progress)
    {
        BarMask.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            _originalBarMaskWidth * progress
            );
    }
}