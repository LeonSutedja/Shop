using System;
namespace Shop.Infrastructure.Customer
{
    /// <summary>
    /// Address is a value type object. Currently is only being use for Customer. This object is potentially to be use for other aggregate entities as well.
    /// </summary>
    public class Address : IComparable<Address>
    {
        public string Street { get; }
        public string Suburb { get; }
        public string Postcode { get; }
        public string Number { get; }

        public Address(string street, string number, string suburb, string postcode)
        {
            Street = street;
            Number = number;
            Suburb = suburb;
            Postcode = postcode;
        }

        public int CompareTo(Address other)
        {
            return ((Street == other.Street) && (Number == other.Number)
                && (Suburb == other.Suburb) && (Postcode == other.Postcode)) ? 0 : 1;
        }
    }
}
