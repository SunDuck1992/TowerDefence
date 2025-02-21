
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace YG
{
    [Serializable]
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

        public int leaderScore = 0;

        public int gold = -1;  // +
        public int gem = -1;  // +

        public int waveCount = -1; // +
        public int enemyCount = -1; // +

        public int upgradeDamageLevel = -1; // +
        public int upgradeSpeedWeaponLevel = -1; // +
        public int upgradeHealthLevel = -1; // +

        //public int MaxDamageLevel = 10;
        //public int MaxFirerateLevel = 5;
        //public int MaxHealthLevel = 20;

        public int weaponIndex = -1; // +
        public int UpgradeLevelTower = -1;

        public List<bool> weaponsIsBuyed = new List<bool>(new bool[3]);

        public List<BuildArea> buildAreas = new List<BuildArea>();
        public List<BuildedAreaInfo> buildedAreas = new List<BuildedAreaInfo>();
        //public List<int> towersType = new List<int>();
        //public List<BuildedAreaInfo> buildedAreaInfos = new List<BuildedAreaInfo>();
        //public Dictionary<BuildArea, int> buildedAreas = new Dictionary<BuildArea, int>();

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
            weaponsIsBuyed[0] = true;
        }
    }

    [Serializable]
    public class BuildedAreaInfo
    {
        public string name; // Изменено на поле
        public int value;   // Изменено на поле
        public bool isBuilded;

        public int improveLevel = 1;

        // Пустой конструктор для сериализации
        public BuildedAreaInfo() { }

        // Конструктор с параметрами
        public BuildedAreaInfo(string name, int prefabValue, bool flag)
        {
            this.name = name;
            value = prefabValue;
            isBuilded = flag;
        }
    }
}
