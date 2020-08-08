using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iniGrav = gravity;
        gravScale = 3;
    }

    public static float planetSize;
    float gravity = variables.gravityConstant;
    float iniGrav;
    float gravScale;
    float density = variables.density;
    public static float speed   = variables.birthSpeed;
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 150, variables.screenY-100));

        GUILayout.Label("Global gravity : " + ((int)(gravity / iniGrav * 100)).ToString() + "%");
        gravity = GUILayout.HorizontalScrollbar(gravity, 0, -iniGrav* gravScale, iniGrav * gravScale);
        variables.gravityConstant = utilities.clamp(gravity, -iniGrav * gravScale, iniGrav * gravScale);

        GUILayout.Label("Planet size : "+((int)(planetSize*10)).ToString()+"Km");
        planetSize = GUILayout.HorizontalScrollbar(planetSize, variables.planetMinSize, variables.planetMinSize, variables.planetMaxSize + variables.planetMinSize);
        variables.size = utilities.clamp(planetSize, variables.planetMinSize, variables.planetMaxSize);

        GUILayout.Label("Planet density : "+((int)(density*100)).ToString());
        density = GUILayout.HorizontalScrollbar(density, 0, 0, 1);
        variables.density = utilities.clamp(density, 0, 3);

        GUILayout.Label("Initial speed : " + ((int)(speed)).ToString() + "Km/h");
        speed = GUILayout.HorizontalScrollbar(speed, 0, 0, 10000);
        variables.birthSpeed = utilities.clamp(speed, 0, 10000);

        if (GUILayout.Button("Create planet"))
        {
            planetCreator.createSphere();
        }

        if (GUILayout.Button("Create anchored planet"))
        {
            planetCreator.createSphere(true);
        }

        if (GUILayout.Button(variables.collisions ? "Collisions : ON" : "Collisions : OFF"))
            variables.collisions = !variables.collisions;

        if (GUILayout.Button(variables.flat ? "2D : ON" : "2D: OFF"))
            variables.flat = !variables.flat;

        if (GUILayout.Button(!variables.paused ? "Pause physics" : "Unpause physics"))
            variables.paused = !variables.paused;

        if (GUILayout.Button("Kill motion"))
        {
            foreach (var sphere in spheres())
            {
                Rigidbody rigidbody = sphere.GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = new Vector3(0, 1, 0);
            }
        }

        if (GUILayout.Button("Clear"))
        {
            foreach (var sphere in spheres())
            {
                Destroy(sphere);
            }
        }

        GUILayout.EndArea();

        /*
        GUILayout.Label("kikou");
        if (GUILayout.Button("Press Me"))
            Debug.Log("Hello!");    
        */
    }

    public static GameObject[] spheres()
    {
        return (GameObject[])GameObject.FindGameObjectsWithTag("planet");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
