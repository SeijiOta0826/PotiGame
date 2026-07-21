using UnityEngine;

// Inspectorで円の生成エリアを直感的に設定・確認できるようにするためのシリアライズ可能なクラス
[System.Serializable]
public class SpawnArea
{
    [SerializeField]
    private Vector2 mMin;

    [SerializeField]
    private Vector2 mMax;

    public Vector2 GetRandomPosition()
    {
        // インスペクタで指定された矩形範囲（最小座標～最大座標）の中からランダムな座標を算出する
        return new Vector2(
            Random.Range(mMin.x, mMax.x),
            Random.Range(mMin.y, mMax.y)
        );
    }

    public bool Contains(Vector2 position)
    {
        // 指定された座標が設定エリアの矩形領域内に完全に収まっているかどうかを判定する
        return position.x >= mMin.x &&
               position.x <= mMax.x &&
               position.y >= mMin.y &&
               position.y <= mMax.y;
    }

    public Vector2 Min => mMin;
    
    public Vector2 Max => mMax;
    
    // 中心座標を計算して返す。Gizmo の描画などでワイヤーキューブを配置する際に利用する
    public Vector2 Center => (mMin + mMax) * 0.5f;
    
    // エリアの幅と高さを計算して返す。これも Gizmo の描画サイズとして利用する
    public Vector2 Size => mMax - mMin;
}
