using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour
{
    public GameObject lockCursorPrefab;

    private GameObject lockCursor;
    private PlayerMove playerMove;
    private bool playerLockState;

    void Start()
    {
        playerMove = transform.GetComponent<PlayerMove>();
        lockCursor = Instantiate(lockCursorPrefab);
        lockCursor.SetActive(false);
    }

    void Update()
    {
        if (playerLockState = playerMove.GetLockOnInfo() && playerMove.lockEnemy != null)
        {
            lockCursor.SetActive(true);
            lockCursor.transform.position =
                playerMove.lockEnemy.transform.position
                + Vector3.up * 2.0f;
        }
        else
        {
            lockCursor.SetActive(false);
        }
    }
}
