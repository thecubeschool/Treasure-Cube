using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum CINEMPlayerInteraction {
    DisableEverything = 0,
    DisableMovement = 1,
    DisableCamera = 2,
    None = 3,
}

public class CinematicManager : MonoBehaviour {

    [Space(10f)]
    [Header("CINEMATIC MANAGER")]

    [Space(10f)]
    [Tooltip("If we want this cinematic to play once, we need to check this.")]
    public bool playOnce = false;

    [Space(5f)]
    [Tooltip("What will happen with players controls when cinematic is played?")]
    public CINEMPlayerInteraction playerInteraction;

    [Space(5f)]
    [Tooltip("Should we disable user-interface when cinematic is playing.")]
    public bool disableUi;

    [Space(5f)]
    [Tooltip("On what, if any, animator will the animation play when cinematic starts.")]
    public Animator cinemaAnimator;

    [Space(5f)]
    [Tooltip("Name of the animation state that will play when cinematic starts.")]
    public string animationToPlay;

    [Space(5f)]
    [Tooltip("If animation that will play on cinematic start is related to some npc (npc is animated by it etc) we place that npc in this field." +
             "!Take in account, that if there is a npc related to cinematic you must place script CinematicNpcAnimate.cs on this gameObject!")]
    public GameObject cinematicNpc;

    [Space(10f)]
    public UnityEvent OnCinematicStart;
    public UnityEvent OnCinematicEnd;

    private GameObject player;
    private GameObject ui;

    private bool cinematicPlaying = false;
    private bool playedOnce = false;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        ui = GameObject.FindGameObjectWithTag("UI");

        if (cinematicNpc != null) {
            CinematicNpcAnimate ca = GetComponent<CinematicNpcAnimate>();
            ca.baseAnimator = cinematicNpc.transform.Find("base").transform.GetComponent<Animator>();
            ca.facialHairAnimator = cinematicNpc.transform.Find("base/facial hair").transform.GetComponent<Animator>();
            ca.hairAnimator = cinematicNpc.transform.Find("base/hair").transform.GetComponent<Animator>();
            ca.headwearAnimator = cinematicNpc.transform.Find("base/helmet").transform.GetComponent<Animator>();
            ca.outfitAnimator = cinematicNpc.transform.Find("base/outfit").transform.GetComponent<Animator>();
            ca.weaponAnimator = cinematicNpc.transform.Find("base/weapon").transform.GetComponent<Animator>();
            ca.overheadText = cinematicNpc.GetComponentInChildren<NPCOverheadTalker>();
        }
    }

    public void CinematicPlay() {
        if (playedOnce == false) {
            if (playerInteraction == CINEMPlayerInteraction.DisableEverything) {
                player.GetComponent<FirstPersonCameraLook>().enabled = false;
                player.GetComponent<FirstPersonCameraLook>().timeSpeedFactor = 0f;
                player.GetComponent<FirstPersonPlayer>().enabled = false;
                player.GetComponent<FirstPersonPlayer>().timeSpeedFactor = 0f;
                player.GetComponentInChildren<FirstPersonAnimate>().stopAnimation = true;
                player.GetComponentInChildren<WeaponMeleeAnimate>().stopAnimation = true;
                player.GetComponentInChildren<WeaponRangedAnimate>().stopAnimation = true;
            }
            else if (playerInteraction == CINEMPlayerInteraction.DisableMovement) {
                player.GetComponent<FirstPersonPlayer>().enabled = false;
                player.GetComponentInChildren<FirstPersonAnimate>().stopAnimation = true;
                player.GetComponentInChildren<WeaponMeleeAnimate>().stopAnimation = true;
                player.GetComponentInChildren<WeaponRangedAnimate>().stopAnimation = true;
            }
            else if (playerInteraction == CINEMPlayerInteraction.DisableCamera) {
                player.GetComponent<FirstPersonCameraLook>().enabled = false;
                player.GetComponent<FirstPersonCameraLook>().timeSpeedFactor = 0f;
                player.GetComponent<FirstPersonPlayer>().timeSpeedFactor = 0f;
            }

            if (disableUi) {
                ui.GetComponent<UIManager>().disableUiCinematic = true;
            }

            if (cinematicNpc != null) {
                cinematicNpc.GetComponent<NPC>().enabled = false;
                cinematicNpc.GetComponent<NPCAiNavmesh>().enabled = false;
                cinematicNpc.GetComponent<NPCAiNavmesh>().initialPosition = transform.position;
            }

            if (cinemaAnimator != null && !string.IsNullOrEmpty(animationToPlay)) {
                //cinemaAnimator.CrossFadeInFixedTime(animationToPlay, 0.1f);
                cinemaAnimator.Play(animationToPlay);

                OnCinematicStart.Invoke();

                if (cinematicPlaying == false) {
                    cinematicPlaying = true;
                }
            }
            else {
                Debug.LogWarning("No cinematic will be player on " + gameObject.name + " because Animator or Animation Name has not been set up.");
            }
        }
    }

    public void CinematicEnd() {

        if (playerInteraction == CINEMPlayerInteraction.DisableEverything) {
            player.GetComponent<FirstPersonCameraLook>().enabled = true;
            player.GetComponent<FirstPersonCameraLook>().timeSpeedFactor = 1f;
            player.GetComponent<FirstPersonPlayer>().enabled = true;
            player.GetComponent<FirstPersonPlayer>().timeSpeedFactor = 1f;
            player.GetComponentInChildren<FirstPersonAnimate>().stopAnimation = false;
            player.GetComponentInChildren<WeaponMeleeAnimate>().stopAnimation = false;
            player.GetComponentInChildren<WeaponRangedAnimate>().stopAnimation = false;
        }
        else if (playerInteraction == CINEMPlayerInteraction.DisableMovement) {
            player.GetComponent<FirstPersonPlayer>().enabled = true;
            player.GetComponentInChildren<FirstPersonAnimate>().stopAnimation = false;
            player.GetComponentInChildren<WeaponMeleeAnimate>().stopAnimation = false;
            player.GetComponentInChildren<WeaponRangedAnimate>().stopAnimation = false;
        }
        else if (playerInteraction == CINEMPlayerInteraction.DisableCamera) {
            player.GetComponent<FirstPersonCameraLook>().enabled = true;
            player.GetComponent<FirstPersonCameraLook>().timeSpeedFactor = 1f;
            player.GetComponent<FirstPersonPlayer>().timeSpeedFactor = 1f;
        }

        if (playOnce) {
            playedOnce = true;
        }

        if (cinematicNpc != null) {
            cinematicNpc.GetComponent<NPC>().enabled = true;
            cinematicNpc.GetComponent<NPCAiNavmesh>().enabled = true;
        }

        OnCinematicEnd.Invoke();

        cinematicPlaying = false;
    }
}
