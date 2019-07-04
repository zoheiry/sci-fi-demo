using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    private CharacterController _controller;

    [SerializeField]
    private float _speed = 3.5f;

    private float _gravity = 9.81f;

    [SerializeField]
    private GameObject _muzzleFlash;

    [SerializeField]
    private GameObject _hitMarkerPrefab;

    [SerializeField]
    private AudioSource _weaponAudio;

    private int _totalAmmo = 200;

    private float _reloadTime = 1.5f;
    private bool _isReloading = false;

    [SerializeField]
    private bool _hasCoin = false;

    [SerializeField]
    private int _ammoRemaining;

    [SerializeField]
    private GameObject _weapon;

    private UIManager _uiManager;

    // Use this for initialization
    void Start () {
        _controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _ammoRemaining = _totalAmmo;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager)
        {
            _uiManager.UpdateAmmo(_ammoRemaining);
        }
    }

    // Update is called once per frame
    void Update() {
        CalculateMovement();

        CheckForWeaponFire();

        CheckForReload();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
	}

    void CheckForWeaponFire()
    {
        if (Input.GetMouseButton(0) && _ammoRemaining > 0 && !_isReloading && _weapon.activeSelf)
        {
            fireWeapon();
        }
        else
        {
            _muzzleFlash.SetActive(false);
            _weaponAudio.Stop();
        }
        
    }

    private void fireWeapon()
    {
        _ammoRemaining--;
        _uiManager.UpdateAmmo(_ammoRemaining);
        _muzzleFlash.SetActive(true);
        if (!_weaponAudio.isPlaying)
        {
            _weaponAudio.Play();
        }
        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            GameObject hitMarker = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(hitMarker, 1f);

            if (hitInfo.transform.tag == "Crate")
            {
                Destructable crate = hitInfo.transform.GetComponent<Destructable>();
                crate.DestroyCrate();
            }
        }
    }

    void CheckForReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _isReloading = true;
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(_reloadTime);
        _ammoRemaining = _totalAmmo;
        _uiManager.UpdateAmmo(_ammoRemaining);
        _isReloading = false;
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * _speed;
        velocity.y -= _gravity;

        velocity = transform.transform.TransformDirection(velocity);
        _controller.Move(velocity * Time.deltaTime);
    }

    public bool hasMoney()
    {
        return _hasCoin;
    }

    public void takeCoin()
    {
        _uiManager.ShowCoinImage();
        _hasCoin = true;
    }

    public void useCoin()
    {
        _hasCoin = false;
    }

    public void EnableWeapon()
    {
        _weapon.SetActive(true);
    }
}
