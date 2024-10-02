using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora.Rtc;
using Agora.Util;
using Agora_RTC_Plugin;
using Photon.Pun;
using UnityEngine.Networking;
using System;
using TMPro;

public class TokenStruct
{
    public string rtcToken;
}
public class NewBehaviourScript : MonoBehaviour
{
    [Header("UI 및 오브젝트")]
    public GameObject faceChatCanvas;
    public Transform faceChatParent;
    public GameObject faceChatSurface;
    public List<VideoSurface> RemoteViews = new List<VideoSurface>();
    private string serverUrl = "https://agora-token-service-production-1bf9.up.railway.app";
    private int ExpireTime = 5000;
    private string uid = "";

    // Fill in your app ID.
    public string _appID = "a7f7ab5261bc49a8ad6f9cdebe6c24bd";
    // Fill in your channel name.
    public string _channelName = "Test";
    // Fill in the temporary token you obtained from Agora Console.
    public string _token = "";
    // A variable to save the remote user uid.
    private uint remoteUid;
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal IRtcEngine RtcEngine;

    [SerializeField]
    private bool sharingScreen = false;
    void Start()
    {
        var item = GameObject.Find("FaceChatCanvas");
        if(item != null)
        {
            faceChatCanvas = GameObject.Find("FaceChatCanvas").transform.GetChild(0).gameObject;
            Transform[] allChildren = faceChatCanvas.transform.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.name == transform.name)
                    return;
                if(child.name == "FaceChatContent")
                {
                    faceChatParent = child;
                }
            }
        }

    }

    void Update()
    {
        if (GameManager.instance.ShareScreen)
        {
            shareScreen();
            GameManager.instance.ShareScreen = false;
        }

    }
    void FetchRenew(string newToken)
    {
        Debug.Log(newToken);
        // Update RTC Engine with new token, which will not expire so soon
        _token = newToken;
        RtcEngine.RenewToken(newToken);
        //Debug.Log(newToken);
    }

    private readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private long CurrentTimeMillis()
    {
        return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
    }

    IEnumerator FetchToken(string url, string channel, string userId, int TimeToLive, Action<string> callback = null)
    {
        //"{0}/rtc/{1}/1/uid/{2}/?expiry={3}", url, channel, userId, TimeToLive)
        UnityWebRequest request = UnityWebRequest.Get(string.Format("{0}/rtc/{1}/1/uid/{2}/?expiry={3}", url, channel, userId, TimeToLive));
        Debug.Log(url + "/rtc/" + channel + "/1/uid/" + userId + "/?expiry=" + TimeToLive);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            callback(null);
            yield break;
        }
        else
        {
            TokenStruct tokenInfo = JsonUtility.FromJson<TokenStruct>(request.downloadHandler.text);
            callback(tokenInfo.rtcToken);
            Join(LocalView);
        }
    }
    private void SetupVideoSDKEngine()
    {
        Debug.Log("RTC 세팅 완료");
        // Create an instance of the video SDK.
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        // Specify the context configuration to initialize the created instance.
        RtcEngineContext context = new RtcEngineContext(_appID, 0, CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING, AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        // Initialize the instance.
        RtcEngine.Initialize(context);
    }
    bool mic = true;
    public void MicOnOff()
    {
        if (GameManager.instance.faceChatOn)
        {
            mic = !mic;
            RtcEngine.EnableLocalAudio(mic);
        }
    }
    bool screen = true;
    public void ScreenOnOff()
    {
        screen = !screen;
        RtcEngine.EnableLocalVideo(screen);
    }

    public void shareScreen()
    {
        if (!sharingScreen)
        {
            // The target size of the screen or window thumbnail (the width and height are in pixels).
            SIZE t = new SIZE(1920, 1080);
            // The target size of the icon corresponding to the application program (the width and height are in pixels)
            SIZE s = new SIZE(360, 240);
            // Get a list of shareable screens and windows
            var info = RtcEngine.GetScreenCaptureSources(t, s, true);
            for(int i = 0; i < info.Length; i++)
            {
                Debug.Log(info[i].sourceName);
            }
            // Get the first source id to share the whole screen.
            ulong dispId = info[2].sourceId;
            // To share a part of the screen, specify the screen width and size using the Rectangle class.
            RtcEngine.StartScreenCaptureByWindowId(System.Convert.ToUInt32(dispId), new Rectangle(),default(ScreenCaptureParameters));
            // Publish the screen track and unpublish the local video track.
            updateChannelPublishOptions(true);
            setupLocalVideo(true);
            sharingScreen = true;
            // Display the screen track in the local view.
            // Change the screen sharing button text.
            // Update the screen sharing state.
        }
        else
        {
            // Stop sharing.
            RtcEngine.StopScreenCapture();
            // Publish the local video track when you stop sharing your screen.
            updateChannelPublishOptions(false);
            setupLocalVideo(false);
            sharingScreen = false;
            // Display the local video in the local view.
            // Update the screen sharing state.
            // Change to the default text of the button when you stop sharing your screen.
        }
    }
    private void setupLocalVideo(bool isScreenSharing)
    {
        if (isScreenSharing)
        {
            GameObject go = LocalView.gameObject;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            //Destroy(LocalView);
            LocalView = go.AddComponent<VideoSurface>();
            RemoteViews.Add(LocalView);
            // Render the screen sharing track on the local view.
            LocalView.SetForUser(0,"", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN_PRIMARY);
        }
        else
        {
            GameObject go = LocalView.gameObject;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            //Destroy(LocalView);
            LocalView = LocalView.gameObject.AddComponent<VideoSurface>();
            RemoteViews.Add(LocalView);
            LocalView.SetForUser(0,"", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA_PRIMARY);
        }
    }
    private void updateChannelPublishOptions(bool publishMediaPlayer)
    {
        ChannelMediaOptions channelOptions = new ChannelMediaOptions();
        channelOptions.publishScreenTrack.SetValue(publishMediaPlayer);
        channelOptions.publishMediaPlayerAudioTrack.SetValue(true);
        channelOptions.publishSecondaryScreenTrack.SetValue(publishMediaPlayer);
        channelOptions.publishCameraTrack.SetValue(!publishMediaPlayer);
        RtcEngine.UpdateChannelMediaOptions(channelOptions);
    }
    private void InitEventHandler()
    {
        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }

    public void Join(VideoSurface video)
    {
        RtcEngine.EnableVideo();
        // Set the user role as broadcaster.
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Set the local video view.
        //video.SetForUser(uint.Parse(uid), _channelName, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
        if (sharingScreen)
        {
            video.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN_PRIMARY);
            sharingScreen = false;
            shareScreen();
        }
        else
        {
            video.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA_PRIMARY);
        }
        // Start rendering local video.
        video.SetEnable(true);

        // Join a channel.
        Debug.Log(_token + " " + _channelName + " 좀 되라 쓰발아");
        RtcEngine.JoinChannel(_token, _channelName, "", uint.Parse(uid));
        // Enable the video module.

    }
    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        // Disable the video modules.
        RtcEngine.DisableVideo();
        // Stops rendering the remote video.
        for (int i = 0; i < RemoteViews.Count; i++)
        {
            RemoteViews[i].SetEnable(false);
        }
        // Stops rendering the local video.
        LocalView.SetEnable(false);
        //faceChatCanvas.SetActive(false);
    }


    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly NewBehaviourScript _videoSample;

        internal UserEventHandler(NewBehaviourScript videoSample)
        {
            _videoSample = videoSample;
        }
        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("You joined channel: " + connection.channelId);
        }
        public override void OnTokenPrivilegeWillExpire(RtcConnection connection, string token)
        {
            // Retrieve a fresh token from the token server.
            _videoSample.StartCoroutine(_videoSample.FetchToken(_videoSample.serverUrl, _videoSample._channelName, _videoSample.uid, _videoSample.ExpireTime, _videoSample.FetchRenew));
            Debug.Log("Token Expired");
        }
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            Debug.Log("Hello");
            // Setup remote view.
            _videoSample.OnUserJoin(uid);
            _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // Save the remote user ID in a variable.
            _videoSample.remoteUid = uid;
        }
        // This callback is triggered when a remote user leaves the channel or drops offline.
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.OnUserOff(uid);
        }

    }
    public void OnUserJoin(uint uid)
    {
        GameObject go = Instantiate(faceChatSurface, faceChatParent);
        RemoteView = go.GetComponent<VideoSurface>();
        RemoteView.uidClone = uid;
        go.transform.eulerAngles = new Vector3(0, 0, 180);
        RemoteViews.Add(RemoteView);
    }
    public void OnUserOff(uint uid)
    {
        for (int i = 0; i < RemoteViews.Count; i++)
        {
            if (RemoteViews[i].uidClone == uid)
            {
                VideoSurface vid = RemoteViews[i];
                vid.SetEnable(false);
                RemoteViews.Remove(vid);
                Destroy(vid.gameObject);
            }
        }
    }
    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("들어옴",this);
            SetupVideoSDKEngine();
            InitEventHandler();
            faceChatCanvas.SetActive(true);
            //GameObject go = Instantiate(faceChatSurface, faceChatParent);
            GameObject go = Instantiate(Resources.Load("FaceChatSurface", typeof(GameObject)),faceChatParent) as GameObject;
            go.transform.eulerAngles = new Vector3(0, 0, 180);
            LocalView = go.GetComponent<VideoSurface>();
            if (_channelName == "")
            {
                Debug.Log("Channel name is required!");
                return;
            }
            uid = collision.GetComponent<PhotonView>().ViewID.ToString();
            StartCoroutine(FetchToken(serverUrl, _channelName, uid, ExpireTime, this.FetchRenew));
            GameManager.instance.faceChatOn = true;
            //Join(LocalView);
            RemoteViews.Add(LocalView);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine && RtcEngine != null)
        {
            Debug.Log("나감",this);
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
            int a = RemoteViews.Count;
            Debug.Log(a);
            for (int i = 0; i < a; i++)
            {
                //Debug.Log(RemoteViews[i].gameObject.name);
                if(RemoteViews[0] != null)
                {
                    Destroy(RemoteViews[0].gameObject);
                }
                RemoteViews.RemoveAt(0);
            }
            Debug.Log("삭제시작");
            a = faceChatParent.childCount;
            for(int i =0; i < faceChatParent.childCount; i++)
            {
                Destroy(faceChatParent.GetChild(0).gameObject);
            }
            GameManager.instance.faceChatOn = false;
            //faceChatCanvas.SetActive(false);
        }
    }


}