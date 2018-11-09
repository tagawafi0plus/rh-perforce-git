namespace Scripts.Sound
{
    public interface ISoundController
    {
        bool Initialized {
            get;
        }
        ISoundPlayer Bgm {
            get;
        }
        ISoundPlayer Se {
            get;
        }
        ISoundPlayer Voice {
            get;
        }
        ILiveSoundPlayer Live {
            get;
        }   
    }
}