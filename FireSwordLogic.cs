// Decompiled with JetBrains decompiler
// Type: FireLord.FireSwordLogic
// Assembly: FireLord, Version=1.1.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 51633F12-6A5F-46B9-B9AF-55B0B570B321
// Assembly location: C:\Users\andre\Documents\FireLord.dll

using FireLord.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace FireLord
{
    internal class FireSwordLogic : MissionLogic
    {
        private static bool _playerFireSwordEnabled;
        private IgnitionLogic _ignitionlogic;
        private Dictionary<Agent, FireSwordLogic.AgentFireSwordData> _agentFireSwordData = new Dictionary<Agent, FireSwordLogic.AgentFireSwordData>();

        public FireSwordLogic(IgnitionLogic ignitionlogic)
        {
            this._ignitionlogic = ignitionlogic;
            this._ignitionlogic.OnAgentDropItem += new IgnitionLogic.OnAgentDropItemDelegate(this.SetDropLockForAgent);
            FireSwordLogic._playerFireSwordEnabled = FireLordConfig.PlayerFireSwordDefaultOn;
        }

        public void SetDropLockForAgent(Agent agent, bool dropLock)
        {
            FireSwordLogic.AgentFireSwordData agentFireSwordData;
            this._agentFireSwordData.TryGetValue(agent, out agentFireSwordData);
            if (agentFireSwordData == null)
                return;
            agentFireSwordData.dropLock = dropLock;
        }

        public bool IsInBattle() => ((MissionBehavior)this).Mission.IsFieldBattle || ((MissionBehavior)this).Mission.IsSiegeBattle || !((MissionBehavior)this).Mission.IsFriendlyMission || Mission.Mode == MissionMode.Duel || Mission.Mode == MissionMode.Stealth || Mission.Mode == MissionMode.Tournament;

        public override void OnAgentCreated(Agent agent)
        {
            if (!this.IsInBattle() || !agent.IsHuman || this._agentFireSwordData.ContainsKey(agent))
                return;
            FireSwordLogic.AgentFireSwordData agentFireSwordData1 = new FireSwordLogic.AgentFireSwordData();
            agentFireSwordData1.agent = agent;
            agent.OnAgentWieldedItemChange += new Action(agentFireSwordData1.OnAgentWieldedItemChange);
            // ISSUE: method pointer
            agent.OnAgentHealthChanged += agentFireSwordData1.OnAgentHealthChanged;
            FireSwordLogic.AgentFireSwordData agentFireSwordData2 = agentFireSwordData1;
            MissionWeapon wieldedWeapon = agent.WieldedWeapon;
            int num = ((MissionWeapon)wieldedWeapon).IsEmpty ? 1 : 0;
            agentFireSwordData2.lastWieldedWeaponEmpty = num != 0;
            if (!agent.IsMainAgent || FireSwordLogic._playerFireSwordEnabled)
                agentFireSwordData1.timer = new MissionTimer(1f);
            this._agentFireSwordData.Add(agent, agentFireSwordData1);
        }

        public override void OnAgentDeleted(Agent agent)
        {
            if (!this.IsInBattle())
                return;
            this._agentFireSwordData.Remove(agent);
        }

        public override void OnMissionTick(float dt)
        {
            if (!this.IsInBattle())
                return;
            foreach (KeyValuePair<Agent, FireSwordLogic.AgentFireSwordData> keyValuePair in this._agentFireSwordData)
            {
                FireSwordLogic.AgentFireSwordData agentFireSwordData = keyValuePair.Value;
                if (keyValuePair.Key.IsMainAgent && !FireSwordLogic._playerFireSwordEnabled)
                    agentFireSwordData.timer = (MissionTimer)null;
                else if (agentFireSwordData.timer != null && agentFireSwordData.timer.Check(false))
                {
                    agentFireSwordData.SetFireSwordEnable(true);
                    agentFireSwordData.timer = (MissionTimer)null;
                }
            }
            if (!Input.IsKeyPressed(FireLordConfig.FireSwordToggleKey) || Agent.Main == null)
                return;
            FireSwordLogic._playerFireSwordEnabled = !FireSwordLogic._playerFireSwordEnabled;
            foreach (KeyValuePair<Agent, FireSwordLogic.AgentFireSwordData> keyValuePair in this._agentFireSwordData)
            {
                if (keyValuePair.Key.IsPlayerControlled)
                {
                    FireSwordLogic.AgentFireSwordData agentFireSwordData = keyValuePair.Value;
                    agentFireSwordData.SetFireSwordEnable(FireSwordLogic._playerFireSwordEnabled);
                    agentFireSwordData.timer = (MissionTimer)null;
                    break;
                }
            }
        }

        public override void OnScoreHit(Agent victim, Agent attacker, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damageHp, float hitDistance, float shotDifficulty)
        {
            if (!this.IsInBattle())
                return;
            FireSwordLogic.AgentFireSwordData agentFireSwordData;
            this._agentFireSwordData.TryGetValue(attacker, out agentFireSwordData);
            agentFireSwordData = _agentFireSwordData.FirstOrDefault(r => r.Key.Character == attacker.Character).Value;
            if (agentFireSwordData == null || !FireLordConfig.IgniteTargetWithFireSword || !agentFireSwordData.enabled || victim == null || !victim.IsHuman)
                return;
            float firebarAdd = isBlocked ? FireLordConfig.IgnitionPerFireSwordHit / 2f : FireLordConfig.IgnitionPerFireSwordHit;
            this._ignitionlogic.IncreaseAgentFireBar(attacker, victim, firebarAdd);
        }

        public class AgentFireSwordData
        {
            public bool enabled;
            public Agent agent;
            public GameEntity entity;
            public Light light;
            public bool dropLock;
            public MissionTimer timer;
            public bool lastWieldedWeaponEmpty;

            public void OnAgentWieldedItemChange()
            {
                if (this.dropLock || this.agent == null)
                    return;
                MissionWeapon wieldedWeapon = this.agent.WieldedWeapon;
                MissionWeapon wieldedOffhandWeapon = this.agent.WieldedOffhandWeapon;
                if (this.lastWieldedWeaponEmpty && !wieldedWeapon.IsEmpty)
                {
                    if (!this.agent.IsMainAgent || FireSwordLogic._playerFireSwordEnabled)
                        this.timer = new MissionTimer(0.1f);
                }
                else
                    this.SetFireSwordEnable(false);
                this.lastWieldedWeaponEmpty = (wieldedWeapon).IsEmpty;
            }

            public void OnAgentHealthChanged(Agent agent, float oldHealth, float newHealth)
            {
                if ((double)newHealth > 0.0)
                    return;
                this.SetFireSwordEnable(false);
            }

            public void SetFireSwordEnable(bool enable)
            {
                if (this.agent == null)
                    return;
                if (enable)
                {
                    this.SetFireSwordEnable(false);
                    EquipmentIndex wieldedItemIndex = this.agent.GetWieldedItemIndex((Agent.HandIndex)0);
                    if (wieldedItemIndex == EquipmentIndex.None)
                        return;
                    GameEntity fromEquipmentSlot = this.agent.GetWeaponEntityFromEquipmentSlot(wieldedItemIndex);
                    MissionWeapon wieldedWeapon = this.agent.WieldedWeapon;
                    if (((MissionWeapon)wieldedWeapon).IsEmpty)
                        return;
                    bool flag1 = FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.All || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Player && this.agent == Agent.Main || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Heroes && this.agent.IsHero || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Companions && this.agent.IsHero && this.agent.Team.IsPlayerTeam || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Allies && this.agent.Team.IsPlayerAlly || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Enemies && !this.agent.Team.IsPlayerAlly;
                    if (!flag1)
                    {
                        switch (FireLordConfig.FireSwordWhitelistType)
                        {
                            case FireLordConfig.WhitelistType.Troops:
                                flag1 = FireLordConfig.FireSwordTroopsWhitelist.Contains(this.agent.Character.StringId);
                                break;
                            case FireLordConfig.WhitelistType.Items:
                                flag1 = FireLordConfig.FireSwordItemsWhitelist.Contains(wieldedWeapon.Item.ToString());
                                break;
                        }
                    }
                    if (!flag1)
                        return;
                    int num1 = (int)Math.Round((double)((MissionWeapon)wieldedWeapon).GetWeaponStatsData()[0].WeaponLength / 10.0);
                    MBAgentVisuals agentVisuals = this.agent.AgentVisuals;
                    if (agentVisuals is null)
                        return;
                    Skeleton skeleton = agentVisuals.GetSkeleton();
                    Light pointLight = Light.CreatePointLight(FireLordConfig.FireSwordLightRadius);
                    pointLight.Intensity = FireLordConfig.FireSwordLightIntensity;
                    pointLight.LightColor = FireLordConfig.FireSwordLightColor;
                    switch (((MissionWeapon)wieldedWeapon).CurrentUsageItem.WeaponClass - 2)
                    {
                        case 0:
                        case WeaponClass.Dagger:
                        case WeaponClass.OneHandedAxe:
                        case WeaponClass.Mace:
                            for (int index = 1; index < num1; ++index)
                            {
                                MatrixFrame matrixFrame1 = new MatrixFrame(Mat3.Identity, new Vec3());
                                MatrixFrame matrixFrame2 = ((MatrixFrame)matrixFrame1).Elevate((float)index * 0.1f);
                                ParticleSystem attachedToEntity = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_game_burning_agent", fromEquipmentSlot, ref matrixFrame2);
                                skeleton.AddComponentToBone(Game.Current.DefaultMonster.MainHandItemBoneIndex, attachedToEntity);
                            }
                            Light light1 = pointLight;
                            MatrixFrame frame1 = pointLight.Frame;
                            MatrixFrame matrixFrame3 = ((MatrixFrame)frame1).Elevate((float)(num1 - 1) * 0.1f);
                            light1.Frame = matrixFrame3;
                            break;
                        case WeaponClass.OneHandedSword:
                        case WeaponClass.TwoHandedSword:
                        case WeaponClass.Pick:
                        case WeaponClass.TwoHandedMace:
                        case WeaponClass.OneHandedPolearm:
                            int num2 = num1 > 19 ? 9 : (num1 > 15 ? 6 : (num1 > 12 ? 5 : (num1 > 10 ? 4 : 3)));
                            for (int index = num1 - 1; index > 0 && index > num1 - num2; --index)
                            {
                                MatrixFrame matrixFrame4 = new MatrixFrame(Mat3.Identity, new Vec3());
                                MatrixFrame matrixFrame5 = matrixFrame4.Elevate((float)index * 0.1f);
                                ParticleSystem attachedToEntity = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_game_burning_agent", fromEquipmentSlot, ref matrixFrame5);
                                skeleton.AddComponentToBone(Game.Current.DefaultMonster.MainHandItemBoneIndex, (GameEntityComponent)attachedToEntity);
                            }
                            Light light2 = pointLight;
                            MatrixFrame frame2 = pointLight.Frame;
                            MatrixFrame matrixFrame6 = frame2.Elevate((float)(num1 - 1) * 0.1f);
                            light2.Frame = matrixFrame6;
                            break;
                        case WeaponClass.TwoHandedAxe:
                            return;
                        default:
                            return;
                    }
                    skeleton.AddComponentToBone(Game.Current.DefaultMonster.MainHandItemBoneIndex, (GameEntityComponent)pointLight);
                    if (this.agent.IsMainAgent && FireLordConfig.IgnitePlayerBody)
                    {
                        int boneCount = (int)skeleton.GetBoneCount();
                        for (sbyte index = 0; (int)index < boneCount; ++index)
                        {
                            MatrixFrame matrixFrame7 = new MatrixFrame(Mat3.Identity, new Vec3(0, 0, 0)).Elevate(index * 0.1f);
                            ParticleSystem attachedToEntity = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_game_burning_agent", fromEquipmentSlot, ref matrixFrame7);
                            skeleton.AddComponentToBone(index, attachedToEntity);
                        }
                    }
                    this.dropLock = true;
                    this.agent.DropItem(wieldedItemIndex, (WeaponClass)0);
                    SpawnedItemEntity firstScriptOfType = fromEquipmentSlot.GetFirstScriptOfType<SpawnedItemEntity>();
                    if (firstScriptOfType != null)
                    {
                        this.agent.OnItemPickup(firstScriptOfType, EquipmentIndex.None, out bool removeItem);
                    }
                    this.dropLock = false;
                    this.light = pointLight;
                    this.entity = fromEquipmentSlot;
                    this.enabled = true;
                }
                else
                {
                    this.enabled = false;
                    if (this.entity is null || this.agent == null)
                        return;
                    MBAgentVisuals agentVisuals = this.agent.AgentVisuals;
                    if (agentVisuals != null)
                    {
                        Skeleton skeleton = agentVisuals.GetSkeleton();
                        if (light != null && skeleton != null)
                            skeleton.RemoveComponent((GameEntityComponent)this.light);
                    }
                    this.entity.RemoveAllParticleSystems();
                }
            }
        }
    }
}
