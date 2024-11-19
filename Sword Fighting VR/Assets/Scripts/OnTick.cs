using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTick : MonoBehaviour
{
    public  delegate void OnTicker();
    public event OnTicker onTickEvent;
    public float intervalTime;

    public static OnTick Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(ExecuteTick());
    }
    private IEnumerator ExecuteTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalTime);
            onTickEvent?.Invoke();

        }
        
    }
}
