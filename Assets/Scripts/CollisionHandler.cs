
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private float timeDelayed = 3.5f;
    public AudioSource audioSource;
    bool isTransitioning = false;

    [SerializeField] AudioClip rocketCrashClip = null;
    [SerializeField] AudioClip rocketArriveClip = null;
    
    private void Start() {
        isTransitioning = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) {
        if(isTransitioning){return;}
        switch(other.gameObject.tag){
            case "startTag":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;

        }
    }

    void StartCrashSequence(){
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(rocketCrashClip);
        this.GetComponent<Movement>().enabled = false;
        
        Invoke("ReloadScene", timeDelayed);
    }
    void StartSuccessSequence(){
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(rocketArriveClip);        
        this.GetComponent<Movement>().enabled = false;
        
        Invoke("LoadNextScene", timeDelayed);
    }
    void ReloadScene(){
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
    void LoadNextScene(){
        
        int nextSceneIndx = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneIndx == SceneManager.sceneCountInBuildSettings){
            nextSceneIndx = 0;
        }
        SceneManager.LoadScene(nextSceneIndx);
    }

}
