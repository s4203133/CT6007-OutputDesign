using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2 position;
    public int width;
    public int length;
    public int id;

    public void Init(Vector2 pos, int w, int l) {
        position = pos;
        width = w;
        length = l;
    }
}
