using Unity.Entities;

namespace Damage {
    public struct DamageEvent : IBufferElementData {
        public readonly Entity Source;
        public readonly int Value;
        
        public DamageEvent(Entity source, int value) {
            Source = source;
            Value = value;
        }
    }
}