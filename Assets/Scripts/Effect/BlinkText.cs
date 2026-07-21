using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;

    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
   
    void Update()
    {
        // テキストの現在の色を取得し、アルファ値のみをサイン波で操作する準備をする
        Color color = text.color;
        
        // Mathf.Sin は -1 ～ 1 の値を返すため、+1 して 0 ～ 2 にし、0.5倍して 0 ～ 1 に正規化する
        // これによりアルファ値が滑らかに0と1の間を往復し、点滅表現となる
        color.a = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        
        // 変更したアルファ値を含む Color をテキストに適用して描画を更新する
        text.color = color;
    }
}
