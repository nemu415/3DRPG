using UnityEngine;

public class ItemBase : MonoBehaviour
{
    protected int m_Num;

    private void Start()
    {
        m_Num = 0;
    }

    public int GetNum()
    {
        Debug.Log($"[GetNum] ID: {this.GetInstanceID()} / ”’l: {m_Num}");
        return m_Num;
    }

    public void Spend()
    {
        m_Num--;
        Debug.Log($"[Spend] ID: {this.GetInstanceID()} / c‚è: {m_Num}");
    }
}
