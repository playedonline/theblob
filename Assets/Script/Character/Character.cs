using System.Collections.Generic;
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
                    Splat();
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

    void Splat()
    {
        Node me = BoardController.Instance.grid.NodeFromWorldPoint(transform.position);
        List<Node> currNeighbors = BoardController.Instance.grid.GetNeighbours(me);
        List<Node> nextNeighbors = new List<Node>();
        currNeighbors.Add(me);
        int splats = 0;
        float chance = 1;
        while (true)
        {
            if (splats >= 100 || currNeighbors.Count == 0)
                break;
            foreach (Node node in currNeighbors)
            {
                if (Random.value < chance)
                {
                    GameObject splatter = (GameObject)Instantiate(Resources.Load("prefabs/core/Splat" + Random.Range(0,8)));
                    splatter.transform.position = node.worldPosition + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
                    splatter.transform.localScale = Random.Range(0.8f, 1.2f) * Vector3.one;
                    splatter.transform.localRotation = Quaternion.AngleAxis(Random.Range(0f, 180f), Vector3.forward);
                    splatter.GetComponent<SpriteRenderer>().color = new Color(1,1,1,Random.Range(0.5f, 0.9f));
                    nextNeighbors.AddRange(BoardController.Instance.grid.GetNeighbours(node));
                    node.splats.Add(splatter);
                    splats++;
                }
            }

            chance *= 0.5f;
            currNeighbors = nextNeighbors;
            nextNeighbors = new List<Node>();
        }
    }

}
