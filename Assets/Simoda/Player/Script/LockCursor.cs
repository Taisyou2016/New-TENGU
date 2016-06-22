using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour
{
    public GameObject lockCursorPrefab;
    public float cursorEnemyPosY = 2.5f;
    public float cursorBossPosY = 5.5f;

    private GameObject lockCursor;
    private PlayerMove playerMove;
    private GameObject player;
    private float cursorPosY;

    void Start()
    {
        playerMove = transform.GetComponent<PlayerMove>();
        player = GameObject.FindGameObjectWithTag("Player");
        lockCursor = Instantiate(lockCursorPrefab);
        lockCursor.SetActive(false);
    }

    void Update()
    {
        if (playerMove.GetLockOnInfo() && playerMove.lockEnemy != null)
        {
            lockCursor.SetActive(true);

            lockCursor.transform.LookAt(player.transform);

            if (playerMove.lockEnemy.tag == "Enemy")
                cursorPosY = cursorEnemyPosY;
            else if (playerMove.lockEnemy.tag == "Boss")
                cursorPosY = cursorBossPosY;

            lockCursor.transform.position =
                playerMove.lockEnemy.transform.position
                + Vector3.up * cursorPosY;
        }
        else
        {
            lockCursor.SetActive(false);
        }
    }
}
