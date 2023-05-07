using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////
//
/*
    게임의 로직을 담당하는 스크립트. 어드벤쳐 형식의 게임의 주요 기능인 이동을 구현하기 위해
    연결리스트로 구현한 Node 클래스, Node 클래스를 통해 실제 이동을 구현할 GameManager 클래스
    를 구현. 
    게임의 핵심적인 요소인 만큼 심혈을 기울여서 로직을 짤 필요가 있음.

    지금은 임의로 구현, 추후 전체적 수정이 매우 필요할 수 있음. 
*/
//
////////////////

public class Node<T> where T : class    //회의를 통해서 어떠한 형식을 가질지 생각해봐야함.  
{
    public int ID;
    public Node<T>[] nextNode_arr;  //여러개의 노드와 연결되어있음
    public Node<T> prevNode;    //이전 노드
    
    public Node(int n_nextNodeNum, int ID){
        if(n_nextNodeNum > 0) nextNode_arr = new Node<T>[n_nextNodeNum];  //동적할당
        else nextNode_arr = null;

        this.ID = ID;
    }
}

public class GameManager : MonoBehaviour
{
    
}
