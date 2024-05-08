using System.Collections.Generic;
using UnityEngine;

public class GenerateManager : MonoBehaviour
{
    public GameObject groundPrefab; // 床のプレハブへの参照
    public GameObject starsPrefab;  // アイテム(星5個)
    public GameObject[] spikesPrefab = new GameObject[6];  // トゲ
    public Transform playerTransform; // プレイヤーのTransformへの参照
    private Queue<GameObject> groundQueue = new Queue<GameObject>(); // 床オブジェクトを管理するキュー
    private Queue<GameObject> starsQueue = new Queue<GameObject>(); // 星オブジェクトを管理するキュー
    private Queue<GameObject> spikeQueue = new Queue<GameObject>(); // 星オブジェクトを管理するキュー
    public int initialGroundCount = 3; // 初期に生成する床の数
    public float groundLength = 60f; // 各床の長さ(z軸)
    public float starsLength = 20.0f; // 星の長さ(z軸)
    public float numberOfStars = 10; // 星の個数
    public float numberOfSpike = 5; // トゲの個数

    private void Start()
    {
        InitializeGround();
        InitializeStars();
        InitializaSpike();
    }

    private void Update()
    {
        // プレイヤーが2個目の床を超えたかどうかをチェック
        if (playerTransform.position.z > groundQueue.ToArray()[1].transform.position.z)
        {
            MoveFirstGroundToLast();
        }
        if (playerTransform.position.z > starsQueue.ToArray()[1].transform.position.z)
        {
            MoveFirstStarsToLast();
        }
        if (playerTransform.position.z > spikeQueue.ToArray()[1].transform.position.z)
        {
            MoveFirstSpikesToLast();
        }
    }

    void InitializeGround()
    {
        GameObject[] existingGrounds = GameObject.FindGameObjectsWithTag("Ground"); // シーン上にある床オブジェクトを検索
        foreach (GameObject ground in existingGrounds)
        {
            groundQueue.Enqueue(ground); // 既存の床オブジェクトをキューに追加
        }

        // 必要な数だけ床オブジェクトを生成してキューに追加
        for (int i = groundQueue.Count; i < initialGroundCount; i++)
        {
            GameObject newGround = Instantiate(
                groundPrefab,
                new Vector3(0, groundPrefab.transform.position.y, i * groundLength),
                Quaternion.identity
            );
            newGround.tag = "Ground"; // 新しい床オブジェクトにタグを設定
            groundQueue.Enqueue(newGround); // キューに新しい床オブジェクトを追加
        }
    }

    void MoveFirstGroundToLast()
    {
        GameObject firstGround = groundQueue.Dequeue(); // キューの最初の床を取り出す

        // キューの最後の床を参照するためにキューの最後の要素を取得
        GameObject lastGround = groundQueue.ToArray()[groundQueue.Count - 1];

        // 最初の床を最後に移動、Z方向に再配置
        firstGround.transform.position = new Vector3(
            0, 
            lastGround.transform.position.y, 
            lastGround.transform.position.z + groundLength
        );

        groundQueue.Enqueue(firstGround); // キューの最後に追加
    }

    void InitializeStars()
    {
        GameObject[] existingStars = GameObject.FindGameObjectsWithTag("Stars");
        foreach (GameObject stars in existingStars)
        {
            starsQueue.Enqueue(stars);
        }

        // アイテム（星）を生成
        for (int i = starsQueue.Count; i < numberOfStars; i++)
        {
            float randomX = Random.Range(-3.2f, 1.6f);

            GameObject newStars = Instantiate(
                starsPrefab,
                new Vector3(
                    randomX, 
                    starsPrefab.transform.position.y, 
                    i * starsLength
                ),
                Quaternion.identity
            );
            newStars.tag = "Stars";
            starsQueue.Enqueue(newStars);
        }
    }

    void SetRenderersEnabled(GameObject obj, bool enabled)
{
    // Rendererコンポーネントがある全ての子要素を取得しそれらの表示状態を変更
    foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
    {
        renderer.enabled = enabled;
    }
}

    void MoveFirstStarsToLast()
    {
        GameObject firstStars = starsQueue.Dequeue();
        GameObject lastStars = starsQueue.ToArray()[starsQueue.Count - 1];

        float randomX = Random.Range(-3f, 1.5f);

        firstStars.transform.position = new Vector3(
            randomX, 
            lastStars.transform.position.y, 
            lastStars.transform.position.z + starsLength
        );

        // firstStarsの子要素のRendererを有効
        SetRenderersEnabled(firstStars, true);

        starsQueue.Enqueue(firstStars);
    }

    void InitializaSpike()
    {
        GameObject[] existingSpike = GameObject.FindGameObjectsWithTag("Spikes");
        foreach (GameObject spike in existingSpike)
        {
            spikeQueue.Enqueue(spike);
        }

        // トゲを生成
        for (int i = spikeQueue.Count; i < numberOfSpike; i++)
        {
            int index = Random.Range(0, 5);

            GameObject newSpike = Instantiate(
                spikesPrefab[index],
                new Vector3(
                    spikesPrefab[index].transform.position.x, 
                    spikesPrefab[index].transform.position.y, 
                    i * 20.0f
                ),
                Quaternion.identity
            );
            newSpike.tag = "Spikes";
            spikeQueue.Enqueue(newSpike);
        }
    }

    void MoveFirstSpikesToLast()
    {
        GameObject firstSpikes = spikeQueue.Dequeue();
        GameObject lastSpikes = spikeQueue.ToArray()[spikeQueue.Count - 1];

        firstSpikes.transform.position = new Vector3(
            lastSpikes.transform.position.x,
            lastSpikes.transform.position.y, 
            lastSpikes.transform.position.z + 25.0f
        );

        spikeQueue.Enqueue(firstSpikes);
    }
}
