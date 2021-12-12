// FloatingOrigin.cs
// Written by Peter Stirling
// 11 November 2010
// Uploaded to Unify Community Wiki on 11 November 2010
// Updated to Unity 5.x particle system by Tony Lovell 14 January, 2016
// fix to ensure ALL particles get moved by Tony Lovell 8 September, 2016
// URL: http://wiki.unity3d.com/index.php/Floating_Origin

//This version was lightly modified by Victor Bréhault June, 2021
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{//Replace all the objects in the scenes at the center of the world when the camera go further than the threshold, in order to avoid visual bugs

    public float threshold = 100.0f;
    public ForestLayoutGenerator layoutGenerator;

    //LateUpdate() is called after the FixedUpdate() and Update() functions
    void LateUpdate()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;  // This ensures that the position's changes will be in the x-z plan (no changes of altitude = coordinate y)

        if (cameraPosition.magnitude > threshold)
        {

            for (int z = 0; z < SceneManager.sceneCount; z++)  //This for-loop is currently unnecessary because the game only have 1 scene
            {
                foreach (GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects())
                {
                    g.transform.position -= cameraPosition;
                }
            }

            //Update of the script in charge of the appearance of new gameObjects in the scenes 
            Vector3 originDelta = Vector3.zero - cameraPosition;
            layoutGenerator.UpdateSpawnOrigin(originDelta);
        }

    }
}


