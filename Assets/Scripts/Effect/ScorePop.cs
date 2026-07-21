using System.Collections;
using UnityEngine;

public class ScorePop : MonoBehaviour
{
    public static ScorePop Instance;

    [SerializeField]
    private float maxScale = 1.3f;

    [SerializeField]
    private float duration = 0.15f;

    private RectTransform rectTransform;
    
    // アニメーション終了後にスケールを正確に戻すための基準値を保持する
    private Vector3 defaultScale;

    // 実行中コルーチンの参照を保持し、連打時に前のアニメーションを中断して再スタートする
    private Coroutine popCoroutine;

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        
        // 最初の Awake 段階でのスケールをデフォルト値として記憶しておく
        defaultScale = rectTransform.localScale;
    }

    public void PlayPop()
    {
        // 既にポップアニメーションが実行中であれば、コルーチンを停止させて初期化の準備をする
        if (popCoroutine != null)
        {
            StopCoroutine(popCoroutine);
        }

        // 新しくポップアニメーションを開始し、コルーチンの参照を保持しておく
        popCoroutine = StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        float time = 0f;
        
        // ── 拡大フェーズ ──
        // duration 秒かけて、デフォルトサイズから maxScale 倍までUIを拡大させる
        while (time < duration)
        {
            // 経過時間から0～1の割合(t)を計算し、スケールを線形補間する
            time += Time.deltaTime;
            float t = time / duration;
            
            rectTransform.localScale = Vector3.Lerp(defaultScale, defaultScale * maxScale, t);
            yield return null;
        }

        time = 0f;
        
        // ── 縮小フェーズ ──
        // 再び duration 秒かけて、拡大したサイズからデフォルトサイズへと戻していく
        while (time < duration)
        {
            // 拡大時と同様に割合(t)を計算し、元のスケールに向かって線形補間する
            time += Time.deltaTime;
            float t = time / duration;
            
            rectTransform.localScale = Vector3.Lerp(defaultScale * maxScale, defaultScale, t);
            yield return null;
        }

        // アニメーション完了後、補間の微小な誤差をなくすためスケールをデフォルト値で上書きする
        rectTransform.localScale = defaultScale;
    }
}
