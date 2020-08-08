using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetCreator : MonoBehaviour {

    
    void Start ()
    {
        
    }

	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && variables.takeControl)
        {
            createSphere();
        }
    }

    public static void createSphere(bool anchoredStar = false)
    {
        Transform cam = GameObject.Find("Main Camera").GetComponent<Transform>();
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.tag = "planet";

        float s = variables.size;
        sphere.transform.localScale = new Vector3(s,s,s);

        Rigidbody gameObjectsRigidBody = sphere.AddComponent<Rigidbody>();

        float radius = s / 2;
        float volume = 4 * Mathf.PI * Mathf.Pow(radius, 2);
        float mass = variables.density * volume;

        gameObjectsRigidBody.mass = mass;

        gameObjectsRigidBody.useGravity = false;
        gameObjectsRigidBody.transform.rotation = cam.rotation;
        gameObjectsRigidBody.AddForce(cam.forward * variables.birthSpeed * mass);

        TrailRenderer tr = sphere.AddComponent<TrailRenderer>();
        Texture tex = Resources.Load("Default-Particle", typeof(Texture)) as Texture;
        tr.material.SetTexture("_MainTex", tex);
        tr.time = 3;
        tr.widthCurve = new AnimationCurve(new Keyframe(s, 0), new Keyframe(0, 1));

        tr.material.color = variables.trailColor(0);

        Vector3 pos = new Vector3(0, 0, 5 + radius);

        sphere.transform.position = cam.TransformPoint(pos);
        planetBehavior pb = sphere.AddComponent<planetBehavior>();
        pb.anchoredStar = anchoredStar;
    }
}
