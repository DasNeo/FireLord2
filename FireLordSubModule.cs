using BUTR.DependencyInjection.Extensions;
using FireLord.Settings;
using FireLord.Settings.MCM;
using MCM.Internal.Extensions;
using TaleWorlds.MountAndBlade;

namespace FireLord
{
    public class FireLordSubModule : MBSubModuleBase
    {
        public static MCMSettings settings = new MCMSettings();

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            IgnitionLogic ignitionlogic = new IgnitionLogic();
            mission.AddMissionBehavior(ignitionlogic);
            mission.AddMissionBehavior(new FireSwordLogic(ignitionlogic));
        }

        protected override void OnSubModuleLoad()
        {
            var services = this.GetServiceContainer();
            if (services is not null)
            {

            }
        }
    }
}
