using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Spine.Unity;
using UnityEngine;

public class Character : MonoBehaviour {

    private const int MaxFeedLevel = 2;

    private CharacterController controller;

    private int feedLevel;

    SkeletonAnimation animation;

    void Start(){
        controller = GetComponent<CharacterController>();
        animation = GetComponent<SkeletonAnimation>();
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>().OverlapPoint(new Vector2(target.x, target.y)))
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

        string animationPrefix = feedLevel == 0 ? "small" : feedLevel == 1 ? "mid" : "big";
        string animationSuffix = controller.isOnPosition ? "idle" : "crwal";
        string animationName = animationPrefix + "_" + animationSuffix;
        if (animation.AnimationName != animationName)
            animation.state.SetAnimation(0, animationName, true);

		transform.localScale = new Vector3(controller.isMovingLeft ? 1 : -1, 1, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Target>() != null)
        {
            feedLevel = Mathf.Min(MaxFeedLevel, feedLevel+1);
            Destroy(other.gameObject);
        }
    }

    void Splat()
    {
        Node me = BoardController.Instance.grid.NodeFromWorldPoint(transform.position);
        List<Node> currNeighbors = BoardController.Instance.grid.GetNeighbours(me);
        List<Node> nextNeighbors = new List<Node>();
        List<Node> splatNodes = new List<Node>();
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
                    splatter.transform.localScale = Random.Range(2f, 3f) * Vector3.one;
                    splatter.transform.localRotation = Quaternion.AngleAxis(Random.Range(0f, 180f), Vector3.forward);
                    splatter.GetComponent<SpriteRenderer>().color = new Color(1,1,1,Random.Range(0.5f, 0.9f));
                    nextNeighbors.AddRange(BoardController.Instance.grid.GetNeighbours(node));
                    node.splats.Add(splatter);
                    if (!splatNodes.Contains(node))
                        splatNodes.Add(node);
                    splats++;
                }
            }

            chance *= 0.4f;
            currNeighbors = nextNeighbors;
            nextNeighbors = new List<Node>();
        }

        List<Enemy> remove = new List<Enemy>();
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            Node enemyNode = BoardController.Instance.grid.NodeFromWorldPoint(enemy.transform.position);
            if (splatNodes.Contains(enemyNode))
            {
                remove.Add(enemy);
                Destroy(enemy.gameObject);
            }
        }

        foreach (Enemy enemy in remove)
            BoardController.Instance.enemies.Remove(enemy);

        MasterAudio.PlaySound("Fart");
    }

}
