namespace CardApi
{
    /// <summary>
    /// An enum containg all possible services result codes.
    /// </summary>
    public enum ServiceResultCode
    {
        /// <summary>
        /// The request was handled succesfully.
        /// </summary>
        Ok = 200,

        /// <summary>
        /// The request contained an error.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// The requested data could not be retrieved.
        /// </summary>
        NotFound = 401
    }
}
