using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private RaymarchRenderer raymarchRenderer;
    [SerializeField] private GameObject rotatingBody;
    [SerializeField] private float rotateSpeed = 60f;
    [SerializeField] private float rotateSpeed4D = 10f;
    [SerializeField] private float rotateSpeed4DModX = 1f;
    [SerializeField] private float rotateSpeed4DModY = 1f;
    [SerializeField] private float rotateSpeed4DModZ = 1f;
    [SerializeField] private float rotateSpeed4DMax = 50f;

    [SerializeField] private Timer invincibleTimer;
    private CreatureSetting creatureSetting;
    private CreatureStat creatureStat;
    private bool isInit = false;

    private void Update()
    {
        if (isInit)
        {
            Rotating();
        }
    }

    public void Init(CreatureSetting setting, CreatureStat stat)
    {
        creatureSetting = setting;
        creatureStat = stat;

        invincibleTimer = TimerManager.Instance.GetTimer();
        invincibleTimer.gameObject.SetActive(true);

        isInit = true;
    }

    private void Rotating()
    {
        rotatingBody.transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);

        raymarchRenderer.rotW.x += rotateSpeed4D * rotateSpeed4DModX * Time.deltaTime;
        raymarchRenderer.rotW.y += rotateSpeed4D * rotateSpeed4DModY * Time.deltaTime;
        raymarchRenderer.rotW.z += rotateSpeed4D * rotateSpeed4DModZ * Time.deltaTime;

        if (Mathf.Abs(raymarchRenderer.rotW.x) > rotateSpeed4DMax)
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

    private bool CheckDeath()
    {
        if (creatureStat.Health <= 0)
        {
            return true;
        }
        return false;
    }

    public void Hurt()
    {
        //Debug.Log(invincibleTimer.gameObject.name);
        if (invincibleTimer.IsFinished())
        {
            //Debug.Log("Hit Enemy " + gameObject.name + ": " + creatureStat.Health);
            AudioManager.Instance.PlaySound("Hit");
            AudioManager.Instance.PlaySound("Score");
            creatureStat.ModifyHealth(-1);
  
            if (!CheckDeath())
            {
                invincibleTimer.StartTimer(0.25f);
            }  
            else 
            {
                if (creatureSetting.CreatureType == CreatureType.Boss)
                {
                    GameManager.Instance.ChangeState(GameState.Win);
                }

                gameObject.SetActive(false);
                PostEffectsManager.Instance.PlayEnemyDeathParticles(transform);
                EventCenter.PostEvent<OnEnemyDeath>(new OnEnemyDeath(gameObject));
                EventCenter.PostEvent<OnGainScore>(new OnGainScore(creatureSetting.Value));
            }
        }
    }
}
