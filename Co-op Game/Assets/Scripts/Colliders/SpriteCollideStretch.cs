using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteCollideStretch : MonoBehaviour
{
    [SerializeField] private SpriteSkin spriteSkin;
    
    [SerializeField] private float stretchStrength = 0.2f;
    [SerializeField] private float influenceRadius = 1.0f;
    [SerializeField] private float returnSpeed = 5f;

    private Transform[] _bones;
    private Vector3[] _defaultLocalPositions;
    
    private void Start()
    {
        _bones = spriteSkin.boneTransforms;
        _defaultLocalPositions = new Vector3[_bones.Length];

        for (var i = 0; i < _bones.Length; i++)
        {
            _defaultLocalPositions[i] = _bones[i].localPosition;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            ApplyStretch(contact.point);
        }
    }

    private void ApplyStretch(Vector2 hitPoint)
    {
        for (var i = 0; i < _bones.Length; i++)
        {
            var bone = _bones[i];

            var dist = Vector2.Distance(bone.position, hitPoint);

            if (dist < influenceRadius)
            {
                var falloff = 1f - (dist / influenceRadius);
                falloff *= falloff;
                falloff = Mathf.Clamp01(falloff);
                                
                var dir = (bone.position - (Vector3)hitPoint).normalized;

                var offset = (dir * stretchStrength * falloff);

                bone.localPosition += offset;
            }
        }
    }

    private void LateUpdate()
    {
        for (var i = 0; i < _bones.Length; i++)
        {
            _bones[i].localPosition = Vector3.Lerp(
                _bones[i].localPosition,
                _defaultLocalPositions[i],
                Time.deltaTime * returnSpeed
            );
        }
    }
}