using System;
using UnityEngine;

public class IsTriggerWithPlayer : MonoBehaviour
{
    public Action TriggerEnterCallback;
    public Action TriggerExitCallback;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            TriggerEnterCallback?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            TriggerExitCallback?.Invoke();
        }
    }
}
