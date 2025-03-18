using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item")]

public class ItemData : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    [SerializeField] private int _amount;

    public int GetAmount() => _amount;

    private void OnValidate()
    {
        #if UNITY_EDITOR
                id = this.name;
                UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
