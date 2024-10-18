using UnityEngine;

namespace RTS.Unit
{
    public abstract class AUnitBase
    {
        public AUnitBase Target { get; protected set; }
        public AUnitBase Attacker { get; protected set; }
        public AUnitView View { get; protected set; }

        public int Id { get; protected set; }
        public int Faction { get; protected set; }

        public AUnitBase(int id, int faction, AUnitView view)
        {
            Id = id;
            Faction = faction;
            View = view;
        }

        public abstract void Fire();
    }
}
