using System.Collections;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;
    [SerializeField] float endScale = 5f;

    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // オブジェクトがアクティブになった瞬間に、自動で波紋アニメーションを開始する
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        float t = 0;

        // 元の色を取得しておき、スケールは波紋の初期状態としてゼロリセットする
        Color color = sprite.color;
        transform.localScale = Vector3.zero;

        // duration で指定された時間が経過するまで、毎フレーム波紋を広げる処理を行う
        while (t < duration)
        {
            // 経過時間を加算し、アニメーションの進行度合い(rate)を0～1の範囲で求める
            t += Time.deltaTime;
            float rate = t / duration;

            // 進行度に合わせてスケールを 0 から endScale まで線形補間し、波紋が広がる動きを作る
            transform.localScale = Vector3.one * Mathf.Lerp(0, endScale, rate);

            // 広がると同時にアルファ値を1から0へ減衰させ、波紋が外側へ消えゆく表現にする
            color.a = 1 - rate;
            sprite.color = color;

            yield return null;
        }

        // 波紋エフェクトが完全に終わったら、不要になったオブジェクトを破棄する
        Destroy(gameObject);
    }
}
