namespace GroceryList_Api.ViewModels
{
    public class UserRegistrationResponseViewModel
    {
        public string Message { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
