using UnityEditor;
using UnityEngine.UIElements;

public static class PropertyDrawerUI
{
    public static VisualElement LabeledContent(string label, VisualElement content)
    {
        var root = new VisualElement();
        root.AddToClassList("labeled-property");

        var labelElement = new Label(label);
        labelElement.AddToClassList("labeled-property-label");
        root.Add(labelElement);

        content.AddToClassList("labeled-property-content");
        root.Add(content);

        return root;
    }

    public static VisualElement ErrorMessage(string message)
    {
        var label = new Label(message);
        label.AddToClassList("error-message");
        return label;
    }

    const string StyleSheetPath = "Assets/Editor/PropertyDrawers.uss";

    public static VisualElement WithStyleSheet(VisualElement element)
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StyleSheetPath);
        element.styleSheets.Add(styleSheet);
        return element;
    }
}
