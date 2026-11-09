namespace FunEvents.Application.Common.Dtos
{
    public sealed record PagoDto(Guid Id, Guid ReservaId, decimal Monto, string MetodoPago, string Estado, DateTime FechaPago);
    public sealed record CreatePagoRequest(Guid ReservaId, decimal Monto, string MetodoPago, string Estado, DateTime FechaPago);
    public sealed record UpdatePagoRequest(decimal Monto, string MetodoPago, string Estado, DateTime FechaPago);
}
