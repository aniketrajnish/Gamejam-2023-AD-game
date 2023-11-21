using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private CreatureSettingsSO creatureSettingsSO;

    private CreatureSettings creatureSettings;
    private CreatureMovement creatureMovement;
    private ICreatureControl creatureControl;

    public bool CanMove { get; private set; }

    private void Awake()
    {
        EventCenter.RegisterEvent<OnGameStateChange>(OnGameStateChange);
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnGameStateChange>(OnGameStateChange);
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
                EnemyControl enemyControl = new EnemyControl(transform);
                enemyControl.Init();
                creatureControl = enemyControl;
                break;
            default:
                break;
        }

        creatureMovement = new CreatureMovement(creatureControl, creatureSettings, transform);
    }

    private void OnGameStateChange(OnGameStateChange data)
    {
        switch (data.State)
        {
            case GameState.Play:
                Init();
                CanMove = true;
                break;
            default:
                CanMove = false;
                break;
        }
    }
}
