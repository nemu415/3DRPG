using UnityEngine;

public class CursorText : MonoBehaviour
{
    public void SetPos(Vector2 pos)
    {
        this.transform.localPosition = pos;
        Debug.Log(pos);

    }

    public void CursorMove(float x, float y)
    {
        this.transform.position = new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0);
    }
    
}
