using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static event Action OnTargetChangedPosition;
    Camera _cam;
    public LayerMask layerMask;
    private Vector3 previousPosition;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, layerMask))
            {
                Vector3 wantedPos = hit.point;
                wantedPos.y = transform.position.y;
                transform.position = wantedPos;
                
            }
            else
            {
                transform.position = previousPosition;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            OnTargetChangedPosition?.Invoke();
        }
    }

    
}
