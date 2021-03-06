﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour
{
    private int rockOwned;
    private int woodOwned;
    private int goldOwned;

    public GameObject buildingsMenu;
    private bool buildingsMenuOpen;

    public Transform houseImage;
    public GameObject housePrefab;
    private Vector3 houseInitialPosition;
    private bool placing;

    private bool attacked = false;
    private bool attacking = false;
    public GameObject attackingShip;
    public GameObject attackingDialog;
    public GameObject attackResultDialog;

    void Start(){
        rockOwned = 0;
        woodOwned = 0;
        goldOwned = 0;

        buildingsMenuOpen = false;
        houseInitialPosition = houseImage.position;
    }

    void LateUpdate(){
        if (placing) {
            houseImage.position = Input.mousePosition;
        }
        if (placing & Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){
                if (hit.transform.gameObject.layer == 11){
                    houseImage.position = houseInitialPosition;
                    Instantiate(housePrefab, hit.point, new Quaternion(0,270,0,1));
                    placing = false;
                } else {
                    houseImage.position = houseInitialPosition;
                    placing = false;
                }
            }
        }

        if (woodOwned >= 5 & !attacked) {
            attackingShip.gameObject.SetActive(true);
            attackingDialog.gameObject.SetActive(true);
            attacked = true;
            attacking = true;
        }
        if (attacking & Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == 12)
                {
                    attackResultDialog.transform.GetChild(0).GetComponent<Text>().text = generateAttackOutcome();
                    attackResultDialog.gameObject.SetActive(true);
                    attackingDialog.gameObject.SetActive(false);
                    attackingShip.gameObject.SetActive(false);

                }
            }
        }
    }
    
    public void AddRock(int n){
        rockOwned += n;
    }

    public void AddWood(int n){
        woodOwned += n;
    }

    public void AddGold(int n){
        goldOwned += n;
    }

    public void showBuildingsMenu(){
        if (buildingsMenuOpen) {
            buildingsMenuOpen = false;
            buildingsMenu.gameObject.SetActive(false);
        } else {
            buildingsMenuOpen = true;
            buildingsMenu.gameObject.SetActive(true);
        }
    }

    public void placeBuilding(){
        placing = true;
    }

    private string generateAttackOutcome() {
        int rockStolen = (int) UnityEngine.Random.Range(1.0F, (float) rockOwned);
        int woodStolen = (int) UnityEngine.Random.Range(1.0F, (float) woodOwned);
        int goldStolen = (int) UnityEngine.Random.Range(1.0F, (float) goldOwned);

        rockOwned -= rockStolen;
        woodOwned -= woodStolen;
        goldOwned -= goldStolen;

        return string.Format("Your village has been attacked by pirates! You lost {0} rock, {1} wood, {2} gold.", rockStolen, woodStolen, goldStolen);
    }
}
