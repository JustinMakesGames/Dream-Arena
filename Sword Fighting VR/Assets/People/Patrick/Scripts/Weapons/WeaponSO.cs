using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class WeaponSO : ScriptableObject
{
    public int damage;
    public bool ignoresArmor;
    public float timePunishment;
}
    