using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class CinemachineVCam : MonoBehaviour
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    public static CinemachineVirtualCamera acvtiveCamera = null;

    public static bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == acvtiveCamera;
    }

    public static void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        acvtiveCamera = camera;

        foreach (var c in cameras)
        {
            if (c != camera && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static IEnumerator Shake()
    {
        Vector3 originalPos = acvtiveCamera.transform.position;
        float elapsed = 0;

        while (elapsed < 0.5f)
        {
            float x = Random.Range(-0.01f, 0.01f);
            float y = Random.Range(-0.01f, 0.01f);

            acvtiveCamera.transform.position = new Vector3(acvtiveCamera.transform.position.x + x,acvtiveCamera.transform.position.y + y, acvtiveCamera.transform.position.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        acvtiveCamera.transform.position = originalPos;
    }

    public static void Register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }

    public static void Unregister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
    }
}
