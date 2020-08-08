using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class planetBehavior : MonoBehaviour {

    private Transform cam;
    private float maxSpeed = 0.000001f;
    private TrailRenderer tr;
    private float radius;
    private float mass;
    private Vector3 p;
    private Vector3 pp;
    private int currentSpeedMeasure;
    int smooth = 30;
    List<float> speeds;
    private float pattern;
    private Renderer rend;
    private Shader shader1;
    private Rigidbody rb;
    private float existence = 0;
    private float minExist = 0.3f;

    public bool anchoredStar = false;

    // Use this for initialization
    void Start ()
    {
        cam    = GameObject.Find("Main Camera").GetComponent<Transform>();
        tr     = this.GetComponent<TrailRenderer>();
        radius = this.GetComponent<SphereCollider>().radius * this.GetComponent<Transform>().localScale.x;
        rb     = this.GetComponent<Rigidbody>();
        mass   = rb.mass;
        p      = this.transform.position;
        pp     = this.transform.position;
        currentSpeedMeasure = 0;
        speeds = new List<float>(new float[smooth]);

        if (anchoredStar)
            tr.time = 0;

        setTexture();
    }

    private Color planetColor()
    {
        float r = utilities.remapRange(radius, variables.planetMinSize, variables.planetMaxSize, 0, 40000);
        return utilities.colorTemperatureToRGB(r);
    }

    
    private void setTexture()
    {
        pattern = Random.Range(0f, 100000f);
        rend = this.GetComponent<Renderer>();
        shader1 = Shader.Find("Standard (Roughness setup)");

        float texScale = utilities.remapRange(Mathf.Pow(radius, 1f / 1.5f), variables.planetMinSize, variables.planetMaxSize, 10, 200);
        
        Texture2D noiseTex = textureGenerator.pinpinTexture(texScale, pattern, texScale/10);
        
        rend.material.mainTexture = noiseTex;
        
        rend.material.shader = shader1;
        rend.material.SetTexture("_ParallaxMap", noiseTex);
        rend.material.SetFloat("_Parallax", 0.5f);
        rend.material.SetTexture("_BumpMap", noiseTex);
        rend.material.SetTexture("_DetailNormalMap", noiseTex);
        rend.material.SetTexture("_DetailMask", noiseTex);
        rend.material.SetColor("_EmissionColor", planetColor());
        rend.material.EnableKeyword("_NORMALMAP");
        rend.material.EnableKeyword("_PARALLAXMAP");
        rend.material.EnableKeyword("_EMISSION");
        
    }
    
    void explosionsAndStuff()
    {
        Object boom = Resources.Load("Explosion");
        Transform bt = new GameObject().transform;
        bt.position = this.transform.position;
        GameObject kaboom = Instantiate(boom, bt) as GameObject;
        kaboom.transform.localScale = this.transform.localScale * 1.5f;
        kaboom.AddComponent<explosionTimer>();
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (variables.collisions)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                float otherMass = collision.gameObject.GetComponent<Rigidbody>().mass;
                if (otherMass >= mass)
                {
                    explosionsAndStuff();
                }
            }
        }
    }

    void collectSpeed()
    {
        float speed = Vector3.Distance(pp, p) * Time.deltaTime;
        speeds[currentSpeedMeasure] = speed;
        currentSpeedMeasure++;
        if (currentSpeedMeasure >= smooth) currentSpeedMeasure = 0;
    }

    private float getAverageSpeed()
    {
        if (speeds.Count > 0)
            return speeds.Average();
        else
            return 0;
    }
    
    void changeTrailColor()
    {
        float speed = getAverageSpeed();
        if (speed > maxSpeed)
        {
            maxSpeed = speed;
        }

        float c = utilities.remapRange(speed, 0, maxSpeed, 0, 1);
        tr.material.color = variables.trailColor(c);
    }

    void changeTrailWidth()
    {
        float camDistance = Vector3.Distance(p, cam.position);
        camDistance = utilities.remapRange(camDistance, 0, 1000, 1, 100);
        tr.widthCurve = new AnimationCurve(new Keyframe(radius*2*camDistance, 0), new Keyframe(0, 1));
    }

    void gravity()
    {
        p = this.transform.position;
        GameObject[] spheres = (GameObject[]) GameObject.FindGameObjectsWithTag("planet");
        float minDist = 1000000000;
        if (anchoredStar)
        {
            this.transform.position = pp;
            rb.angularVelocity = new Vector3(0, 0.5f, 0);
        }
        else
        {
            foreach (var sphere in spheres)
            {
                Vector3 s = sphere.transform.position;

                if (p != s)
                {
                    Vector3 vec = (s - p).normalized; // vector

                    float sr = sphere.GetComponent<SphereCollider>().radius;
                    float dist = Mathf.Pow(Vector3.Distance(s, p) / variables.distanceScale - radius - sr, 2);

                    if (dist < minDist) minDist = dist;

                    float massFactor     = mass * sphere.GetComponent<Rigidbody>().mass;
                    rb.AddForce(vec * massFactor * variables.gravityConstant / dist);
                }

            }
            tr.time = existence >= minExist ? utilities.remapRange(minDist, radius * 1f, radius * 4f, 0, 5) : 0;
        }
        if (variables.flat)
        {
            
            Vector3 flatPos = rb.transform.position;
            flatPos.y = 0;
            rb.transform.position = flatPos;
        }
    }

    private bool paused = false;
    private Vector3 velocity;
    void pauseUnpause()
    {
        if (variables.paused && !paused)
        {
            velocity = rb.velocity;
            rb.isKinematic = true;
            paused = true;
        }
        else if (!variables.paused && paused)
        {
            rb.isKinematic = false;
            rb.velocity = velocity;
            paused = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        existence += existence < minExist ? Time.deltaTime : 0;
        pauseUnpause();
        gravity();
        collectSpeed();
        changeTrailColor();
        pp = this.transform.position;  
    }
}
