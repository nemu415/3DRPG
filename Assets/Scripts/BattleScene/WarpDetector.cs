using UnityEngine;

public class WarpDetector : MonoBehaviour
{
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void LateUpdate()
    {
        // 直前のフレームでは(0,0,0)じゃなかったのに、このフレームで(0,0,0)になった瞬間をキャッチ
        if (transform.position == Vector3.zero && lastPosition != Vector3.zero)
        {
            // コンソールに、この瞬間のプログラムの実行履歴（コールスタック）をすべて吐き出す
            Debug.LogError($"[警告] (0,0,0)へのワープを検知しました！", this);
            Debug.Break(); // ゲームを自動で一時停止（ポーズ）する
        }
        lastPosition = transform.position;
    }
}
