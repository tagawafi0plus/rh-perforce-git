namespace Scripts.Sound
{
    public interface ISoundVolumeController
    {
        float Bgm {
            set;
            get;
        }
        
        float Se {
            set;
            get;
        }
        
        float Voice {
            set;
            get;
        }
        
        void Save();
    }
}