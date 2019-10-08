﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using CSCore;
using CSCore.SoundIn;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.CoreAudioAPI;

public class AudioHelperX : MonoBehaviour
{
    /// <summary>
    /// This dictionary was to store all of the audio devices so the rest of the application could use them without
    /// having to continually query the system. This dictionary would be shared to the in-game scripts.
    /// </summary>
    private Dictionary<int, AudioEditorDevice> editorDevices;

    //public static void ShowWindow()
    //{
    //    EditorWindow.GetWindow(typeof(AudioHelper));
    //}

    private void Start()
    {
        Debug.Log("I AM AWAKE");
        if (editorDevices == null)
            editorDevices = new Dictionary<int, AudioEditorDevice>();

        // this retrieves all of the system's ACTIVE audio devices
        MMDeviceCollection devices = MMDeviceEnumerator.EnumerateDevices(DataFlow.All, DeviceState.Active);

        // this retrieves all of the system's ACTIVE capture devices (e.g., microphones)
        //MMDeviceCollection devices = MMDeviceEnumerator.EnumerateDevices(DataFlow.Capture, DeviceState.Active);

        // this retrieves all of the system's ACTIVE output devices (speakers, monitors)
        //MMDeviceCollection devices = MMDeviceEnumerator.EnumerateDevices(DataFlow.Render, DeviceState.Active);

        for (var i = 0; i < devices.Count; i++)
            editorDevices.Add(i, new AudioEditorDevice { AudioDevice = devices[i], Selected = false });
        print(editorDevices.Count);
        

        DoIt();
    }

    private void DoIt()
    {
        //GUILayout.Label("Audio Devices", EditorStyles.boldLabel);

        // this creates a checkbox list; supporting multiple audio devices would be slick
        // think main playback device and one capture device (a microphone) or multiple microphones
        // and the default system audio device...and so on....
        //for (var i = 0; i < editorDevices.Count; i++)
        //    editorDevices[i].Selected = EditorGUILayout.ToggleLeft(editorDevices[i].AudioDevice.FriendlyName, editorDevices[i].Selected);

        //if (GUILayout.Button("Save and close"))
        //{
            // only one selected audio device is retrieved
            // support for multiple would be amazing (think one or more microphones plus the main playback device)                
            var selectedDevice = editorDevices.FirstOrDefault(d => d.Value.Selected);

            // grab the audio source from the scene
            GameObject audioSource = GameObject.Find("Loopback Audio Source");

            // grab a reference to the LoopbackAudio script; there's an MMDevice
            // variable there that stores the selected device to be used when creating
            // the objects needed for the visualizer
            LoopbackAudio script = audioSource.GetComponent<LoopbackAudio>();
            script.SelectedAudioDevice = selectedDevice.Value.AudioDevice;
        //}
    }
}
