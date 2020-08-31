using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int lives = 3;
    [HideInInspector] public LineRenderer wallRender = null;
    private Vector3 wallStart = Vector3.zero;
    private Vector3 wallEnd = Vector3.zero;
    [SerializeField] private GameObject wallPrefab = null;
    private EdgeCollider2D wall = null;
    [SerializeField] private int wallMultiplier = 1;
    [SerializeField] private float maxWallLength = 1; //For hardcore + arcade only
    [SerializeField] private int maxWalls = 3; //For build only
    private List<GameObject> walls = null;

    private Camera mainCam = null;
    [SerializeField] private EnemySpawn enemySpawn = null;
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private float bulletSpeedFactor = 1;
    private GameObject _bullet;
    private bool allow = true;

    private void Start()
    {
        maxWalls = GameManager.Instance.getMode() == GameManager.GameMode.BUILD ? maxWalls : 1;
        walls = new List<GameObject>();

        mainCam = UIManager.Instance.mainCam;
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    //Create class/object for wall or line
    private void createLine()
    {
        GameObject line = Instantiate(wallPrefab);
        walls.Add(line);
        wallRender = line.GetComponent<LineRenderer>();
        wall = line.GetComponent<EdgeCollider2D>();
    }

    private void Shoot()
    {
        if (!_bullet && allow)
        {
            _bullet = Instantiate(bullet, transform.position, Quaternion.identity);
            _bullet.GetComponent<Bullet>().setSpeed((enemySpawn.speed.x + enemySpawn.speed.y) * bulletSpeedFactor);
        }
    }

#if UNITY_ANDROID || UNITY_IOS
    [HideInInspector] public Touch[] touch;
    private bool firstTouch = false;
    private bool letGo = false;
    private bool firstTap = false;
    private bool firstHold = false;

    private void TouchHandler()
    {
        firstTouch = Input.touchCount > 0 ? true : false;
        touch = firstTouch ? Input.touches : null;
        firstTap = (firstTouch && touch[0].phase == TouchPhase.Began) ? true : false;
        firstHold = (firstTouch && touch[0].phase == TouchPhase.Moved) ? true : false;
        letGo = (firstTouch && touch[0].phase == TouchPhase.Ended) ? true : false;
    }

    private void Update()
    {
        TouchHandler();
        DrawWall();

        if (lives <= 0)
        {
            UIManager.Instance.GameOver();
            allow = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }
    }

    private void DrawWall()
    {     
        if (firstTap)
        {
            if (walls.Count >= maxWalls)
            {
                Destroy(walls[0]);
                walls.Remove(walls[0]);
            }
            createLine();

            wallStart = mainCam.ScreenToWorldPoint(touch[0].position);
            wallStart.z = 0;
            wall.points = new Vector2[2] { wallStart, wallStart };
            wallRender.SetPosition(0, wallStart);
            wallRender.SetPosition(1, wallStart);
        }
        else if (firstHold && wallRender)
        {
            wallEnd = mainCam.ScreenToWorldPoint(touch[0].position);
            wallEnd.z = 0;
            wall.points = new Vector2[2] { wallStart, wallEnd };
            wallRender.SetPosition(1, wallEnd);
        }
        else if (letGo && wallRender)
        {
            wallEnd = mainCam.ScreenToWorldPoint(touch[0].position);
            wallEnd.z = 0;
            wall.points = new Vector2[2] { wallStart, wallEnd };
            wallRender.SetPosition(1, wallEnd);

            if (GameManager.Instance.getMode() == GameManager.GameMode.ARCADE)
            {
                int plus = (int)((-Mathf.Abs(wallEnd.x - wallStart.x) + -Mathf.Abs(wallEnd.y - wallStart.y)) + maxWallLength);

                if (plus < 0)
                {
                    GameManager.Instance.addScore(plus * wallMultiplier);
                    UIManager.Instance.updateScore(wallRender.transform.position, plus);
                }
            }

            if (GameManager.Instance.getMode() == GameManager.GameMode.HARDCORE && ((Mathf.Abs(wallEnd.x - wallStart.x) + Mathf.Abs(wallEnd.y - wallStart.y)) > maxWallLength))
            {
                lives--;
                UIManager.Instance.anim.SetTrigger("rad");
            }
        }
    }

#elif UNITY_STANDALONE || UNITY_EDITOR
    private void Update()
    {
        DrawWall();

        if (lives <= 0)
        {
            UIManager.Instance.GameOver();
            allow = false;
        }      
    }

    private void DrawWall()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (walls.Count >= maxWalls)
            {
                Destroy(walls[0]);
                walls.Remove(walls[0]);
            }
            createLine();

            wallStart = mainCam.ScreenToWorldPoint(Input.mousePosition);
            wallStart.z = 0;
            wall.points = new Vector2[2] { wallStart, wallStart };
            wallRender.SetPosition(0, wallStart);
            wallRender.SetPosition(1, wallStart);
        }
        else if (Input.GetButton("Fire1") && wallRender)
        {
            wallEnd = mainCam.ScreenToWorldPoint(Input.mousePosition);
            wallEnd.z = 0;
            wall.points = new Vector2[2] { wallStart, wallEnd };
            wallRender.SetPosition(1, wallEnd);
        }
        else if (Input.GetButtonUp("Fire1") && wallRender)
        {
            wallEnd = mainCam.ScreenToWorldPoint(Input.mousePosition);
            wallEnd.z = 0;
            wall.points = new Vector2[2] { wallStart, wallEnd };
            wallRender.SetPosition(1, wallEnd);

            if (GameManager.Instance.getMode() == GameManager.GameMode.ARCADE)
            {
                int plus = (int)((-Mathf.Abs(wallEnd.x - wallStart.x) + -Mathf.Abs(wallEnd.y - wallStart.y)) * wallMultiplier + maxWallLength);

                if (plus < 0)
                {
                    GameManager.Instance.addScore(plus);
                    UIManager.Instance.updateScore(wallRender.transform.position, plus);
                }
            }

            if (GameManager.Instance.getMode() == GameManager.GameMode.HARDCORE && (Mathf.Abs(wallEnd.x - wallStart.x) + Mathf.Abs(wallEnd.y - wallStart.y) > maxWallLength))
            {
                lives--;
                UIManager.Instance.anim.SetTrigger("rad");
            }
        }
    }
#endif
}
