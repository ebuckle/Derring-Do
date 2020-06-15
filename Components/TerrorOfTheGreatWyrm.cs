using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;

namespace Derring_Do
{
    public class TerrorOfTheGreatWyrmDemoralize : ContextAction
    {
        public BlueprintBuff Buff;
        public BlueprintBuff GreaterBuff;

        public override string GetCaption()
        {
            return "Demoralize target with action when over duration";
        }

        public override void RunAction()
        {
            MechanicsContext context = ElementsContext.GetData<MechanicsContext.Data>()?.Context;
            UnitEntityData maybeCaster = context?.MaybeCaster;
            if (maybeCaster == null || !this.Target.IsUnit)
            {
                UberDebug.LogError((UnityEngine.Object)this, (object)"Unable to apply buff: no context found", (object[])Array.Empty<object>());
            }
            else
            {
                int dc = 10 + this.Target.Unit.Descriptor.Progression.CharacterLevel + this.Target.Unit.Stats.Wisdom.Bonus;
                try
                {
                    RuleSkillCheck ruleSkillCheck = context.TriggerRule<RuleSkillCheck>(new RuleSkillCheck(maybeCaster, StatType.CheckIntimidate, dc));
                    if (!ruleSkillCheck.IsPassed)
                        return;
                    int num1 = 1 + (ruleSkillCheck.RollResult - dc) / 5 + (!(bool)maybeCaster.Descriptor.State.Features.FrighteningThug ? 0 : 1);
                    if ((bool)maybeCaster.Descriptor.State.Features.FrighteningThug && num1 >= 4)
                        this.Target.Unit.Descriptor.AddBuff(this.GreaterBuff, context, new TimeSpan?(1.Rounds().Seconds));
                    if (num1 >= 3)
                    {
                        this.Target.Unit.Descriptor.AddBuff(this.GreaterBuff, context, new TimeSpan?(1.Rounds().Seconds));
                        num1++; //Mimicks the fact that the lesser buff is applied after this greater buff wears off.
                    }
                    Kingmaker.UnitLogic.Buffs.Buff buff1 = this.Target.Unit.Descriptor.AddBuff(this.Buff, context, new TimeSpan?(num1.Rounds().Seconds));
                }
                finally
                {
                }
            }
        }
    }
}
