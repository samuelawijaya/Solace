using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private int sceneIndex;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        //Play Animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load Scene 
        SceneManager.LoadScene(sceneIndex);
    }
}
