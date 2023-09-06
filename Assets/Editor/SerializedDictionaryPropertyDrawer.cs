using static PropertyDrawerUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using System;

[CustomPropertyDrawer(typeof(SerializedDictionary<,>))]
public class SerializedDictionaryPropertyDrawer : PropertyDrawer
{
    class KeyList
    {
        readonly List<uint?> _keyHashes = new();
        readonly Dictionary<uint, HashSet<int>> _keyHashIndexes = new();

        int _uninitializedKeyCount;

        public KeyList(int keyCount)
        {
            _uninitializedKeyCount = keyCount;
            for (; keyCount > 0; keyCount--)
                _keyHashes.Add(null);
        }

        public bool Initialized => _uninitializedKeyCount == 0;

        public bool IsInitialized(int index) => _keyHashes[index] != null;

        public void Set(int index, uint hash)
        {
            if (_keyHashes[index] == null)
                _uninitializedKeyCount--;

            if (_keyHashes[index] is uint oldHash)
                RemoveKeyHashIndex(oldHash, index);
            _keyHashes[index] = hash;
            AddKeyHashIndex(hash, index);
        }

        public void Insert(int index)
        {
            _keyHashes.Insert(index, null);
            _uninitializedKeyCount++;
        }

        public void Remove(int index)
        {
            var hash = (uint)_keyHashes[index];
            RemoveKeyHashIndex(hash, index);
            _keyHashes.RemoveAt(index);
        }

        public bool HasDuplicate(int index)
        {
            var hash = (uint)_keyHashes[index];
            return _keyHashIndexes[hash].Count > 1;
        }

        void AddKeyHashIndex(uint hash, int index)
        {
            if (!_keyHashIndexes.TryGetValue(hash, out var indexes))
            {
                indexes = new HashSet<int>();
                _keyHashIndexes[hash] = indexes;
            }
            indexes.Add(index);
        }

        void RemoveKeyHashIndex(uint hash, int index)
        {
            var indexes = _keyHashIndexes[hash];
            indexes.Remove(index);
            if (!indexes.Any())
                _keyHashIndexes.Remove(hash);
        }
    }

    class PairList
    {
        readonly SerializedProperty _pairsProp;
        readonly List<PairElement> _pairElements;
        readonly KeyList _keys;

        public PairList(SerializedProperty pairsProp)
        {
            _pairsProp = pairsProp;
            _pairElements = new();
            _keys = new KeyList(pairsProp.arraySize);

            for (int count = 0; count < pairsProp.arraySize; count++)
                _pairElements.Add(null);
        }

        public PairElement CreatePair() => new();

        public void BindPair(int index, PairElement pairElement)
        {
            _pairElements[index] = pairElement;

            var pair = _pairsProp.GetArrayElementAtIndex(index);
            var isRebinding = _keys.IsInitialized(index);

            pairElement.BindPair(pair, isRebinding, hash => OnKeyChanged(index, hash));
        }

        public void UnbindPair(int index, PairElement pairElement)
        {
            if (index < _pairElements.Count)
                _pairElements[index] = null;

            pairElement.UnbindPair();
        }

        public void AddPair(int index)
        {
            _pairElements.Insert(index, null);
            _keys.Insert(index);
        }

        public void RemovePair(int index)
        {
            _pairElements.RemoveAt(index);
            _keys.Remove(index);

            UpdatePairStyles();
        }

        void OnKeyChanged(int index, uint hash)
        {
            _keys.Set(index, hash);

            UpdatePairStyles();
        }

        void UpdatePairStyles()
        {
            if (!_keys.Initialized)
                return;

            for (var index = 0; index < _pairElements.Count; index++)
                _pairElements[index].StyleAsDuplicate(_keys.HasDuplicate(index));
        }
    }

    class PairElement : BindableElement
    {
        readonly PropertyField _keyField;
        readonly PropertyField _valueField;

        Action<uint> _onKeyChanged;

        public PairElement()
        {
            AddToClassList("pair");

            _keyField = new PropertyField() { label = string.Empty, bindingPath = "Key" };
            _keyField.AddToClassList("pair-element");
            Add(_keyField);

            _valueField = new PropertyField() { label = string.Empty, bindingPath = "Value" };
            _valueField.AddToClassList("pair-element");
            Add(_valueField);
        }

        public void StyleAsDuplicate(bool duplicate)
        {
            if (duplicate)
                this.Q("unity-text-input")?.AddToClassList("invalid-text-input");
            else
                this.Q("unity-text-input")?.RemoveFromClassList("invalid-text-input");
        }

        public void BindPair(SerializedProperty pairProp, bool rebinding, Action<uint> onKeyChanged)
        {
            _onKeyChanged = onKeyChanged;

            if (!rebinding)
                _keyField.RegisterCallback<SerializedPropertyChangeEvent>(OnKeyChanged);

            this.BindProperty(pairProp);

            if (rebinding)
                _keyField.RegisterCallback<SerializedPropertyChangeEvent>(OnKeyChanged);
        }

        public void UnbindPair()
        {
            _onKeyChanged = null;

            _keyField.UnregisterCallback<SerializedPropertyChangeEvent>(OnKeyChanged);

            _keyField.Unbind();
            _valueField.Unbind();
        }

        void OnKeyChanged(SerializedPropertyChangeEvent evt)
        {
            if (evt.target != evt.currentTarget)
                return;

            var keyProp = evt.changedProperty;
            _onKeyChanged(keyProp.contentHash);
        }
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty dictionaryProp)
    {
        var pairsProp = dictionaryProp.FindPropertyRelative("_pairs");

        var pairList = new PairList(pairsProp);

        var listView = new ListView
        {
            headerTitle = preferredLabel,
            showFoldoutHeader = true,
            showBoundCollectionSize = false,
            showAddRemoveFooter = true,
            showBorder = true,
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            makeItem = pairList.CreatePair,
            bindItem = (element, index) => pairList.BindPair(index, (PairElement)element),
            unbindItem = (element, index) => pairList.UnbindPair(index, (PairElement)element),
        };

        listView.itemsAdded += indexes => pairList.AddPair(indexes.First());
        listView.itemsRemoved += indexes => pairList.RemovePair(indexes.First());

        listView.BindProperty(pairsProp);

        return WithStyleSheet(listView);
    }
}
