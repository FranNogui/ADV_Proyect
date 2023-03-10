using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionExplosion : MonoBehaviour
{
    public GameObject particlePrefab;

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameObject o = Instantiate(particlePrefab, collision.contacts[0].point + new Vector3(0.0f, .01f, 0.0f), Quaternion.identity);
            StartCoroutine(AutoDestroy(o));
        }
    }

    IEnumerator AutoDestroy(GameObject o)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(o);
    }
}
