using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour
{
    public GameObject lockCursorPrefab;
    public float cursorEnemyPosY = 2.5f;
    public float cursorBossPosY = 5.5f;
    public float cursorBossShortPosY = 2.5f;

    private GameObject lockCursor;
    private PlayerMove playerMove;
    private GameObject player;
    private float cursorPosY;
    private Vector3 lookAtPosition;

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
            lookAtPosition = Camera.main.transform.position;

            if (playerMove.lockEnemy.tag == "Enemy"|| playerMove.lockEnemy.tag == "Kakashi")
            {
                cursorPosY = cursorEnemyPosY;
                lookAtPosition.y = cursorPosY;

                lockCursor.transform.position =
                     playerMove.lockEnemy.transform.position
                     + Vector3.up * cursorPosY;
            }
            else if (playerMove.lockEnemy.tag == "Boss")
            {
                if ((player.transform.position - playerMove.lockEnemy.transform.position).magnitude > 5.0f)
                {
                    cursorPosY = cursorBossPosY;
                    lookAtPosition.y = playerMove.lockEnemy.transform.position.y + cursorBossPosY;

                    lockCursor.transform.position =
                         playerMove.lockEnemy.transform.position
                         + Vector3.up * cursorPosY;
                }
                else
                {
                    cursorPosY = cursorBossShortPosY;
                    lookAtPosition.y = playerMove.lockEnemy.transform.position.y + cursorBossShortPosY;

                    lockCursor.transform.position =
                        playerMove.lockEnemy.transform.position
                        + (player.transform.position - playerMove.lockEnemy.transform.position).normalized * 1.5f
                        + Vector3.up * cursorPosY;
                }
            }

            //print(lookAtPosition);
            lockCursor.transform.LookAt(lookAtPosition);
            lockCursor.transform.rotation = Quaternion.Euler(0, lockCursor.transform.eulerAngles.y, lockCursor.transform.eulerAngles.z);
        }
        else
        {
            lockCursor.SetActive(false);
        }
    }
}
