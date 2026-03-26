using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; 
    public float stepSize = 1f;

    // We slaan de joystick waarden hier op
    private float currentX = 0f;
    private float currentY = 0f;

    public void MoveLeft()
    {
        transform.Translate(Vector3.left * stepSize);
    }
    
    public void MoveRight()
    {
        transform.Translate(Vector3.right * stepSize);
    }

    public void Jump()
    {
        Debug.Log("Jump!");
    }

    // We updaten nu alleen de waarden wanneer er een signaal binnenkomt
    public void MoveWithJoystick(float x, float y)
    {
        // Voeg een minteken toe om de richting om te draaien. 
        // TIP: Als alleen links/rechts OF onder/boven omgekeerd is, 
        // verwijder dan het minteken bij de as die wel al goed werkte!
        currentX = x;
        currentY = -y;
    }

    // De Unity Update functie wordt elke frame aangeroepen voor soepele beweging
    void Update()
    {
        Vector3 movement = new Vector3(currentX, currentY, 0); 
        
        // Zorg ervoor dat hij alleen beweegt als er input is
        if (movement.sqrMagnitude > 0)
        {
            // Omdat we in Update() zitten, gebruiken we Time.deltaTime voor consistente en soepele snelheid
            transform.Translate(movement * speed * Time.deltaTime);
        }
    }
}
