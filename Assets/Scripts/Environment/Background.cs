using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Material bg = null;
    [SerializeField] private GameObject cometPrefab = null;
    [SerializeField] private float scrollSpeed = 1;
    [SerializeField] private Vector2 cometCooldownRange = Vector2.zero; // x = minimum, y = maximum

    private void Start()
    {
        StartCoroutine(SpawnComets());
    }

    private void Update()
    {
        bg.mainTextureOffset += new Vector2(0, scrollSpeed * Time.deltaTime);
    }

    private IEnumerator SpawnComets()
    {
        float timer = Random.Range(cometCooldownRange.x, cometCooldownRange.y);

        while (true)
        {
            yield return new WaitForSeconds(timer);
            Instantiate(cometPrefab);
            timer = Random.Range(cometCooldownRange.x, cometCooldownRange.y);
        }
    }

    private void OnDestroy()
    {
        bg.mainTextureOffset = Vector2.zero;
    }
}
