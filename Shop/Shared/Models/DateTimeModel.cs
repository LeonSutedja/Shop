using System;

namespace Shop.Shared.Models.Models
{
    public class DateTimeModel
    {
        private readonly DateTime? _dateTime;
        public DateTimeModel(DateTime? dateTime) { _dateTime = dateTime; }
        public override string ToString() => (_dateTime == null) 
            ? "" 
            : _dateTime.Value.ToString("dd/MM/yyyy");
    }
}