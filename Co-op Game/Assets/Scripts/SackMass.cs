using UnityEngine;

public class SackMass : MonoBehaviour
{
    private FileManager _fileManager;
    private Rigidbody2D _rb2D;
    
    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _fileManager = FileManager.Instance;
        CalculateMass();
        
    }
    

    private void CalculateMass()
    {

        var data = _fileManager.Load();
        var playerCount = data.PlayerInfo.Count;
        switch (playerCount)
        {
            case 1:
                _rb2D.linearDamping = 3.5f;
                break;
            case 2:
                _rb2D.linearDamping = 6.5f;
                break;
            case 3:
                _rb2D.linearDamping = 8f;
                break;
        }
    }
    
}
