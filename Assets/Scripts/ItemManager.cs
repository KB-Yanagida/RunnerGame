using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject itemPrefab; // アイテムのプレハブ
    public Transform player; // プレイヤーのTransform
    public float spawnRate = 5f; // アイテム生成の間隔（秒）
    public float spawnDistance = 20f; // プレイヤーの前方に生成する距離
    public float despawnDistance = 30f; // プレイヤーからこの距離以上離れたアイテムを削除
    public int numberOfItems = 5; // 生成するアイテムの個数
    public float itemSpacing = 2f; // 生成するアイテム間の間隔
    private float nextSpawnTime = 0f; // 次にアイテムを生成する時間

    void Update()
    {
        // アイテムを定期的に生成
        if (Time.time >= nextSpawnTime)
        {
            SpawnItem();
            nextSpawnTime = Time.time + spawnRate;
        }

        // 遠くにあるアイテムを削除
        DespawnItems();
    }

    void SpawnItem()
    {
        if (itemPrefab != null)
        {
            float randomX = Random.Range(-2f, 2f);

            for (int i = 0; i < numberOfItems; i++)
            {
                // Y座標はアイテムプレハブ自体のY座標を使用
                float prefabY = itemPrefab.transform.position.y;
                
                // Z座標はプレイヤーの前方に基づく
                Vector3 spawnPosition = player.position + player.forward * (spawnDistance + i * itemSpacing);
                
                // 生成位置のX座標とY座標を調整
                spawnPosition.x += randomX;
                spawnPosition.y = prefabY;
                
                // アイテムの回転を取得して適用
                Quaternion spawnRotation = itemPrefab.transform.rotation;
                
                // アイテムを生成
                GameObject itemInstance = Instantiate(itemPrefab, spawnPosition, spawnRotation);
                
                // インスタンスにタグを設定
                itemInstance.tag = "Item";
            }
        }
        else
        {
            // itemPrefabがnullの場合の警告を出力
            Debug.LogWarning("itemPrefabが設定されていません。");
        }
    }

    void DespawnItems()
    {
        // シーン内の全アイテムを検索
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (var item in allItems)
        {
            // プレイヤーから一定距離以上離れたアイテムを削除
            if (Vector3.Distance(player.position, item.transform.position) > despawnDistance)
            {
                Destroy(item);
            }
        }
    }
}
