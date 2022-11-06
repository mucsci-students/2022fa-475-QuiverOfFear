using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateLevels : MonoBehaviour
{
    public List<GameObject> scenes;

    void OnEnable()
    {
        foreach (GameObject scene in scenes)
        {
            scene.SetActive(false);
        }
        scenes[0].SetActive(true);
    }

    public void MoveLeft()
    {
        foreach (GameObject scene in scenes)
        {
            if (scene.activeSelf)
            {
                // If first scene (woods), set last scene to active
                if (scenes.IndexOf(scene) == 0)
                    scenes[scenes.Count - 1].SetActive(true);
                // Else set it to the scene before
                else
                    scenes[scenes.IndexOf(scene) - 1].SetActive(true);

                scene.SetActive(false);
                return;
            }
        }
    }

    public void MoveRight()
    {
        foreach (GameObject scene in scenes)
        {
            if (scene.activeSelf)
            {
                // If last scene, set first scene to active
                if (scenes.IndexOf(scene) == scenes.Count - 1)
                    scenes[0].SetActive(true);
                // Else set it to the scene after
                else
                    scenes[scenes.IndexOf(scene) + 1].SetActive(true);


                scene.SetActive(false);
                return;
            }
        }
    }
}
