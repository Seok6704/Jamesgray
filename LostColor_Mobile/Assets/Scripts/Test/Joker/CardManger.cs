using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManger : MonoBehaviour
{
    [Header("CARD Info")]
    [SerializeField] int num;
    [SerializeField] string type;
    [Header("Move Speed")]
    public float speed = 10;

    public Sprite back_img;
    Sprite img;
    Vector3 pos;

    public void setCard(int n, string type, Sprite img){
        num = n; this.type = type; this.img = img;
    }

    void Awake() {
        if(transform.name == "Card_tmp"){
            num = 0; type = "temp_card";
            img = back_img;
            pos = transform.position;
        }
    }

    

    public void Draw() {
        if(transform.parent.name != "Player") return;
        pos = transform.parent.GetChild(0).GetChild(2).position;
        transform.GetComponent<SpriteRenderer>().sprite = img;
    }

    public void Drawed(){   
        if(transform.parent.name != "Player") return;
        int count = transform.parent.childCount;
        pos = transform.parent.GetChild(0).GetChild(count - 2).position;
        transform.GetComponent<SpriteRenderer>().sprite = img;
    }
    private void Update() {
        if(transform.position != pos) {
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        }
    }
}
