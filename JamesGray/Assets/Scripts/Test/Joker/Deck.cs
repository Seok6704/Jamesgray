using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{   
    public UnityEvent CardHasDrawed;
    public GameObject player, com1, com2, com3, card_tmp;
    Sprite[] sprites;
    Vector3 deck_pos;
    bool[] card; //기본적으로 False 로 초기화됨
    Stack<int> deck = new Stack<int>(); //덱 스택을 만듬

    [Header ("Range of Card")]
    [SerializeField] int min = 0;
    [SerializeField] int max = 53;
    void reRange(){     //랜덤함수의 범위 조정
        while(card[min] && min < max) min++;
        while(card[max - 1] && max >= min) max--;
    }
    void shuffle(){  //deck을 셔플하는 함수
        int temp, deck_cnt = 0;
        while(deck_cnt < max){   //덱의 마지막 카드가 결정되면 반복문 종료
            temp = Random.Range(min, max);  //max값은 exclusive됨
            if(!card[temp]) {               //이미 덱에 포함된 카드면 무시, 아니라면 덱에 포함
                card[temp] = true;
                deck.Push(temp);
                deck_cnt++;
                //if(temp == min || temp == max) reRange();   //효율을 높이기 위해 최소값이나 최대값을 만나면 범위 재조정 // 오류가 가끔 등장함 인덱스 오류 체크해볼것
            }
        }
        for(int i = 0; i < 52; i++) card[i] = false;    //다음 셔플을 위해 초기화
        min = 0; max = 52;
    }
    (int, string, Sprite) whatCard(int n){
        if(n <= 12) {
            return (n + 1, "heart", sprites[n]);
        }
        else if(n <= 25) {
            return (n - 12, "clover", sprites[n]);
        }
        else if(n <= 38) {
            return (n - 25, "tri", sprites[n]);
        }
        else {
            if(n == 52) return (-1, "joker", sprites[53]);
            else return (n - 38, "heart", sprites[n]);
        }
    }


    void DrawCard(GameObject owner){
        GameObject c = Instantiate(card_tmp, deck_pos,Quaternion.identity); //씬 내부에 존재하는 카드 객체 복사
        c.transform.name = "Card";
        c.transform.SetParent(owner.transform); //해당 소유자로 상속
        (int n, string type, Sprite img) = whatCard(deck.Pop());    //무슨 카드인지 계산
        c.GetComponent<CardManger>().setCard(n, type, img);
        //ardHasDrawed.Invoke();
        c.GetComponent<CardManger>().Draw();
    }
    private void Awake() {
        sprites = Resources.LoadAll<Sprite>("poker");
        card = new bool[max];
        deck_pos = transform.GetChild(0).position;
        shuffle();
    }

    private void Start() {
        
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            if(deck.Count != 0)
            DrawCard(player);
            else Debug.Log("Deck is Empty");
        }
    }
}
