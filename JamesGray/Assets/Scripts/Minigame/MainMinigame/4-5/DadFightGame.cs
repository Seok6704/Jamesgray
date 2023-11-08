using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadFightGame : MonoBehaviour
{
    bool left, right, up, down; // 패턴 회피용 변수
    int ran;

    void Start()
    {
        
    }

    void DadAttack()
    {
        ran = Random.Range(0, 4);
        
    }
}
