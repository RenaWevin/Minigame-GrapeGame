
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    void Start() {
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("Main");
    }

}
