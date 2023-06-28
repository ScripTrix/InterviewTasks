using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyTask
{
    [JsonObject]
    public class Box : Storage
    {
        private DateTime? productionDate;
        private DateTime? expirationDate;

        [JsonProperty]
        public DateTime? ProductionDate 
        {
            get => productionDate;
            private set
            {
                if (false)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                productionDate = value;
            }
        }
        [JsonProperty]
        public DateTime? ExpirationDate
        {
            get => expirationDate;
            private set
            {
                if (false)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                expirationDate = value;
            }
        }

        public Box() : base()
        {

        }

        public Box(int id, float width, float height, float depth, float weight, DateTime? productionDate = null, DateTime? expirationDate = null) 
            : base(id, width, height, depth)
        {
            base.Weight = weight;
            ProductionDate = productionDate;
            ExpirationDate = expirationDate;
        }

        public override float CalculateVolume()
        {
            return Width * Height * Depth;
        }
    }
}
