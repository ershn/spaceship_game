using UnityEngine;

public class ProgressBarGraphics : MonoBehaviour
{
    public Vector2 LocalPosition;

    public ProgressBarController ProgressBarPrefab;

    ProgressBarController _progressBar;

    public void SetProgress(float total)
    {
        if (_progressBar == null)
            _progressBar = CreateBar();

        _progressBar.SetProgress(total);

        if (total >= 1f)
            Destroy(_progressBar.gameObject);
    }

    public void Reset()
    {
        if (_progressBar != null)
            Destroy(_progressBar.gameObject);
    }

    ProgressBarController CreateBar()
    {
        var position = transform.TransformPoint(LocalPosition);
        return Instantiate(ProgressBarPrefab, position, Quaternion.identity, transform);
    }
}
