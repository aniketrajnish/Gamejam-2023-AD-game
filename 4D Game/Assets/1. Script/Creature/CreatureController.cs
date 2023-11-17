using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private CreatureSettingsSO creatureSettingsSO;
    
    private CreatureSettings creatureSettings;
    private CreatureMovement creatureMovement;
    private ICreatureControl creatureControl;

    public bool CanMove { get; private set; }

    private void Start()
    {
        Init();
        CanMove = true;
    }

    private void Update()
    {
       if(CanMove)
        {
            creatureControl.ReadInput();
            creatureMovement.Move();
        }
    }

    private void Init()
    {
        creatureSettings = creatureSettingsSO.CreatureSettings.Clone();

        switch (creatureSettings.CreatureType)
        {
            case CreatureType.Player:
                creatureControl = new PlayerControl();
                break;
            case CreatureType.Enemy:
                creatureControl = new EnemyControl();
                break;
            default:
                break;
        }

        creatureMovement = new CreatureMovement(creatureControl, creatureSettings, transform);
    }
}
