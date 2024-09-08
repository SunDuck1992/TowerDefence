using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerShooter : MonoBehaviour
{
    public const float Radius = 5f;

    [SerializeField] private float _couldown;
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Transform _view;
    [SerializeField] private Rotate _rotate;
    [SerializeField] private GameUnit _self;


    private UISettings _uISettings;
    private float _damage;
    private BulletPool _bulletPool;
    private PlayerUpgradeSystem _playerUpgradeSystem;
    private bool _isMassiveDamage;
    private int _weaponIndex;
    private Weapon _currentWeapon;
    private GameUnit _target;
    private TargetController _targetController;

    public bool IsShooting { get; private set; }

    [Inject]
    public void Construct(BulletPool bulletPool, PlayerUpgradeSystem playerUpgradeSystem, GameConfigProxy gameConfigProxy, UISettings uISettings, TargetController targetController)
    {
        _targetController = targetController;
        _uISettings = uISettings;
        _bulletPool = bulletPool;
        _playerUpgradeSystem = playerUpgradeSystem;
        _couldown = gameConfigProxy.Config.PlayerConfig.Couldown;
        _damage = gameConfigProxy.Config.PlayerConfig.Damage;

        _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.ValueChanged += IncreaseDamage;
        _uISettings.MassDamageButton.EnableBonus.AddListener(ActivateMassDamage);
        _uISettings.MassDamageButton.DisableBonus.AddListener(DeactivateMassDamage);
        ChangeWeapon(_weaponIndex);
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
    }

    private void OnDestroy()
    {
        _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.ValueChanged -= IncreaseDamage;
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

                yield return new WaitForSeconds(_currentWeapon.FireRate);
            }
            else
            {
                IsShooting = false;
            }

            yield return null;
        }
    }

    public void ActivateMassDamage()
    {
        _isMassiveDamage = true;
    }

    public void DeactivateMassDamage()
    {
        _isMassiveDamage = false;
    }

    public void IncreaseShootSpeed(float speed)
    {
        _couldown -= speed * _playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.Value;
    }

    private void IncreaseDamage(int damage)
    {
        _damage += damage * _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.Value;
        Debug.Log(_damage);
    }

    private void OnHit(Bullet bullet, Enemy enemy)
    {
        if (_isMassiveDamage)
        {
            var enemies = _targetController.GetAllTargets(enemy, Radius, true);

            foreach (var e in enemies)
            {
                if (e != enemy)
                {
                    e.TakeDamage(bullet.Damage * 0.7f);
                }
            }
        }

        enemy.TakeDamage(bullet.Damage);
    }

    private void BulletComplete(Bullet bullet)
    {
        bullet.Hit -= OnHit;
        bullet.Died -= BulletComplete;
    }
}

