using static PropertyDrawerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(PolymorphicAttribute))]
public class PolymorphicPropertyDrawer : PropertyDrawer
{
    const string NoneString = "None";

    bool AllowNull => ((PolymorphicAttribute)attribute).AllowNull;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root;
        try
        {
            ValidatePropertyType(property);

            var types = DerivedTypes();
            var initialType = InitPropertyType(property, types, AllowNull);
            var content = CreateContent(property);
            var selector = CreateTypeSelector(
                types,
                initialType,
                AllowNull,
                type =>
                {
                    ChangePropertyType(property, type);
                    UpdateContent(content, property);
                }
            );

            root = new VisualElement();
            root.Add(LabeledContent(preferredLabel, selector));
            root.Add(content);
        }
        catch (ArgumentException e)
        {
            root = LabeledContent(preferredLabel, ErrorMessage(e.Message));
        }
        return WithStyleSheet(root);
    }

    void ValidatePropertyType(SerializedProperty property)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
            throw new ArgumentException("Not a SerializeReference property");
    }

    Type BaseType => fieldInfo.FieldType;

    IEnumerable<Type> DerivedTypes()
    {
        var typeCollection = TypeCache.GetTypesDerivedFrom(BaseType);
        if (!typeCollection.Any())
            throw new ArgumentException("No derived type found");

        var types = typeCollection.Where(type => type.FullName != null);
        if (!types.Any())
            throw new ArgumentException("No suitable derived type found");

        return types;
    }

    Type InitPropertyType(SerializedProperty property, IEnumerable<Type> types, bool allowNone)
    {
        if (!allowNone && property.managedReferenceValue == null)
            ChangePropertyType(property, types.First());
        return property.managedReferenceValue?.GetType();
    }

    void ChangePropertyType(SerializedProperty property, Type type)
    {
        if (property.managedReferenceValue?.GetType() == type)
            return;

        var value = type != null ? Activator.CreateInstance(type) : null;
        property.managedReferenceValue = value;
        property.serializedObject.ApplyModifiedProperties();
    }

    VisualElement CreateTypeSelector(
        IEnumerable<Type> types,
        Type selectedType,
        bool allowNone,
        Action<Type> onSelection
    )
    {
        var typeNames = new List<string>();
        if (allowNone)
            typeNames.Add(NoneString);
        typeNames.AddRange(types.Select(t => t.FullName));

        var currentTypeName = selectedType?.FullName ?? NoneString;

        var dropdown = new DropdownField(typeNames, currentTypeName);

        dropdown.RegisterValueChangedCallback(evt =>
        {
            var typeName = evt.newValue;
            Type type = null;
            if (typeName != NoneString)
                type = types.First(type => type.FullName == typeName);
            onSelection(type);
        });

        return dropdown;
    }

    VisualElement CreateContent(SerializedProperty property)
    {
        var content = new VisualElement();
        content.AddToClassList("indented-content");
        AddPropertyFields(content, property);
        return content;
    }

    void UpdateContent(VisualElement content, SerializedProperty property)
    {
        content.Clear();
        AddPropertyFields(content, property);
        content.Bind(property.serializedObject);
    }

    void AddPropertyFields(VisualElement container, SerializedProperty property)
    {
        foreach (SerializedProperty prop in property.Copy())
            container.Add(new PropertyField(prop));
    }
}
