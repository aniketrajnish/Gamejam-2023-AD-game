using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int currentScore = 0;
    private Inventory inventory;
    private Timer attackTimer;
    private Timer invincibleTimer;
    private bool isAttacking = false;
    private bool isChangingDimension = false;
    private bool canChangeDimension = false;
    private bool isInit = false;

    private CreatureSetting creatureSetting;
    private CreatureStat creatureStat;
    private ItemSetting currentAttackItem;
    private ItemSetting currentDimensionItem;

    [SerializeField] private RaymarchRenderer playerShadow;
    [SerializeField] private float wOffset = 1f;
    [SerializeField] private float maxWRot = 181f;
    private Raymarcher raymarcher;
    private float currentWPos = 0;

    private void Awake()
    {
        EventCenter.RegisterEvent<OnCollision4D>(OnCollision4D);
    }
    private void Start()
    {
        inventory = new Inventory();
        attackTimer = TimerManager.Instance.GetTimer();
        attackTimer.gameObject.SetActive(true);
        raymarcher = Camera.main.GetComponent<Raymarcher>();

        currentWPos = raymarcher.wPos;
    }

    private void Update()
    {
        if (isInit)
        {
            CheckAttackMode();
            ChangingDimension();
        }     
    }

    private void CheckAttackMode()
    {
        if (attackTimer.IsFinished() && isAttacking)
        {
            isAttacking = false;
            inventory.RemoveItem(currentAttackItem);
            Debug.Log("Attack Finished");
        }
    }

    private void StartChangingDimension()
    {
        if (canChangeDimension)
        {
            canChangeDimension = false;
            isChangingDimension = true;
            inventory.RemoveItem(currentDimensionItem);

            //LevelManager.Instance.ChangeLevel(1);
            EventCenter.PostEvent<OnDimensionChanging>(new OnDimensionChanging(true));
            Debug.Log("Change dimension");
        }
    }

    private void ChangingDimension()
    {
        if (isChangingDimension)
        {
            raymarcher.wPos = Mathf.Lerp(raymarcher.wPos, currentWPos + wOffset, 0.1f);
            raymarcher.wRot.y = Mathf.Lerp(raymarcher.wRot.y, maxWRot, 0.1f);
            if (Mathf.Abs(raymarcher.wRot.y - maxWRot) < 0.1f)
            {
                raymarcher.wRot.y = 0;
                raymarcher.wPos = currentWPos + wOffset;

                if ((int)raymarcher.wPos >= LevelManager.Instance.LevelCount)
                {
                    raymarcher.wPos = 0;
                }
                currentWPos = raymarcher.wPos;
                playerShadow.posW = raymarcher.wPos;

                int levelIndex = (int)currentWPos;
                isChangingDimension = false;
                LevelManager.Instance.ChangeLevel(levelIndex);
                EventCenter.PostEvent<OnDimensionChanging>(new OnDimensionChanging(false));
            }
        }
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnCollision4D>(OnCollision4D);
    }

    public void Init(CreatureSetting setting, CreatureStat stat)
    {
        creatureSetting = setting;
        creatureStat = stat;
        creatureStat.OnHealthChanged += healthBar.OnHealthChanged;

        invincibleTimer = TimerManager.Instance.GetTimer();
        invincibleTimer.gameObject.SetActive(true);

        isInit = true;
    }

    private void ActivateItem(ItemController itemController)
    {
        ItemSetting item = itemController.PickUpItem();
        inventory.AddItem(item);

        switch (item.Type)
        {
            case ItemType.Attack:
                attackTimer.StartTimer(item.Value);
                isAttacking = true;
                break;
            case ItemType.Dimension:
                canChangeDimension = true;
                StartChangingDimension();
                break;
            case ItemType.Score:
                currentScore += item.Value;
                break;
        }

        Debug.Log("Pick up: " + item.Name);
    }

    private void OnCollision4D(OnCollision4D data)
    {
        if(creatureSetting.CreatureType == CreatureType.Player)
        {
            if (data.collidedObject.tag == "Item")
            {
                ItemController itemController
                    = data.collidedObject.GetComponentInParent<ItemController>();
                if (itemController != null)
                {
                    ActivateItem(itemController);
                }
            }

            if (data.collidedObject.tag == "Enemy" && invincibleTimer.IsFinished())
            {
                if (!isAttacking)
                {
                    Hurt();
                }
                else 
                {
                    data.collidedObject.GetComponentInParent<EnemyBehavior>().gameObject.SetActive(false);
                    EventCenter.PostEvent<OnEnemyDeath>(new OnEnemyDeath(data.collidedObject));
                }
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
