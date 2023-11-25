using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private CreatureSetting creatureSetting;
    private Inventory inventory;
    private Timer attackTimer;
    private bool isAttacking = false;
    private bool canChangeDimension = false;

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
    }

    private void Update()
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

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnCollision4D>(OnCollision4D);
    }

    private void OnCollision4D(OnCollision4D data)
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
}
