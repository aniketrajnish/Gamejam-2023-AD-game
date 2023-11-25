using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    private Inventory inventory;
    private Timer attackTimer;
    private Timer invincibleTimer;
    private bool isAttacking = false;
    private bool canChangeDimension = false;
    private bool isInit = false;

    private CreatureSetting creatureSetting;
    private CreatureStat creatureStat;
    private ItemSetting currentAttackItem;
    private ItemSetting currentDimensionItem;

    private void Awake()
    {
        EventCenter.RegisterEvent<OnCollision4D>(OnCollision4D);
    }
    private void Start()
    {
        inventory = new Inventory();
        attackTimer = TimerManager.Instance.GetTimer();
        attackTimer.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isInit)
        {
            if (attackTimer.IsFinished() && isAttacking)
            {
                isAttacking = false;
                inventory.RemoveItem(currentAttackItem);
                Debug.Log("Attack Finished");
            }

            if (Input.GetKeyDown(KeyCode.Space) && canChangeDimension)
            {
                canChangeDimension = false;
                inventory.RemoveItem(currentDimensionItem);
                Debug.Log("Change dimension");
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
