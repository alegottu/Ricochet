using UnityEngine;

public class Background : MonoBehaviour
{
    private Material bg = null;
    private float speed = 1;

    [SerializeField] private GameObject comet = null;
    Vector3 cometMaxScale = Vector3.one;
    [SerializeField] private float scaleDivisorMax = 1;
    [SerializeField] private float scaleDivisorMin = 0;
    private float timer = 0;
    [SerializeField] private float timerMax = 1;
    [SerializeField] private float timerMin = 0;

    private void Start()
    {
        bg = gameObject.GetComponent<Renderer>().material;

        cometMaxScale = comet.GetComponent<Transform>().localScale;
        timer = Random.Range(timerMin, timerMax);
    }

    private void Update()
    {
        bg.mainTextureOffset += new Vector2(0, speed);
        speed = GameManager.Instance.bgSpeed;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            gameObject.GetComponent<Animator>().SetTrigger("comet");
        }
    }

    private void CometEnd()
    {
        timer = Random.Range(timerMin, timerMax);
        comet.transform.localScale = cometMaxScale;
    }

    private void CometStart()
    {
        //float amountX = GameManager.Instance.bounds.position.x + GameManager.Instance.bounds.localScale.x * Random.Range(-0.5f, 0.5f);
        //float amountY = GameManager.Instance.bounds.position.x + GameManager.Instance.bounds.localScale.x * Random.Range(-0.5f, 0.5f);
        //comet.transform.position = new Vector3(amountX, amountY);
        comet.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));

        float scaleDivisor = Random.Range(scaleDivisorMin, scaleDivisorMax);
        comet.transform.localScale = new Vector3(cometMaxScale.x / scaleDivisor, cometMaxScale.y / scaleDivisor);
    }
}
