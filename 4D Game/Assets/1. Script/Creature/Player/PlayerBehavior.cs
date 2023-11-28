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
    private bool isLastLevel = false;
    private bool isUsingMachine = false;

    private void Awake()
    {
        EventCenter.RegisterEvent<OnCollision4D>(OnCollision4D);
        EventCenter.RegisterEvent<OnScorePointRespawn>(OnScorePointRespawn);
    }
    private void Start()
    {
        inventory = new Inventory();
        attackTimer = TimerManager.Instance.GetTimer();
        attackTimer.gameObject.SetActive(true);
        raymarcher = Camera.main.GetComponent<Raymarcher>();

        currentWPos = raymarcher.wPos;
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnCollision4D>(OnCollision4D);
        EventCenter.UnRegisterEvent<OnScorePointRespawn>(OnScorePointRespawn);
    }

    private void Update()
    {
        if (isInit)
        {
            CheckAttackMode();
            ChangingDimension();
            UsingMachine();
        }     
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

    private void UsingMachine()
    {
        if (isUsingMachine)
        {
            if (Input.GetKey(KeyCode.E))
            {
                EventCenter.PostEvent<OnUsingMachine>(new OnUsingMachine(Time.deltaTime));
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                EventCenter.PostEvent<OnUsingMachine>(new OnUsingMachine(-Time.deltaTime));
            }
        } 
    }

    private void CheckAttackMode()
    {
        if (attackTimer.IsFinished() && isAttacking)
        {
            isAttacking = false;
            inventory.RemoveItem(currentAttackItem);
            EventCenter.PostEvent<OnPlayerAttackMode>(new OnPlayerAttackMode(currentAttackItem.Value - (int)attackTimer.currentTime));
            AudioManager.Instance.PlaySound("Alarm");
            //Debug.Log("Attack Finished");
        }
        else if (isAttacking)
        {
            EventCenter.PostEvent<OnPlayerAttackMode>(new OnPlayerAttackMode(currentAttackItem.Value - (int)attackTimer.currentTime));
        }
    }

    private void StartChangingDimension()
    {
        if (canChangeDimension)
        {
            canChangeDimension = false;
            isChangingDimension = true;
            inventory.RemoveItem(currentDimensionItem);

            if ((int)raymarcher.wPos + wOffset >= LevelManager.Instance.LevelCount)
            {
                isLastLevel = true;
            }
            else
            {
                isLastLevel = false;
            }

            //LevelManager.Instance.ChangeLevel(1);
            EventCenter.PostEvent<OnDimensionChanging>(new OnDimensionChanging(true, (int)(currentWPos+ wOffset)));
            //Debug.Log("Change dimension");
        }
    }

    private void ChangingDimension()
    {
        if (isChangingDimension)
        {
            if (isLastLevel)
            {
                raymarcher.wPos = Mathf.Lerp(raymarcher.wPos, 0, 0.1f);
            }
            else
            {
                raymarcher.wPos = Mathf.Lerp(raymarcher.wPos, currentWPos + wOffset, 1f);
            }

            raymarcher.wRot.y = Mathf.Lerp(raymarcher.wRot.y, maxWRot, 0.1f);
            if (Mathf.Abs(raymarcher.wRot.y - maxWRot) < 4f)
            {
                raymarcher.wRot.y = 0;

                if (isLastLevel)
                {
                    raymarcher.wPos = 0;
                }
                else
                {
                    raymarcher.wPos = currentWPos + wOffset;
                }

                currentWPos = raymarcher.wPos;
                playerShadow.posW = raymarcher.wPos;

                int levelIndex = (int)currentWPos;
                isChangingDimension = false;
                LevelManager.Instance.ChangeLevel(levelIndex);
                EventCenter.PostEvent<OnDimensionChanging>(new OnDimensionChanging(false, levelIndex));
                isLastLevel = false;
            }
        }
    }

    private void ActivateItem(ItemController itemController)
    {
        ItemSetting item = itemController.PickUpItem();

        if (inventory.HasItem(item) || item == null)
        {
            return;
        }
        inventory.AddItem(item);

        switch (item.Type)
        {
            case ItemType.Attack:
                attackTimer.StartTimer(item.Value);
                currentAttackItem = item;
                isAttacking = true;
                AudioManager.Instance.PlaySound("Attack");
                break;
            case ItemType.Dimension:
                canChangeDimension = true;
                currentDimensionItem = item;
                StartChangingDimension();
                AudioManager.Instance.PlaySound("Dimension");
                break;
            case ItemType.Score:
                currentScore += item.Value;
                EventCenter.PostEvent<OnGainScore>(new OnGainScore(currentScore));
                AudioManager.Instance.PlaySound("Score");
                break;
            default:
                break;
        }

        Debug.Log("Pick up: " + item.Name);
    }

    private void Hurt()
    {
        AudioManager.Instance.PlaySound("Hit");
        creatureStat.ModifyHealth(-1);
        invincibleTimer.StartTimer(1.5f);
        Debug.Log("Ouch: " + creatureStat.Health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (creatureSetting.CreatureType == CreatureType.Player && isInit)
        {
            if (other.gameObject.tag == "Item")
            {
                ItemController itemController
                    = other.gameObject.GetComponentInParent<ItemController>();

                if (itemController != null && itemController.gameObject.activeInHierarchy)
                {
                    //Debug.Log(itemController.gameObject.name);
                    ActivateItem(itemController);
                }
            }

            if(other.gameObject.tag == "Machine")
            {
                isUsingMachine = true;
                EventCenter.PostEvent<OnNearMachine>(new OnNearMachine(isUsingMachine));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (creatureSetting.CreatureType == CreatureType.Player && isInit)
        {
            if (other.gameObject.tag == "Machine")
            {
                isUsingMachine = false;
                EventCenter.PostEvent<OnNearMachine>(new OnNearMachine(isUsingMachine));
            }
        }
    }

    private void OnCollision4D(OnCollision4D data)
    {
        if(creatureSetting.CreatureType == CreatureType.Player && isInit)
        {
            if (data.collidedObject.tag == "Item")
            {
                ItemController itemController
                    = data.collidedObject.GetComponentInParent<ItemController>();
                if (itemController != null && itemController.gameObject.activeInHierarchy)
                {
                    ActivateItem(itemController);
                }
            }

            if (data.collidedObject.tag == "Enemy")
            {
                if (!isAttacking && invincibleTimer.IsFinished())
                {
                    Hurt();
                }
                else if(isAttacking)
                { 
                    if(data.collidedObject.activeInHierarchy)
                        data.collidedObject.GetComponentInParent<EnemyBehavior>().Hurt();
                }
            }
        }
    }

    private void OnScorePointRespawn(OnScorePointRespawn data)
    {
        inventory.RemoveItem(data.RespawnedItem);
    }
}
