// Decompiled with JetBrains decompiler
// Type: FireLord.FireLordSubModule
// Assembly: FireLord, Version=1.1.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 51633F12-6A5F-46B9-B9AF-55B0B570B321
// Assembly location: C:\Users\andre\Documents\FireLord.dll

using FireLord.Settings;
using TaleWorlds.MountAndBlade;

namespace FireLord
{
    public class FireLordSubModule : MBSubModuleBase
    {
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            IgnitionLogic ignitionlogic = new IgnitionLogic();
            mission.AddMissionBehavior((MissionBehavior)ignitionlogic);
            //mission.AddMissionBehavior((MissionBehavior)new FireArrowLogic(ignitionlogic));
            mission.AddMissionBehavior((MissionBehavior)new FireSwordLogic(ignitionlogic));
        }

        protected override void OnSubModuleLoad() => FireLordConfig.Init();

        //protected override void OnGameStart(Game game, IGameStarter gameStarterObject) => game.GameTextManager.LoadGameTexts(string.Format("{0}/Modules/{1}/ModuleData/module_strings.xml", (object)BasePath.Name, (object)"FireLord"));
    }
}
