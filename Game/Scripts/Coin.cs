using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField]
    private AudioClip _audioClip;

    private bool _isTouchingPlayer = false;


    private Player _player;

    private Renderer _renderer;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_player)
                {
                    _player.takeCoin();
                }
                AudioSource.PlayClipAtPoint(_audioClip, this.transform.position, 0.6f);
                Destroy(this.gameObject);
            }
        }
    }

}
