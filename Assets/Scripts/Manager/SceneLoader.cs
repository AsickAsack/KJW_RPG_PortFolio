using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    private static SceneLoader instance = null;

    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneLoader>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "SceneLoader";
                    DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<SceneLoader>();
                }
            }
            return instance;
        }
    }


    #region 로딩화면있는 로드씬(인덱스)
    public void Loading_LoadScene(int i)
    {
        
        StartCoroutine(SceneLoading(i));
    }

    IEnumerator SceneLoading(int i)
    {
        yield return SceneManager.LoadSceneAsync("LoadingScene");
        yield return StartCoroutine(Loading(i));

    }

    IEnumerator Loading(int i)
    {

        Slider loadingSlider = GameObject.Find("LoadingBar")?.GetComponent<Slider>(); //LoadingProgress라는 이름을 가진 게임오브젝트를 찾아서 널이 아니라면?
        AsyncOperation ao = SceneManager.LoadSceneAsync(i);
        Text LoadingText = loadingSlider.GetComponent<Loading>().LoadingText;

        //씬로딩이 끝나기 전까진 씬을 활성화하지 않는다.
        ao.allowSceneActivation = false;

        //isDone == false -> 로딩중 / true ->로딩이 끝
        while (!ao.isDone)
        {
            if (loadingSlider != null)
            { 
                loadingSlider.value = ao.progress + 0.1f;
                LoadingText.text = "로딩중... " + (loadingSlider.value * 100).ToString("N0") + "%";
            }

            if (Mathf.Approximately(ao.progress, 0.9f))
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    #endregion

    #region 로딩화면있는 로드씬(스트링)
    public void Loading_LoadScene(string Scene)
    {
        
        StartCoroutine(SceneLoading(Scene));

    }

    IEnumerator SceneLoading(string Scene)
    {
        yield return SceneManager.LoadSceneAsync("LoadingScene");
        yield return StartCoroutine(Loading(Scene));

    }

    IEnumerator Loading(string Scene)
    {

        Slider loadingSlider = GameObject.Find("LoadingBar")?.GetComponent<Slider>(); //LoadingProgress라는 이름을 가진 게임오브젝트를 찾아서 널이 아니라면?
        //Text LoadingText = loadingSlider.get
        AsyncOperation ao = SceneManager.LoadSceneAsync(Scene);
        //씬로딩이 끝나기 전까진 씬을 활성화하지 않는다.
        ao.allowSceneActivation = false;

        //isDone == false -> 로딩중 / true ->로딩이 끝
        while (!ao.isDone)
        {
            if (loadingSlider != null)
                loadingSlider.value = ao.progress + 0.1f;

            if (Mathf.Approximately(ao.progress, 0.9f))
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    #endregion

    //로딩화면 없는 로드씬(인덱스)
    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    //로딩화면 없는 로드씬(스트링)
    public void LoadScene(string Scene)
    {
        SceneManager.LoadSceneAsync(Scene);
    }

}
