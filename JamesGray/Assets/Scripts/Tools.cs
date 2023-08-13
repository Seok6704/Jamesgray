using System.IO;
using UnityEngine;

/// <summary>
/// 프로그래밍 하면서 자주 쓰이는 도구들 모음.
/// </summary>
namespace Tools
{
    /// <summary>
    /// 에셋번들을 불러오는 클래스, 런타임중에 객체나 데이터를 불러오려면 에셋번들을 사용해야한다고함.
    /// </summary>
    public class AssetBundleManager
    {
        AssetBundle assetBundle;

        public AssetBundleManager(string bundleName)    //객체 생성시 번들이름으로 된 에셋번들 불러오기
        {
            assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, bundleName));
            if(ReferenceEquals(assetBundle, null))
            {
                Debug.Log("Faild to load AssetBundle!");
                return;
            }
        }
        public GameObject LoadAsset(string assetName)   //불러오고 싶은 파일을 게임 오브젝트로 불러옴 
        {
            return assetBundle.LoadAsset<GameObject>(assetName);
        }
    }
}