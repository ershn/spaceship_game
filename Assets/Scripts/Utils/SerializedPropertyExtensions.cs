using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

public static class SerializedPropertyExtensions
{
    public static IEnumerable<object> FindAncestorObjects(this SerializedProperty property)
    {
        var ancestors = new Stack<object>();

        var ancestor = (object)property.serializedObject.targetObject;
        ancestors.Push(ancestor);

        var subPaths = ParsePropertyPath(property.propertyPath).SkipLast(1);
        foreach (var subPath in subPaths)
        {
            if (subPath is string fieldName)
                ancestor = ancestor.GetType().GetField(fieldName).GetValue(ancestor);
            else
                ancestor = ((Array)ancestor).GetValue((int)subPath);

            ancestors.Push(ancestor);
        }

        return ancestors;
    }

    static IEnumerable<object> ParsePropertyPath(string path)
    {
        var subPaths = Regex.Replace(path, @"Array\.data\[(\d+)\]", "$1").Split(".");
        return subPaths.Select<string, object>(
            subPath => int.TryParse(subPath, out var index) ? index : subPath
        );
    }
}
