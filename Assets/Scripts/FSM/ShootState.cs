using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : State
{

    private float _fireRate = 1f;
    private float _shotTimer;

    public override void Action()
    {
        Shoot();
    }

    private void Shoot()
    {
        if(IsAllowedToShoot())
        {
            _shotTimer = Time.time;
            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            projectile.layer = NPC.PROJECTILE_LAYER;
            projectile.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
            Vector3 projRot = transform.rotation.eulerAngles;
            projRot.x += 90;
            projectile.transform.rotation = Quaternion.Euler(projRot);
            projectile.transform.position = transform.position + transform.forward * 2;
            projectile.GetComponent<Renderer>().material.color = Color.yellow;
            Rigidbody rb = projectile.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = transform.forward * 30;
            Destroy(projectile, 5);
        }
    }

    

    private bool IsAllowedToShoot()
    {
        return Time.time - _fireRate >= _shotTimer;
    }

}
