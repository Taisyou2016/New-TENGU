using UnityEngine;
using System.Collections;

public class KamaitachiGeneration : MonoBehaviour
{
    public GameObject kamaitachiPrefab;
    public float speed = 10.0f; //かまいたちの速度

    void Start()
    {

    }

    void Update()
    {

    }

    public void KamaitachiGeneration1() //パターン1 → ←
    {
        Vector3 position =
            transform.position
            + transform.forward * 3.0f;

        GameObject kamaitachi = Instantiate(kamaitachiPrefab, position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;

        kamaitachi.GetComponent<Kamaitachi>().SetCollisionScale(new Vector3(5, 1, 2));
        kamaitachi.GetComponent<Kamaitachi>().SetParticleScale(new Vector3(4, 4, 4));
        kamaitachi.GetComponent<Kamaitachi>().Move(transform.forward, speed);
        kamaitachi.transform.Rotate(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        //print(transform.eulerAngles.y + "_" + kamaitachi.transform.eulerAngles.y);
    }

    public void KamaitachiGeneration2() //パターン2 ↓ ↑
    {
        Vector3 position =
            transform.position
            + transform.forward * 3.0f
            + transform.up * 2.0f;

        GameObject kamaitachi = Instantiate(kamaitachiPrefab, position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;

        kamaitachi.GetComponent<Kamaitachi>().SetCollisionScale(new Vector3(5, 1, 2));
        kamaitachi.GetComponent<Kamaitachi>().SetParticleScale(new Vector3(4, 4, 4));
        kamaitachi.GetComponent<Kamaitachi>().Move(transform.forward, speed);
        kamaitachi.transform.Rotate(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90.0f);
    }

    public void KamaitachiGeneration3() //パターン3 ↙ ↗
    {
        Vector3 position =
            transform.position
            + transform.forward * 3.0f
            + transform.up;

        GameObject kamaitachi = Instantiate(kamaitachiPrefab, position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;

        kamaitachi.GetComponent<Kamaitachi>().SetCollisionScale(new Vector3(4, 1, 2));
        kamaitachi.GetComponent<Kamaitachi>().SetParticleScale(new Vector3(3, 3, 3));
        kamaitachi.GetComponent<Kamaitachi>().Move(transform.forward, speed);
        kamaitachi.transform.Rotate(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 45.0f);
    }

    public void KamaitachiGeneration4() //パターン4 ↘ ↖
    {
        Vector3 position =
            transform.position
            + transform.forward * 3.0f
            + transform.up;

        GameObject kamaitachi = Instantiate(kamaitachiPrefab, position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;

        kamaitachi.GetComponent<Kamaitachi>().SetCollisionScale(new Vector3(4, 1, 2));
        kamaitachi.GetComponent<Kamaitachi>().SetParticleScale(new Vector3(3, 3, 3));
        kamaitachi.GetComponent<Kamaitachi>().Move(transform.forward, speed);
        kamaitachi.transform.Rotate(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 45.0f);
    }
}
