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

    public void HandleCollisions(ColliderType colliderType, bool hasArmor, float armorDamageReduction, int damage)
    {
        //Taking armor into account however how armor works in the future is subject to change.
        if (hasArmor) damage = Mathf.RoundToInt(damage * armorDamageReduction);
        TakeDamage(damage);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //die
        }
    }
}
