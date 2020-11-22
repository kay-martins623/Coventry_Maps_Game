using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System;
using TMPro;

public class LocationLookup : MonoBehaviour
{
    public TextMeshProUGUI gpsText;
    public TextMeshProUGUI refreshingText;
    private float latitude;
    private float longitude;
    public float waitTime;
    public float waitInterval;
    private bool isLocationStarted;
    private string timeStamp;

    // Start is called before the first frame update
    void Start()
    {
        //User needs to allow the location permission to be authorised in order for Location lookup to work
        //If the user hasn't already authorised location, we request it for them
        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation)){
            Permission.RequestUserPermission(Permission.FineLocation);
        }   
        longitude = 0f;
        latitude = 0f;
        refreshingText.text = "";
        isLocationStarted = false;
        StartCoroutine(getLocation());
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
    }

    IEnumerator getLocation(){
        //if it's not enabled, we tell the user the app cannot work without it
        //and exit coroutine
        if(!Input.location.isEnabledByUser){
            gpsText.text = "You haven't enabled location! App cannot work properly :(";
            yield break;
        }

        //We start gathering location data (with an accuracy of 5 meters & the minumum distance for updates to 5 meters)
        Input.location.Start(5, 5);

        //Wait for the location service to initialise and have the wait time decrement
        //if location service fails to initialise or we "time out", we send notices on what happened
        while(Input.location.status == LocationServiceStatus.Initializing && waitTime > 0){
            gpsText.text = "Gathering data. Please wait.";
            yield return new WaitForSeconds(1);
            waitTime -= Time.deltaTime;
        }

        if(waitTime <= 0){
            gpsText.text = "connection failed :(";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            gpsText.text = "Unable to determine device location";
            yield break;
        }

        //the location service has successfully been instanciated, so we're able to get the
        //last updated information. I get the longitude and latitude and display it on the UI
        else{
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            timeStamp = DateTime.Now.ToString("hh:mm:ss tt");
            isLocationStarted = true;
            gpsText.text = "Latitude: " + latitude + "\nLongitude: " + longitude + "\nLast update: " + timeStamp;
            refreshingText.text = timeStamp;
            yield return new WaitForSeconds(1);  
        }
        Input.location.Stop();

        //if it has successfully started, we start the function that updates automatically at a 
        //given time interval
        if(isLocationStarted){
            StartCoroutine(updateLocation());
        }
        
    }

    ///<summary>
    ///Updates the longitude and latitude of a person's location at a given time interval
    ///</summary>
    IEnumerator updateLocation(){
        WaitForSeconds updateTime = new WaitForSeconds(waitInterval);
        while(true){
            Input.location.Start(5, 5);
            while(Input.location.status == LocationServiceStatus.Initializing && waitTime > 0){
                refreshingText.text = "Refreshing...";
                yield return new WaitForSeconds(1);
                waitTime -= Time.deltaTime;
            }

            if(waitTime <= 0){
                gpsText.text = "connection failed :(";
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                gpsText.text = "Unable to determine device location";
                yield break;
            }

            else{
                latitude = Input.location.lastData.latitude;
                longitude = Input.location.lastData.longitude;
                timeStamp = DateTime.Now.ToString("hh:mm:ss tt");
                gpsText.text = "Latitude: " + latitude + "\nLongitude: " + longitude + "\nLast update: " + timeStamp;
                refreshingText.text = timeStamp;
            }
            Input.location.Stop();
            yield return updateTime;
        }  
    }
}
