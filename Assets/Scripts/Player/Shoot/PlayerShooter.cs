using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerShooter : MonoBehaviour
{
    public const float Radius = 5f;

    //[SerializeField] private float _couldown;
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Transform _view;
    [SerializeField] private Rotate _rotate;
    [SerializeField] private GameUnit _self;
    [SerializeField] private float _multyplieChangåCharacteristickValue;


    private UISettings _uISettings;
    private float _damage;
    private BulletPool _bulletPool;
    private PlayerUpgradeSystem _playerUpgradeSystem;
    private bool _isMassiveDamage;
    private int _weaponIndex;
    private Weapon _currentWeapon;
    private GameUnit _target;
    private TargetController _targetController;
    private PlayerWallet _playerWallet;
    private float _couldown;
    

    public bool IsShooting { get; private set; }

    [Inject]
    public void Construct(BulletPool bulletPool, PlayerUpgradeSystem playerUpgradeSystem, GameConfigProxy gameConfigProxy, UISettings uISettings, TargetController targetController, PlayerWallet playerWallet)
    {
        _targetController = targetController;
        _uISettings = uISettings;
        _bulletPool = bulletPool;
        _playerWallet = playerWallet;
        _playerUpgradeSystem = playerUpgradeSystem;     
        _damage = gameConfigProxy.Config.PlayerConfig.Damage;

        _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.ValueChanged += UpdateDamage;
        _playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.ValueChanged += UpdateShootSpeed;
        _uISettings.MassDamageButton.EnableBonus.AddListener(ActivateMassDamage);
        _uISettings.MassDamageButton.DisableBonus.AddListener(DeactivateMassDamage);
        ChangeWeapon(_weaponIndex);

        _couldown = _currentWeapon.FireRate;
    }

    ~PlayerShooter()
    {
        _uISettings.MassDamageButton.EnableBonus.RemoveAllListeners();
        _uISettings.MassDamageButton.DisableBonus.RemoveAllListeners();
    }

    public void ChangeWeapon(int indexWeapon)
    {
        _currentWeapon?.DeActivate();

        _weaponIndex = indexWeapon;
        _currentWeapon = _weapons[_weaponIndex];
        _currentWeapon.Activate();
        UpdateShootSpeed();
        UpdateDamage();
    }

    private void OnDestroy()
    {
        _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.ValueChanged -= UpdateDamage;
        _playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.ValueChanged -= UpdateShootSpeed;
    }

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 rotate = _target.transform.position - _currentWeapon.WeaponPoint.transform.position;
            rotate.y = 0;
            _rotate.Direction = rotate;
            _rotate.IsShooting = true;
        }
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            _target = _targetController.GetTarget(_self, 10, true);
            if (_target != null)
            {
                for (int i = 0; i < _currentWeapon.CountBullet; i++)
                {
                    Bullet bullet = _bulletPool.Spawn();
                    bullet.Hit += OnHit;
                    bullet.Died += BulletComplete;

                    _currentWeapon.Shoot(bullet);
                }

                IsShooting = true;

                yield return new WaitForSeconds(_couldown);
            }
            else
            {
                IsShooting = false;
            }

            yield return null;
        }
    }

    public void ActivateMassDamage(int cost)
    {
        if (_playerWallet.TrySpendGem(cost))
        {
            _isMassiveDamage = true;
        }
    }

    public void DeactivateMassDamage(int cost)
    {
        _isMassiveDamage = false;
    }

    public void UpdateShootSpeed()
    {
        _couldown = _currentWeapon.ChangeFirerate(_playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.Value);
        //Debug.Log(_couldown + " - firerate");
    }

    private void UpdateDamage()
    {
        _damage = _currentWeapon.ChangeDamage(_playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.Value);
        //Debug.Log(_damage + " - damage");
    }

    private void OnHit(Enemy enemy)
    {
        if (_isMassiveDamage)
        {
            var enemies = _targetController.GetAllTargets(enemy, Radius, true);

            foreach (var e in enemies)
            {
                if (e != enemy)
                {
                    e.TakeDamage(_damage * 0.7f);
                }
            }
        }

        enemy.TakeDamage(_damage);
    }

    private void BulletComplete(Bullet bullet)
    {
        bullet.Hit -= OnHit;
        bullet.Died -= BulletComplete;
    }
}

