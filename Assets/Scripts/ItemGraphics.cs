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

    public void ToLowMassSprite() => _spriteRenderer.sprite = _itemDef.LowMassSprite;
    public void ToNormalMassSprite() => _spriteRenderer.sprite = _itemDef.NormalMassSprite;
    public void ToHighMassSprite() => _spriteRenderer.sprite = _itemDef.HighMassSprite;
}
