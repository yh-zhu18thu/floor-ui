using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{

    private Transform m_Transform ;
public float attackTimer;
 public float attackTime;
 private bool open ;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>() ;
        attackTimer = 0;
  attackTime = 10f;
  open = true ;
        
    }

    public void OpenDoor(){
        transform.rotation=Quaternion.Euler(-90f,90f,0.0f);
    }
    public void CloseDoor(){
        transform.rotation=Quaternion.Euler(-90f,0.0f,0.0f);
    }
    // Update is called once per frame
    public void changeStatus()
    {
        open = !open ;
        if(open){
            OpenDoor();
        }
        else{
            CloseDoor();
        }
//         if (attackTimer>0)
//    attackTimer-= Time.deltaTime;
//   if (attackTimer<0)
//    attackTimer=0;
//   if(attackTimer == 0)
//   {
//       if(open){
//           OpenDoor();
//           open = false ;
//       }
//       else{
//           CloseDoor();
//           open = true ; 
//       }
//    attackTimer =attackTime;
  }

}