using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Newtonsoft.Json;

namespace Derring_Do
{
    [ComponentName("Add feature if owner has no armor or light armor")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [AllowMultipleComponents]
    public class SwashbucklerNoArmorOrLightArmorNimbleFeatureUnlock : OwnedGameLogicComponent<UnitDescriptor>, IUnitActiveEquipmentSetHandler, IUnitEquipmentHandler, IGlobalSubscriber
    {
        public BlueprintUnitFact NewFact;
        [JsonProperty]
        private Fact m_AppliedFact;

        public override void OnFactActivate()
        {
            this.CheckEligibility();
        }

        public override void OnFactDeactivate()
        {
            this.RemoveFact();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            this.CheckEligibility();
        }

        public void CheckEligibility()
        {
            if ((!this.Owner.Body.Armor.HasArmor || !this.Owner.Body.Armor.Armor.Blueprint.IsArmor) || (this.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup == ArmorProficiencyGroup.Light))
            {
                this.AddFact();
            }
            else
            {
                this.RemoveFact();
            }
        }

        public void AddFact()
        {
            if (this.m_AppliedFact != null)
                return;
            this.m_AppliedFact = this.Owner.AddFact(this.NewFact, (MechanicsContext)null, (FeatureParam)null);
        }

        public void RemoveFact()
        {
            if (this.m_AppliedFact == null)
                return;
            this.Owner.RemoveFact(this.m_AppliedFact);
            this.m_AppliedFact = (Fact)null;
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            if (slot.Owner != this.Owner)
                return;
            this.CheckEligibility();
        }

        public new void OnTurnOn()
        {
            this.CheckEligibility();
        }
    }

}
