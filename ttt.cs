using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    // 使用陣列儲存路徑的關鍵點 (Vector2)
    public Vector2[] pathPoints;
    private int currentIndex = 0;
    
    public float speed = 5f;
    public float jumpArcHeight = 2f; // 控制跳躍時額外隆起的弧度高度
    
    private bool isFinished = false; // 用來記錄是否已經到達終點

    void Update()
    {
        // 如果已經到達終點，或是陣列是空的，就直接停止執行後續程式碼
        if (isFinished || pathPoints == null || pathPoints.Length == 0) return;

        Vector2 target = pathPoints[currentIndex];
        Vector2 currentPos = transform.position;

        // 計算目前點與目標點的距離
        float distance = Vector2.Distance(currentPos, target);

        // 基本移動 (Vector2 平移)
        transform.position = Vector2.MoveTowards(currentPos, target, speed * Time.deltaTime);

        // 處理跳躍的垂直拋物線起伏
        if (target.y > 0.1f || currentPos.y > 0.1f)
        {
            float prevDist = 1f;
            if (currentIndex > 0)
            {
                prevDist = Mathf.Max(Vector2.Distance(pathPoints[currentIndex - 1], target), 0.001f);
            }
            
            float progress = 1f - (distance / prevDist);
            float verticalOffset = Mathf.Sin(progress * Mathf.PI) * jumpArcHeight;
            
            if (progress > 0 && progress < 1)
            {
                Vector2 newPos = transform.position;
                newPos.y = Mathf.Lerp(currentPos.y, target.y, progress) + (verticalOffset * 0.2f);
                transform.position = newPos;
            }
        }

        // 到達目標點後的處理
        if (distance < 0.05f)
        {
            // 檢查是不是還有下一個點
            if (currentIndex < pathPoints.Length - 1)
            {
                currentIndex++; // 還沒到終點，前往下一個點
            }
            else
            {
                isFinished = true; // 已經是最後一個點了，設定為到達終點並停止
                Debug.Log("人物已順利抵達終點！");
            }
        }
    }
}