using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulResource : MonoBehaviour
{

    public int soulAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            Resources.ins.AddResource(new Resource(ResourceTypes.soul, soulAmount));
            Destroy(gameObject);
        }
    }
}
