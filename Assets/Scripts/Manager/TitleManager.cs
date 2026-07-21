using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private const string GAME_SCENE_NAME = "Game";
    
    // マウス左クリックまたは画面タップに対応するボタン番号
    private const int LEFT_MOUSE_BUTTON = 0;

    private void Start()
    {
        // シーン開始時のフェードイン中に誤ってタップされ、多重遷移するのを防ぐため入力をロックする
        FadeManager.Instance.LockInput();
    }

    void Update()
    {
        // FadeManager側で入力がロックされている期間は、クリック判定を行わない
        if (!FadeManager.Instance.CanInput) return;

        // 画面のどこかがタップまたはクリックされた瞬間を検知する
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            // 次のシーン遷移中に再度クリックされるのを防ぐため、直ちに入力をロックする
            FadeManager.Instance.LockInput();
            
            // ゲームシーンへのフェードアウト遷移を開始する
            FadeManager.Instance.ChangeScene(GAME_SCENE_NAME);
        }
    }
}
