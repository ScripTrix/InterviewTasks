using Newtonsoft.Json;

namespace MonopolyTask
{
    [JsonObject]
    public class Storage
    {
        private int id;
        private float width;
        private float height;
        private float depth;
        private float weight;

        [JsonProperty]
        public int Id
        {
            get => id;
            protected set
            {
                if (value < 0)
                    throw new System.ArgumentOutOfRangeException();
                id = value;
            }
        }
        [JsonProperty]
        public float Width
        {
            get => width;
            protected set
            {
                if(value < 0)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                width = value;
            }
        }
        [JsonProperty]
        public float Height 
        {
            get => height;
            protected set
            {
                if (value < 0)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                height = value;
            }
        }
        [JsonProperty]
        public float Depth 
        {
            get => depth;
            protected set
            {
                if (value < 0)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                depth = value;
            }
        }
        [JsonProperty]
        public float Weight 
        {
            get => weight;
            protected set
            {
                if (value < 0)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                weight = value;
            }
        }

        public Storage()
        {

        }

        public Storage(int id, float width, float height, float depth)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
        }

        public virtual float CalculateVolume()
        {
            throw new System.NotImplementedException();
        }
    }
}