using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicNpcAnimate : MonoBehaviour {

    [HideInInspector]
    public Animator baseAnimator;
    [HideInInspector]
    public Animator hairAnimator;
    [HideInInspector]
    public Animator facialHairAnimator;
    [HideInInspector]
    public Animator headwearAnimator;
    [HideInInspector]
    public Animator outfitAnimator;
    [HideInInspector]
    public Animator weaponAnimator;
    [HideInInspector]
    public NPCOverheadTalker overheadText;

    public void CINEMA_NpcIdle() {
        baseAnimator.SetBool("walk", false);
        hairAnimator.SetBool("walk", false);
        facialHairAnimator.SetBool("walk", false);
        headwearAnimator.SetBool("walk", false);
        outfitAnimator.SetBool("walk", false);
        weaponAnimator.SetBool("walk", false);
        baseAnimator.SetBool("attack", false);
        hairAnimator.SetBool("attack", false);
        facialHairAnimator.SetBool("attack", false);
        headwearAnimator.SetBool("attack", false);
        outfitAnimator.SetBool("attack", false);
        weaponAnimator.SetBool("attack", false);
    }

    public void CINEMA_NpcMove() {
        baseAnimator.SetBool("walk", true);
        hairAnimator.SetBool("walk", true);
        facialHairAnimator.SetBool("walk", true);
        headwearAnimator.SetBool("walk", true);
        outfitAnimator.SetBool("walk", true);
        weaponAnimator.SetBool("walk", true);
        baseAnimator.SetBool("attack", false);
        hairAnimator.SetBool("attack", false);
        facialHairAnimator.SetBool("attack", false);
        headwearAnimator.SetBool("attack", false);
        outfitAnimator.SetBool("attack", false);
        weaponAnimator.SetBool("attack", false);
    }

    public void CINEMA_NpcAttack() {
        baseAnimator.SetBool("walk", false);
        hairAnimator.SetBool("walk", false);
        facialHairAnimator.SetBool("walk", false);
        headwearAnimator.SetBool("walk", false);
        outfitAnimator.SetBool("walk", false);
        weaponAnimator.SetBool("walk", false);
        baseAnimator.SetBool("attack", true);
        hairAnimator.SetBool("attack", true);
        facialHairAnimator.SetBool("attack", true);
        headwearAnimator.SetBool("attack", true);
        outfitAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);
    }

    public void CINEMA_NpcTalk(string whatToSay) {
        overheadText.NpcTalk(whatToSay);
    }
}
