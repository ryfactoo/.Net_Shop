using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNet_lab_lista_10.ViewModesl
{
    public class ShippingViewModel
    {

        public List<ArticleCartItemViewModel> CartItems { get; set; }


        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Postal Code should be in the format XX-XXX")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Street is required.if your city doesn't have a street, enter the city name")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Building Number is required")]
        public string BuildingNumber { get; set; }

        [Required(ErrorMessage = "Please select a payment method.")]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

    }


    public enum PaymentMethod
    {
        Blik,
        CreditCard,
        PayPal,
        BankTransfer,
        CashOnDelivery
    }
}
