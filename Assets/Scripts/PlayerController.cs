using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4.0f; // プレイヤーの移動速度
    public float moveSpeedForward = 4.0f; // プレイヤーの前進速度

    void Update()
    {
        // 左右の移動
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(moveHorizontal, 0, 0);

        // 常に前進させる
        transform.Translate(0, 0, moveSpeedForward * Time.deltaTime);

        // タッチ入力（スクリーンの左右で移動）
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    // 画面の左半分をタッチした時、左に移動
                    transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
                }
                else
                {
                    // 画面の右半分をタッチした時、右に移動
                    transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                }
            }
        }

        // プレイヤーのx座標を制限
        float clampedX = Mathf.Clamp(transform.position.x, -1.8f, 3.8f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Star")
        {
            Renderer renderer = other.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
    }
}
