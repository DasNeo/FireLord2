using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FireLord.Settings.FireLordConfig;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using MCM.Abstractions.Attributes.v1;
using TaleWorlds.CampaignSystem;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Base.Global;
using MCM.Common;

namespace FireLord.Settings.MCM
{
    public class MCMSettings : AttributeGlobalSettings<MCMSettings>
    {
        public override string Id => "FireLord";
        public override string DisplayName => "Fire Lord";
        public override string FolderName => "FireLord";
        public override string FormatType => "xml";

        [SettingProperty("Fire Sword Toggle Key", RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword")]
        public string fireSwordToggleKey { get; set; } = "C";
        [SettingProperty("Player Fire Sword Default on", HintText = "If enabled the player starts with the fire sword enabled each battle", RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword")]
        public bool playerFireSwordDefaultOn { get; set; } = true;
        [SettingProperty("Ignite Player Body", HintText = "If enabled ignites the player. This is only for visuals.", RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword")]
        public bool ignitePlayerBody { get; set; } = false;
        [SettingProperty("Allowed Unit Types", RequireRestart = false, HintText = "Enable Fire Sword for a group of characters")]
        [SettingPropertyGroup("Fire Sword")]
        public Dropdown<string> fireSwordAllowedUnitType { get; set; } = new Dropdown<string>(new string[]
        {
            "None",
            "Player",
            "Heroes",
            "Companions",
            "Allies",
            "Enemies",
            "All",
        }, 1);
        [SettingProperty("Fire Sword Whitelist Type", HintText = "When set to 0 everyone gets Fire Sword. On 1 only specified Troops get them. On 2 only specified Items get them.", RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword")]
        public Dropdown<string> fireSwordWhitelistType { get; set; } = new Dropdown<string>(new string[]
        {
            "Disabled",
            "Troops",
            "Items"
        }, 0);

        [SettingProperty("Fire Sword Troop Whitelist", HintText = "Comma-separated list of troop names that should get the fire sword.")]
        [SettingPropertyGroup("Fire Sword")]
        public string fireSwordTroopsWhitelist { get; set; } = "";

        [SettingProperty("Fire Sword Item Whitelist", HintText = "Comma-separated list of items that are fire swords.", RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword")]
        public string fireSwordItemsWhitelist { get; set; } = "";

        [SettingProperty("Fire Sword Light Color Red", 0, 255, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Fire Sword Light/Color")]
        public int fireSwordLightColorR { get; set; } = 216;
        [SettingProperty("Fire Sword Light Color Green", 0, 255, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Fire Sword Light/Color")]
        public int fireSwordLightColorG { get; set; } = 138;
        [SettingProperty("Fire Sword Light Color Blue", 0, 255, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Fire Sword Light/Color")]
        public int fireSwordLightColorB { get; set; } = 0;

        [SettingProperty("Fire Sword Light Radius", 0, 20, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Fire Sword Light")]
        public float fireSwordLightRadius { get; set; } = 5f;
        [SettingProperty("Fire Sword Light Intensity", 0, 10, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Fire Sword Light")]
        public float fireSwordLightIntensity { get; set; } = 4f;
        [SettingProperty("Fire Sword ignites Target", RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition")]
        public bool igniteTargetWithFireSword { get; set; } = true;
        [SettingProperty("Max value of Ignition Bar", 0, 10000, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition")]
        public float ignitionBarMax { get; set; } = 100f;
        [SettingProperty("Ignition per Sword hit", 0, 10000, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition")]
        public float ignitionPerFireSwordHit { get; set; } = 100f;
        [SettingProperty("Ignition drop per second", 0, 10000, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition")]
        public float ignitionDropPerSecond { get; set; } = 10f;
        [SettingProperty("Ignition duration", 0, 1000, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition")]
        public float ignitionDurationInSecond { get; set; } = 5f;
        [SettingProperty("Ignition Light Color Red", 0, 255, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition/Light/Color")]
        public int ignitionLightColorR { get; set; } = 216;
        [SettingProperty("Ignition Light Color Green", 0, 255, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition/Light/Color")]
        public int ignitionLightColorG { get; set; } = 138;
        [SettingProperty("Ignition Light Color Blue", 0, 255, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition/Light/Color")]
        public int ignitionLightColorB { get; set; } = 0;
        [SettingProperty("Ignition Light Radius", 0, 20, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition/Light")]
        public float ignitionLightRadius { get; set; } = 7f;
        [SettingProperty("Ignition Light Intensity", 0, 10, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition/Light")]
        public float ignitionLightIntensity { get; set; } = 4f;
        [SettingProperty("Ignition deals damage", RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition")]
        public bool ignitionDealDamage { get; set; } = true;
        [SettingProperty("Ignition friendly fire", RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition")]
        public bool ignitionFriendlyFire { get; set; } = true;
        [SettingProperty("Ignition damage per second", 0, 100, RequireRestart = false)]
        [SettingPropertyGroup("Fire Sword/Ignition")]
        public int ignitionDamagePerSecond { get; set; } = 10;
    }
}
