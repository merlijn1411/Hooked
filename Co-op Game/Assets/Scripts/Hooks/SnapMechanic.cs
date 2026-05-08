using System.Collections;
using UnityEngine;

public class SnapMechanic : MonoBehaviour
{
    public IEnumerator Snap(Transform target)
    {
        target.position = transform.position;
        yield return new WaitForSeconds(0);
        StartCoroutine(Snap(target));
    }
}
