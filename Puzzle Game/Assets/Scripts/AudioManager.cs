using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    [HideInInspector] public bool titleScreen = true;
    [HideInInspector] public bool toNextLevel = false;

    public static AudioManager instance;

    private bool gameStart = false;
    private bool toLevel = false;
    private bool toTitle = false;
    private float titleTime;
    private AudioSource windBlow;
    private AudioSource titleTrack;
    private AudioSource trackTwo;

    void Awake() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            Debug.Log("AudioManager Destroyed");
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        Play("WindBlowing");
        titleTime = Time.time;
        for (int i = 0; i < GetComponents<AudioSource>().Length; i++) {
            if (GetComponents<AudioSource>()[i].clip.name == "Wind Blowing") {
                windBlow = GetComponents<AudioSource>()[i];
            }
        }

    }
    private void Update() {
        if (windBlow.volume < 0.2f && titleScreen) {
            windBlow.volume += 0.1f * Time.deltaTime;
        }
        if (titleScreen && Time.time - titleTime > 4f && !gameStart) {
            FindObjectOfType<AudioManager>().Play("Track1");
            gameStart = true;
            Debug.Log("Track 1 Played");
            for (int i = 0; i < GetComponents<AudioSource>().Length; i++) {
                if (GetComponents<AudioSource>()[i].clip.name == "Puzzle Game Track") {
                    titleTrack = GetComponents<AudioSource>()[i];
                }
            }
            titleTrack.volume = 1f;
        }
        if (toLevel && titleTrack != null) {
            if (titleTrack.volume >= 0f) {
                titleTrack.volume -= 1f * Time.deltaTime;
                if (titleTrack.volume == 0f) {
                    toLevel = false;
                    toTitle = false;
                    StopSound(titleTrack);
                }
            }
        }
        if (toTitle && trackTwo != null) {
            if (trackTwo.volume >= 0f) {
                trackTwo.volume -= 1f * Time.deltaTime;
                windBlow.pitch = Mathf.Min(windBlow.pitch + (1f * Time.deltaTime), 1f);
                windBlow.volume = Mathf.Min(windBlow.volume + (1f * Time.deltaTime), 0.2f);
                if (trackTwo.volume == 0f) {
                    toLevel = false;
                    toTitle = false;
                    StopSound(trackTwo);
                    trackTwo = null;
                }
            }
        }
    }
    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        Debug.Log("Started playing " + name);
        s.source.Play();
    }
    public void StopSound(AudioSource sound) {
        Debug.Log("Stopped playing " + sound.clip.name);
        sound.Stop();
    }
    public void ChangeTitleScreen(bool isTitle) {
        if (titleScreen == isTitle) return;
        titleScreen = isTitle;
        if (titleScreen) {
            gameStart = false;
            titleTime = Time.time;
        }
    }
    public void TitleVolumeOff() {
        toLevel = true;
    }
    public void MainTrackVolumeOff() {
        if (trackTwo == null) return;
        toTitle = true;
    }
    public void TrackTwoChange(bool toPlayer) {
        float trackTime = 0f;
        if (toNextLevel) {
            toNextLevel = false;
            return;
        }
        if (toPlayer) {
            Play("Track2Player");
            if (trackTwo != null) {
                trackTime = trackTwo.time;
                StopSound(trackTwo);
            }
            for (int i = 0; i < GetComponents<AudioSource>().Length; i++) {
                if (GetComponents<AudioSource>()[i].clip.name == "Puzzle Game Track 2(Player)") {
                    trackTwo = GetComponents<AudioSource>()[i];
                }
            }
            windBlow.pitch = 1f;
            windBlow.volume = 0.1f;
            trackTwo.time = trackTime;
            trackTwo.volume = 1f;
        }
        else {
            Play("Track2Statue");
            if (trackTwo != null) {
                trackTime = trackTwo.time;
                StopSound(trackTwo);
            }
            for (int i = 0; i < GetComponents<AudioSource>().Length; i++) {
                if (GetComponents<AudioSource>()[i].clip.name == "Puzzle Game Track 2(Statue)") {
                    trackTwo = GetComponents<AudioSource>()[i];
                }
            }
            windBlow.pitch = 0.3f;
            windBlow.volume = 0.1f;
            trackTwo.time = trackTime;
            trackTwo.volume = 1f;
        }
    }
}
