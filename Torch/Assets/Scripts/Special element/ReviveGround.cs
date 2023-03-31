using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveGround : MonoBehaviour
{
    public Transform orignRotaRock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ShootFire"))
        {
            ShootFire shootFire = collision.gameObject.GetComponent<ShootFire>();
            shootFire.StopAndAttach(orignRotaRock);
        }
    }
}
