using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private CreatureSettingSO creatureSettingSO;

    private CreatureSetting creatureSetting;
    private CreatureStat creatureStat;
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
        if(creatureStat.Health <= 0)
        {
            CanMove = false;
            Debug.Log("Dead");
        }

        if(CanMove)
        {
            creatureControl.ReadInput();
            creatureMovement.Move();
        }
    }

    private void Init()
    {
        creatureSetting = creatureSettingSO.CreatureSetting.Clone();
        creatureStat = new CreatureStat(creatureSetting.DefaultHealth, creatureSetting.Speed);

        switch (creatureSetting.CreatureType)
        {
            case CreatureType.Player:
                creatureControl = new PlayerControl();
                GetComponent<PlayerBehavior>().Init(creatureSetting, creatureStat);
                break;
            case CreatureType.Enemy:
                EnemyControl enemyControl = new EnemyControl(transform);
                enemyControl.Init();
                creatureControl = enemyControl;
                break;
            default:
                break;
        }

        creatureMovement = new CreatureMovement(creatureControl, creatureSetting, transform);
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
