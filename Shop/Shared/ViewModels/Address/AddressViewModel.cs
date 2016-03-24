using Shop.Infrastructure.Customer;

namespace Shop.Shared.Models.ViewModels
{
    public class AddressViewModel
    {
        public string Street { get; }
        public string Suburb { get; }
        public string PostCode { get; }
        public string StreetNumber { get; }
        public string Unit { get; }
        public AddressViewModel(Address address)
        {
            Street = address.Street;
            Suburb = address.Suburb.ToUpper();
            PostCode = address.Postcode;
            Unit = address.Unit;
        }
    }
}