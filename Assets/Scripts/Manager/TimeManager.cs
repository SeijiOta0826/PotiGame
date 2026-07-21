using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    // リザルトシーンへ遷移するためのシーン名を定数化
    private const string RESULT_SCENE_NAME = "Result";

    [SerializeField]
    private TMP_Text mTimeText;

    [SerializeField]
    private Image mTimerRing;  

    [SerializeField]
    private float mfTimeLimit = 20.0f;

    private float mfCurrentTime;

    // タイムアップ後の処理を止めるためのガードフラグ
    private bool mbIsGameFinished = false;

    void Start()
    {
        // ゲーム開始時の残り時間を制限時間にセットする
        mfCurrentTime = mfTimeLimit;
        
        // 初期状態の時間をUIテキストに反映させておく
        mTimeText.text = "" + mfCurrentTime;
    }

    void Update()
    {
        // 既にゲームが終了している場合は以降のカウントダウン処理を行わない
        if (mbIsGameFinished) return;

        // 前フレームからの経過時間を減算し、残り時間を更新する
        mfCurrentTime -= Time.deltaTime;
        
        // 残り時間の割合（0〜1）を計算し、円形ゲージの表示に反映させる
        mTimerRing.fillAmount = mfCurrentTime / mfTimeLimit;

        // 残り時間が0以下になった場合、タイムアップ処理へ移行する
        if (mfCurrentTime <= 0f)
        {
            // 時間を0に固定し、ゲーム終了フラグを立てて二重処理を防ぐ
            mfCurrentTime = 0f;
            mbIsGameFinished = true;

            // リザルト画面へのフェード遷移を開始する
            FadeManager.Instance.ChangeScene(RESULT_SCENE_NAME);
            
            // 多重遷移や誤タップを防ぐため、遷移開始と同時に入力をロックする
            FadeManager.Instance.LockInput();
        }

        // 残り0.1秒などの端数を切り上げて「1秒」と表示することで、0秒表示の違和感をなくす
        mTimeText.text = Mathf.CeilToInt(mfCurrentTime) + "s";
    }
}
