using UnityEngine;

public class RedCoin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 90f;

    void Update() => transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.CollectRedCoin();
        Destroy(gameObject);
    }
}