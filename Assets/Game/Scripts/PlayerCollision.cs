using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameObject SparkEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Point Cube")
        {
            gameObject.GetComponent<PlayerScore>().IncreaseScoreAndTime();
            GameObject effect = Instantiate(SparkEffect, collision.transform.position, collision.transform.rotation);
            Destroy(effect, 5f);
            Destroy(collision.gameObject);
        }
    }
}
