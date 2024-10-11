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
    bool spacePressedDown, spacePressedUp;
    [SerializeField] float mainThrust = 36f;
    [SerializeField] float rotationThrust = 50f; 
    Vector3 direction = new Vector3(0, 2.5f, 0);
    [SerializeField]float power = 20f;
    public float radius = 5f;
    public float upforce = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
        rocketBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        String[] names = Input.GetJoystickNames();
        //Debug.Log(names);
        spacePressedDown = false;
        spacePressedUp = false;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            spacePressedDown = true;
            spacePressedUp = false;
        }
        else if(Input.GetKeyUp(KeyCode.Space)){
            spacePressedDown = false;
            spacePressedUp = true;
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        ProcessThrust();
        ProcessRotating();
        //Invoke("Explosion", 5);
    }
    void ProcessThrust(){
        if(Input.GetKey(KeyCode.Space)){
            rocketBody.AddRelativeForce(direction * mainThrust * Time.deltaTime);
        }
        if(spacePressedDown){
            Debug.Log("Space down");
            if(!audioSource.isPlaying){
                audioSource.Play();
            }
            
        }
        else if(spacePressedUp){
            Debug.Log("space up");
            audioSource.Stop();
        }
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
    void ProcessRotating(){
        foreach(char c in Input.inputString){
            //Debug.Log(c);
        }
    
        if(Input.GetKey(KeyCode.A)){
            rotateMethod(rotationThrust);
        

        }
        else if(Input.GetKey(KeyCode.D)){
            rotateMethod(-rotationThrust);

        }

    }
    void rotateMethod(float rotationThisFrame){
        rocketBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rocketBody.freezeRotation = false;
    }


}
