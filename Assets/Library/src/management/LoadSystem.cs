using System;
using System.Collections.Generic;
using System.Linq;
using Library.src.combat.Weapon;
using Library.src.management.units;
using Library.src.management.weapons;
using Library.src.util;
using UnityEngine;

namespace Library.src.management
{
    public class LoadSystem : MonoBehaviour
    {
        public static Dictionary<string, UnitLoadData> LoadUnits()
        {
            if (!(Resources.Load(EnvironmentUtil.RESOURCE_PLAYER_UNIT) is TextAsset playerJson)) throw new Exception(); //TODO Game shouldn't load.
            var units = JsonUtility.FromJson<UnitWrapper>(playerJson.text).units;
            if (!(Resources.Load(EnvironmentUtil.RESOURCE_HOSTILE_UNIT) is TextAsset hostileJson)) throw new Exception(); //TODO Game shouldn't load.
            units.AddRange(JsonUtility.FromJson<UnitWrapper>(hostileJson.text).units);

            return units.ToDictionary(unit => unit.name);
            
        }
        
        public static Dictionary<int, Weapon> LoadWeapons()
        {
            if (!(Resources.Load(EnvironmentUtil.RESOURCE_WEAPONS) is TextAsset weaponJson)) throw new Exception(); //TODO Game shouldn't load.
            return JsonUtility
                .FromJson<WeaponDataWrapper>(weaponJson.text)
                .weapons.ToDictionary(weapon => weapon.id);
        }
    }
}