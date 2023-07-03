using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    2023.05.18 임 권
    유니티나 .NET에서 기본적으로 제공하지 않는 자료구조 구현을 위한 스크립트
    using DataStructure; 을 사용하여 불러올수있음.
    현재 기본 노드와 우선순위 큐가 구현되어 있습니다.
*/

namespace DataStructure
{
    /*
        우선순위 큐

        더 테스트 해봐야 알겠지만, 1회 테스트 결과 Dequeue, Enqueue 결과 문제 없음.
        메모리 누수 현상이 있는지 좀더 연구 필요.
        구글링 결과 C#에는 메모리 해제 기능이 GC외에는 없기 때문에 개발자가 임의로 메모리를 해제하는 것이 어려움
        값이 더이상 참조되지 않으면 어느 시점에서 GC가 메모리를 해제함. 시점은 알 수 없음
        Unsafe 키워드를 사용하여 C++문법으로 구현하는 방법이 있지만, 관리되고 있는 자료형(클래스를 의미하는듯)을 사용하면 unsafe 불가
        ref로 선언하는 것은 C# 11.0에서 부터 지원... Unity는 C# 9.0 지원....
        메모리 누수 발생시 리스트로 구현하는 것이 좋을것같음. 

    */
    public class PriorityQueue<T>
    {
        public int count;
        int lastDegree;
        Node head, last, latest;    //각각 Root, 새로 삽입될 위치의 부모노드, 가장 최근에 삽입된 노드를 의미
        MODE Mode;

        public PriorityQueue(string mode = "GREATER")  //생성자 기본값 오름차순
        {
            InitQ();
            Mode = mode == "GREATER" ? MODE.GREATER : MODE.LESS;
        }
        void InitQ()
        {
            head = null;    //우선순위큐(트리) 루트 노드
            last = null;    //삽입을 쉽게하기 위한 
            latest = null;  //삭제를 쉽게하기 위한
            count = 0;
        }
        public void Enqueue(T data, int pri)
        {
            Node newNode = new Node(data, pri), temp;
            temp = newNode;
            if(count == 0)  //우선순위 큐가 비어있다면
            {
                head = newNode;
                last = newNode;
                newNode.pre = null; //Root이므로 null 값을 가진다.
                lastDegree = 0;
            }
            else if(last.left == null)  //마지막 노드의 왼쪽이 비어있을때
            {
                last.left = newNode;
                newNode.pre = last;
            }
            else                        //마지막 노드의 오른쪽 자식이 비어있을때, 삽입을 하고 마지막 노드를 검색한다.
            {
                last.right = newNode;
                newNode.pre = last;
                SearchLast(ref head, 0);
            }
            
            while(temp.pre != null && (Mode == MODE.GREATER ? temp.pre.priority > temp.priority : temp.pre.priority < temp.priority))  //우선순위에 따라 위치 변경
            {
                Swap(ref temp, ref temp.pre);
                temp = temp.pre;
            }

            latest = newNode;
            count++;
        }

        public T Dequeue()
        {
            if(count == 0)
            {
                Debug.Log("Empty Q!");
                return default(T);
            }

            Node temp = head, iter = null;
            T data = head.data;
            
            if(latest == head) 
            {
                InitQ();
                return data;
            }

            head.data = latest.data;
            head.priority = latest.priority;

            if(latest.pre.right != null)    //가장 최근에 삽입된 노드가 왼쪽이라면 왼쪽을 null, 오른쪽이라면 오른쪽 null
            {
                latest.pre.right = null;
            }
            else
            {
                latest.pre.left = null;
            }

            //둘 중 하나라도 null이 아니면서 우선순위의 변경이 필요한 값이 있을때 반복
            if(Mode == MODE.GREATER)
            {
                while(temp.priority > (temp.left != null ? temp.left.priority : temp.priority) || temp.priority > (temp.left != null ? temp.left.priority : temp.priority))
                {
                    iter = temp.right != null ? (temp.right.priority > temp.left.priority ? temp.left : temp.right) : temp.left;
                    Swap(ref temp, ref iter);
                    temp = iter;
                }
            }
            else
            {
                while(temp.priority < (temp.left != null ? temp.left.priority : temp.priority) || temp.priority < (temp.left != null ? temp.left.priority : temp.priority))
                {
                    iter = temp.right != null ? (temp.right.priority < temp.left.priority ? temp.left : temp.right) : temp.left;
                    Swap(ref temp, ref iter);
                    temp = iter;
                }
            }

            SearchLast(ref head, 0);    //마지막 노드 찾기
            SearchLatest(ref head, 0);

            /*if(last.left == null)
                latest = last.right;
            else
                latest = last.left;*/

            count--;
            return data;
        }

