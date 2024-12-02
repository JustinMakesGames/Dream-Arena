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
}
