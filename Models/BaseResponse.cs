namespace my_shoppinglist_api.Models
{
    public class BaseResponse<T>
    {
        public bool Succes {get; set;}
        public T Data {get; set;}
        public string Error {get; set;}
    }
}