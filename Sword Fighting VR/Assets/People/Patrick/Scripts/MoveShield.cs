using UnityEngine;
public class MoveShield : MonoBehaviour
{
    [SerializeField] private Transform point;
    
    private void FixedUpdate()
    {
        if (transform.position != point.position)
        {
            transform.position = point.position;
            transform.rotation = point.rotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Weapon"))
        {
            print("it worked");
            StartCoroutine(transform.root.GetComponentInChildren<IKControl>().HandleKnockback());
        }
    }
}
