using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public Rigidbody rocketBody = null;
    public AudioSource audioSource = null;
    [SerializeField] AudioClip rocketLaunch = null;

    //bool spacePressedDown, spacePressedUp;
    [SerializeField] float mainThrust = 0f;
    [SerializeField] float rotationThrust = 0f; 
    Vector3 direction = new Vector3(0, 2.5f, 0);
    [SerializeField]float power = 20f;
    public float radius = 5f;
    public float upforce = 1f;

    private InputSystem_Actions playerAction;

    private void OnEnable() {

        playerAction = new InputSystem_Actions();
        playerAction.Player.Enable();
        rocketBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        AudioListener.pause = false;
        
    }
    private void OnDisable(){
        playerAction.Player.Disable();
    }

    private void FixedUpdate() {
        
        ProcessThrust();
        ProcessRotating();
    }
    public void ProcessThrust(){
        rocketBody.AddRelativeForce(direction * mainThrust * Time.deltaTime);
    }

    void Explosion(){
        Vector3 explosionPosion = rocketBody.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosion, radius);
        foreach(Collider hit in colliders){
            Rigidbody eachRb = hit.GetComponent<Rigidbody>();
            if(eachRb != null){
                eachRb.AddExplosionForce(power, explosionPosion, radius, upforce, ForceMode.Impulse);
            }

        }
    }
    public void ProcessRotating(){
        rotateMethod(rotationThrust);

    }

    public void SetMovementVector(InputAction.CallbackContext context){
        Vector2 playerMovement = context.ReadValue<Vector2>();
        if(playerMovement.x < 0){
            rotationThrust = 50f;
        }
        else if(playerMovement.x >0){
            rotationThrust = -50f;
        }else{
            rotationThrust = 0f;
        }
    }
    public void SetRocketJump(InputAction.CallbackContext context){
        
        if(context.started || context.performed){
            //audioSource.Play();
            audioSource.PlayOneShot(rocketLaunch);
            mainThrust = 35f;
        }else{
            mainThrust = 0f;
        }
        
    }

    public void rotateMethod(float rotation){
        
        rocketBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
        rocketBody.freezeRotation = false;
    }

    

}
