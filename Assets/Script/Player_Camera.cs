using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    
    public GameObject player; // プレイヤーのゲームオブジェクト
    private int CameraPosition_Z = -10; // カメラ追従の際のZ座標
    
    void Update()
    {
        // プレイヤーを追従
         transform.position = new Vector3(player.transform.position.x, player.transform.position.y, CameraPosition_Z);   
    }
}
