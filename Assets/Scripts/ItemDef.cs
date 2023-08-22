using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject, ITemplatedPrefab, IWorldLayerGet, IAmountModeGet
{
    public WorldLayer WorldLayer => WorldLayer.Item;

    [SerializeField]
    GameObject _template;
    public GameObject Template => _template;

    [SerializeField]
    GameObject _prefab;
    public GameObject Prefab
    {
        get => _prefab;
        set => _prefab = value;
    }

    [SerializeField, SerializeReference, Polymorphic]
    AmountMode _amountMode;
    public virtual AmountMode AmountMode => _amountMode;

    public AmountSprite[] AmountSprites;
}
