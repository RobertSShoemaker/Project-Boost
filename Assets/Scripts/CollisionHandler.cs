using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    int currentSceneIndex;
    Collider[] colls;

    [SerializeField] float loadDelay = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;


    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        audioSource = GetComponent<AudioSource>();
        colls = GetComponents<BoxCollider>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        DebugLoad();
        DebugCollision();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled) { return; }

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

    //Debug load next level
    void DebugLoad()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    //Disable collision for debugging
    void DebugCollision()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; //toggle collision

            //My original logic is here; much easier way is above in the OnCollisionEnter method
            /*
            //this will toggle between true and false without needing to explicitly change it like commented below
            collisionDisabled = !collisionDisabled; 
            if (!collisionDisabled)
            {
                //this will disable all of the colliders on our object
                foreach (Collider col in colls)
                {
                    col.enabled = false;
                }
                //collisionDisabled = true;
            } 
            else
            {
                //this will enable all of the colliders on our object
                foreach (Collider col in colls)
                {
                    col.enabled = true;
                }
                //collisionDisabled = false;
            }
            */
        }
    }

    //load the next level after a short delay when the player crashes
    //disable controls so that the player can't move after crashing
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
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
