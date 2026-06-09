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
        return m_Num;
    }

    public void Spend()
    {
        m_Num--;
    }
}
