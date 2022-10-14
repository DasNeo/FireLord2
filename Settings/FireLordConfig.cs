// Decompiled with JetBrains decompiler
// Type: FireLord.Settings.FireLordConfig
// Assembly: FireLord, Version=1.1.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 51633F12-6A5F-46B9-B9AF-55B0B570B321
// Assembly location: C:\Users\andre\Documents\FireLord.dll

using FireLord.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace FireLord.Settings
{
    public static class FireLordConfig
    {
        public static string ConfigPath = BasePath.Name + "Modules/FireLord/FireLordConfig.ini";
        public static IniHelper iniHelper = new IniHelper(FireLordConfig.ConfigPath, "Fire Lord");
        private static InputKey _fireArrowToggleKey;
        private static int _fireArrowAllowedTimeStart;
        private static int _fireArrowAllowedTimeEnd;
        private static bool _useFireArrowsOnlyInSiege;
        private static bool _allowFireThrownWeapon;
        private static FireLordConfig.UnitType _fireArrowAllowedUnitType;
        private static FireLordConfig.WhitelistType _fireArrowWhitelistType = FireLordConfig.WhitelistType.Disabled;
        private static List<string> _fireArrowTroopsWhitelist = new List<string>();
        private static List<string> _fireArrowItemsWhitelist = new List<string>();
        private static float _chancesOfFireArrow;
        private static float _stickedArrowsBurningTime;
        private static Vec3 _fireArrowLightColor;
        private static float _fireArrowLightRadius;
        private static float _fireArrowLightIntensity;
        private static InputKey _fireSwordToggleKey;
        private static bool _playerFireSwordDefaultOn;
        private static bool _ignitePlayerBody;
        private static FireLordConfig.UnitType _fireSwordAllowedUnitType;
        private static FireLordConfig.WhitelistType _fireSwordWhitelistType = FireLordConfig.WhitelistType.Disabled;
        private static List<string> _fireSwordTroopsWhitelist = new List<string>();
        private static List<string> _fireSwordItemsWhitelist = new List<string>();
        private static Vec3 _fireSwordLightColor;
        private static float _fireSwordLightRadius;
        private static float _fireSwordLightIntensity;
        private static bool _igniteTargetWithFireArrow;
        private static bool _igniteTargetWithFireSword;
        private static float _ignitionBarMax;
        private static float _ignitionPerFireArrow;
        private static float _ignitionPerFireSwordHit;
        private static float _ignitionDropPerSecond;
        private static float _ignitionDurationInSecond;
        private static Vec3 _ignitionLightColor;
        private static float _ignitionLightRadius;
        private static float _ignitionLightIntensity;
        private static bool _ignitionDealDamage;
        private static bool _ignitionFriendlyFire;
        private static int _ignitionDamagePerSecond;

        public static InputKey FireArrowToggleKey
        {
            get => FireLordConfig._fireArrowToggleKey;
            set
            {
                FireLordConfig._fireArrowToggleKey = value;
                FireLordConfig.iniHelper.Set(nameof(FireArrowToggleKey), value.ToString());
            }
        }

        public static int FireArrowAllowedTimeStart
        {
            get => FireLordConfig._fireArrowAllowedTimeStart;
            set
            {
                FireLordConfig._fireArrowAllowedTimeStart = value;
                FireLordConfig.iniHelper.SetInt(nameof(FireArrowAllowedTimeStart), value);
            }
        }

        public static int FireArrowAllowedTimeEnd
        {
            get => FireLordConfig._fireArrowAllowedTimeEnd;
            set
            {
                FireLordConfig._fireArrowAllowedTimeEnd = value;
                FireLordConfig.iniHelper.SetInt(nameof(FireArrowAllowedTimeEnd), value);
            }
        }

        public static bool UseFireArrowsOnlyInSiege
        {
            get => FireLordConfig._useFireArrowsOnlyInSiege;
            set
            {
                FireLordConfig._useFireArrowsOnlyInSiege = value;
                FireLordConfig.iniHelper.SetBool(nameof(UseFireArrowsOnlyInSiege), value);
            }
        }

        public static bool AllowFireThrownWeapon
        {
            get => FireLordConfig._allowFireThrownWeapon;
            set
            {
                FireLordConfig._allowFireThrownWeapon = value;
                FireLordConfig.iniHelper.SetBool(nameof(AllowFireThrownWeapon), value);
            }
        }

        public static FireLordConfig.UnitType FireArrowAllowedUnitType
        {
            get => FireLordConfig._fireArrowAllowedUnitType;
            set
            {
                FireLordConfig._fireArrowAllowedUnitType = value;
                FireLordConfig.iniHelper.SetInt(nameof(FireArrowAllowedUnitType), (int)value);
            }
        }

        public static FireLordConfig.WhitelistType FireArrowWhitelistType
        {
            get => FireLordConfig._fireArrowWhitelistType;
            set
            {
                FireLordConfig._fireArrowWhitelistType = value;
                FireLordConfig.iniHelper.SetInt(nameof(FireArrowWhitelistType), (int)value);
            }
        }

        public static List<string> FireArrowTroopsWhitelist
        {
            get => FireLordConfig._fireArrowTroopsWhitelist;
            set
            {
                FireLordConfig._fireArrowTroopsWhitelist = value;
                FireLordConfig.iniHelper.Set(nameof(FireArrowTroopsWhitelist), string.Join(",", value.ToArray()));
            }
        }

        public static List<string> FireArrowItemsWhitelist
        {
            get => FireLordConfig._fireArrowItemsWhitelist;
            set
            {
                FireLordConfig._fireArrowItemsWhitelist = value;
                FireLordConfig.iniHelper.Set(nameof(FireArrowItemsWhitelist), string.Join(",", value.ToArray()));
            }
        }

        public static float ChancesOfFireArrow
        {
            get => FireLordConfig._chancesOfFireArrow;
            set
            {
                FireLordConfig._chancesOfFireArrow = value;
                FireLordConfig.iniHelper.SetInt(nameof(ChancesOfFireArrow), (int)value);
            }
        }

        public static float StickedArrowsBurningTime
        {
            get => FireLordConfig._stickedArrowsBurningTime;
            set
            {
                FireLordConfig._stickedArrowsBurningTime = value;
                FireLordConfig.iniHelper.SetInt(nameof(StickedArrowsBurningTime), (int)value);
            }
        }

        public static Vec3 FireArrowLightColor
        {
            get => FireLordConfig._fireArrowLightColor;
            set
            {
                FireLordConfig._fireArrowLightColor = value;
                FireLordConfig.iniHelper.SetInt("FireArrowLightColorR", (int)Math.Round((double)value.x * (double)byte.MaxValue));
                FireLordConfig.iniHelper.SetInt("FireArrowLightColorG", (int)Math.Round((double)value.y * (double)byte.MaxValue));
                FireLordConfig.iniHelper.SetInt("FireArrowLightColorB", (int)Math.Round((double)value.z * (double)byte.MaxValue));
            }
        }

        public static float FireArrowLightRadius
        {
            get => FireLordConfig._fireArrowLightRadius;
            set
            {
                FireLordConfig._fireArrowLightRadius = value;
                FireLordConfig.iniHelper.SetFloat(nameof(FireArrowLightRadius), value);
            }
        }

        public static float FireArrowLightIntensity
        {
            get => FireLordConfig._fireArrowLightIntensity;
            set
            {
                FireLordConfig._fireArrowLightIntensity = value;
                FireLordConfig.iniHelper.SetFloat(nameof(FireArrowLightIntensity), value);
            }
        }

        public static InputKey FireSwordToggleKey
        {
            get => FireLordConfig._fireSwordToggleKey;
            set
            {
                FireLordConfig._fireSwordToggleKey = value;
                FireLordConfig.iniHelper.Set(nameof(FireSwordToggleKey), value.ToString());
            }
        }

        public static bool PlayerFireSwordDefaultOn
        {
            get => FireLordConfig._playerFireSwordDefaultOn;
            set
            {
                FireLordConfig._playerFireSwordDefaultOn = value;
                FireLordConfig.iniHelper.SetBool(nameof(PlayerFireSwordDefaultOn), value);
            }
        }

        public static bool IgnitePlayerBody
        {
            get => FireLordConfig._ignitePlayerBody;
            set
            {
                FireLordConfig._ignitePlayerBody = value;
                FireLordConfig.iniHelper.SetBool(nameof(IgnitePlayerBody), value);
            }
        }

        public static FireLordConfig.UnitType FireSwordAllowedUnitType
        {
            get => FireLordConfig._fireSwordAllowedUnitType;
            set
            {
                FireLordConfig._fireSwordAllowedUnitType = value;
                FireLordConfig.iniHelper.SetInt(nameof(FireSwordAllowedUnitType), (int)value);
            }
        }

        public static FireLordConfig.WhitelistType FireSwordWhitelistType
        {
            get => FireLordConfig._fireSwordWhitelistType;
            set
            {
                FireLordConfig._fireSwordWhitelistType = value;
                FireLordConfig.iniHelper.SetInt(nameof(FireSwordWhitelistType), (int)value);
            }
        }

        public static List<string> FireSwordTroopsWhitelist
        {
            get => FireLordConfig._fireSwordTroopsWhitelist;
            set
            {
                FireLordConfig._fireSwordTroopsWhitelist = value;
                FireLordConfig.iniHelper.Set(nameof(FireSwordTroopsWhitelist), string.Join(",", value.ToArray()));
            }
        }

        public static List<string> FireSwordItemsWhitelist
        {
            get => FireLordConfig._fireSwordItemsWhitelist;
            set
            {
                FireLordConfig._fireSwordItemsWhitelist = value;
                FireLordConfig.iniHelper.Set(nameof(FireSwordItemsWhitelist), string.Join(",", value.ToArray()));
            }
        }

        public static Vec3 FireSwordLightColor
        {
            get => FireLordConfig._fireSwordLightColor;
            set
            {
                FireLordConfig._fireSwordLightColor = value;
                FireLordConfig.iniHelper.SetInt("FireSwordLightColorR", (int)Math.Round((double)value.x * (double)byte.MaxValue));
                FireLordConfig.iniHelper.SetInt("FireSwordLightColorG", (int)Math.Round((double)value.y * (double)byte.MaxValue));
                FireLordConfig.iniHelper.SetInt("FireSwordLightColorB", (int)Math.Round((double)value.z * (double)byte.MaxValue));
            }
        }

        public static float FireSwordLightRadius
        {
            get => FireLordConfig._fireSwordLightRadius;
            set
            {
                FireLordConfig._fireSwordLightRadius = value;
                FireLordConfig.iniHelper.SetFloat(nameof(FireSwordLightRadius), value);
            }
        }

        public static float FireSwordLightIntensity
        {
            get => FireLordConfig._fireSwordLightIntensity;
            set
            {
                FireLordConfig._fireSwordLightIntensity = value;
                FireLordConfig.iniHelper.SetFloat(nameof(FireSwordLightIntensity), value);
            }
        }

        public static bool IgniteTargetWithFireArrow
        {
            get => FireLordConfig._igniteTargetWithFireArrow;
            set
            {
                FireLordConfig._igniteTargetWithFireArrow = value;
                FireLordConfig.iniHelper.SetBool(nameof(IgniteTargetWithFireArrow), value);
            }
        }

        public static bool IgniteTargetWithFireSword
        {
            get => FireLordConfig._igniteTargetWithFireSword;
            set
            {
                FireLordConfig._igniteTargetWithFireSword = value;
                FireLordConfig.iniHelper.SetBool(nameof(IgniteTargetWithFireSword), value);
            }
        }

        public static float IgnitionBarMax
        {
            get => FireLordConfig._ignitionBarMax;
            set
            {
                FireLordConfig._ignitionBarMax = value;
                FireLordConfig.iniHelper.SetFloat(nameof(IgnitionBarMax), value);
            }
        }

        public static float IgnitionPerFireArrow
        {
            get => FireLordConfig._ignitionPerFireArrow;
            set
            {
                FireLordConfig._ignitionPerFireArrow = value;
                FireLordConfig.iniHelper.SetFloat(nameof(IgnitionPerFireArrow), value);
            }
        }

        public static float IgnitionPerFireSwordHit
        {
            get => FireLordConfig._ignitionPerFireSwordHit;
            set
            {
                FireLordConfig._ignitionPerFireSwordHit = value;
                FireLordConfig.iniHelper.SetFloat(nameof(IgnitionPerFireSwordHit), value);
            }
        }

        public static float IgnitionDropPerSecond
        {
            get => FireLordConfig._ignitionDropPerSecond;
            set
            {
                FireLordConfig._ignitionDropPerSecond = value;
                FireLordConfig.iniHelper.SetFloat(nameof(IgnitionDropPerSecond), value);
            }
        }

        public static float IgnitionDurationInSecond
        {
            get => FireLordConfig._ignitionDurationInSecond;
            set
            {
                FireLordConfig._ignitionDurationInSecond = value;
                FireLordConfig.iniHelper.SetFloat(nameof(IgnitionDurationInSecond), value);
            }
        }

        public static Vec3 IgnitionLightColor
        {
            get => FireLordConfig._ignitionLightColor;
            set
            {
                FireLordConfig._ignitionLightColor = value;
                FireLordConfig.iniHelper.SetInt("IgnitionLightColorR", (int)Math.Round((double)value.x * (double)byte.MaxValue));
                FireLordConfig.iniHelper.SetInt("IgnitionLightColorG", (int)Math.Round((double)value.y * (double)byte.MaxValue));
                FireLordConfig.iniHelper.SetInt("IgnitionLightColorB", (int)Math.Round((double)value.z * (double)byte.MaxValue));
            }
        }

        public static float IgnitionLightRadius
        {
            get => FireLordConfig._ignitionLightRadius;
            set
            {
                FireLordConfig._ignitionLightRadius = value;
                FireLordConfig.iniHelper.SetFloat(nameof(IgnitionLightRadius), value);
            }
        }

        public static float IgnitionLightIntensity
        {
            get => FireLordConfig._ignitionLightIntensity;
            set
            {
                FireLordConfig._ignitionLightIntensity = value;
                FireLordConfig.iniHelper.SetFloat(nameof(IgnitionLightIntensity), value);
            }
        }

        public static bool IgnitionDealDamage
        {
            get => FireLordConfig._ignitionDealDamage;
            set
            {
                FireLordConfig._ignitionDealDamage = value;
                FireLordConfig.iniHelper.SetBool(nameof(IgnitionDealDamage), value);
            }
        }

        public static bool IgnitionFriendlyFire
        {
            get => FireLordConfig._ignitionFriendlyFire;
            set
            {
                FireLordConfig._ignitionFriendlyFire = value;
                FireLordConfig.iniHelper.SetBool(nameof(IgnitionFriendlyFire), value);
            }
        }

        public static int IgnitionDamagePerSecond
        {
            get => FireLordConfig._ignitionDamagePerSecond;
            set
            {
                FireLordConfig._ignitionDamagePerSecond = value;
                FireLordConfig.iniHelper.SetInt(nameof(IgnitionDamagePerSecond), value);
            }
        }

        public static void Init()
        {
            if (!File.Exists(FireLordConfig.ConfigPath))
            {
                FireLordConfig.FireArrowToggleKey = (InputKey)47;
                FireLordConfig.FireArrowAllowedTimeStart = 0;
                FireLordConfig.FireArrowAllowedTimeEnd = 24;
                FireLordConfig.UseFireArrowsOnlyInSiege = false;
                FireLordConfig.AllowFireThrownWeapon = true;
                FireLordConfig.FireArrowAllowedUnitType = FireLordConfig.UnitType.All;
                FireLordConfig.FireArrowWhitelistType = FireLordConfig.WhitelistType.Disabled;
                FireLordConfig.FireArrowTroopsWhitelist = new List<string>();
                FireLordConfig.FireArrowItemsWhitelist = new List<string>();
                FireLordConfig.ChancesOfFireArrow = 100f;
                FireLordConfig.StickedArrowsBurningTime = 8f;
                FireLordConfig.FireArrowLightColor = new Vec3(0.847f, 0.541f, 0.0f, -1f);
                FireLordConfig.FireArrowLightRadius = 3f;
                FireLordConfig.FireArrowLightIntensity = 120f;
                FireLordConfig.FireSwordToggleKey = (InputKey)46;
                FireLordConfig.PlayerFireSwordDefaultOn = false;
                FireLordConfig.IgnitePlayerBody = false;
                FireLordConfig.FireSwordAllowedUnitType = FireLordConfig.UnitType.All;
                FireLordConfig.FireSwordWhitelistType = FireLordConfig.WhitelistType.Disabled;
                FireLordConfig.FireSwordTroopsWhitelist = new List<string>();
                FireLordConfig.FireSwordItemsWhitelist = new List<string>();
                FireLordConfig.FireSwordLightColor = new Vec3(0.847f, 0.541f, 0.0f, -1f);
                FireLordConfig.FireSwordLightRadius = 5f;
                FireLordConfig.FireSwordLightIntensity = 85f;
                FireLordConfig.IgniteTargetWithFireArrow = true;
                FireLordConfig.IgniteTargetWithFireSword = true;
                FireLordConfig.IgnitionBarMax = 100f;
                FireLordConfig.IgnitionPerFireArrow = 75f;
                FireLordConfig.IgnitionPerFireSwordHit = 100f;
                FireLordConfig.IgnitionDropPerSecond = 10f;
                FireLordConfig.IgnitionLightColor = new Vec3(0.847f, 0.541f, 0.0f, -1f);
                FireLordConfig.IgnitionLightRadius = 7f;
                FireLordConfig.IgnitionLightIntensity = 125f;
                FireLordConfig.IgnitionDurationInSecond = 5f;
                FireLordConfig.IgnitionDealDamage = true;
                FireLordConfig.IgnitionFriendlyFire = false;
                FireLordConfig.IgnitionDamagePerSecond = 10;
            }
            else
            {
                FireLordConfig._fireArrowToggleKey = (InputKey)Enum.Parse(typeof(InputKey), FireLordConfig.iniHelper.Get("FireArrowToggleKey", "V"));
                FireLordConfig._fireArrowAllowedTimeStart = FireLordConfig.iniHelper.GetInt("FireArrowAllowedTimeStart");
                FireLordConfig._fireArrowAllowedTimeEnd = FireLordConfig.iniHelper.GetInt("FireArrowAllowedTimeEnd", 24);
                FireLordConfig._useFireArrowsOnlyInSiege = FireLordConfig.iniHelper.GetBool("UseFireArrowsOnlyInSiege", true);
                FireLordConfig._allowFireThrownWeapon = FireLordConfig.iniHelper.GetBool("AllowFireThrownWeapon", true);
                FireLordConfig._fireArrowAllowedUnitType = (FireLordConfig.UnitType)FireLordConfig.iniHelper.GetInt("FireArrowAllowedUnitType", 6);
                FireLordConfig._fireArrowWhitelistType = (FireLordConfig.WhitelistType)FireLordConfig.iniHelper.GetInt("FireArrowWhitelistType");
                FireLordConfig._fireArrowTroopsWhitelist = new List<string>((IEnumerable<string>)FireLordConfig.iniHelper.Get("FireArrowTroopsWhitelist").Split(new string[1]
                {
          ","
                }, StringSplitOptions.RemoveEmptyEntries));
                FireLordConfig._fireArrowItemsWhitelist = new List<string>((IEnumerable<string>)FireLordConfig.iniHelper.Get("FireArrowItemsWhitelist").Split(new string[1]
                {
          ","
                }, StringSplitOptions.RemoveEmptyEntries));
                FireLordConfig._chancesOfFireArrow = FireLordConfig.iniHelper.GetFloat("ChancesOfFireArrow", 100f);
                FireLordConfig._stickedArrowsBurningTime = FireLordConfig.iniHelper.GetFloat("StickedArrowsBurningTime", 8f);
                FireLordConfig._fireArrowLightColor = new Vec3((float)FireLordConfig.iniHelper.GetInt("FireArrowLightColorR", 216) / (float)byte.MaxValue, (float)FireLordConfig.iniHelper.GetInt("FireArrowLightColorG", 138) / (float)byte.MaxValue, (float)FireLordConfig.iniHelper.GetInt("FireArrowLightColorB") / (float)byte.MaxValue, -1f);
                FireLordConfig._fireArrowLightRadius = FireLordConfig.iniHelper.GetFloat("FireArrowLightRadius", 3f);
                FireLordConfig._fireArrowLightIntensity = FireLordConfig.iniHelper.GetFloat("FireArrowLightIntensity", 120f);
                FireLordConfig._fireSwordToggleKey = (InputKey)Enum.Parse(typeof(InputKey), FireLordConfig.iniHelper.Get("FireSwordToggleKey", "C"));
                FireLordConfig._playerFireSwordDefaultOn = FireLordConfig.iniHelper.GetBool("PlayerFireSwordDefaultOn");
                FireLordConfig._ignitePlayerBody = FireLordConfig.iniHelper.GetBool("IgnitePlayerBody");
                FireLordConfig._fireSwordAllowedUnitType = (FireLordConfig.UnitType)FireLordConfig.iniHelper.GetInt("FireSwordAllowedUnitType", 6);
                FireLordConfig._fireSwordWhitelistType = (FireLordConfig.WhitelistType)FireLordConfig.iniHelper.GetInt("FireSwordWhitelistType");
                FireLordConfig._fireSwordTroopsWhitelist = new List<string>((IEnumerable<string>)FireLordConfig.iniHelper.Get("FireSwordTroopsWhitelist").Split(new string[1]
                {
          ","
                }, StringSplitOptions.RemoveEmptyEntries));
                FireLordConfig._fireSwordItemsWhitelist = new List<string>((IEnumerable<string>)FireLordConfig.iniHelper.Get("FireSwordItemsWhitelist").Split(new string[1]
                {
          ","
                }, StringSplitOptions.RemoveEmptyEntries));
                FireLordConfig._fireSwordLightColor = new Vec3((float)FireLordConfig.iniHelper.GetInt("FireSwordLightColorR", 216) / (float)byte.MaxValue, (float)FireLordConfig.iniHelper.GetInt("FireSwordLightColorG", 138) / (float)byte.MaxValue, (float)FireLordConfig.iniHelper.GetInt("FireSwordLightColorB") / (float)byte.MaxValue, -1f);
                FireLordConfig._fireSwordLightRadius = FireLordConfig.iniHelper.GetFloat("FireSwordLightRadius", 5f);
                FireLordConfig._fireSwordLightIntensity = FireLordConfig.iniHelper.GetFloat("FireSwordLightIntensity", 85f);
                FireLordConfig._igniteTargetWithFireArrow = FireLordConfig.iniHelper.GetBool("IgniteTargetWithFireArrow", true);
                FireLordConfig._igniteTargetWithFireSword = FireLordConfig.iniHelper.GetBool("IgniteTargetWithFireSword", true);
                FireLordConfig._ignitionBarMax = FireLordConfig.iniHelper.GetFloat("IgnitionBarMax", 100f);
                FireLordConfig._ignitionPerFireArrow = FireLordConfig.iniHelper.GetFloat("IgnitionPerFireArrow", 75f);
                FireLordConfig._ignitionPerFireSwordHit = FireLordConfig.iniHelper.GetFloat("IgnitionPerFireSwordHit", 100f);
                FireLordConfig._ignitionDropPerSecond = FireLordConfig.iniHelper.GetFloat("IgnitionDropPerSecond", 10f);
                FireLordConfig._ignitionLightColor = new Vec3((float)FireLordConfig.iniHelper.GetInt("IgnitionLightColorR", 216) / (float)byte.MaxValue, (float)FireLordConfig.iniHelper.GetInt("IgnitionLightColorG", 138) / (float)byte.MaxValue, (float)FireLordConfig.iniHelper.GetInt("IgnitionLightColorB") / (float)byte.MaxValue, -1f);
                FireLordConfig._ignitionLightRadius = FireLordConfig.iniHelper.GetFloat("IgnitionLightRadius", 7f);
                FireLordConfig._ignitionLightIntensity = FireLordConfig.iniHelper.GetFloat("IgnitionLightIntensity", 125f);
                FireLordConfig._ignitionDurationInSecond = FireLordConfig.iniHelper.GetFloat("IgnitionDurationInSecond", 5f);
                FireLordConfig._ignitionDealDamage = FireLordConfig.iniHelper.GetBool("IgnitionDealDamage", true);
                FireLordConfig._ignitionFriendlyFire = FireLordConfig.iniHelper.GetBool("IgnitionFriendlyFire", true);
                FireLordConfig._ignitionDamagePerSecond = FireLordConfig.iniHelper.GetInt("IgnitionDamagePerSecond", 10);
            }
        }

        public enum UnitType
        {
            None,
            Player,
            Heroes,
            Companions,
            Allies,
            Enemies,
            All,
        }

        public enum WhitelistType
        {
            Disabled,
            Troops,
            Items,
        }
    }
}
