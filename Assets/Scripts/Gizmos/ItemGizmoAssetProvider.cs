using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ItemDefHolder))]
public class ItemGizmoAssetProvider : MonoBehaviour, IGizmoAssetProvider
{
    public Object GizmoAsset
    {
        get
        {
            var itemDef = GetComponent<ItemDefHolder>().ItemDef;
            if (itemDef == null)
                return null;

            return itemDef.AmountSprites.FirstOrDefault().Sprite;
        }
    }
}
