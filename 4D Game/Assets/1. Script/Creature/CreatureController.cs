using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private CreatureSettingSO creatureSettingSO;

    private CreatureSetting creatureSetting;
    private CreatureMovement creatureMovement;
    private ICreatureControl creatureControl;

    public bool CanMove { get; private set; }

    private void Awake()
    {
        EventCenter.RegisterEvent<OnGameStateChange>(OnGameStateChange);
        EventCenter.RegisterEvent<OnCollision4D>(OnCollision4D);
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnGameStateChange>(OnGameStateChange);
        EventCenter.UnRegisterEvent<OnCollision4D>(OnCollision4D);
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
        creatureSetting = creatureSettingSO.CreatureSetting.Clone();

        switch (creatureSetting.CreatureType)
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

    private void OnCollision4D(OnCollision4D data)
    {

    }
}
