using static PropertyDrawerUI;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AmountAttribute))]
public class AmountPropertyDrawer : PropertyDrawer
{
    class AmountField : TextField
    {
        public event Action<bool> OnValidityChanged;

        readonly AmountType _amountType;
        readonly SerializedProperty _property;

        public AmountField(AmountType amountType, SerializedProperty property)
        {
            _amountType = amountType;
            _property = property;

            this.RegisterValueChangedCallback(OnValueChanged);
            RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);

            ResetField();
        }

        bool _valid;
        public bool Valid
        {
            get => _valid;
            set
            {
                _valid = value;
                OnValidityChanged?.Invoke(value);
            }
        }

        void OnValueChanged(ChangeEvent<string> evt)
        {
            if (AmountString.TryParse(_amountType, evt.newValue, out var amount))
            {
                Valid = true;
                _property.ulongValue = amount;
                _property.serializedObject.ApplyModifiedProperties();
            }
            else
                Valid = false;
        }

        void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return || evt.character == '\n')
            {
                evt.PreventDefault();
                evt.StopPropagation();
            }
            if (evt.character == '\n' && Valid)
                ResetField();
        }

        void ResetField()
        {
            var amountString = AmountString.Format(_amountType, _property.ulongValue);
            SetValueWithoutNotify(amountString);
        }
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement content;
        try
        {
            ValidatePropertyType(property);

            var amountType = DetermineAmountType(property);
            var amountField = new AmountField(amountType, property);
            var validityIndicator = new ValidityIndicator();
            amountField.OnValidityChanged += valid => validityIndicator.Valid = valid;

            content = new();
            content.AddToClassList("validated-field");

            content.Add(amountField);
            amountField.AddToClassList("validated-field-value");

            content.Add(validityIndicator);
            validityIndicator.AddToClassList("validated-field-validity");
        }
        catch (ArgumentException e)
        {
            content = ErrorMessage(e.Message);
        }
        return WithStyleSheet(LabeledContent(preferredLabel, content));
    }

    void ValidatePropertyType(SerializedProperty property)
    {
        if (property.numericType != SerializedPropertyNumericType.UInt64)
            throw new ArgumentException("Not a ulong property");
    }

    AmountType DetermineAmountType(SerializedProperty property)
    {
        var nullableAmountType = ((AmountAttribute)attribute).AmountType;
        if (nullableAmountType is AmountType amountType)
            return amountType;

        var amountMode = FindAmountMode(property);
        if (amountMode != null)
            return amountMode.AmountType;

        throw new ArgumentException("Could not determine the amount type");
    }

    AmountMode FindAmountMode(SerializedProperty property)
    {
        foreach (var ancestorObject in property.FindAncestorObjects())
        {
            switch (ancestorObject)
            {
                case IAmountModeGet objectConf:
                    return objectConf.AmountMode;
                case Component comp when comp.TryGetComponent<IAmountModeGet>(out var compConf):
                    return compConf.AmountMode;
            }
        }
        return null;
    }
}
