using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;

public class TESTDATA : MonoBehaviour
{
    Nodetemp head = new Nodetemp(1);
    private void Awake() {
        
        Nodetemp two = new Nodetemp(2);
        Nodetemp three = new Nodetemp(3);

        head.next = two;
        two.next = three;

        
    }
    private void Start() {
        //Debug.Log(head.next.next.data);
        PriorityQueue<int> Q = new PriorityQueue<int>();
        Q.Enqueue(10,1);
        Q.Enqueue(11,2);
        Q.Enqueue(100,3);

        Q.Print();
    }

    public class Nodetemp
    {
        public int data;
        public Nodetemp pre, next;

        public Nodetemp(int a)
        {
            data = a;
            pre = null;
            next = null;
        }
    }
}


