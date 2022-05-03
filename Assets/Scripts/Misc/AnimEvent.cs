using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public UnityEvent[] Events;

    public void TriggerEvent(int eventNum) => Events[eventNum]?.Invoke();
}