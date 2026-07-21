using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// シーン遷移時のフェードイン・フェードアウトおよび入力ロックを管理するシングルトンクラス。
// DontDestroyOnLoad によって破棄されないため、全シーンを通してUIや入力状態の管理が可能。
public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
  
    [SerializeField]
    private Image fadeImage;
    
    [SerializeField]
    private float inputLockTime = 0.5f;

    private bool canInput = true;

    // TitleManager や ResultManager がタップ入力を受け付けるか判定するために参照するプロパティ
    public bool CanInput => canInput;

    private void Awake()
    {
        // 既にインスタンスが存在する場合は重複を破棄し、シングルトン状態を維持する
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(string sceneName)
    {
        // シーン遷移の一連のフェード処理をコルーチンで非同期に実行する
        StartCoroutine(ChangeSceneCoroutine(sceneName));
    }

    public void LockInput()
    {
        // UIの誤操作を防ぐための一定時間ロック処理を開始する
        StartCoroutine(LockCoroutine());
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        // 画面が完全に黒くなるまでフェードアウトを待機する
        yield return FadeOut(1f);
        
        // フェードアウト状態で対象シーンをロードし、裏で切り替える
        SceneManager.LoadScene(sceneName);
        
        // ロード直後の1フレームを待機し、新しいシーンの初期化が終わるのを待つ
        yield return null;
        
        // 画面が完全に明るくなるまでフェードインを待機する
        yield return FadeIn(1f);
    }

    private IEnumerator FadeOut(float duration)
    {
        float time = 0f;

        // 指定された時間をかけてアルファ値を 0 から 1 へと上げていき、画面を暗転させる
        while (time < duration)
        {
            time += Time.deltaTime;
            
            Color color = fadeImage.color;
            color.a = Mathf.Clamp01(time / duration);
            fadeImage.color = color;
            
            yield return null;
        }
    }
    
    private IEnumerator FadeIn(float duration)
    {
        float time = 0f;

        // 指定された時間をかけてアルファ値を 1 から 0 へと下げていき、画面を明るくする
        while (time < duration)
        {
            time += Time.deltaTime;
            
            Color color = fadeImage.color;
            color.a = Mathf.Clamp01(1f - time / duration);
            fadeImage.color = color;
            
            yield return null;
        }
    }

    private IEnumerator LockCoroutine()
    {
        // 入力を即座に無効化し、指定時間だけ待機した後に再び有効化する
        canInput = false;
        yield return new WaitForSeconds(inputLockTime);
        canInput = true;
    }
}
