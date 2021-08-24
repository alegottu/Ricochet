using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Data", menuName = "Inventory Data", order = 4)]
public class InventoryData : ScriptableObject
{
    [SerializeField] private GameObject[] _itemPrefabs = null;
    public GameObject[] itemPrefabs { get { return _itemPrefabs; } }

    [SerializeField] private int[] _spawnChances = null;
    public int[] spawnChances { get { return _spawnChances; } }
}
