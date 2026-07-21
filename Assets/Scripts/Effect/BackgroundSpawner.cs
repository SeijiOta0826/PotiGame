using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject circlePrefab;

    [SerializeField]
    private int createCount = 15;

    private void Start()
    {
        // 規定の数だけループを回して背景の円を生成する
        for (int i = 0; i < createCount; i++)
        {
            // 階層（Hierarchy）上に大量のオブジェクトが散らかるのを防ぐため、自身を親として生成する
            Instantiate(circlePrefab, transform);
        }
    }
}
