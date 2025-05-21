using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item")]

public class ItemData : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    public string itemName;
    public Sprite icon;
    [SerializeField] private Enums.ObjectType _type;
    [SerializeField] private Enums.CollectionableType _collectionableType;
    [SerializeField] private int _amount;

    public Enums.ObjectType GetItemType() => _type;
    public Enums.CollectionableType GetCollectionableType() => _collectionableType;
    public int GetAmount() => _amount;

    private void OnValidate()
    {
        #if UNITY_EDITOR
                id = this.name;
                UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
