using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    public float TextureScale = 20.0f;

    public int ScoreMultiplyer = 1;

    public void SetScoreMultiplyer(int scoreMultiplyer)
    {
        this.ScoreMultiplyer = scoreMultiplyer;
        if (scoreMultiplyer == 1)
        {
            ResetMultiplyerEffect();
        }
    }

    // Use this for initialization
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.mainTextureScale = new Vector2(TextureScale, TextureScale);
    }

    // Update is called once per frame
    void Update()
    {
        switch (ScoreMultiplyer)
        {
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    void Level2Effect()
    {

    }

    void Level3Effect()
    {

    }

    void Level4Effect()
    {

    }

    void ResetMultiplyerEffect()
    {

    }
}
