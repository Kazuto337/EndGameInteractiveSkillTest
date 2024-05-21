using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    [SerializeField] HouseBehavior roofDetected;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private float _rayLength = 100f;

    private void Update()
    {
        PerformRaycast();
    }

    private void PerformRaycast()
    {
        if (_camera == null || _player == null)
        {
            Debug.LogWarning("Camera or Player is not assigned.");
            return;
        }

        Vector3 direction = (_player.position - _camera.transform.position).normalized;
        Ray ray = new Ray(_camera.transform.position, direction);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * _rayLength, Color.yellow);

        if (Physics.Raycast(ray, out hit, _rayLength))
        {
            switch (hit.collider.tag)
            {
                case "Roof":
                    roofDetected = hit.collider.GetComponent<HouseBehavior>();
                    roofDetected.TurnOffRoof();
                    break;
                case "Player":
                    if (roofDetected != null)
                    {
                        roofDetected.TurnOnRoof();
                        roofDetected = null;
                        break;
                    }                   
                    break;
                default:
                    break;
            }
            Debug.Log("Hit object: " + hit.collider.name);
        }
        else
        {
            if (roofDetected == null)
            {
                return;
            }
            roofDetected.TurnOnRoof();
            roofDetected = null;
            Debug.Log("No object hit.");
        }
    }
}
