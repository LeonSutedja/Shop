using System;

namespace Shop.Shared.Models
{
    public class DateTimeModel
    {
        private readonly DateTime? _dateTime;
        public DateTimeModel(DateTime? dateTime) { _dateTime = dateTime; }
        public override string ToString() => _dateTime?.ToString("dd/MM/yyyy") ?? "";
    }
}