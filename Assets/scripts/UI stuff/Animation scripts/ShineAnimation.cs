using UnityEngine;
using System.Collections;

public class ShineAnimation : MonoBehaviour {

    public Sprite[] AnimationFrames;
    int frame;
    bool animating;
    public SpriteRenderer sprite;
    public SpriteRenderer sprite2;
    float time = 0;

    void Start()
    {
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in renderers)
        {
            if (rend.gameObject.name == "shine animation") sprite = rend;
            else sprite2 = rend;
        }
    }

    public void Animate()
    {
        animating = true;
        time = Time.time;
    }

    void Update()
    {
        if (animating)
        {
            if (Time.time - time > .03f)
            {
                if ((frame < AnimationFrames.Length - 1))
                {
                    sprite.sprite = AnimationFrames[frame];
                    if (frame > 0) sprite2.sprite = AnimationFrames[frame - 1];
                    time = Time.time;
                    frame++;
                    frame++;
                }
                else
                {
                    animating = false;
                    frame = 0;
                    sprite.sprite = AnimationFrames[0];
                    sprite2.sprite = AnimationFrames[0];
                }
            }
        }
    }
}
