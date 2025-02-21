using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Zenject;

public class PlayerShooter : MonoBehaviour
{
    public const float Radius = 7f;

    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Transform _view;
    [SerializeField] private Rotate _rotate;
    [SerializeField] private GameUnit _self;
    [SerializeField] private float _multyplieChangåCharacteristickValue;
    [SerializeField] private ParticleSystem _hitBulletParticle;
    [SerializeField] private ParticleSystem _massiveHitBulletParticle;

    private UISettings _uISettings;
    private float _damage;
    private BulletPool _bulletPool;
    private PlayerUpgradeSystem _playerUpgradeSystem;
    private bool _isMassiveDamage;
    private int _weaponIndex;
    private GameUnit _target;
    private TargetController _targetController;
    private PlayerWallet _playerWallet;
    private float _couldown;

    public Weapon CurrentWeapon { get; private set; }
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

        YandexGame.GetDataEvent += SetCurrentWeapon;
        SetCurrentWeapon();

        _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.ValueChanged += UpdateDamage;
        _playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.ValueChanged += UpdateShootSpeed;
        _uISettings.MassDamageButton.EnableBonus.AddListener(ActivateMassDamage);
        _uISettings.MassDamageButton.DisableBonus.AddListener(DeactivateMassDamage);

        ChangeWeapon(_weaponIndex);
        _couldown = CurrentWeapon.FireRate;
    }

    ~PlayerShooter()
    {
        _uISettings.MassDamageButton.EnableBonus.RemoveAllListeners();
        _uISettings.MassDamageButton.DisableBonus.RemoveAllListeners();       
    }

    public void ChangeWeapon(int indexWeapon)
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon?.DeActivate();
        }

        _weaponIndex = indexWeapon;
        CurrentWeapon = _weapons[_weaponIndex];
        CurrentWeapon.Activate();
        UpdateShootSpeed();
        UpdateDamage();
    }

    private void OnDestroy()
    {
        _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.ValueChanged -= UpdateDamage;
        _playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.ValueChanged -= UpdateShootSpeed;
        YandexGame.GetDataEvent -= SetCurrentWeapon;
    }

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 rotate = _target.transform.position - CurrentWeapon.WeaponPoint.transform.position;
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
                for (int i = 0; i < CurrentWeapon.CountBullet; i++)
                {
                    Bullet bullet = _bulletPool.Spawn();
                    bullet.Hit += OnHit;
                    bullet.Died += BulletComplete;

                    CurrentWeapon.Shoot(bullet);
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
        _couldown = CurrentWeapon.ChangeFirerate(_playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.Value);
    }

    private void UpdateDamage()
    {
        _damage = CurrentWeapon.ChangeDamage(_playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.Value);
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
                    Instantiate(_massiveHitBulletParticle, enemy.DeathParticlePoint.position, Quaternion.identity);
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

    private void SetCurrentWeapon()
    {
        if (YandexGame.savesData.weaponIndex != -1)
        {
            _weaponIndex = YandexGame.savesData.weaponIndex;
        }
        else
        {
            _weaponIndex = 0;
        }

        ChangeWeapon(_weaponIndex);
    }
}

