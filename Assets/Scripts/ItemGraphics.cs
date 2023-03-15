using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ItemDefHolder))]
public class ItemGraphics : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    ItemDef _itemDef;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _itemDef = GetComponent<ItemDefHolder>().ItemDef;
    }

    public void ToLowAmountSprite() => _spriteRenderer.sprite = _itemDef.LowAmountSprite;
    public void ToNormalAmountSprite() => _spriteRenderer.sprite = _itemDef.NormalAmountSprite;
    public void ToHighAmountSprite() => _spriteRenderer.sprite = _itemDef.HighAmountSprite;
}
