using static PropertyDrawerUI;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AmountAttribute))]
public class AmountPropertyDrawer : PropertyDrawer
{
    class AmountField
    {
        public event Action<bool> OnValidityChanged;

        readonly AmountType _amountType;
        readonly SerializedProperty _property;
        readonly TextField _textField;

        public AmountField(AmountType amountType, SerializedProperty property)
        {
            _amountType = amountType;
            _property = property;

            _textField = new();
            _textField.RegisterValueChangedCallback(OnValueChange);
            _textField.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);

            ResetField();
        }

        public VisualElement VisualElement => _textField;

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

        void OnValueChange(ChangeEvent<string> evt)
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
            _textField.SetValueWithoutNotify(amountString);
        }
    }

    class ValidityIndicator
    {
        readonly Image _image = new();

        public ValidityIndicator()
        {
            Valid = true;
        }

        public VisualElement VisualElement => _image;

        public bool Valid
        {
            set
            {
                if (value)
                {
                    _image.AddToClassList("success-image");
                    _image.RemoveFromClassList("failure-image");
                }
                else
                {
                    _image.RemoveFromClassList("success-image");
                    _image.AddToClassList("failure-image");
                }
            }
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

            content.Add(amountField.VisualElement);
            amountField.VisualElement.AddToClassList("validated-field-value");

            content.Add(validityIndicator.VisualElement);
            validityIndicator.VisualElement.AddToClassList("validated-field-validity");
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
