using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private float stayTime = 0f; // 시작시간
    private float requiredTime = 2f; //종료시간

    // 2초동안 머무르면 Player 발사
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stayTime += Time.deltaTime;
            if (stayTime >= requiredTime)
            {
                LaunchPlayer(other.gameObject);
            }
        }
    }

    // 나가면 머무른시간 초기화
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) stayTime = 0f;
    }
    
    // 플레이어의 rigidbody 가져와서 발사.
    private void LaunchPlayer(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 launchDir = new Vector3(-1, 1, 0);
            float power = 500f;

            rb.velocity = Vector3.zero;
            rb.AddForce(launchDir.normalized * power, ForceMode.Impulse);
            // 이러면 -1, 1방향으로 같은 힘으로 움직여야 하는 거 아닌가? 왜 x가 이상하지
        }
    }
}
