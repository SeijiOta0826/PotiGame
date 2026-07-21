using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // 1回のクリックで加算される基本スコア
    public static int clickScore = 10;

    // シーン遷移後もリザルト画面で最終スコアを参照できるよう static で保持する
    public static int mnScore;

    [SerializeField]
    private TMP_Text mScoreText;

    void Start()
    {
        // static フィールドは前シーンの値が残るため、ゲーム開始時に明示的に0へリセットする
        mnScore = 0;
    }

    void Update()
    {
        // 現在のスコアを文字列に変換し、リアルタイムにUIのテキストへ反映させる
        mScoreText.text = "" + mnScore;
    }
}
