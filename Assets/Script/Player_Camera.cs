using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    
    public GameObject player; // プレイヤーのゲームオブジェクト
    
    void Update()
    {
        // プレイヤーを追従
         transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);   
    }
}
