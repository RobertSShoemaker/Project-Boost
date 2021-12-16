using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    //how far and what direction the object will move
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    //lower value for a faster speed; higher value for slower speed
    [SerializeField] float period = 2f;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //continually growing over time
        float cycles = Time.time / period; 

        //constant value of 6.283 or 2PI
        const float tau = Mathf.PI * 2;
        //going from -1 to 1;
        //Think of how the cycles variable will continue getting bigger, going from 0 to 360 to 720 around the circle continually;
        //Even though it's constantly getting bigger it still remains within the regular values that will come from SIN;
        //So it will continue forever between -1 to 1
        float rawSinWave = Mathf.Sin(cycles * tau);

        //recalculated to go from 0 to 1 so it's cleaner; otherwise the object would start oscillating from the midpoint, instead of the beginning
        movementFactor = (rawSinWave + 1f) / 2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
