using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 1000f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    //Move Rocket
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    //Rotate rocket
    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();

        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    //Move rocket upwards relative to the rocket, play engine audio and particle effects
    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainBoosterParticles.isPlaying)
        {
            mainBoosterParticles.Play();
        }
    }

    //Stop the thrust audio and particles
    private void StopThrusting()
    {
        audioSource.Stop();
        mainBoosterParticles.Stop();
    }

    //Rotate the rocket to the left and play the right booster particles
    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightBoosterParticles.isPlaying)
        {
            rightBoosterParticles.Play();
        }
    }

    //Rotate the rocket to the right and play the left booster particles
    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftBoosterParticles.isPlaying)
        {
            leftBoosterParticles.Play();
        }
    }

    //Stop the booster particles
    private void StopRotating()
    {
        rightBoosterParticles.Stop();
        leftBoosterParticles.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        //freeze rotation so we can manually rotate
        rb.freezeRotation = true; 
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        //unfreeze rotation so the physics system can take over
        rb.freezeRotation = false;
    }
}
