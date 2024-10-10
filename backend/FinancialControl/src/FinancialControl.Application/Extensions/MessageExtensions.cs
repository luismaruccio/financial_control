namespace FinancialControl.Application.Extensions
{
    public static class MessageExtensions
    {
        public static string WithParameters(this string message, params object[] parameters)
        {
            return string.Format(message, parameters);
        }
    }

}
