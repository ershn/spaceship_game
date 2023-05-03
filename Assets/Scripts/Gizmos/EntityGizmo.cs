using System.IO;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(IGizmoDef))]
public class EntityGizmo : MonoBehaviour
{
    IGizmoDef _gizmoDef;

    void OnDrawGizmos()
    {
        _gizmoDef ??= GetComponent<IGizmoDef>();

        var asset = _gizmoDef.GizmoAsset;
        if (asset != null && FetchIconFile(asset, out var fileName))
            Gizmos.DrawIcon(transform.position, fileName);
    }

    bool FetchIconFile(Object asset, out string fileName)
    {
        var texture = GetAssetPreview(asset);
        if (texture == null)
        {
            fileName = null;
            return false;
        }
        else
        {
            fileName = FetchIconFile(asset.name, texture);
            return true;
        }
    }

    static Texture2D GetAssetPreview(Object asset)
    {
        var texture = AssetPreview.GetAssetPreview(asset);
        if (texture == null)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.ImportAsset(path);
                texture = AssetPreview.GetAssetPreview(asset);
            }
        }
        return texture;
    }

    Hash128? _cachedIconHash;
    string _cachedIconFileName;

    string FetchIconFile(string name, Texture2D texture)
    {
        if (texture.imageContentsHash == _cachedIconHash)
            return _cachedIconFileName;

        var fileName = CreateIconFile(name, texture);
        _cachedIconHash = texture.imageContentsHash;
        _cachedIconFileName = fileName;
        return fileName;
    }

    static string CreateIconFile(string name, Texture2D texture)
    {
        var gizmoDirPath = $"{Application.dataPath}/Gizmos";
        var fileName = $"{name}.{texture.imageContentsHash}.png";
        var filePath = $"{gizmoDirPath}/{fileName}";
        if (!File.Exists(filePath))
        {
            var existingFilePaths = Directory.GetFiles(gizmoDirPath, $"{name}.*.png");
            foreach (var path in existingFilePaths)
            {
                File.Delete(path);
                File.Delete($"{path}.meta");
            }

            var pngBytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, pngBytes);
        }
        return fileName;
    }
}