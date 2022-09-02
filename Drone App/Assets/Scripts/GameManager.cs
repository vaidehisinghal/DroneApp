using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public DroneController droneController;
    public Button flyButton;
    public Button landButton;
    // Start is called before the first frame update
    void Start()
    {
        flyButton.onClick.AddListener(ActionOnClickFly);
        landButton.onClick.AddListener(ActionOnClickLand);
    }
    

    // Update is called once per frame
    void Update()
    {
        float spX= Input.GetAxis("Horizontal");
        float spZ= Input.GetAxis("Vertical");

        droneController.Move(spX,spZ);
    }
    void ActionOnClickFly(){
        if(droneController.isIdle()){
            droneController.TakeOff();
            flyButton.gameObject.SetActive(false);
        }

    }
    void ActionOnClickLand(){
        if(droneController.isFlying()){
            droneController.Land();
            flyButton.gameObject.SetActive(true);
        }
    }

    public void ActionOnClickFwdHold()
    {

    }
    public void ActionOnClickBwdHold()
    {

    }
    public void ActionOnClickLeftHold()
    {

    }
    public void ActionOnClickRightHold()
    {

    }
    public void ActionOnClickFwdRelease()
    {

    }
    public void ActionOnClickBwdRelease()
    {

    }
    public void ActionOnClickLeftRelease()
    {

    }
    public void ActionOnClickRightRelease()
    {

    }
}
