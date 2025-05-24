using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float timer;

    void Start() {
        timer = 0;
    }

    void FixedUpdate() {
        timer += Time.fixedDeltaTime;
        if(timer > 500) {
            CastleTarget.OnSuccess?.Invoke();
            SceneManager.LoadScene(0);
        }
    }
}
