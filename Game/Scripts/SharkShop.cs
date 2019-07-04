using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkShop : MonoBehaviour {

    [SerializeField]
    private AudioClip _clip;

    private Player _player;
    private UIManager _uiManager;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            _player = other.GetComponent<Player>();
            _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            if (_player.hasMoney())
            {
                _player.useCoin();
                AudioSource.PlayClipAtPoint(_clip, transform.position);
                _uiManager.HideCoingImage();
                _player.EnableWeapon();
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        }
    }
}
