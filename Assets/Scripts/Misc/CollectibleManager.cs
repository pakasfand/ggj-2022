namespace Misc
{
    public class CollectibleManager : Singleton<CollectibleManager>
    {
        public void PushPreCheckpoint()
        {
            
        }

        public void PushPostCheckpoint()
        {
            
        }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}