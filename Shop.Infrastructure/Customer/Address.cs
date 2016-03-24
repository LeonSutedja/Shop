using System;
namespace Shop.Infrastructure.Customer
{
    /// <summary>
    /// Address is a value type object. Currently is only being use for Customer. This object is potentially to be use for other aggregate entities as well.
    /// </summary>
    public class Address : IComparable<Address>
    {
        public string Street { get; }
        public string StreetNumber { get; }
        public string Unit { get; }
        public string Suburb { get; }
        public string Postcode { get; }

        public Address(string street, string streetNumber, string unit, string suburb, string postcode)
        {
            Street = street;
            StreetNumber = streetNumber;
            Unit = unit;
            Suburb = suburb;
            Postcode = postcode;
        }

        public int CompareTo(Address other)
        {
            return ((Street == other.Street) && (StreetNumber == other.StreetNumber)
                && (Suburb == other.Suburb) && (Postcode == other.Postcode)) ? 0 : 1;
        }
    }
}
