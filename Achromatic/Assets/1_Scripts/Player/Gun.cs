using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    #region Declare a variable

    Zoom zoom;

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;

    [Header("Hit Effects")]
    [SerializeField]
    private GameObject impactEffect;

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponsetting;
    [SerializeField]
    private int armo;
    public bool reloading = false;

    private float lastAttackTime = 0;
    private float currentSpread = 0;
    private float currentSpreadVelocity;
    private float stability;
    private float maxSpread;

    Animator anim;
    #endregion

    #region Preprocessing Function

    void Start()
    {
        zoom = GetComponentInParent<Zoom>();
        anim = GetComponent<Animator>();
        stability = weaponsetting.stability;
        maxSpread = weaponsetting.maxSpread;
    }
    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
    }
    #endregion

    #region Attack Function
    public void StartWeaponAction(int type=0)
    {
        if (armo > 0 && reloading == false)
        {
            if (type == 0)
            {
                if (weaponsetting.isAuto == true)
                {
                    StartCoroutine("OnAttackLoop");
                }
                else
                {
                    OnAttack();
                }
            }
        }
    }
    public void StopWeaponAction(int type=0)
    {
        if (type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    private IEnumerator OnAttackLoop()
    {
        while(armo > 0 && !reloading)
        {
            OnAttack();
            yield return null;
        }

        StopWeaponAction();
    }
    public void OnAttack()
    {
        if(Time.time - lastAttackTime > weaponsetting.attackRate)
        {
            lastAttackTime = Time.time;

            var xError = Utility.GetRandomNormalDistribution(0f, currentSpread);
            var yError = Utility.GetRandomNormalDistribution(0f, currentSpread);
            var fireDirection = zoom.Cam.transform.forward;

            fireDirection = Quaternion.AngleAxis(yError, Vector3.up) * fireDirection;
            fireDirection = Quaternion.AngleAxis(xError, Vector3.right) * fireDirection;

            currentSpread += 1f / stability;

            RaycastHit hit;

            if(Physics.Raycast(zoom.Cam.transform.position, fireDirection, out hit, weaponsetting.attackDistance))
            {
                Mob mob = hit.transform.GetComponent<Mob>();
                if(mob != null)
                {
                    mob.TakeDamage(weaponsetting.attackDamage);
                }

                GameObject ImpactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(ImpactGo, 0.5f);
            }
            armo--;
            StartCoroutine("OnMuzzleFlashEffect");
        }
    }
    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponsetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }
    #endregion
    
    #region Reloading Function
    private void Reload()
    {
        if (reloading || armo == weaponsetting.maxarmo) return;
        else
        {
            reloading = true;
            anim.SetBool("Reloading", true);

            StartCoroutine(ReloadRoutine());
        }
    }
    IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(weaponsetting.reloadtime - 0.2f);

        anim.SetBool("Reloading", false);
        StartCoroutine(AnimationReloadRoutine());
    }
    IEnumerator AnimationReloadRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        armo = weaponsetting.maxarmo;
        reloading = false;
    }
    #endregion

    #region Update Function
    private void Update()
    {
        if (zoom.zoommode == true)
        {
            stability = weaponsetting.stability * 2;
            maxSpread = weaponsetting.maxSpread / 2;
        }
        else
        {
            stability = weaponsetting.stability;
            maxSpread = weaponsetting.maxSpread;
        }
        currentSpread = Mathf.SmoothDamp(currentSpread, 0f, ref currentSpreadVelocity, 1f / weaponsetting.restoreFromRecoilSpeed);
        currentSpread = Mathf.Clamp(currentSpread, 0f, maxSpread);

        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }
    #endregion
}
