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


    #region �ε�ȭ���ִ� �ε��(�ε���)
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

        Slider loadingSlider = GameObject.Find("LoadingBar")?.GetComponent<Slider>(); //LoadingProgress��� �̸��� ���� ���ӿ�����Ʈ�� ã�Ƽ� ���� �ƴ϶��?
        AsyncOperation ao = SceneManager.LoadSceneAsync(i);
        Text LoadingText = loadingSlider.GetComponent<Loading>().LoadingText;

        //���ε��� ������ ������ ���� Ȱ��ȭ���� �ʴ´�.
        ao.allowSceneActivation = false;

        //isDone == false -> �ε��� / true ->�ε��� ��
        while (!ao.isDone)
        {
            if (loadingSlider != null)
            { 
                loadingSlider.value = ao.progress + 0.1f;
                LoadingText.text = "�ε���... " + (loadingSlider.value * 100).ToString("N0") + "%";
            }

            if (Mathf.Approximately(ao.progress, 0.9f))
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    #endregion

    #region �ε�ȭ���ִ� �ε��(��Ʈ��)
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

        Slider loadingSlider = GameObject.Find("LoadingBar")?.GetComponent<Slider>(); //LoadingProgress��� �̸��� ���� ���ӿ�����Ʈ�� ã�Ƽ� ���� �ƴ϶��?
        //Text LoadingText = loadingSlider.get
        AsyncOperation ao = SceneManager.LoadSceneAsync(Scene);
        //���ε��� ������ ������ ���� Ȱ��ȭ���� �ʴ´�.
        ao.allowSceneActivation = false;

        //isDone == false -> �ε��� / true ->�ε��� ��
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

    //�ε�ȭ�� ���� �ε��(�ε���)
    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    //�ε�ȭ�� ���� �ε��(��Ʈ��)
    public void LoadScene(string Scene)
    {
        SceneManager.LoadSceneAsync(Scene);
    }

}
