using UnityEngine;

// カメラ範囲をワールド座標で保持し、円の生成範囲などの計算に利用する静的ユーティリティ。
// 依存関係として、ゲーム開始時に GameManager.Start() から Initialize() が呼ばれることを前提とする。
public static class CameraArea
{
    public static float Left;
    public static float Right;
    public static float Top;
    public static float Bottom;

    public static void Initialize()
    {
        Camera cam = Camera.main;

        // ビューポート（画面の左下が0,0、右上が1,1の正規化座標）をワールド空間の座標に変換する
        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1f, 1f));

        // 変換したワールド座標のXY成分から、画面の上下左右の境界値をそれぞれの変数にキャッシュする
        Left   = min.x;
        Right  = max.x;
        Bottom = min.y;
        Top    = max.y;
    }
}
