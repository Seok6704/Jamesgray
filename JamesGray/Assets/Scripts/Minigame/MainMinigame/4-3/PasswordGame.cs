using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

/*
미니게임 4-3 관련 문서입니다.
*/

public class PasswordGame : MonoBehaviour
{
    //Dictionary<int, int> pWord = new Dictionary<int, int> (); 딕셔너리로 구성하여, Value 값을 호출할 수 있는 횟수로 하려 했으나, 랜덤성을 부여하는 과정에서 적합하지 않아 반려함.

    List<int> password = new List<int> (); // 비밀번호 리스트 0~9
    int ran; // 랜덤 변수
    public TextMeshProUGUI text; // 입력된 암호 텍스트
    string answer = "36"; // 정답 암호 텍스트
    int now; // 현재 리스트에서 꺼낸 암호
    bool fail;

    void Start()
    {
        SetpWord(); // 리스트 세팅
    }

    public void OnStartClick() // 시작 버튼 클릭 후, 비밀번호 재생 시작
    {
        StartCoroutine("EnterPassword");
    }

    void SetpWord() // 패스워드 세팅, 리스트를 다시 초기화 하기 위해 제작, 초기화 하지 않을 경우, 암호가 랜덤으로 지급되어, 후에 입력하는 암호가 먼저 나올 경우, 입력할 수 없게 됨.
    {
        for(int i = 0; i < 10; i++)
        {
            password.Add(i);
        }
    }

    public void EnterClick() // 입력 버튼 클릭
    {
        if(!fail)
        {
            text.text += now.ToString(); // 현재 암호 텍스트에 입력
            StopCoroutine("EnterPassword");
            SetpWord();
            StartCoroutine("EnterPassword");
        
            if(text.text == answer)
            {
                Debug.Log("정답입니다.");
                StopCoroutine("EnterPassword");
            }
        }
    }

    IEnumerator EnterPassword() // 암호 재생 코루틴
    {
        while(true)
        {
            if(text.text.Length >= 3)
            {
                fail = true;
                Debug.Log("패스워드 길이 초과");
                Debug.Log("실패입니다.");
                StopCoroutine("EnterPassword");
            }
            if(password.Count <= 0)
            {
                fail = true;
                Debug.Log("숫자 끝!");
                Debug.Log("실패하셨습니다.");
                StopCoroutine("EnterPassword");
            }
            else
            {
                ran = Random.Range(0, password.Count - 1);
                Debug.Log(password[ran]);
                now = password[ran];
                password.RemoveAt(ran);
            }
            yield return new WaitForSeconds(2f);
        }
    }

    /*void EnterpWord()
    {
        ran = Random.Range(0, 10);
        if(pWord.Any(item => item.Value.Equals(1)) || pWord.Any(item => item.Value.Equals(2)))
        {
            if(pWord[ran] != 0)
            {
                Debug.Log(ran);
                pWord[ran] -= 1;
            }
            else
            {
                EnterpWord();
            }
        }
        else flag = false;
    }*/


}
