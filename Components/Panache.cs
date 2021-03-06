﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.UnitLogic.Parts;

namespace Derring_Do
{
    public class SpendPanache : ContextAction
    {
        public BlueprintAbilityResource resource;
        public int amount;

        public override string GetCaption()
        {
            return "Spend Panache";
        }

        public override void RunAction()
        {
            var owner = this.Context.MaybeOwner;
            if (owner == null)
            {
                return;
            };
            owner.Descriptor.Resources.Spend(resource, amount);
        }
    }

    [ComponentName("Restore Panache on Kill and Crit")]
    public class RestorePanacheAttackRollTrigger : GameLogicComponent, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookHandler<RuleAttackWithWeaponResolve>, IRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookSubscriber, IRulebookHandler<RuleAttackWithWeaponResolve>
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
        }

        public void OnEventAboutToTrigger(RuleAttackWithWeaponResolve evt)
        {
        }

        public void OnEventDidTrigger(RuleAttackWithWeaponResolve evt)
        {
            if (this.WaitForAttackResolve)
            {
                this.TryRunActions(evt.AttackWithWeapon, evt);
            }
        }

        private void TryRunActions(RuleAttackWithWeapon rule, RuleAttackWithWeaponResolve evt)
        {
            var valid_target = this.CheckValidTarget(rule);
            if ((this.CheckCondition(rule) && valid_target) || ((IsRanged && CheckProjectile(evt)) && valid_target))
            {
                using (new ContextAttackData(rule.AttackRoll, null))
                {
                    IFactContextOwner factContextOwner2 = base.Fact as IFactContextOwner;
                    if (factContextOwner2 != null)
                    {
                        factContextOwner2.RunActionInContext(this.Action, rule.Initiator);
                    }
                }
            }
        }

        private bool CheckCondition(RuleAttackWithWeapon evt)
        {
            ItemEnchantment itemEnchantment = base.Fact as ItemEnchantment;
            ItemEntity itemEntity = (itemEnchantment != null) ? itemEnchantment.Owner : null;
            if (itemEntity != null && itemEntity != evt.Weapon)
            {
                return false;
            }
            if (this.CriticalHit && (!evt.AttackRoll.IsCriticalConfirmed || evt.AttackRoll.FortificationNegatesCriticalHit))
            {
                return false;
            }
            if (this.ReduceHPToZero)
            {
                return evt.MeleeDamage != null && !evt.MeleeDamage.IsFake && evt.Target.HPLeft <= 0 && evt.Target.HPLeft + evt.MeleeDamage.Damage > 0;
            }
            if (this.CheckWeaponCategory && this.Category != evt.Weapon.Blueprint.Category)
            {
                return false;
            }
            if (this.OnlyOnFullAttack && !evt.IsFullAttack)
            {
                return false;
            }
            if (this.OnlyOnFirstAttack && !evt.IsFirstAttack)
            {
                return false;
            }
            if (this.DuelistWeapon && !Extensions.isSwashbucklerWeapon(evt.Weapon.Blueprint, evt.Initiator.Descriptor))
            {
                return false;
            }
            return true;
        }

        private bool CheckValidTarget(RuleAttackWithWeapon evt)
        {
            if (evt.Target.Descriptor.Progression.CharacterLevel < (evt.Initiator.Descriptor.Progression.CharacterLevel / 2))
            {
                return false;
            }
            if (evt.Target.Descriptor.State.IsHelpless || evt.Target.Descriptor.State.IsUnconscious)
            {
                return false;
            }
            if (CriticalHit && evt.Initiator.Descriptor.HasFact(deadly_stab_buff))
            {
                return false;
            }
            if (IsInspiredBlade && evt.Initiator.Descriptor.HasFact(InspiredBlade.inspired_strike_critical_buff))
            {
                return false;
            }
            return true;
        }

        private bool CheckProjectile(RuleAttackWithWeaponResolve evt)
        {
            if (ReduceHPToZero && !(evt.Target.HPLeft <= 0 && evt.Target.HPLeft + evt.Damage.Damage > 0))
            {
                return false;
            }
            if (this.CriticalHit && (!evt.AttackWithWeapon.Projectile.AttackRoll.IsCriticalConfirmed || evt.AttackWithWeapon.Projectile.AttackRoll.FortificationNegatesCriticalHit))
            {
                return false;
            }
            if ((evt.AttackWithWeapon.Weapon.Blueprint.Category == WeaponCategory.Dagger || evt.AttackWithWeapon.Weapon.Blueprint.Category == WeaponCategory.Starknife) && evt.AttackWithWeapon.Weapon.Blueprint.IsRanged)
            {
                return true;
            }
            return false;
        }

        public bool IsRanged;

        public bool OnlyOnFirstAttack;

        public bool OnlyOnFullAttack;

        public BlueprintBuff deadly_stab_buff;

        public bool IsInspiredBlade;

        public bool WaitForAttackResolve = true;

        public bool CriticalHit;

        public bool ReduceHPToZero;

        public WeaponCategory Category;

        public bool CheckWeaponCategory;

        public bool DuelistWeapon = true;

        public ActionList Action;
    }
}