        public T Peek()
        {
            return head.data;
        }

        /*
            새로 노드가 삽입될 위치의 부모 노드를 찾는 함수입니다.
            후위 순회 방식을 사용하여 차수가 가장 낮으면서 가장 왼쪽에 있는 노드를 last에 삽입합니다.
        */
        void SearchLast(ref Node node, int degree) 
        {
            if(degree == 0) lastDegree = count;       //최저 차수를 찾기위해 lastDegree를 count로 초기화 (노드의 수는 차수보다 무조건 크므로)
            if(node.left != null)   //L
            {
                SearchLast(ref node.left, degree + 1);
            }
            if(node.right != null)  //R
            {
                SearchLast(ref node.right, degree + 1);
            }
            else if(lastDegree > degree)    //D         오른쪽이 비어있으면 빈 노드로 간주(오른쪽이 비었는데 왼쪽이 비어있을수는 없으므로), 가장 차수가 낮아야함.
            {
                lastDegree = degree;
                last = node;
            }
        }
        /*
            가장 최신 노드(즉 Dequeue될 노드를 찾는 연산은 위 SearchLast 함수와 정 반대로 가장 오른쪽의 차수가 가장 높은 노드이므로)
            R L D 순으로 검색
        */
        void SearchLatest(ref Node node, int degree)
        {
            if(degree == 0) lastDegree = 0;
            if(node.right != null)
            {
                SearchLatest(ref node.right, degree + 1);
            }
            if(node.left != null)
            {
                SearchLatest(ref node.left, degree + 1);
            }
            if(lastDegree < degree)
            {
                lastDegree = degree;
                latest = node;
            }
        }
        

        void Swap(ref Node A, ref Node B)
        {
            T tempData = A.data;
            int tempPri = A.priority;

            A.data = B.data;
            A.priority = B.priority;

            B.data = tempData;
            B.priority = tempPri;
        }

        public void Print()
        {
            //printQ(ref head, 0);
            printTree(ref head);
        }
        void printQ(ref Node node, int num)  //DLR
        {
            Debug.Log(num + " : " + node.data);
            if(node.left != null)   //L
            {
                printQ(ref node.left, num + 1);
            }
            if(node.right != null)  //R
            {
                printQ(ref node.right, num + 1);
            }
        }

        void printTree(ref Node head) //인자값으로 주어진 노드를 루트노드로 하여 자식노드들의 구조를 출력 너비우선탐색
        {
            Queue<Node> tempQ = new Queue<Node>();
            tempQ.Enqueue(head);

            string result = "";
            int count = 0, changeDegree = 1;
            while(tempQ.Count != 0) //큐가 빌때까지 반복
            {
                Node temp = tempQ.Dequeue();
                result += temp.data + " ";
                count++;
                if(count == changeDegree)
                {
                    result += "\n";
                    changeDegree *= 2;
                    count = 0;
                }
                if(temp.left != null)
                {
                    tempQ.Enqueue(temp.left);
                }
                if(temp.right != null)
                {
                    tempQ.Enqueue(temp.right);
                }
            }
            Debug.Log(result);
        }

        class Node : NodeClass<T>   //기본 노드에서 우선순위큐를 위해 여러 데이터를 추가한다.
        {   
            public int priority {get; set;}
            public Node pre, left, right;
            public Node(T data, int priority)
            {
                this.data = data;
                this.priority = priority;

                pre = null;
                left = null;
                right = null;
            }
        }
        enum MODE
        {
            GREATER,
            LESS
        }
    }

    public class NodeClass<T>      //기본 노드로 데이터를 가지고 있다.
    {
        public T data {get; set;}
    }

    /// <summary>
    /// Double Ended Queue를 연결리스트를 이용해 간단히 구현
    /// </summary>
    public class DEQ<T>
    {
        LinkedList<T> deq;

        public DEQ()
        {
            deq = new LinkedList<T>();
        }
        public int GetCount()
        {
            return deq.Count;
        }

        public void RearEnqueue(T item)
        {
            deq.AddLast(item);
        }
        public void FrontEnqueue(T item)
        {
            deq.AddFirst(item);
        }
        public T FrontDequeue()
        {
            LinkedListNode<T> node = deq.First;
            deq.RemoveFirst();
            return node.Value;
        }
        public T RearDequeue()
        {
            LinkedListNode<T> node = deq.Last;
            deq.RemoveLast();
            return node.Value;
        }
        public void Clear()
        {
            deq.Clear();
        }
    }
}
