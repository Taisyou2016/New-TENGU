using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour
{
    public Animator playerAnimator;
    public float aniSpeed;
    public PlayerMove playerMove;

    void Start()
    {
        playerAnimator = transform.GetComponent<Animator>();
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (playerAnimator.speed == 0.0f)
        {
            if (playerMove.currentGroundHit)
            {
                playerAnimator.speed = aniSpeed;
            }
        }
    }

    public void AniStop()
    {
        aniSpeed = playerAnimator.speed;
        playerAnimator.speed = 0.0f;
    }
}