using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
   [SerializeField] InputAction thrust;
   [SerializeField] InputAction rotation;
   [SerializeField]float thrustStrength = 100f;
   [SerializeField]float rotationStrength = 100f;
   [SerializeField]ParticleSystem rightThrustParticle;
   [SerializeField]ParticleSystem leftThrustParticle;
   [SerializeField]ParticleSystem MainThrustParticle;
   [SerializeField] AudioClip mainEngine;
   [SerializeField]Light controlLight;
   Rigidbody rb;
   AudioSource audioSource;
   private float _fuel = 1f;
   public float Fuel { get { return _fuel; } set { _fuel = value; } }

   private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        controlLight.enabled = false;
    }
    
    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }
   private void FixedUpdate()
    {
      ProcessThrust();
      ProcessRotation();
    }
    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            // its a add force to a game object locally 
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

        if (!MainThrustParticle.isPlaying)
        {
            MainThrustParticle.Play();
            controlLight.enabled = true;
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        MainThrustParticle.Stop();
        controlLight.enabled = false;
    }
    private void ProcessRotation()
    {
      float rotationInput = rotation.ReadValue<float>();
      Debug.Log(rotationInput);
      if (rotationInput > 0)
        {
            RightRotation();
        }
        else if (rotationInput < 0)
        {
            leftRotation();
        }
        else
        {
            StopRotation();
        }
    }
    private void RightRotation()
    {
        ApplyRotation(-rotationStrength);
        if (!leftThrustParticle.isPlaying)
        {
            rightThrustParticle.Stop();
            leftThrustParticle.Play();
        }
    }
    private void leftRotation()
    {
        ApplyRotation(rotationStrength);
        if (!rightThrustParticle.isPlaying)
        {
            leftThrustParticle.Stop();
            rightThrustParticle.Play();
        }
    }
    private void StopRotation()
    {
        leftThrustParticle.Stop();
        rightThrustParticle.Stop();
    }
    private void ApplyRotation(float rotationThisFrame)
    {
      rb.freezeRotation = true;
      transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime); //0,0,1
      rb.freezeRotation = false;
    }
}