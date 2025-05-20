using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform thisTranform;

    private void Start() {
        thisTranform = transform;
    }

    void Update()
    {
        thisTranform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
