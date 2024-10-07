using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Loading : MonoBehaviour
{
    public GameObject LoadingUI;
    public Image LoadingBar;
    private float fill = 0f;

    public async void AsyncLoad(string levelName) {
        fill = 0f;
        LoadingBar.fillAmount = 0f;

        AsyncOperation loading = SceneManager.LoadSceneAsync(levelName);
        loading.allowSceneActivation = false;
        
        LoadingUI.SetActive(true);

        do {
            await Task.Delay(100);
            fill = loading.progress / 0.9f;
        } while (loading.progress < 0.9f); // This is a wierd thing, but unity loads scene at 90% and at that point its ready
    
        await Task.Delay(1000);

        Time.timeScale = 1;
        loading.allowSceneActivation = true;
    }

    private void FixedUpdate() {
        LoadingBar.fillAmount = Mathf.MoveTowards(LoadingBar.fillAmount, fill, 3 * Time.fixedDeltaTime);
    }
}
