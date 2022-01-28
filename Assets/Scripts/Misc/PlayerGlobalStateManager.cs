namespace Misc
{
    public class PlayerGlobalStateManager : Singleton<PlayerGlobalStateManager>
    {
        public uint mechanicalCount { get; private set;  }
        public uint naturalCount { get; private set; }

        public void AddMechanical(uint collected) => mechanicalCount += collected;
        public void AddNatural(uint collected) => naturalCount += collected;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}