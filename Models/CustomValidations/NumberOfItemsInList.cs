using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models.CustomValidations
{
    public class NumberOfItemsInList : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public NumberOfItemsInList(int min=0, int max=int.MaxValue)
        {
            _min = min;
            _max = max;
        }

        public override bool IsValid(object value)
        {
            if (!(value is IList list)) return false;

            return list.Count >= _min && list.Count <= _max;
        }
    }
}
