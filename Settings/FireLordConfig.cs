using FireLord.Settings.MCM;
using FireLord.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace FireLord.Settings
{
    public static class FireLordConfig
    {
        public static string ConfigPath = BasePath.Name + "Modules/FireLord/FireLordConfig.ini";
        public static IniHelper iniHelper = new IniHelper(ConfigPath, "Fire Lord");

        public static InputKey FireSwordToggleKey
        {
            get => (InputKey)Enum.Parse(typeof(InputKey), MCMSettings.Instance.fireSwordToggleKey);
        }

        public static bool PlayerFireSwordDefaultOn
        {
            get => MCMSettings.Instance.playerFireSwordDefaultOn;
        }

        public static bool IgnitePlayerBody
        {
            get => MCMSettings.Instance.ignitePlayerBody;
        }

        public static UnitType FireSwordAllowedUnitType
        {
            get => (UnitType)Enum.Parse(typeof(UnitType), MCMSettings.Instance.fireSwordAllowedUnitType.SelectedValue);
        }

        public static WhitelistType FireSwordWhitelistType
        {
            get => (WhitelistType)Enum.Parse(typeof(WhitelistType), MCMSettings.Instance.fireSwordWhitelistType.SelectedValue);
        }

        public static List<string> FireSwordTroopsWhitelist
        {
            get
            {
                var troops = MCMSettings.Instance.fireSwordTroopsWhitelist.Split(',');
                return troops.ToList();
            }
        }

        public static List<string> FireSwordItemsWhitelist
        {
            get
            {
                var items = MCMSettings.Instance.fireSwordItemsWhitelist.Split(',');
                return items.ToList();
            }
        }
        private static int fireSwordLightColorR { get => MCMSettings.Instance.fireSwordLightColorR; }
        private static int fireSwordLightColorG { get => MCMSettings.Instance.fireSwordLightColorG; }
        private static int fireSwordLightColorB { get => MCMSettings.Instance.fireSwordLightColorB; }
        public static Vec3 FireSwordLightColor
        {
            get => new Vec3(fireSwordLightColorR, fireSwordLightColorG, fireSwordLightColorB);
        }

        public static float FireSwordLightRadius
        {
            get => MCMSettings.Instance.fireSwordLightRadius;
        }

        public static float FireSwordLightIntensity
        {
            get => MCMSettings.Instance.fireSwordLightIntensity;
        }

        public static bool IgniteTargetWithFireSword
        {
            get => MCMSettings.Instance.igniteTargetWithFireSword;
        }

        public static float IgnitionBarMax
        {
            get => MCMSettings.Instance.ignitionBarMax;
        }

        public static float IgnitionPerFireSwordHit
        {
            get => MCMSettings.Instance.ignitionPerFireSwordHit;
        }

        public static float IgnitionDropPerSecond
        {
            get => MCMSettings.Instance.ignitionDropPerSecond;
        }

        public static float IgnitionDurationInSecond
        {
            get => MCMSettings.Instance.ignitionDurationInSecond;
        }

        private static int ignitionLightColorR { get => MCMSettings.Instance.ignitionLightColorR; }
        private static int ignitionLightColorG { get => MCMSettings.Instance.ignitionLightColorG; }
        private static int ignitionLightColorB { get => MCMSettings.Instance.ignitionLightColorB; }

        public static Vec3 IgnitionLightColor
        {
            get => new Vec3(ignitionLightColorR, ignitionLightColorG, ignitionLightColorB);
        }

        public static float IgnitionLightRadius
        {
            get => MCMSettings.Instance.ignitionLightRadius;
        }

        public static float IgnitionLightIntensity
        {
            get => MCMSettings.Instance.ignitionLightIntensity;
        }

        public static bool IgnitionDealDamage
        {
            get => MCMSettings.Instance.ignitionDealDamage;
        }

        public static bool IgnitionFriendlyFire
        {
            get => MCMSettings.Instance.ignitionFriendlyFire;
        }

        public static int IgnitionDamagePerSecond
        {
            get => MCMSettings.Instance.ignitionDamagePerSecond;
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
