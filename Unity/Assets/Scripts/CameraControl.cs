using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject staticCamera;

    //  Alana girince sabit kamerayı aktif edelim
    void OnTriggerEnter2D(Collider2D c2d) {
        if(c2d.name == "utku") {
            staticCamera.SetActive(true);
        }
    }

    //  Alandan çıkınca sabit kamerayı pasif edelim
    void OnTriggerExit2D(Collider2D c2d) {
        if(c2d.name == "utku") {
            staticCamera.SetActive(false);
        }
    }
}
