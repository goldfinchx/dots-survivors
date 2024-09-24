using Unity.Entities;

namespace Damage {
    public struct DamageEvent : IBufferElementData {
        public readonly Entity Attacker;
        public readonly int Value;
        
        public DamageEvent(Entity attacker, int value) {
            Attacker = attacker;
            Value = value;
        }
        
    }
}