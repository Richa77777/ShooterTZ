using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GunsDatabase", menuName = "Game/Guns Database")]
public class GunsDatabase : ScriptableObject
{
    [SerializeField] private List<GunEntry> _guns = new();

    public IReadOnlyList<GunEntry> Guns => _guns;

    private void OnValidate()
    {
        GenerateUniqueIDs();
    }
    
    private void GenerateUniqueIDs()
    {
        HashSet<int> usedIDs = new();
        Dictionary<Gun, int> gunIDMap = new();

        for (int i = 0; i < _guns.Count; i++)
        {
            Gun gunPrefab = _guns[i].GunPrefab;

            if (gunIDMap.ContainsKey(gunPrefab))
            {
                Debug.LogWarning($"Weapon with prefab {gunPrefab.name} already has an ID. Reassigning.");
                _guns[i].SetID(gunIDMap[gunPrefab]);
            }
            else
            {
                int newID = i;
                while (usedIDs.Contains(newID))
                {
                    newID++;
                }

                _guns[i].SetID(newID);
                usedIDs.Add(newID);
                gunIDMap[gunPrefab] = newID;
            }
        }
    }

    public GunEntry GetGunEntryById(int id)
    {
        return _guns.FirstOrDefault(g => g.ID == id);
    }
    
    public Gun GetGunPrefabById(int id)
    {
        return _guns.FirstOrDefault(g => g.ID == id)?.GunPrefab;
    }

    public int GetIdByPrefab(Gun gunPrefab)
    {
        GunEntry gunEntry = _guns.Find(g => g.GunPrefab == gunPrefab);
        return gunEntry?.ID ?? -1;
    }

    public bool EqualsGun(Gun gun1, Gun gun2)
    {
        return gun1 != null && gun2 != null && gun1.ID == gun2.ID;
    }
}

[System.Serializable]
public class GunEntry
{
    [SerializeField] private int _id;
    [SerializeField] private Gun _gunPrefab;
    [SerializeField] private Sprite _icon;

    public int ID => _id;
    public Gun GunPrefab => _gunPrefab;
    public Sprite Icon => _icon;

    public void SetID(int newID)
    {
        _id = newID;
    }
}