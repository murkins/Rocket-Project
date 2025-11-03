using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour {


    private Rigidbody2D landerRigidbody2D;
    
    private void Awake() {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        
    }
    
    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            landerRigidbody2D.AddForce(transform.up * (force * Time.deltaTime));
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
        if (Keyboard.current.rightArrowKey.isPressed) {
            float turnSpeed = -100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {

        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on the Terrain!");
            return;
        }
        
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude  > softLandingVelocityMagnitude)
        {
            //Landed too hard!
            Debug.Log("Landed too hard!");
            return;
        }
        
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector)
        {
            //Landed on too steep angle!
            Debug.Log("Landed on too steep angle!");
            return;
        }
        
        
        Debug.Log("Successful landing!");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;
        
        Debug.Log("landingAngleScore: " + landingAngleScore);
        Debug.Log("landingSpeedScore: " + landingSpeedScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
        
        Debug.Log("score: " + score);
    }
}
