using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(IGizmoDef))]
public class EntityGizmo : MonoBehaviour
{
    IGizmoDef _gizmoDef;
    string _iconFilename;

    void OnDrawGizmos()
    {
        _gizmoDef ??= GetComponent<IGizmoDef>();

        if (CreateIcon(out var filename))
            Gizmos.DrawIcon(transform.position, filename);
    }

    bool CreateIcon(out string filename)
    {
        if (_iconFilename == null)
        {
            if (_gizmoDef.GizmoAsset != null)
                _iconFilename = CreateIcon(_gizmoDef.GizmoAsset);
        }
        else
        {
            if (_gizmoDef.GizmoAsset == null)
                _iconFilename = null;
        }

        filename = _iconFilename;
        return _iconFilename != null;
    }

    static string CreateIcon(Object obj)
    {
        var iconFilename = $"{obj.name}.png";
        var iconPath = $"{Application.dataPath}/Gizmos/{iconFilename}";
        if (!File.Exists(iconPath))
        {
            var texture = AssetPreview.GetAssetPreview(obj);
            var pngBytes = texture.EncodeToPNG();
            File.WriteAllBytes(iconPath, pngBytes);
        }
        return iconFilename;
    }
}