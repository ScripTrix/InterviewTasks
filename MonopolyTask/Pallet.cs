using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonopolyTask
{
    [Serializable]
    public class Pallet : Storage
    {
        private const float PALLETE_WEIGHT = 30f;
        [JsonProperty]
        private List<Box> boxes;
        [JsonProperty]
        public DateTime? ExpirationDate { get; private set; }

        public Pallet() : base()
        {
            boxes = new List<Box>();
        }

        public Pallet(int id, float width, float height, float depth)
            : base(id, width, height, depth)
        {
            boxes = new List<Box>();
            CalculateParams();
        }

        public Box this[int index]
        {
            get
            {
                if (index >= boxes.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    return boxes[index];
                }
            }
        }

        public int BoxesCount() => boxes.Count();

        public void AddBox(Box box)
        {
            if (box.Width > Width || box.Height > Height)
                throw new System.ArgumentException();
            boxes.Add(box);
            CalculateParams();
        }

        public void RemoveBox(int id)
        {
            var box = boxes.Find(x => x.Id == id);
            if (box == null)
                return;
            boxes.Remove(box);
            CalculateParams();
        }

        public void RemoveBox(Box box)
        {
            boxes.Remove(box);
            CalculateParams();
        }

        public void CalculateParams()
        {
            CalculateVolume();
            CalculateWeight();
            CalculateExpirationDate();
        }

        public override float CalculateVolume()
        {
            var totalVolume = Width * Height * Depth;
            foreach (var box in boxes)
            {
                totalVolume += box.CalculateVolume();
            }
            return totalVolume;
        }

        public float CalculateWeight()
        {
            var totalWeight = PALLETE_WEIGHT;
            foreach (var box in boxes)
            {
                totalWeight += box.Weight;
            }
            Weight = totalWeight;
            return totalWeight;
        }

        public DateTime? CalculateMaxExpirationDate()
        {
            DateTime? maxDate = null;
            foreach (var box in boxes)
            {

                if (box.ExpirationDate == null)
                {
                    var boxExp = box.ProductionDate.Value.AddDays(100);
                    if (maxDate != null)
                    {
                        if (maxDate < boxExp)
                        {
                            maxDate = boxExp;
                        }
                    }
                    else
                    {
                        maxDate = boxExp;
                    }
                }
                else
                {
                    if (maxDate != null)
                    {
                        if (maxDate < box.ExpirationDate)
                        {
                            maxDate = box.ExpirationDate;
                        }
                    }
                    else
                    {
                        maxDate = box.ExpirationDate;
                    }
                }
            }
            return maxDate;
        }

        public DateTime? CalculateExpirationDate()
        {

            foreach (var box in boxes)
            {
                if(box.ExpirationDate == null && box.ProductionDate == null)
                {
                    throw new ArgumentNullException();
                }
                if (ExpirationDate == null)
                {
                    if (box.ExpirationDate != null)
                    {
                        ExpirationDate = box.ExpirationDate;
                    }
                    else
                    {
                        ExpirationDate = box.ProductionDate.Value.AddDays(100);
                    }
                }
                else
                {
                    if (box.ExpirationDate != null)
                    {
                        if (box.ExpirationDate < ExpirationDate)
                        {
                            ExpirationDate = box.ExpirationDate;
                        }
                    }
                    else
                    {
                        var tmpDate = box.ProductionDate.Value.AddDays(100);
                        if (tmpDate < ExpirationDate)
                        {
                            ExpirationDate = tmpDate;
                        }
                    }
                }
            }
            return ExpirationDate;
        }
    }
}
