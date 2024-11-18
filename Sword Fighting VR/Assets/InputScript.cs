
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    public bool isPressed;
    
    private void Update()
    {
        if (isPressed)
        {
            print("it works");
        }
    }

    private void OnPrinting(InputValue inputValue)
    {      
        
        
            print("YUPPIE YAHOO");
        
           
    }

}
