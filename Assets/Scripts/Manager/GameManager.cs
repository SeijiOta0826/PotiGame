using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Circle circlePrefab;

    [SerializeField]
    private SpriteRenderer ripplePrefab;

    [SerializeField]
    private SpawnArea mSpawnArea;

    // 現在クリックを受け付ける唯一の円を保持し、消滅中の古い円へのタップを弾く（連打防止）
    private Circle currentActiveCircle;

    private void Awake()
    {
        // 他のクラスから GameManager のインスタンスにアクセスできるようシングルトンとして登録する
        Instance = this;
    }
 
    private void Start()
    {
        // カメラの描画範囲を計算し、円を生成する際の座標計算の基準を初期化する
        CameraArea.Initialize();
        
        // 初回の円を画面内のランダムな位置に生成してゲームを開始する
        SpawnCircle();
    }

    public void CircleClicked(Circle circle)
    {
        // 生成済みオブジェクト（最新の円）以外からのクリックイベントは破棄し、連打バグを防ぐ
        if (currentActiveCircle != circle) return;

        // 次の円が生成されるまでの間、新たなクリックを受け付けないよう参照をクリアする
        currentActiveCircle = null;

        // クリックした位置に波紋エフェクトを生成し、視覚的なフィードバックを与える
        Instantiate(ripplePrefab, circle.transform.position, Quaternion.identity);
        
        // 円自身に消滅エフェクトを実行させ、徐々にフェードアウトしながら削除させる
        circle.GetComponent<CircleEffect>().DestroyEffect();
        
        // 現在のスコアに定数を加算し、結果をスコア管理クラスに反映させる
        ScoreManager.mnScore += ScoreManager.clickScore;
        
        // スコア加算と同時にUIをポップさせて点数が入ったことを強調する
        ScorePop.Instance.PlayPop();
        
        // 前の円の処理が完了したため、次のターゲットとなる新しい円を生成する
        SpawnCircle();
    }
    
    private void SpawnCircle()
    {
        // SpawnArea の設定に基づいて、指定された矩形範囲内からランダムな座標を取得する
        Vector2 pos = mSpawnArea.GetRandomPosition();
        
        // 取得した座標に円のプレハブを生成し、回転はデフォルト状態にする
        Circle newCircle = Instantiate(circlePrefab, pos, Quaternion.identity);
        
        // 新しく生成した円を現在アクティブな円として記録し、次のクリック判定の対象とする
        currentActiveCircle = newCircle;
    }

    private void OnDrawGizmosSelected()
    {
        // 開発用にエディタ上で生成可能エリアの境界線を描画し、視覚的に確認しやすくする
        Gizmos.color = Color.green;
        
        // SpawnArea の中心とサイズを利用して、緑色のワイヤーフレームで矩形を描画する
        Gizmos.DrawWireCube(mSpawnArea.Center, mSpawnArea.Size);
    }
}
