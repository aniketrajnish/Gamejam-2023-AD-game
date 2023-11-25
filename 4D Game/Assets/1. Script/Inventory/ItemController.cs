using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private RaymarchRenderer raymarchRenderer;
    [SerializeField] private float rotateSpeed = 60f;
    [SerializeField] private float rotateSpeed4D = 10f;
    [SerializeField] private float rotateSpeed4DModX = 1f;
    [SerializeField] private float rotateSpeed4DModY = 1f;
    [SerializeField] private float rotateSpeed4DModZ = 1f;
    [SerializeField] private float rotateSpeed4DMax = 50f;
    [SerializeField] private ItemSettingSO ItemSettingSO;
    [SerializeField] private GameObject itemShape;

    private ItemSetting itemSetting;
    private Timer respawnTimer;

    private void Start()
    {
        itemSetting = ItemSettingSO.ItemSetting.Clone();
        respawnTimer = TimerManager.Instance.GetTimer();
        respawnTimer.gameObject.SetActive(true);
    }

    void Update()
    {
        Rotating();
        Respawn();
    }

    public ItemSetting PickUpItem()
    {
        itemShape.SetActive(false);
        respawnTimer.StartTimer(itemSetting.RespawnTime);
        return itemSetting;
    }

    private void Respawn()
    {
        if(respawnTimer.IsFinished())
        {
            itemShape.SetActive(true);
        }
    }

    private void Rotating()
    {
        transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);

        switch (itemSetting.Type)
        {
            case ItemType.Attack:
                raymarchRenderer.rotW.x += rotateSpeed4D * rotateSpeed4DModX * Time.deltaTime;
                raymarchRenderer.rotW.y += rotateSpeed4D * rotateSpeed4DModY * Time.deltaTime;
                raymarchRenderer.rotW.z += rotateSpeed4D * rotateSpeed4DModZ * Time.deltaTime;
                break;
            case ItemType.Dimension:
                raymarchRenderer.rotW.x += rotateSpeed4D * rotateSpeed4DModX * Time.deltaTime;
                //raymarchRenderer.rotW.y += rotateSpeed4D * rotateSpeed4DModY * Time.deltaTime;
                raymarchRenderer.rotW.z += rotateSpeed4D * rotateSpeed4DModZ * Time.deltaTime;
                break;
        }

        if(Mathf.Abs(raymarchRenderer.rotW.x) > rotateSpeed4DMax)
        {
            raymarchRenderer.rotW.x = rotateSpeed4DMax * rotateSpeed4DModX;
            rotateSpeed4DModX *= -1;
        }

        if (Mathf.Abs(raymarchRenderer.rotW.y) > rotateSpeed4DMax)
        {
            raymarchRenderer.rotW.y = rotateSpeed4DMax * rotateSpeed4DModY;
            rotateSpeed4DModY *= -1;
        }

        if (Mathf.Abs(raymarchRenderer.rotW.z) > rotateSpeed4DMax)
        {
            raymarchRenderer.rotW.z = rotateSpeed4DMax * rotateSpeed4DModZ;
            rotateSpeed4DModZ *= -1;
        }
           
    }
}
