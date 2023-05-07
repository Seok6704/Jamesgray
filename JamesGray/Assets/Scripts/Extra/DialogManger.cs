using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
///////////////
/*      ////!!!!!!!!!!! json 으로 변경하므로 삭제될 예정

        ---------------Dialog 관리 스크립트-----------------

        Npc로 부터 ID값을 받아서 해당 Npc의 이벤트를 총괄하는 스크립트 (아마도)


        Dialog Logic    (<<<< 중    요 !!! >>>>)
    -   
    -   플레이어가 NPC나 사물 (Object Layer로 되어 있는 것들) 과 상호작용을 하면 해당 오브젝트에 있는 NPC Manager 스크립트에서 Dialog Manager 스크립트를 불러옵니다.
    -   따라서 이 스크립트의 실질적 시작점은 SetDialog( ) 입니다. (물론 초기값을 설정하는 Awake()가 먼저는 맞지만...) 
    -   SetDialog() 함수는 NPC로 부터 받아온 변수들을 토대로 어느 이벤트를 출력해야할지 알아내어 해당 다이얼로그와 스프라이트들을 불러옵니다.
    -   그리고 자연스러운 대화 다이얼로그 출력을 위해 코루틴을 사용하여 한글자씩 출력합니다.

    -   < Dialog 작성법 >
    -   아래에 SetDialogContents 라는 함수가 있을 텐데 이미 적혀져있는 다이얼로그를 잘 보고 따라 적으면 됩니다만, 좀 복잡한 로직이므로 설명을 읽어보고 적도록 합시다.
    -   NPC는 고유의 ID값을 가지고 있으며 이 값은 바뀌지 않습니다. 
    -   NPC의 ID값만을 가지고 이벤트를 구성하면 NPC당 한개의 이벤트밖에 못 구현하므로 각 NPC에는 story 변수가 존재합니다. 이 변수를 증가시켜 스토리의 진행을 표현합니다.
    -   그러므로 Dialog 딕셔너리의 인덱스는 ID + story (ID 가 101 이고 story 가 0 이라면 101 + 0 => 1010 <string 연산> 을 따릅니다.) 로 결정되어 해당 키값을 검색하여 대화를 출력합니다.
    -   같은 원리로 스프라이트도 저장됩니다.    각 대화 한줄당 스프라이트도 할당되어야합니다. (다 똑같은 스프라이트일지라도 !)
    -   그리고 대화 끝에 이동할 씬도 저장할 수 있는데 지금은 비활성화되었습니다.
    -   그리고 다른 다이얼로그 형식과 같이 딕셔너리에 저장하면 됩니다!

    -   NPC 다이얼로그를 추가했다면 NPC를 Objcet Layer로 변경하는 것을 잊지 말기!
*/
///////////////

public class DialogManger : MonoBehaviour  
{
    JSONManager js; //Debug


    [Header("GameObject which has TMPro")]
    public GameObject tmp_NpcName;
    public GameObject tmp_Dialog;

    [Header("NPC Image")]
    public Image npcImage;

    public UnityEvent DialogOn, DialogOff;

    Dictionary<int, string[]> dic_Dialogs; //대화를 위한 대화 목록 딕셔너리 선언
    Dictionary<int, int> dic_Dialogs_Num;  //각 이벤트의 얼마의 대화가 있는지 기록
    Dictionary<int, int[]> dic_Sprites; //각 이벤트의 문장마다의 스프라이트 정보
    Dictionary<int, string> dic_Scenes; //각 이벤트의 씬이 있을경우 이곳에 경로 저장

    void Awake() 
    {          //시작부터 비활성화 상태일때 Start로 하면 오류 발생함.
        //js = new JSONManager(SceneManager.GetActiveScene().name);
        dic_Dialogs = new Dictionary<int, string[] >(); 
        dic_Dialogs_Num = new Dictionary<int, int>();
        dic_Sprites = new Dictionary<int, int[]>();
        dic_Scenes = new Dictionary<int, string>();
        SetDialogContents();                        //이 함수에서 이벤트 내용을 저장하시오.
    }

    string SetPath(int ID, int type = 0) 
    {  //Resources 폴더내 에셋 경로를 만들어주는 함수
        string fileName = "Sprites/";
        fileName += ID.ToString();
        fileName += type.ToString();
        //fileName += ".png";   // Resources.Load() 는 뒤의 파일명을 붙이면 동작하지 않음
        //Debug.Log(fileName);
        return fileName;
    }
    string SetPath_4Digit(int code) 
    {       //위 함수와 같은 동작, 스프라이트 이름만 알면됨
        string fileName = "Sprites/";
        fileName += code.ToString();
        return fileName;
    }

