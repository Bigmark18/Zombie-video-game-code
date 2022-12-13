using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [Header("Player Punch Var")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float punchingRange = 0.3f;

    [Header("Punch Effects")]
    public GameObject WoodedEffect;

    public void Punch()
    {
        RaycastHit hitInfo;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, punchingRange))
        {
            Debug.Log(hitInfo.transform.name);

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Target target = hitInfo.transform.GetComponent<Target>();
            Target2 target2 = hitInfo.transform.GetComponent<Target2>();


            if(objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
            }
            else if(target !=null)
            {
                target.zombieHitDamage(giveDamageOf);
                  
            }
             else if(target2 !=null)
            {
                target2.zombieHitDamage(giveDamageOf);
                
            }
        }
    }
}