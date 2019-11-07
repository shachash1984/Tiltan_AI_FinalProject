using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSense : Sense
{
    public float visionDepth = 100f;
    public float visionWidth = 120f;
    public float visionHeight = 60f;
    

    private void Update()
    {
        RaycastHit hitInfo;
        if(!detectedEnemy && Physics.BoxCast(transform.position, new Vector3(visionWidth/2, visionHeight/2, visionDepth/2), transform.forward, out hitInfo, Quaternion.identity))
        {
            if (hitInfo.collider.gameObject.layer == NPC.ENEMY_LAYER)
            {
                detectedEnemy = hitInfo.collider.GetComponent<Enemy>();
                Detected(this, detectedEnemy);
            }
        }
    }


}
