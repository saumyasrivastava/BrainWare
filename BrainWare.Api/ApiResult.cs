namespace BrainWare.Api
{
    public class ApiResult<T>
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public T Data { get; set; }
    }
}