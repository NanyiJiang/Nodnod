using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    //An AudioSource object so the music can be played .
    private AudioSource _audioSource;

    //A float array that stores channel 0 of the audio samples.
    private float[] _audioSamplesCh0;

    //A float array that stores channel 1 of the audio samples.
    private float[] _audioSamplesCh1;

    //Only sample channel 0
    public bool MonoChannel = false;

    //A float array that stores the audio samples. Min = 64. Max = 8192.
    public int AudioSamplesSize = 64;

    //Height factor of the cube movement
    public float CubeHeightFactor = 15;

    //Horizontal spacing of the cubes  
    public int CubeSpacing = 10;

    //Filter out noise of the data. (Only create a trail of if the Y position of the main cube is bigger than this value)
    public int NoiseFliter = 2;

    //A reference to the cube prefab ref
    public GameObject CubeRef;

    //A reference to the trailing cube prefab ref
    public GameObject TrailingCubeRef;

    //The length of time the trailing cube will be shown
    public int TrailingCubeDestoryDelay = 10;

    //The transform attached to this game object
    private Transform _transform;

    //An array that stores the transforms of all instantiated cubes
    private Transform[] _cubeTransforms;

    //The velocity that the cubes will drop
    public Vector3 GravityFactor = new Vector3(0.0f, 250.0f, 0.0f);

    void Awake()
    {
        //Get and store a reference to the following attached components:  
        //AudioSource  
        this._audioSource = GetComponent<AudioSource>();
        //Transform  
        this._transform = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start()
    {
        this._audioSamplesCh0 = new float[this.AudioSamplesSize];
        this._audioSamplesCh1 = new float[this.AudioSamplesSize];

        //The cubesTransform array should be initialized with the same length as the samples array  
        this._cubeTransforms = new Transform[this.AudioSamplesSize * 2 - 1];

        this._transform.position = new Vector3(this._transform.position.x - this.AudioSamplesSize * this.CubeSpacing, this._transform.position.y, this._transform.position.z);

        //For each cubeTransforms mirrored
        for (var i = 0; i < this._cubeTransforms.Length; ++i)
        {
            //Create a temporary GameObject, that will serve as a reference to the most recent cloned cube and instantiate a cube placing it at the right side of the previous one
            GameObject tempCube = (GameObject)Instantiate(this.CubeRef, new Vector3(this._transform.position.x + i * this.CubeSpacing, this._transform.position.y, this._transform.position.z), Quaternion.identity);

            //Get the recently instantiated cube transform component
            this._cubeTransforms[i] = tempCube.GetComponent<Transform>();

            //Make the cube a child of the game object
            this._cubeTransforms[i].parent = this._transform;
        }


    }

    // Update is called once per frame
    void Update()
    {
        //Obtain the FFT sample from channel 0 of the frequency bands of the attached AudioSource
        this._audioSource.GetSpectrumData(this._audioSamplesCh0, 0, FFTWindow.BlackmanHarris);

        if (this.MonoChannel == false)
        {
            //Obtain the FFT sample from channel 1 of the frequency bands of the attached AudioSource
            this._audioSource.GetSpectrumData(this._audioSamplesCh1, 1, FFTWindow.BlackmanHarris);
        }

        int midPoint = this.AudioSamplesSize - 1;

        float f0;

        if (this.MonoChannel == false)
        {
            f0 = (_audioSamplesCh0[0] + _audioSamplesCh1[0]) / 2;
        }
        else
        {
            f0 = _audioSamplesCh0[0];
        }

        Vector3 cubePos = GetNewCubePosition(midPoint, 0, f0);

        UpdateCubeTransform(cubePos, midPoint);

        //For each sample get the new position
        for (int i = 1; i < this.AudioSamplesSize; ++i)
        {
            cubePos = GetNewCubePosition(midPoint - i, i, this._audioSamplesCh0[i]);

            UpdateCubeTransform(cubePos, midPoint - i);

            if (this.MonoChannel == false)
            {
                cubePos = GetNewCubePosition(midPoint + i, i, this._audioSamplesCh1[i]);
            }
            else
            {
                cubePos = GetNewCubePosition(midPoint + i, i, this._audioSamplesCh0[i]);
            }

            UpdateCubeTransform(cubePos, midPoint + i);
        }
    }

    Vector3 GetNewCubePosition(int index, int sampleIndex, float sampleFft)
    {
        //Set the cubePos Vector3 to the same value as the position of the corresponding cube. However, set it's Y element according to the current sample.
        return new Vector3(this._cubeTransforms[index].position.x, Mathf.Clamp(sampleFft * (50 + sampleIndex * sampleIndex), 0, 50) * CubeHeightFactor + this._transform.position.y, this._cubeTransforms[index].position.z);
    }

    void UpdateCubeTransform(Vector3 cubePos, int cubeIndex)
    {
        if (TrailingCubeRef == null)
        {
            this._cubeTransforms[cubeIndex].position = cubePos;
            return;
        }

        //If the new cubePos.y is greater than the current cube position  
        if (cubePos.y >= this._cubeTransforms[cubeIndex].position.y)
        {
            //Set the cube to the new Y position  
            this._cubeTransforms[cubeIndex].position = cubePos;

            //Spawn trailling cube
            if (cubePos.y >= NoiseFliter)
            {
                CreateTrailingCube(cubePos);
            }
        }
        else if (this._cubeTransforms[cubeIndex].position.y > 0)
        {
            //The spectrum line is below the cube, make it fall  

            this._cubeTransforms[cubeIndex].position -= this.GravityFactor;
        }
    }

    void CreateTrailingCube(Vector3 cubePos)
    {
        if (TrailingCubeRef == null)
        {
            return;
        }
        //Transform
        Transform cubeTransform = GetComponent<Transform>();

        //Create a temporary GameObject, that will serve as a reference to the most recent cloned cube and instantiate a cube placing it at the right side of the previous one
        GameObject tempCube = (GameObject)Instantiate(this.TrailingCubeRef, new Vector3(cubePos.x, cubePos.y, cubePos.z), Quaternion.identity);

        Destroy(tempCube, TrailingCubeDestoryDelay);
    }
}
