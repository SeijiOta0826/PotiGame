using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "Title";
    
    // マウス左クリックまたは画面タップに対応するボタン番号
    private const int LEFT_MOUSE_BUTTON = 0;

    private void Start()
    {
        // リザルト画面表示直後のフェードイン中にタップされ、タイトルへ即座に戻ってしまうのを防ぐ
        FadeManager.Instance.LockInput();
    }

    private void Update()
    {
        // FadeManager側で入力が許可されていない間は、画面のタップ入力を無視する
        if (!FadeManager.Instance.CanInput) return;

        // 画面のどこかがタップまたはクリックされた瞬間を検知する
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            // 遷移中に連打されてコルーチンが複数走るのを防ぐため、直ちに入力をロックする
            FadeManager.Instance.LockInput();
            
            // タイトルシーンへのフェードアウト遷移を開始する
            FadeManager.Instance.ChangeScene(TITLE_SCENE_NAME);
        }
    }
}
