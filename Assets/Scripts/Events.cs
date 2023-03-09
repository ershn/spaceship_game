using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class IntEvent : UnityEvent<int> {}

[Serializable]
public class FloatEvent : UnityEvent<float> {}

[Serializable]
public class Vector2IntEvent : UnityEvent<Vector2Int> {}

[Serializable]
public class Vector2Event : UnityEvent<Vector2> {}

[Serializable]
public class GameObjectEvent : UnityEvent<GameObject> {}

[Serializable]
public class ItemDefEvent : UnityEvent<ItemDef> {}

[Serializable]
public class TaskEvent : UnityEvent<ITask> {}