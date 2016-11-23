using UnityEngine;

public class Character : MonoBehaviour {

    private const int MaxFeedLevel = 1;

    private CharacterController controller;

    private int feedLevel;
    private float maxFeedLevelBlinkTime;
    private bool maxFeedLevelBlinkState;

    void Start(){
        controller = GetComponent<CharacterController>();
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector3.Distance(target, transform.position) < 1.5f * GetComponent<CircleCollider2D>().radius)
            {
                if (feedLevel == MaxFeedLevel)
                {
                    feedLevel = 0;
                    GameObject splatter = (GameObject)Instantiate(Resources.Load("prefabs/core/Splatter"));
                    splatter.transform.position = transform.position;
                    StartCoroutine(BoardController.Instance.maskUpdater.AddSplatter(splatter.GetComponent<SpriteRenderer>()));
                }
            }
            else
            {
                controller.SetTarget(target);
            }
        }

        if (feedLevel == MaxFeedLevel && Time.time > maxFeedLevelBlinkTime)
        {
            maxFeedLevelBlinkTime += 0.3f;
            maxFeedLevelBlinkState = !maxFeedLevelBlinkState;
            float colorLevel = maxFeedLevelBlinkState ? 1 - 0.2f * feedLevel : 1;
            GetComponent<SpriteRenderer>().color = new Color(colorLevel, 1, colorLevel);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Target>() != null)
        {
            feedLevel = Mathf.Min(MaxFeedLevel, feedLevel+1);
            float colorLevel = 1 - 0.2f * feedLevel;
            maxFeedLevelBlinkTime = Time.time;
            GetComponent<SpriteRenderer>().color = new Color(colorLevel, 1, colorLevel);
            Destroy(other.gameObject);
        }
    }

}
