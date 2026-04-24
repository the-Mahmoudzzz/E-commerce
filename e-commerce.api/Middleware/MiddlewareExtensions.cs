namespace e_commerce.api.Middleware
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds the global exception handling middleware to the pipeline
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
