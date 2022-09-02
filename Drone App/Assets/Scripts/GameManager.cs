using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
public class GameManager : MonoBehaviour
{
    public DroneController droneController;
    public Button flyButton;
    public Button landButton;
    public GameObject ctrl;
    public GameObject drone;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    List<ARRaycastHit> hitResult= new List<ARRaycastHit>();
    struct DroneAnimationControls
    {
        public bool moving;
        public bool interpolatingASC;
        public bool interpolatingDESC;
        public float axis;
        public float dirn;
    }

    DroneAnimationControls MovingLeft;
    DroneAnimationControls MovingBack;
    // Start is called before the first frame update
    void Start()
    {
        flyButton.onClick.AddListener(ActionOnClickFly);
        landButton.onClick.AddListener(ActionOnClickLand);
    }
    

    // Update is called once per frame
    void Update()
    {
        //float spX= Input.GetAxis("Horizontal");
        //float spZ= Input.GetAxis("Vertical");

        UpdateControls(ref MovingLeft);
        UpdateControls(ref MovingBack);
        droneController.Move(MovingLeft.axis* MovingLeft.dirn,MovingBack.axis*MovingBack.dirn);

        if(droneController.isIdle()){
            UpdateAR();
        }
    }

    void UpdateAR()
    {
        Vector2 positionScreenSpace = Camera.current.ViewportToScreenPoint(new Vector2(0.5f,0.5f));
        raycastManager.Raycast(positionScreenSpace, hitResult, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds);
        if(hitResult.Count>0){
            if(planeManager.GetPlane(hitResult[0].trackableId).alignment==UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp){
                Pose pose= hitResult[0].pose;
                drone.transform.position = pose.position;
                drone.SetActive(true);
            }
        }
    }

    void UpdateControls(ref DroneAnimationControls controls){
        if(controls.moving || controls.interpolatingASC || controls.interpolatingDESC){
            if(controls.interpolatingASC){
                controls.axis+= 0.05f;
                if(controls.axis>=1.0f){
                    controls.axis=1.0f;
                    controls.interpolatingASC=false;
                    controls.interpolatingDESC=true;
                }
            }
            else if(!controls.moving){
                controls.axis-= 0.05f;
                if(controls.axis<=0.0f){
                    controls.axis=0.0f;
                    controls.interpolatingDESC=false;
                }
            }
        }
    }
    void ActionOnClickFly(){
        if(droneController.isIdle()){
            droneController.TakeOff();
            flyButton.gameObject.SetActive(false);
            ctrl.SetActive(true);
        }

    }
    void ActionOnClickLand(){
        if(droneController.isFlying()){
            droneController.Land();
            flyButton.gameObject.SetActive(true);
            ctrl.SetActive(false);
        }
    }

    public void ActionOnClickFwdHold()
    {
        MovingBack.moving=true;
        MovingBack.interpolatingASC=true;
        MovingBack.dirn=1.0f;
    }
    public void ActionOnClickBwdHold()
    {
        MovingBack.moving=true;
        MovingBack.interpolatingASC=true;
        MovingBack.dirn=-1.0f;
    }
    public void ActionOnClickLeftHold()
    {
        MovingLeft.moving=true;
        MovingLeft.interpolatingASC=true;
        MovingLeft.dirn=-1.0f;
    }
    public void ActionOnClickRightHold()
    {
        MovingLeft.moving=true;
        MovingLeft.interpolatingASC=true;
        MovingLeft.dirn=1.0f;

    }
    public void ActionOnClickFwdRelease()
    {
        MovingBack.moving=false;
    }
    public void ActionOnClickBwdRelease()
    {
        MovingBack.moving=false;
    }
    public void ActionOnClickLeftRelease()
    {
        MovingLeft.moving=false;
    }
    public void ActionOnClickRightRelease()
    {
        MovingLeft.moving=false;
    }
}
