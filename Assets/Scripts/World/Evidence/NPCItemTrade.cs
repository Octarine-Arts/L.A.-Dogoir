using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCItemTrade : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private UnityEvent TradeEvents;

    private void Awake() => EventManager.I.OnTradeItem += OnTradeItem;

    private void OnTradeItem(string itemName) 
    {
        if (this.itemName == itemName) TradeEvents?.Invoke(); 
    }
}
