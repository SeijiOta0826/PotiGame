using System.Collections;
using UnityEngine;

public class CircleEffect : MonoBehaviour
{
    // 消滅時に元のスケールからどれだけ拡大するかを決定する倍率
    private const float DESTROY_SCALE_MULTIPLIER = 1.4f;

    [SerializeField]
    private float destroyTime = 0.15f;
    
    private SpriteRenderer spriteRenderer;

    // 消滅処理の二重起動を防ぐためのガードフラグ
    private bool isDestroying;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DestroyEffect()
    {
        // 既に消滅コルーチンが走っている場合は重複実行を防ぐ
        if (isDestroying) return;

        // 消滅フラグを立ててからフェードアウトのコルーチンを開始する
        isDestroying = true;
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        // アニメーション開始時のスケールと、目標とする拡大スケールを計算する
        Vector3 startScale = transform.localScale;
        Vector3 endScale   = startScale * DESTROY_SCALE_MULTIPLIER;

        // startColor を先に取得し、色相・彩度を保ったままアルファ値のみを変化させる準備をする
        Color startColor = spriteRenderer.color;
        
        float timer = 0f;

        // 設定された消滅時間を経過するまで毎フレーム補間処理を繰り返す
        while (timer < destroyTime)
        {
            // 前フレームからの経過時間を加算し、0～1の進行割合(t)を算出する
            timer += Time.deltaTime;
            float t = timer / destroyTime;

            // スケールを開始値から終了値に向かって線形補間し、徐々に拡大させる
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            // アルファ値を1(不透明)から0(透明)に向かって補間し、徐々にフェードアウトさせる
            Color color = startColor;
            color.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = color;

            yield return null;
        }

        // アニメーションが完全に終了した後、このオブジェクト自体を破棄する
        Destroy(gameObject);
    }
}
