using UnityEngine;

public class Circle : MonoBehaviour
{
    public void OnMouseDown()
    {
        // コライダーを即無効化し、消滅アニメーション中の再クリックを物理的にも遮断する
        GetComponent<Collider2D>().enabled = false;
        
        // GameManagerに自身のインスタンスを渡し、処理対象の判定やスコア加算を委ねる
        GameManager.Instance.CircleClicked(this);
    }
}
