using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Background : MonoBehaviour
{
    public float TextureScale = 20.0f;

    public int ScoreMultiplyer = 1;

    public GameObject Lvl2EffectsObj;
    public Vector3[] Lvl2EffectsLocations;
    private List<FlareSystem> _level2FlareSystem;

    public GameObject Lvl3EffectsObj;
    public Vector3[] Lvl3EffectsLocations;
    private List<FlareSystem> _level3FlareSystem;

    public GameObject Lvl4EffectsObj;
    public Vector3[] Lvl4EffectsLocations;
    private List <FlareSystem> _level4FlareSystem;

    public float partScale = 0.5f;

    private class FlareSystem
    {
        public GameObject ParticleObject;
        public ParticleSystem[] ParticleSystems;
        public bool IsActive;
    }

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

        SetUpLevel2Effects();
        SetUpLevel3Effects();
        SetUpLevel4Effects();
    }

    // Update is called once per frame
    void Update()
    { 
        switch (ScoreMultiplyer)
        {
            case 2:
                Level2Effects(true);
                break;
            case 4:
                Level3Effects(true);
                break;
            case 8:
                Level4Effects(true);
                break;
            default:
                ResetMultiplyerEffect();
                break;
        }
    }

    void SetUpLevel2Effects()
    {
        if (Lvl2EffectsObj == null)
        {
            return;
        }
        _level2FlareSystem = new List<FlareSystem>();

        foreach (var pos in Lvl2EffectsLocations)
        {
            FlareSystem thisFlareSystem = new FlareSystem();
            thisFlareSystem.ParticleObject = Instantiate(Lvl2EffectsObj, pos, Quaternion.identity) as GameObject;

            thisFlareSystem.ParticleSystems = thisFlareSystem.ParticleObject.GetComponentsInChildren<ParticleSystem>();
            thisFlareSystem.ParticleObject.SetActive(false);
            thisFlareSystem.ParticleObject.transform.parent = this.transform;
            thisFlareSystem.ParticleObject.transform.localScale = new Vector3(partScale, partScale, partScale);
            thisFlareSystem.IsActive = false;

            _level2FlareSystem.Add(thisFlareSystem);
        }
    }

    void SetUpLevel3Effects()
    {
        if (Lvl3EffectsObj == null)
        {
            return;
        }
        _level3FlareSystem = new List<FlareSystem>();

        foreach (var pos in Lvl3EffectsLocations)
        {
            FlareSystem thisFlareSystem = new FlareSystem();
            thisFlareSystem.ParticleObject = Instantiate(Lvl3EffectsObj, pos, Quaternion.identity) as GameObject;

            thisFlareSystem.ParticleSystems = thisFlareSystem.ParticleObject.GetComponentsInChildren<ParticleSystem>();
            thisFlareSystem.ParticleObject.SetActive(false);
            thisFlareSystem.ParticleObject.transform.parent = this.transform;
            thisFlareSystem.ParticleObject.transform.localScale = new Vector3(partScale, partScale, partScale);
            thisFlareSystem.IsActive = false;

            _level3FlareSystem.Add(thisFlareSystem);
        }
    }

    void SetUpLevel4Effects()
    {
        if (Lvl4EffectsObj == null)
        {
            return;
        }
        _level4FlareSystem = new List<FlareSystem>();

        foreach (var pos in Lvl4EffectsLocations)
        {
            FlareSystem thisFlareSystem = new FlareSystem();
            thisFlareSystem.ParticleObject = Instantiate(Lvl4EffectsObj, pos, Quaternion.identity) as GameObject;
            thisFlareSystem.ParticleSystems = thisFlareSystem.ParticleObject.GetComponentsInChildren<ParticleSystem>();
            thisFlareSystem.ParticleObject.SetActive(false);
            thisFlareSystem.ParticleObject.transform.parent = this.transform;
            thisFlareSystem.ParticleObject.transform.localScale = new Vector3(partScale, partScale, partScale);
            thisFlareSystem.IsActive = false;

            _level4FlareSystem.Add(thisFlareSystem);
        }
    }

    void Level2Effects(bool toggle)
    {
        if (Lvl2EffectsObj == null)
        {
            return;
        }

        foreach (FlareSystem fs in _level2FlareSystem)
        {
            UpdateEffects(fs, toggle);
        }
    }

    void Level3Effects(bool toggle)
    {
        if (Lvl3EffectsObj == null)
        {
            return;
        }

        foreach (FlareSystem fs in _level3FlareSystem)
        {
            UpdateEffects(fs, toggle);
        }
    }

    void Level4Effects(bool toggle)
    {
        if (Lvl4EffectsObj == null)
        {
            return;
        }

        foreach (FlareSystem fs in _level4FlareSystem)
        {
            UpdateEffects(fs, toggle);
        }
    }

    void UpdateEffects(FlareSystem fs, bool toggle)
    {
        fs.ParticleObject.SetActive(toggle);

        if (toggle && fs.IsActive == false)
        {
            foreach (ParticleSystem pSys in fs.ParticleSystems)
            {
                pSys.Clear();
                pSys.Play();
            }
        }
        fs.IsActive = toggle;
    }

    void ResetMultiplyerEffect()
    {
        Level2Effects(false);
        Level3Effects(false);
        Level4Effects(false);
    }
}
