namespace CardApi
{
    /// <summary>
    /// An object created and returned by a service containing the service code as wel as the data it got.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class ServiceResponse<TData> where TData : class
    {
        public TData? Data { get; init; }
        public bool SuccesFull { get; init; } = true;
        public ServiceResultCode ServiceResultCode { get; init; }
        public string Message { get; init; } = String.Empty;
    }

    /// <summary>
    /// An object created and returned by a service containing the service code.
    /// </summary>
    public class ServiceResponse
    {
        public bool SuccesFull { get; init; } = true;
        public ServiceResultCode ServiceResultCode { get; init; }
        public string Message { get; init; } = String.Empty;
    }
}
