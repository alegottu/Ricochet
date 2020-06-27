using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    [HideInInspector] public LineRenderer wallRender = null;
    private Vector3 wallStart = Vector3.zero;
    private Vector3 wallEnd = Vector3.zero;
    [SerializeField] private Material lineMat = null;
    private EdgeCollider2D wall = null;
    [SerializeField] private int wallMultiplier = 1;
    [SerializeField] private float maxWallLength = 1; //For hardcore only

    private Touch[] touch;
    private bool firstTouch = false;
    private bool letGo = false;
    private bool firstTap = false;
    private bool firstHold = false;

    [SerializeField] private GameObject bullet = null;
    [HideInInspector] public GameObject _bullet;
    private Collider2D col = null;

    public int lives = 3;

    private void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
    }

    private void Update()
    {
        TouchHandler();
        DrawWall();

        if (lives <= 0)
        {
            GameManager.Instance.UnloadLevel();
            GameManager.Instance.LoadLevel("GameOver");
        }
    }

    private void FixedUpdate()
    {
        Shoot(gameObject);
    }

    private void DrawWall()
    {     
        if (firstTap)
        {
            if (wallRender != null)
            {
                Destroy(wallRender.gameObject);
            }
            createLine();

            wallStart = Camera.main.ScreenToWorldPoint(touch[0].position);
            wallStart.z = 0;
            wallRender.SetPosition(0, wallStart);
            wallRender.SetPosition(1, wallStart);
        }
        else if (firstHold && wallRender)
        {
            wallEnd = Camera.main.ScreenToWorldPoint(touch[0].position);
            wallEnd.z = 0;
            wallRender.SetPosition(1, wallEnd);
        }
        else if (letGo)
        {
            if (wallRender)
            {
                if (wall != null)
                {
                    Destroy(wall);
                }

                wallEnd = Camera.main.ScreenToWorldPoint(touch[0].position);
                wallEnd.z = 0;
                wall = wallRender.gameObject.AddComponent<EdgeCollider2D>();
                wall.points = new Vector2[2] { wallStart, wallEnd };
                wallRender.SetPosition(1, wallEnd);

                if (GameManager.Instance.getMode() == GameManager.GameMode.ARCADE)
                {
                    GameManager.Instance.addScore((int)(-Mathf.Abs(wallEnd.x - wallStart.x) + -Mathf.Abs(wallEnd.y - wallStart.y)) * wallMultiplier);
                }

                if (GameManager.Instance.getMode() == GameManager.GameMode.HARDCORE && (Mathf.Abs(wallEnd.x - wallStart.x) + Mathf.Abs(wallEnd.y - wallStart.y) > maxWallLength))
                {
                    lives--;
                }
            }
        }
    }

    private void TouchHandler()
    {
        firstTouch = Input.touchCount > 0 ? true : false;
        touch = firstTouch ? Input.touches : null;
        firstTap = (firstTouch && touch[0].phase == TouchPhase.Began) ? true : false;
        firstHold = (firstTouch && touch[0].phase == TouchPhase.Moved) ? true : false;
        letGo = (firstTouch && touch[0].phase == TouchPhase.Ended) ? true : false;
    }

    private void createLine()
    {
        wallRender = new GameObject("Wall").AddComponent<LineRenderer>();
        wallRender.material = lineMat;
        wallRender.gameObject.layer = 8;
        wallRender.positionCount = 2;
        wallRender.startWidth = 0.1f; wallRender.endWidth = 0.1f;
        wallRender.useWorldSpace = false;
        wallRender.numCapVertices = 1;
    }

    private void Shoot(GameObject target)
    {
        if (_bullet == null)
        {
            _bullet = Instantiate(bullet, target.transform.position, Quaternion.identity);
            col.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        col.isTrigger = false;
    }
}
