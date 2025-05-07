using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    private PlayerStat stat;
    private PlayerController controller;
    [SerializeField]private PlayerVisual visual;

    void Start()
    {
        stat = new PlayerStat(new Dictionary<StatType,Stat>() {
            {StatType.Speed, new Stat(StatType.Speed,5) }
        });
        controller = GetComponent<PlayerController>();
        controller.Init(stat,visual);
        
    }

}
