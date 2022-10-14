// Decompiled with JetBrains decompiler
// Type: FireLord.FireArrowLogic
// Assembly: FireLord, Version=1.1.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 51633F12-6A5F-46B9-B9AF-55B0B570B321
// Assembly location: C:\Users\andre\Documents\FireLord.dll

using FireLord.Settings;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace FireLord
{
    public class FireArrowLogic : MissionLogic
    {
        private bool _initialized;
        private bool _fireArrowEnabled;
        private IgnitionLogic _ignitionlogic;
        private Dictionary<Mission.Missile, ParticleSystem> _missileParticles = new Dictionary<Mission.Missile, ParticleSystem>();
        private List<FireArrowLogic.ArrowFireData> _hitArrowFires = new List<FireArrowLogic.ArrowFireData>();

        public FireArrowLogic(IgnitionLogic ignitionlogic) => this._ignitionlogic = ignitionlogic;

        public void Initialize()
        {
            float timeOfDay = Mission.Current.Scene.TimeOfDay;
            this._fireArrowEnabled = FireLordConfig.FireArrowAllowedTimeStart > FireLordConfig.FireArrowAllowedTimeEnd ? (double)timeOfDay >= (double)FireLordConfig.FireArrowAllowedTimeStart || (double)timeOfDay <= (double)FireLordConfig.FireArrowAllowedTimeEnd : (double)timeOfDay >= (double)FireLordConfig.FireArrowAllowedTimeStart && (double)timeOfDay <= (double)FireLordConfig.FireArrowAllowedTimeEnd;
            this._fireArrowEnabled = ((this._fireArrowEnabled ? 1 : 0) & (FireLordConfig.UseFireArrowsOnlyInSiege ? (Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.Siege ? 1 : 0) : 1)) != 0;
        }

        public bool IsInBattle() => (Mission.Mode == MissionMode.Battle || ((MissionBehavior)this).Mission.Mode == MissionMode.Duel || ((MissionBehavior)this).Mission.Mode == MissionMode.Stealth || ((MissionBehavior)this).Mission.Mode == MissionMode.Tournament);

        [HandleProcessCorruptedStateExceptions]
        public override void OnMissionTick(float dt)
        {
            if (!this.IsInBattle())
                return;
            if (!this._initialized && Mission.Current != null && Agent.Main != null)
            {
                this._initialized = true;
                this.Initialize();
            }
            if (Input.IsKeyPressed(FireLordConfig.FireArrowToggleKey))
            {
                this._fireArrowEnabled = !this._fireArrowEnabled;
                InformationManager.DisplayMessage(new InformationMessage(((object)GameTexts.FindText("ui_fire_arrow_" + (this._fireArrowEnabled ? "enabled" : "disabled"), (string)null)).ToString()));
            }
            if (this._hitArrowFires.Count <= 0)
                return;
            List<FireArrowLogic.ArrowFireData> arrowFireDataList = new List<FireArrowLogic.ArrowFireData>();
            foreach (FireArrowLogic.ArrowFireData hitArrowFire in this._hitArrowFires)
            {
                try
                {
                    if (hitArrowFire.timer.Check(true))
                    {
                        if (!hitArrowFire.delete)
                        {
                            hitArrowFire.delete = true;
                            GameEntity entity = ((GameEntityComponent)hitArrowFire.bigFireParticle).GetEntity();
                            if (entity != null)
                            {
                                entity.GetLight().Intensity = Math.Max(FireLordConfig.FireArrowLightIntensity - 35f, 0.0f);
                                entity.RemoveComponent((GameEntityComponent)hitArrowFire.bigFireParticle);
                            }
                        }
                        else
                        {
                            GameEntity entity = hitArrowFire.missile.Entity;
                            if (entity != null)
                            {
                                entity.RemoveAllParticleSystems();
                                Light light = entity.GetLight();
                                if (light != null)
                                    entity.RemoveComponent((GameEntityComponent)light);
                            }
                            arrowFireDataList.Add(hitArrowFire);
                        }
                    }
                } catch(Exception ex)
                {
                    ;
                    _hitArrowFires.Remove(hitArrowFire);
                }
            }
            foreach (FireArrowLogic.ArrowFireData arrowFireData in arrowFireDataList)
                this._hitArrowFires.Remove(arrowFireData);
        }

        public override void OnAgentShootMissile(
          Agent shooterAgent,
          EquipmentIndex weaponIndex,
          Vec3 position,
          Vec3 velocity,
          Mat3 orientation,
          bool hasRigidBody,
          int forcedMissileIndex)
        {
            if (!this.IsInBattle() || !this._fireArrowEnabled)
                return;
            bool flag1 = false;
            MissionWeapon missionWeapon = shooterAgent.Equipment[weaponIndex];
            switch (missionWeapon.CurrentUsageItem.WeaponClass - 12)
            {
                case 0:
                case WeaponClass.Dagger:
                case WeaponClass.TwoHandedSword:
                case WeaponClass.OneHandedAxe:
                    flag1 = true;
                    break;
                case WeaponClass.Pick:
                case WeaponClass.TwoHandedMace:
                case WeaponClass.OneHandedPolearm:
                    flag1 = FireLordConfig.AllowFireThrownWeapon;
                    break;
            }
            bool flag2 = ((((flag1 ? 1 : 0) & (FireLordConfig.FireArrowAllowedUnitType == FireLordConfig.UnitType.All || FireLordConfig.FireArrowAllowedUnitType == FireLordConfig.UnitType.Player && shooterAgent == Agent.Main || FireLordConfig.FireArrowAllowedUnitType == FireLordConfig.UnitType.Heroes && shooterAgent.IsHero || FireLordConfig.FireArrowAllowedUnitType == FireLordConfig.UnitType.Companions && shooterAgent.IsHero && shooterAgent.Team.IsPlayerTeam || FireLordConfig.FireArrowAllowedUnitType == FireLordConfig.UnitType.Allies && shooterAgent.Team.IsPlayerAlly ? 1 : (FireLordConfig.FireArrowAllowedUnitType != FireLordConfig.UnitType.Enemies ? 0 : (!shooterAgent.Team.IsPlayerAlly ? 1 : 0)))) != 0 ? 1 : 0) & (shooterAgent == Agent.Main ? 1 : ((double)MBRandom.RandomFloatRanged(100f) < (double)FireLordConfig.ChancesOfFireArrow ? 1 : 0))) != 0;
            if (!flag2)
            {
                switch (FireLordConfig.FireArrowWhitelistType)
                {
                    case FireLordConfig.WhitelistType.Troops:
                        flag2 = FireLordConfig.FireArrowTroopsWhitelist.Contains(((MBObjectBase)shooterAgent.Character).StringId);
                        break;
                    case FireLordConfig.WhitelistType.Items:
                        MissionWeapon wieldedWeapon = shooterAgent.WieldedWeapon;
                        if (!(wieldedWeapon).IsEmpty)
                        {
                            flag2 = FireLordConfig.FireArrowItemsWhitelist.Contains(((object)((MissionWeapon)wieldedWeapon).Item).ToString());
                            WeaponData ammoWeaponData = ((MissionWeapon)wieldedWeapon).GetAmmoWeaponData(true);
                            if (((WeaponData)ammoWeaponData).IsValid())
                            {
                                flag2 |= FireLordConfig.FireArrowItemsWhitelist.Contains(((object)((WeaponData)ammoWeaponData).GetItemObject()).ToString());
                                break;
                            }
                            break;
                        }
                        break;
                }
            }
            if (!flag2)
                return;

            foreach (Mission.Missile missile in Mission.Current.Missiles)
            {
                if (missile.ShooterAgent == shooterAgent && !this._missileParticles.ContainsKey(missile))
                {
                    MatrixFrame localFrame = new MatrixFrame(Mat3.Identity, new Vec3(0, 0, 0));
                    ParticleSystem attachedToEntity = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_game_burning_agent", missile.Entity, ref localFrame);
                    Light pointLight = Light.CreatePointLight(FireLordConfig.FireArrowLightRadius);
                    pointLight.Intensity = FireLordConfig.FireArrowLightIntensity;
                    pointLight.LightColor = FireLordConfig.FireArrowLightColor;
                    missile.Entity.AddLight(pointLight);
                    this._missileParticles.Add(missile, attachedToEntity);
                    break;
                }
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public override void OnMissileCollisionReaction(
          Mission.MissileCollisionReaction collisionReaction,
          Agent attacker,
          Agent victim,
          sbyte attachedBoneIndex)
        {
            if (!this.IsInBattle() || !this._fireArrowEnabled)
                return;
            Dictionary<Mission.Missile, ParticleSystem> dictionary = new Dictionary<Mission.Missile, ParticleSystem>();
            foreach (Mission.Missile missile in Mission.Current.Missiles)
            {
                if (this._missileParticles.ContainsKey(missile))
                {
                    dictionary.Add(missile, this._missileParticles[missile]);
                    this._missileParticles.Remove(missile);
                }
            }
            foreach (KeyValuePair<Mission.Missile, ParticleSystem> missileParticle in this._missileParticles)
            {
                try
                {
                    Mission.Missile key = missileParticle.Key;
                    Light light1 = key.Entity.GetLight(); // here
                    if (victim != null)
                    {
                        key.Entity.RemoveAllParticleSystems();
                        if (light1 != null)
                            key.Entity.RemoveComponent((GameEntityComponent)light1);
                    }
                    else
                    {
                        ParticleSystem particleSystem = missileParticle.Value;
                        MatrixFrame matrixFrame1 = particleSystem.GetLocalFrame();
                        MatrixFrame matrixFrame2 = ((MatrixFrame)matrixFrame1).Elevate(0.6f);
                        particleSystem.SetLocalFrame(ref matrixFrame2);
                        if (light1 != null)
                        {
                            Light light2 = light1;
                            matrixFrame1 = light1.Frame;
                            MatrixFrame matrixFrame3 = ((MatrixFrame)matrixFrame1).Elevate(0.15f);
                            light2.Frame = matrixFrame3;
                            light1.Intensity = FireLordConfig.FireArrowLightIntensity;
                        }
                        FireArrowLogic.ArrowFireData arrowFireData = new FireArrowLogic.ArrowFireData();

                        matrixFrame2 = ((MatrixFrame)matrixFrame1).Elevate(0.6f);
                        arrowFireData.bigFireParticle = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_campfire", key.Entity, ref matrixFrame2);
                        arrowFireData.timer = new MissionTimer(FireLordConfig.StickedArrowsBurningTime / 2f);
                        arrowFireData.missile = key;
                        this._hitArrowFires.Add(arrowFireData);
                    }
                    if (FireLordConfig.IgniteTargetWithFireArrow && victim != null && victim.IsHuman && (FireLordConfig.IgnitionFriendlyFire || attacker.IsEnemyOf(victim)))
                    {
                        float firebarAdd = attachedBoneIndex == (sbyte)18 ? FireLordConfig.IgnitionPerFireArrow / 2f : FireLordConfig.IgnitionPerFireArrow;
                        this._ignitionlogic.IncreaseAgentFireBar(attacker, victim, firebarAdd);
                    }
                } catch(Exception ex)
                {
                    ;
                }
            }
            this._missileParticles = dictionary;
        }

        public class ArrowFireData
        {
            public bool delete;
            public MissionTimer timer;
            public Mission.Missile missile;
            public ParticleSystem bigFireParticle;
        }
    }
}
