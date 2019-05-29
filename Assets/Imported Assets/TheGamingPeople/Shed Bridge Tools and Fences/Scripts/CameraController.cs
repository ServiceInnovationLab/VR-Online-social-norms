using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject CameraShed;
	public GameObject CameraFences;
	public GameObject CameraBridge;
	public GameObject CameraTools;

	private void Start() 
	{
		EnableCamera("camShed");
		CameraFences.SetActive(false);
		CameraBridge.SetActive(false);
		CameraTools.SetActive(false);
	}

	public void EnableCamera(string camera)
	{
		if(camera == "camShed")
		{
			CameraShed.SetActive(true);
			CameraFences.SetActive(false);
			CameraBridge.SetActive(false);
			CameraTools.SetActive(false);
		}

		if(camera == "camTools")
		{
			CameraTools.SetActive(true);
			CameraShed.SetActive(false);
			CameraFences.SetActive(false);
			CameraBridge.SetActive(false);			
		}

		if(camera == "camBridge")
		{
			CameraBridge.SetActive(true);	
			CameraTools.SetActive(false);
			CameraShed.SetActive(false);
			CameraFences.SetActive(false);
		}

		if(camera == "camFences")
		{
			CameraFences.SetActive(true);
			CameraTools.SetActive(false);
			CameraShed.SetActive(false);			
			CameraBridge.SetActive(false);			
		}
	}

}
