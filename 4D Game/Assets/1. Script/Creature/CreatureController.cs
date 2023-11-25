using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private CreatureSettingSO creatureSettingSO;

    private CreatureSetting creatureSetting;
    private CreatureStat creatureStat;
    private CreatureMovement creatureMovement;
    private ICreatureControl creatureControl;
    private Timer invincibleTimer;

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
                creatureStat.OnHealthChanged += healthBar.OnHealthChanged;
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
        invincibleTimer = TimerManager.Instance.GetTimer();
        invincibleTimer.gameObject.SetActive(true);
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
        if (creatureSetting.CreatureType == CreatureType.Player)
        {
            if (data.collidedObject.tag == "Enemy" && invincibleTimer.IsFinished())
            {
                Hurt();
            }
        }
    }

    private void Hurt()
    {
        creatureStat.ModifyHealth(-1);
        invincibleTimer.StartTimer(1.5f);
        Debug.Log("Ouch: " + creatureStat.Health);
    }
}
