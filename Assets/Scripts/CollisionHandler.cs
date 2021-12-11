using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    int currentSceneIndex;

    [SerializeField] float loadDelay = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    AudioSource audioSource;

    bool isTransitioning = false;


    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    //load the next level after a short delay when the player crashes
    //disable controls so that the player can't move after crashing
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        //todo add particle effect upon crash
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        //todo add particle effect upon success
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadDelay);
    }
    private void ReloadLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    //Load the next level when the rocket reaches the landing pad
    void LoadNextLevel()
    {
        int nextSceneIndex = currentSceneIndex + 1;
        //if we are on the last level, then load the first level
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
