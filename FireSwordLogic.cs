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
        private Dictionary<Agent, AgentFireSwordData> _agentFireSwordData = new Dictionary<Agent, AgentFireSwordData>();

        public FireSwordLogic(IgnitionLogic ignitionlogic)
        {
            _ignitionlogic = ignitionlogic;
            _ignitionlogic.OnAgentDropItem += new IgnitionLogic.OnAgentDropItemDelegate(SetDropLockForAgent);
            _playerFireSwordEnabled = FireLordConfig.PlayerFireSwordDefaultOn;
        }

        public void SetDropLockForAgent(Agent agent, bool dropLock)
        {
            AgentFireSwordData agentFireSwordData;
            _agentFireSwordData.TryGetValue(agent, out agentFireSwordData);
            if (agentFireSwordData == null)
                return;
            agentFireSwordData.dropLock = dropLock;
        }

        public bool IsInBattle() => Mission.IsFieldBattle || Mission.IsSiegeBattle || !Mission.IsFriendlyMission || Mission.Mode == MissionMode.Duel || Mission.Mode == MissionMode.Stealth || Mission.Mode == MissionMode.Tournament;

        public override void OnAgentCreated(Agent agent)
        {
            if (!IsInBattle() || !agent.IsHuman || _agentFireSwordData.ContainsKey(agent))
                return;
            AgentFireSwordData agentFireSwordData1 = new AgentFireSwordData();
            agentFireSwordData1.agent = agent;
            agent.OnAgentWieldedItemChange += new Action(agentFireSwordData1.OnAgentWieldedItemChange);
            agent.OnAgentHealthChanged += agentFireSwordData1.OnAgentHealthChanged;
            AgentFireSwordData agentFireSwordData2 = agentFireSwordData1;
            MissionWeapon wieldedWeapon = agent.WieldedWeapon;
            int num = wieldedWeapon.IsEmpty ? 1 : 0;
            agentFireSwordData2.lastWieldedWeaponEmpty = num != 0;
            if (!agent.IsMainAgent || _playerFireSwordEnabled)
                agentFireSwordData1.timer = new MissionTimer(1f);
            _agentFireSwordData.Add(agent, agentFireSwordData1);
        }

        public override void OnAgentDeleted(Agent agent)
        {
            if (!IsInBattle())
                return;
            _agentFireSwordData.Remove(agent);
        }

        public override void OnMissionTick(float dt)
        {
            if (!IsInBattle())
                return;
            foreach (KeyValuePair<Agent, AgentFireSwordData> keyValuePair in _agentFireSwordData)
            {
                AgentFireSwordData agentFireSwordData = keyValuePair.Value;
                if (keyValuePair.Key.IsMainAgent && !_playerFireSwordEnabled)
                    agentFireSwordData.timer = null;
                else if (agentFireSwordData.timer != null && agentFireSwordData.timer.Check(false))
                {
                    agentFireSwordData.SetFireSwordEnable(true);
                    agentFireSwordData.timer = null;
                }
            }
            if (!Input.IsKeyPressed(FireLordConfig.FireSwordToggleKey) || Agent.Main == null)
                return;
            _playerFireSwordEnabled = !_playerFireSwordEnabled;
            foreach (KeyValuePair<Agent, AgentFireSwordData> keyValuePair in _agentFireSwordData)
            {
                if (keyValuePair.Key.IsPlayerControlled)
                {
                    AgentFireSwordData agentFireSwordData = keyValuePair.Value;
                    agentFireSwordData.SetFireSwordEnable(_playerFireSwordEnabled);
                    agentFireSwordData.timer = null;
                    break;
                }
            }
        }

        public override void OnScoreHit(Agent victim, Agent attacker, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damageHp, float hitDistance, float shotDifficulty)
        {
            if (!IsInBattle())
                return;
            AgentFireSwordData agentFireSwordData;
            _agentFireSwordData.TryGetValue(attacker, out agentFireSwordData);
            agentFireSwordData = _agentFireSwordData.FirstOrDefault(r => r.Key.Character == attacker.Character).Value;
            if (agentFireSwordData == null || !FireLordConfig.IgniteTargetWithFireSword || !agentFireSwordData.enabled || victim == null || !victim.IsHuman)
                return;
            float firebarAdd = isBlocked ? FireLordConfig.IgnitionPerFireSwordHit / 2f : FireLordConfig.IgnitionPerFireSwordHit;
            _ignitionlogic.IncreaseAgentFireBar(attacker, victim, firebarAdd);
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
                if (dropLock || agent == null)
                    return;
                MissionWeapon wieldedWeapon = agent.WieldedWeapon;
                MissionWeapon wieldedOffhandWeapon = agent.WieldedOffhandWeapon;
                if (lastWieldedWeaponEmpty && !wieldedWeapon.IsEmpty)
                {
                    if (!agent.IsMainAgent || _playerFireSwordEnabled)
                        timer = new MissionTimer(0.1f);
                }
                else
                    SetFireSwordEnable(false);
                lastWieldedWeaponEmpty = (wieldedWeapon).IsEmpty;
            }

            public void OnAgentHealthChanged(Agent agent, float oldHealth, float newHealth)
            {
                if ((double)newHealth > 0.0)
                    return;
                SetFireSwordEnable(false);
            }

            public void SetFireSwordEnable(bool enable)
            {
                if (agent == null)
                    return;
                if (enable)
                {
                    SetFireSwordEnable(false);
                    EquipmentIndex wieldedItemIndex = agent.GetWieldedItemIndex(0);
                    if (wieldedItemIndex == EquipmentIndex.None)
                        return;
                    GameEntity fromEquipmentSlot = agent.GetWeaponEntityFromEquipmentSlot(wieldedItemIndex);
                    MissionWeapon wieldedWeapon = agent.WieldedWeapon;
                    if (wieldedWeapon.IsEmpty)
                        return;
                    bool flag1 = FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.All || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Player && agent == Agent.Main || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Heroes && agent.IsHero || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Companions && agent.IsHero && agent.Team.IsPlayerTeam || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Allies && agent.Team.IsPlayerAlly || FireLordConfig.FireSwordAllowedUnitType == FireLordConfig.UnitType.Enemies && !agent.Team.IsPlayerAlly;
                    if (!flag1)
                    {
                        switch (FireLordConfig.FireSwordWhitelistType)
                        {
                            case FireLordConfig.WhitelistType.Troops:
                                flag1 = FireLordConfig.FireSwordTroopsWhitelist.Contains(agent.Character.StringId);
                                break;
                            case FireLordConfig.WhitelistType.Items:
                                flag1 = FireLordConfig.FireSwordItemsWhitelist.Contains(wieldedWeapon.Item.ToString());
                                break;
                        }
                    }
                    if (!flag1)
                        return;
                    int num1 = (int)Math.Round(wieldedWeapon.GetWeaponStatsData()[0].WeaponLength / 10.0);
                    MBAgentVisuals agentVisuals = agent.AgentVisuals;
                    if (agentVisuals is null)
                        return;
                    Skeleton skeleton = agentVisuals.GetSkeleton();
                    Light pointLight = Light.CreatePointLight(FireLordConfig.FireSwordLightRadius);
                    pointLight.Intensity = FireLordConfig.FireSwordLightIntensity;
                    pointLight.LightColor = FireLordConfig.FireSwordLightColor;
                    switch (wieldedWeapon.CurrentUsageItem.WeaponClass - 2)
                    {
                        case 0:
                        case WeaponClass.Dagger:
                        case WeaponClass.OneHandedAxe:
                        case WeaponClass.Mace:
                            for (int index = 1; index < num1; ++index)
                            {
                                MatrixFrame matrixFrame1 = new MatrixFrame(Mat3.Identity, new Vec3());
                                MatrixFrame matrixFrame2 = matrixFrame1.Elevate(index * 0.1f);
                                ParticleSystem attachedToEntity = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_game_burning_agent", fromEquipmentSlot, ref matrixFrame2);
                                skeleton.AddComponentToBone(Game.Current.DefaultMonster.MainHandItemBoneIndex, attachedToEntity);
                            }
                            Light light1 = pointLight;
                            MatrixFrame frame1 = pointLight.Frame;
                            MatrixFrame matrixFrame3 = frame1.Elevate((num1 - 1) * 0.1f);
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
                                MatrixFrame matrixFrame5 = matrixFrame4.Elevate(index * 0.1f);
                                ParticleSystem attachedToEntity = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_game_burning_agent", fromEquipmentSlot, ref matrixFrame5);
                                skeleton.AddComponentToBone(Game.Current.DefaultMonster.MainHandItemBoneIndex, attachedToEntity);
                            }
                            Light light2 = pointLight;
                            MatrixFrame frame2 = pointLight.Frame;
                            MatrixFrame matrixFrame6 = frame2.Elevate((num1 - 1) * 0.1f);
                            light2.Frame = matrixFrame6;
                            break;
                        case WeaponClass.TwoHandedAxe:
                            return;
                        default:
                            return;
                    }
                    skeleton.AddComponentToBone(Game.Current.DefaultMonster.MainHandItemBoneIndex, pointLight);
                    if (agent.IsMainAgent && FireLordConfig.IgnitePlayerBody)
                    {
                        int boneCount = skeleton.GetBoneCount();
                        for (sbyte index = 0; index < boneCount; ++index)
                        {
                            MatrixFrame matrixFrame7 = new MatrixFrame(Mat3.Identity, new Vec3(0, 0, 0)).Elevate(index * 0.1f);
                            ParticleSystem attachedToEntity = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_game_burning_agent", fromEquipmentSlot, ref matrixFrame7);
                            skeleton.AddComponentToBone(index, attachedToEntity);
                        }
                    }
                    dropLock = true;
                    agent.DropItem(wieldedItemIndex, 0);
                    SpawnedItemEntity firstScriptOfType = fromEquipmentSlot.GetFirstScriptOfType<SpawnedItemEntity>();
                    if (firstScriptOfType != null)
                    {
                        agent.OnItemPickup(firstScriptOfType, EquipmentIndex.None, out bool removeItem);
                    }
                    dropLock = false;
                    light = pointLight;
                    entity = fromEquipmentSlot;
                    enabled = true;
                }
                else
                {
                    enabled = false;
                    if (entity is null || agent == null)
                        return;
                    MBAgentVisuals agentVisuals = agent.AgentVisuals;
                    if (agentVisuals != null)
                    {
                        Skeleton skeleton = agentVisuals.GetSkeleton();
                        if (light != null && skeleton != null)
                            skeleton.RemoveComponent(light);
                    }
                    entity.RemoveAllParticleSystems();
                }
            }
        }
    }
}
