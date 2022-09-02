using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    enum DroneState{
        DRONE_STATE_IDLE,
        DRONE_STATE_START_TAKINGOFF,
        DRONE_STATE_TAKINGOFF,
        DRONE_STATE_FLYING,
        DRONE_STATE_MOVINGUP,
        DRONE_STATE_START_LANDING,
        DRONE_STATE_LANDING,
        DRONE_STATE_LANDED,
        DRONE_STATE_ENGINE_STOP
    }

    DroneState currState;
    Animator anim;
    Vector3 Speed= new Vector3(0.0f,0.0f,0.0f);
    public float speedMulti= 1.0f;
    // Start is called before the first frame update
    public bool isIdle(){
        return(currState==DroneState.DRONE_STATE_IDLE);
    }

    public void TakeOff(){
        currState= DroneState.DRONE_STATE_START_TAKINGOFF;
    }

    public bool isFlying(){
        return(currState==DroneState.DRONE_STATE_FLYING);
    }

    public void Land(){
        currState= DroneState.DRONE_STATE_START_LANDING;
    }

    void Start()
    {   
        anim= GetComponent<Animator>();
        
        currState=DroneState.DRONE_STATE_IDLE;
    }
    public void Move(float speedX, float speedZ){
        Speed.x=speedX;
        Speed.z=speedZ;

        UpdateDrone();
    }
    // Update is called once per frame
    void UpdateDrone()
    {
        switch(currState)
        {
            case DroneState.DRONE_STATE_IDLE:
                break;
            case DroneState.DRONE_STATE_START_TAKINGOFF:
                anim.SetBool("TakeOff",true);
                currState=DroneState.DRONE_STATE_TAKINGOFF;
                break;
            case DroneState.DRONE_STATE_TAKINGOFF:
                if(anim.GetBool("TakeOff")==false){
                    currState= DroneState.DRONE_STATE_MOVINGUP;
                }
                break;
            case DroneState.DRONE_STATE_MOVINGUP:
                if(anim.GetBool("MoveUp")==false){
                    currState= DroneState.DRONE_STATE_FLYING;
                }
                break;
            case DroneState.DRONE_STATE_FLYING:
                float angleZ= -30.0f* Speed.x* 60.0f * Time.deltaTime;
                float angleX= 30.0f* Speed.z* 60.0f * Time.deltaTime;
                Vector3 rotation= transform.localRotation.eulerAngles;
                transform.localPosition+= Speed * speedMulti * Time.deltaTime;
                transform.localRotation= Quaternion.Euler(angleX, rotation.y, angleZ);
                break;
            case DroneState.DRONE_STATE_START_LANDING:
                anim.SetBool("MoveDown",true);
                currState=DroneState.DRONE_STATE_LANDING;
                break;
            case DroneState.DRONE_STATE_LANDING:
                if(anim.GetBool("MoveDown")==false){
                    currState= DroneState.DRONE_STATE_LANDED;
                }
                break;
            case DroneState.DRONE_STATE_LANDED:
                anim.SetBool("Land",true);
                currState= DroneState.DRONE_STATE_ENGINE_STOP;
                break;
            case DroneState.DRONE_STATE_ENGINE_STOP:
                if(anim.GetBool("Land")==false){
                    currState=DroneState.DRONE_STATE_IDLE;
                }
                break;
        }
    }
}
