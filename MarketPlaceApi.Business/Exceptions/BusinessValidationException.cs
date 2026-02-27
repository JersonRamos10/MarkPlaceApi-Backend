namespace MarketPlaceApi.Business.Exceptions
{
    public class BusinessValidationException : Exception
    {

        public string? FieldName {get; private set;}
        public BusinessValidationException() : base()
        {
            
        }

        public BusinessValidationException(string message) : base(message)
        {
            
        }

        public BusinessValidationException(string message, Exception innerException): base(message, innerException)
        {
            
        }

        public BusinessValidationException(string message, string fieldName) : base(message)
        {
            FieldName = fieldName;
        }
    }
}