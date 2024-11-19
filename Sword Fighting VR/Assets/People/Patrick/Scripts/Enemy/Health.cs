using UnityEngine;
public enum ColliderType {
    HEAD,
    BODY,
    ARM,
    LEG
}
public class Health : MonoBehaviour
{
    //We don't want scripts to be able to edit the health directly, so we're going to make it private but make sure it's still readable.
    [SerializeField] private int health;
    public int GetHealth() => health;
    
    [SerializeField] private float headDamageModifier, bodyDamageModifier, legDamageModifier, armDamageModifier;

    public void CalculateDamage(ColliderType colType, bool hasArmor, float armorDamageReduction, int baseDamage)
    {
        //When you hit the head we want it to do more damage than if you hit the arms, so we're gonna add a modifier.
        int damage = Mathf.RoundToInt(colType switch
        {
            ColliderType.HEAD => baseDamage * headDamageModifier,
            ColliderType.BODY => baseDamage * bodyDamageModifier,
            ColliderType.LEG => baseDamage * legDamageModifier,
            ColliderType.ARM => baseDamage * armDamageModifier,
            _ => baseDamage
        });
        //Taking armor into account however how armor works in the future is subject to change.
        if (hasArmor) damage = Mathf.RoundToInt(damage * armorDamageReduction);
        TakeDamage(damage);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            print("Killed " + gameObject.name);
            Destroy(gameObject);
            
        }
    }
}
