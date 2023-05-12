using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHolder : MonoBehaviour
{

    public GameObject playerObj;
    public GameObject fire;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ShootFire"))
        {
            Destroy(collision.gameObject);
            fire.SetActive(true);

            // º§ªÓplayer
            playerObj.SetActive(true);
            PlayerController playerController = playerObj.GetComponent<PlayerController>();
            playerController.SetForce(Vector2.up * 20);

            //…Ë÷√camera follow player
            CameraMgr.GetInstance().ChangeFollow(playerObj.transform);
            CameraMgr.GetInstance().SetCurrentCameraOffsetY(4.4f);
            CameraMgr.GetInstance().SetDamping(new Vector2(0.5f, 1));
            CameraMgr.GetInstance().SetDeathZone(0.15f, 0);

        }
    }
}
