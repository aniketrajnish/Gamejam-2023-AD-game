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
        EventCenter.RegisterEvent<OnDimensionChanging>(OnDimensionChanging);
        CanMove = false;
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnGameStateChange>(OnGameStateChange);
        EventCenter.UnRegisterEvent<OnDimensionChanging>(OnDimensionChanging);
    }

    private void Update()
    {
        if(CanMove)
        {
            creatureControl.ReadInput();
            creatureMovement.Move();

            if (creatureStat.Health <= 0)
            {
                CanMove = false;
                //Debug.Log("Dead");
            }
        }
    }

    public void Init()
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
                GetComponent<EnemyBehavior>().Init(creatureSetting, creatureStat);  
                break;
            case CreatureType.Elite:
                EnemyControl eliteControl = new EnemyControl(transform);
                eliteControl.Init();
                creatureControl = eliteControl;
                GetComponent<EnemyBehavior>().Init(creatureSetting, creatureStat);
                break;
            case CreatureType.Boss:
                EnemyControl bossControl = new EnemyControl(transform);
                bossControl.Init();
                creatureControl = bossControl;
                GetComponent<EnemyBehavior>().Init(creatureSetting, creatureStat);
                break;
            default:
                break;
        }

        CanMove = true;
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

    private void OnDimensionChanging(OnDimensionChanging data)
    {
        CanMove = !data.isChanging;
    }
}