    public void SetDialog(string s_name, int ID, int i_Story, bool b_visited) 
    {   //Npc로 부터 불러와질 함수 해당 npc의 ID값 등을 받아온다.
        
        //gameObject.SetActive(true);           //npc로 부터 호출되어지므로 엑티브로 변경 Awake를 제외하면 사실상 이 스크립트의 시작부분

        string temp = ID.ToString() + i_Story.ToString();   //npc ID값과 해당 Npc의 스토리 번호를 조합하여 다이얼로그 인덱스 구성
        int i_dialogNum = int.Parse(temp);                  //한 것을 정수형을 변환         ...     하여 알맞은 다이얼 로그 찾을 수 있도록 함.
        //if(!dic_Dialogs.ContainsKey (i_dialogNum)) Debug.Log("Critical Warnings : No Key!");
        DialogOn.Invoke();

        npcImage.sprite = Resources.Load<Sprite>(SetPath(ID)) as Sprite;    //해당 npc의 스프라이트 가져오기
        tmp_NpcName.GetComponent<TextOutputManager>().PrintDirect(s_name); //해당 npc의 이름을 띄우기

        StartCoroutine(LoadTyping(i_dialogNum));    //한글자씩 출력해주는 코루틴 함수 호출
    }

    IEnumerator WaitKey(int i_dialogNum) 
    {     //마지막 문장을 사라지지 않게 대기하게 해주는 코루틴 함수
        while(true) 
        {
            if(Input.GetKeyDown(KeyCode.Space)) break;
            yield return null;
        }
        //gameObject.SetActive(false);
        //LoadScene(i_dialogNum);
        DialogOff.Invoke();
    }

    IEnumerator LoadTyping(int i_dialogNum) 
    {   //한글자씩 출력하게 해주는 함수
        int index = 0;
        string[] text = new string[dic_Dialogs_Num[i_dialogNum]];
        int[] sprites = new int[dic_Dialogs_Num[i_dialogNum]];
        
        text = dic_Dialogs[i_dialogNum];
        sprites = dic_Sprites[i_dialogNum];

        while(index < text.Length) 
        {
            if(Input.GetKeyDown(KeyCode.Space) || index == 0) {
                tmp_Dialog.GetComponent<TextOutputManager>().Typing(text[index]);
                npcImage.sprite = Resources.Load<Sprite>(SetPath_4Digit(sprites[index++])) as Sprite;
                yield return new WaitForSeconds(0.3f);
            }
            yield return null;
        }
        StartCoroutine(WaitKey(i_dialogNum));
    } 

    void LoadScene(int i_dialogNum) 
    {
        if(dic_Scenes.ContainsKey(i_dialogNum)) SceneManager.LoadScene(dic_Scenes[i_dialogNum]);
    }

    void SetDialogContents() //여기에 다이얼로그 딕셔너리 내용 추가
    {      
        string [] event_101 = new string[] 
        {
            "안녕!!",
            "안녀녀녀녕!!!!",
            "안이앙아아아아아아녕ㄴ!!!!!!!!!!!!!!!!",
            "POP",
            "POP POP",
            "POPOPOPOPOPOPOPOPOPOPOPOPOPOPOP",
        };
        int [] sprite_101 = new int[] 
        {
            1010,
            1011,
            1010,
            1011,
            1010,
            1011
        };
        string scene_101 = "SoundGameScene";  //이동할 씬 이름, 폴더 경로는 필요없음.
        dic_Dialogs_Num.Add(1010, event_101.Length);
        dic_Dialogs.Add(1010, event_101);
        dic_Sprites.Add(1010, sprite_101);
        dic_Scenes.Add(1010, scene_101);
        if(event_101.Length != sprite_101.Length) Debug.Log("Warning! dialogNum & spriteNum dosen't Match!");

        string [] event_102 = new string[]
        {
            "안녕!",
            "테스트"
        };
        int [] sprite_102 = new int[]
        {
            1020,
            1020
        };
        dic_Dialogs_Num.Add(1020, event_102.Length);
        dic_Dialogs.Add(1020, event_102);
        dic_Sprites.Add(1020, sprite_102);
    }
    
    
}
