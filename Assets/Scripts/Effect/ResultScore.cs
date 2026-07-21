using System.Collections;
using TMPro;
using UnityEngine;

public class ResultScore : MonoBehaviour
{
    private const int COUNT_START_VALUE = 0;
    private const float PUNCH_SCALE_MULTIPLIER = 1.3f;
    private const float PUNCH_DURATION = 0.1f;
    private const string SCORE_TEXT_PREFIX = "Score : ";

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private float countTime = 1.0f;

    private void Start()
    {
        // リザルトシーン開始直後にスコアのカウントアップ演出を走らせる
        StartCoroutine(CountUp());
    }

    private IEnumerator CountUp()
    {
        // 前のシーンから持ち越された最終スコアを取得する
        int targetScore = ScoreManager.mnScore;
        float timer = 0f;

        // 指定された countTime 秒をかけて、0から最終スコアまで UI 上の数値を増加させる
        while (timer < countTime)
        {
            // 経過時間から進行割合(t)を算出し、スコアを線形補間する
            timer += Time.deltaTime;
            float t = timer / countTime;
            
            // 中間状態の小数を四捨五入して整数スコアとしてUIに表示する
            int score = Mathf.RoundToInt(Mathf.Lerp(COUNT_START_VALUE, targetScore, t));
            scoreText.text = score.ToString();

            yield return null;
        }

        // カウントアップ終了時、ズレを補正するために確実に最終スコアを文字列でセットする
        scoreText.text = SCORE_TEXT_PREFIX + targetScore.ToString();

        // スコアが確定したことを視覚的に強調するため、最後にパンチ(拡大・縮小)エフェクトを再生する
        yield return StartCoroutine(Punch());
    }

    private IEnumerator Punch()
    {
        // アニメーション開始時の基準スケールと、拡大時の最大スケールを定義する
        Vector3 start = Vector3.one;
        Vector3 big   = Vector3.one * PUNCH_SCALE_MULTIPLIER;

        float timer = 0f;
        
        // ── 拡大フェーズ ──
        // 短時間(PUNCH_DURATION)で一気にUIを拡大させる
        while (timer < PUNCH_DURATION)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, big, timer / PUNCH_DURATION);
            yield return null;
        }

        timer = 0f;
        
        // ── 縮小フェーズ ──
        // 拡大フェーズと同等の時間をかけて、元のサイズに勢いよく戻す
        while (timer < PUNCH_DURATION)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(big, start, timer / PUNCH_DURATION);
            yield return null;
        }
    }
}
