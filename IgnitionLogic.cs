using FireLord.Settings;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace FireLord
{
    public class IgnitionLogic : MissionLogic
    {
        public Dictionary<Agent, AgentFireData> AgentFireDatas = new Dictionary<Agent, AgentFireData>();
        private static int[] _ignitionBoneIndexes = new int[14]
        {
      0,
      1,
      2,
      3,
      5,
      6,
      7,
      9,
      12,
      13,
      15,
      17,
      22,
      24
        };

        public event OnAgentDropItemDelegate OnAgentDropItem;

        public void IncreaseAgentFireBar(Agent attacker, Agent victim, float firebarAdd)
        {
            if (AgentFireDatas.ContainsKey(victim))
            {
                AgentFireData agentFireData = AgentFireDatas[victim];
                if (agentFireData.isBurning)
                    return;
                agentFireData.firebar += firebarAdd;
                agentFireData.attacker = attacker;
            }
            else
            {
                AgentFireData agentFireData = new AgentFireData();
                agentFireData.firebar += firebarAdd;
                agentFireData.attacker = attacker;
                AgentFireDatas.Add(victim, agentFireData);
            }
        }

        public bool IsInBattle() => Mission.Mode == MissionMode.Battle || Mission.Mode == MissionMode.Duel || Mission.Mode == MissionMode.Stealth || Mission.Mode == MissionMode.Tournament;

        public override void OnMissionTick(float dt)
        {
            if (!IsInBattle() || AgentFireDatas.Count <= 0)
                return;
            List<Agent> agentList = new List<Agent>();
            foreach (KeyValuePair<Agent, AgentFireData> agentFireData1 in AgentFireDatas)
            {
                Agent key = agentFireData1.Key;
                if (!key.IsActive())
                {
                    agentList.Add(key);
                    break;
                }
                AgentFireData agentFireData2 = agentFireData1.Value;
                if (agentFireData2.isBurning)
                {
                    if (FireLordConfig.IgnitionDealDamage && agentFireData2.damageTimer.Check(true) && key.IsActive())
                    {
                        Blow blow = CreateBlow(agentFireData2.attacker, key);
                        key.RegisterBlow(blow, new AttackCollisionData());
                        if (agentFireData2.attacker == Agent.Main)
                            InformationManager.DisplayMessage(new InformationMessage(Regex.Replace(GameTexts.FindText("ui_delivered_burning_damage", null).ToString(), "\\d+", string.Concat(blow.InflictedDamage))));
                        else if (key == Agent.Main)
                            InformationManager.DisplayMessage(new InformationMessage(Regex.Replace(GameTexts.FindText("ui_received_burning_damage", null).ToString(), "\\d+", string.Concat(blow.InflictedDamage)), Color.ConvertStringToColor("#D65252FF")));
                    }
                    if (agentFireData2.burningTimer.Check(false))
                    {
                        if (agentFireData2.fireEntity != null)
                        {
                            foreach (ParticleSystem particle in agentFireData2.particles)
                                agentFireData2.fireEntity.RemoveComponent(particle);
                            if (agentFireData2.fireLight != null)
                            {
                                agentFireData2.fireLight.Intensity = 0.0f;
                                MBAgentVisuals agentVisuals = key.AgentVisuals;
                                if (agentVisuals != null)
                                {
                                    Skeleton skeleton = agentVisuals.GetSkeleton();
                                    if (skeleton != null)
                                        skeleton.RemoveComponent(agentFireData2.fireLight);
                                }
                            }
                            agentFireData2.fireEntity = null;
                            agentFireData2.fireLight = null;
                        }
                        agentFireData2.firebar = 0.0f;
                        agentFireData2.isBurning = false;
                    }
                }
                else if (agentFireData2.firebar >= (double)FireLordConfig.IgnitionBarMax)
                {
                    agentFireData2.isBurning = true;
                    agentFireData2.burningTimer = new MissionTimer(FireLordConfig.IgnitionDurationInSecond);
                    agentFireData2.damageTimer = new MissionTimer(1f);
                    EquipmentIndex wieldedItemIndex = key.GetWieldedItemIndex(0);
                    if (wieldedItemIndex == EquipmentIndex.None)
                        return;
                    GameEntity fromEquipmentSlot = key.GetWeaponEntityFromEquipmentSlot(wieldedItemIndex);
                    MBAgentVisuals agentVisuals = key.AgentVisuals;
                    if (agentVisuals == null)
                        return;
                    Skeleton skeleton = agentVisuals.GetSkeleton();
                    agentFireData2.particles = new ParticleSystem[_ignitionBoneIndexes.Length];
                    for (byte index = 0; index < _ignitionBoneIndexes.Length; ++index)
                    {
                        MatrixFrame localFrame = new MatrixFrame(Mat3.Identity, new Vec3(0, 0, 0));
                        ParticleSystem attachedToEntity = ParticleSystem.CreateParticleSystemAttachedToEntity("psys_campfire", fromEquipmentSlot, ref localFrame);
                        skeleton.AddComponentToBone((sbyte)_ignitionBoneIndexes[index], attachedToEntity);
                        agentFireData2.particles[index] = attachedToEntity;
                    }
                    if (OnAgentDropItem != null)
                        OnAgentDropItem(key, true);
                    key.DropItem(wieldedItemIndex, 0);
                    SpawnedItemEntity firstScriptOfType = fromEquipmentSlot.GetFirstScriptOfType<SpawnedItemEntity>();
                    if (firstScriptOfType != null)
                    {
                        bool flag;
                        key.OnItemPickup(firstScriptOfType, EquipmentIndex.None, out flag);
                    }
                    agentFireData2.fireEntity = fromEquipmentSlot;
                    if (OnAgentDropItem != null)
                        OnAgentDropItem(key, false);
                    Light pointLight = Light.CreatePointLight(FireLordConfig.IgnitionLightRadius);
                    pointLight.Intensity = FireLordConfig.IgnitionLightIntensity;
                    pointLight.LightColor = FireLordConfig.IgnitionLightColor;
                    skeleton.AddComponentToBone(0, pointLight);
                    agentFireData2.fireLight = pointLight;
                }
                else
                {
                    agentFireData2.firebar -= dt * FireLordConfig.IgnitionDropPerSecond;
                    agentFireData2.firebar = Math.Max(agentFireData2.firebar, 0.0f);
                }
            }
            foreach (Agent key in agentList)
            {
                AgentFireData agentFireData = AgentFireDatas[key];
                GameEntity fireEntity = agentFireData.fireEntity;
                if (fireEntity != null)
                    fireEntity.RemoveAllParticleSystems();
                MBAgentVisuals agentVisuals = key.AgentVisuals;
                if (agentVisuals != null)
                {
                    Skeleton skeleton = agentVisuals.GetSkeleton();
                    if (skeleton != null && agentFireData.fireLight != null)
                        skeleton.RemoveComponent(agentFireData.fireLight);
                }
                AgentFireDatas.Remove(key);
            }
        }

        private Blow CreateBlow(Agent attacker, Agent victim)
        {
            Blow blow = new Blow(attacker.Index);
            blow.DamageType = DamageTypes.Blunt;
            blow.BlowFlag = BlowFlags.ShrugOff;
            blow.BlowFlag |= BlowFlags.NoSound;
            blow.BoneIndex = victim.Monster.HeadLookDirectionBoneIndex;
            blow.GlobalPosition = victim.Position;
            blow.GlobalPosition.z = blow.GlobalPosition.z + victim.GetEyeGlobalHeight();
            blow.BaseMagnitude = 0;
            blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1);
            blow.InflictedDamage = FireLordConfig.IgnitionDamagePerSecond;
            blow.SwingDirection = victim.LookDirection;
            blow.SwingDirection.Normalize();
            blow.Direction = blow.SwingDirection;
            blow.DamageCalculated = true;
            return blow;
        }

        public delegate void OnAgentDropItemDelegate(Agent agent, bool dropLock);

        public class AgentFireData
        {
            public bool isBurning;
            public float firebar;
            public MissionTimer burningTimer;
            public GameEntity fireEntity;
            public Light fireLight;
            public Agent attacker;
            public MissionTimer damageTimer;
            public ParticleSystem[] particles;
        }
    }
}
