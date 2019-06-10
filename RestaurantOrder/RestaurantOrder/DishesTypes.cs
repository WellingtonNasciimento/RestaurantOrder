using System.Collections.Generic;

namespace RestaurantOrder
{
    class DishesTypes
    {
        public List<Type> type { get; set; }
    }

    public class Type
    {
        public string code { get; set; }
        public string name { get; set; }
        public string morning { get; set; }
        public string night { get; set; }
    }
}