using UnityEngine;

public class ItemGraphics : MonoBehaviour, ITemplate<ItemDef>
{
    public void Template(ItemDef itemDef)
    {
        GetComponent<SpriteRenderer>().sprite = itemDef.AmountSprites[0].Sprite;
    }

    SpriteRenderer _spriteRenderer;
    ItemDef _itemDef;

    AmountSprite[] _amountSprites;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _itemDef = GetComponent<ItemDefHolder>().ItemDef;

        _amountSprites = _itemDef.AmountSprites;
    }

    public void ToAmountSprite(ulong amount)
    {
        for (int index = _amountSprites.Length - 1; index >= 0; index--)
        {
            var amountSprite = _amountSprites[index];
            if (amount >= amountSprite.MinAmount)
            {
                _spriteRenderer.sprite = amountSprite.Sprite;
                break;
            }
        }
    }
}
