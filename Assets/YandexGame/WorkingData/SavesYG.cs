
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public float playerHealth; 
        public float weaponDamage;
        public float weaponSpeed;
        public int gold = -1;  // +
        public int gem = -1;  // +

        public int waveCount = -1; // +
        public int enemyCount = -1; // +

        public int upgradeDamageLevel = -1; // +
        public int upgradeSpeedWeaponLevel = -1; // +
        public int upgradeHealthLevel = -1; // +

        public int weaponIndex = -1;
        public int UpgradeLevelTower = -1;

        public List<bool> weaponsIsBuyed = new List<bool>(new bool[3]);

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
            weaponsIsBuyed[0] = true;
            //weaponsIsBuyed[0] = true;
        }
    }
}
