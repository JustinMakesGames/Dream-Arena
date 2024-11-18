using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool ignoresArmor;
    [SerializeField] private int damage;
    public int GetDamage() => damage;


}
