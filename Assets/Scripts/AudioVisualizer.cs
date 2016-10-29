using UnityEngine;
using System.Collections;

public class AudioVisualizer : MonoBehaviour
{
    //An AudioSource object so the music can be played 
    private AudioSource _aSource;

    //A float array that stores the audio samples
    private float[] _audioSamples;

    //A float array that stores the audio samples
    public int AudioSamplesSize = 64;

    public float CubeHeightFactor = 15;

    public int NoiseFliter = 2;

    public int CubeSpacing = 10;

    //A reference to the cube prefab
    public GameObject CubeRef;

    //A reference to the secondary cube prefab
    public GameObject SecondaryCubeRef;

    public int DestoryDelay = 10;

    //The transform attached to this game object
    private Transform _goTransform;

    //An array that stores the transforms of all instantiated cubes
    private Transform[] _cubeTransforms;

    //The velocity that the cubes will drop
    public Vector3 GravityFactor = new Vector3(0.0f,2.0f,0.0f);

    void Awake()
    {
        //Get and store a reference to the following attached components:  
        //AudioSource  
        this._aSource = GetComponent<AudioSource>();
        //Transform  
        this._goTransform = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start ()
    {
        this._audioSamples = new float[AudioSamplesSize];

        //The cubesTransform array should be initialized with the same length as the samples array  
        this._cubeTransforms = new Transform[this._audioSamples.Length*2-1];

        this._goTransform.position = new Vector3(this._goTransform.position.x - AudioSamplesSize * CubeSpacing, this._goTransform.position.y, this._goTransform.position.z);

        //For each cubeTransforms mirrored
        for (var i = 0; i < this._cubeTransforms.Length; ++i)
        {
            //Create a temporary GameObject, that will serve as a reference to the most recent cloned cube and instantiate a cube placing it at the right side of the previous one
            GameObject tempCube = (GameObject)Instantiate(this.CubeRef, new Vector3(this._goTransform.position.x + i * CubeSpacing, _goTransform.position.y, _goTransform.position.z), Quaternion.identity);
            //Get the recently instantiated cube transform component
            this._cubeTransforms[i] = tempCube.GetComponent<Transform>();
            //Make the cube a child of the game object
            this._cubeTransforms[i].parent = this._goTransform;
        }


    }

    // Update is called once per frame
    void Update ()
    {
	    //Obtain the FFT sample from the frequency bands of the attached AudioSource
        this._aSource.GetSpectrumData(this._audioSamples, 0, FFTWindow.BlackmanHarris);

        int midPoint = AudioSamplesSize - 1;

        Vector3 cubePos = new Vector3();

        cubePos.Set(this._cubeTransforms[midPoint].position.x, Mathf.Clamp(this._audioSamples[0] * (50), 0, 50) * CubeHeightFactor, this._cubeTransforms[midPoint].position.z);

        //If the new cubePos.y is greater than the current cube position  
        if (cubePos.y >= this._cubeTransforms[midPoint].position.y)
        {
            //Set the cube to the new Y position  
            this._cubeTransforms[midPoint].position = cubePos;
            if (cubePos.y >= NoiseFliter)
            {
                CreateSecondaryCube(cubePos);
            }
        }
        else
        {
            //The spectrum line is below the cube, make it fall  
            this._cubeTransforms[midPoint].position -= this.GravityFactor;
        }

        //For each sample  
        for (int i = 1; i < this._audioSamples.Length; ++i)
        {
            /*Set the cubePos Vector3 to the same value as the position of the corresponding 
             * cube. However, set it's Y element according to the current sample.*/
            cubePos.Set(this._cubeTransforms[midPoint+i].position.x, Mathf.Clamp(this._audioSamples[i] * (50 + i * i), 0, 50) * CubeHeightFactor, this._cubeTransforms[midPoint+i].position.z);
            //If the new cubePos.y is greater than the current cube position  
            if (cubePos.y >= this._cubeTransforms[midPoint+i].position.y)
            {
                //Set the cube to the new Y position  
                this._cubeTransforms[midPoint+i].position = cubePos;
                if (cubePos.y >= NoiseFliter)
                {
                    CreateSecondaryCube(cubePos);
                }
            }
            else if (this._cubeTransforms[midPoint + i].position.y > 0)
            {
                //The spectrum line is below the cube, make it fall  

                this._cubeTransforms[midPoint + i].position -= this.GravityFactor;
            }

            cubePos.Set(this._cubeTransforms[midPoint - i].position.x, Mathf.Clamp(this._audioSamples[i] * (50 + i * i), 0, 50) * CubeHeightFactor, this._cubeTransforms[midPoint - i].position.z);
            //If the new cubePos.y is greater than the current cube position  
            if (cubePos.y >= this._cubeTransforms[midPoint - i].position.y)
            {
                //Set the cube to the new Y position  
                this._cubeTransforms[midPoint - i].position = cubePos;
                if (cubePos.y >= NoiseFliter)
                {
                    CreateSecondaryCube(cubePos);
                };
            }
            else if (this._cubeTransforms[midPoint - i].position.y > 0)
            {
                //The spectrum line is below the cube, make it fall  
                this._cubeTransforms[midPoint - i].position -= this.GravityFactor;
            }

            /*Set the position of each vertex of the line based on the cube position. 
             * Since this method only takes absolute World space positions, it has 
             * been subtracted by the current game object position.*/
            //_lineRenderer.SetPosition(i, this._cubePos - this._goTransform.position);
        }
    }


    void CreateSecondaryCube(Vector3 cubePos)
    {
        //Transform
        Transform cubeTransform = GetComponent<Transform>();

        //Create a temporary GameObject, that will serve as a reference to the most recent cloned cube and instantiate a cube placing it at the right side of the previous one
        GameObject tempCube = (GameObject)Instantiate(this.SecondaryCubeRef, new Vector3(cubePos.x, cubePos.y, cubePos.z), Quaternion.identity);

        //Get the recently instantiated cube transform component
        //Transform secondaryCubeTransform = secondaryCubeTransform = tempCube.GetComponent<Transform>();

        //Make the cube a child of the game object
        //secondaryCubeTransform.parent = cubeTransform;

        Destroy(tempCube, DestoryDelay);
    }
}
