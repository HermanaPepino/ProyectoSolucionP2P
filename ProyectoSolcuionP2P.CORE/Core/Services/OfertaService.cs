using ProyectoSolucionP2P.CORE.Core.DTOs;
using ProyectoSolucionP2P.CORE.Core.Entities;
using ProyectoSolucionP2P.CORE.Core.Interfaces;

namespace ProyectoSolucionP2P.CORE.Core.Services
{
    public class OfertaService : IOfertaService
    {
        private readonly IOfertaRepository _repo;
        private readonly IUsuarioRepository _usuarioRepo;

        public OfertaService(IOfertaRepository repo, IUsuarioRepository usuarioRepo)
        {
            _repo = repo;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<IEnumerable<OfertaDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(MapToDto);

        public async Task<OfertaDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<OfertaDto> CreateAsync(OfertaDto dto)
        {
            var usuario = await _usuarioRepo.GetByIdAsync(dto.UsuarioId);

            if (usuario == null)
                throw new InvalidOperationException("El usuario no existe.");

            if (!PuedeOperar(usuario))
                throw new InvalidOperationException("Debes verificar tu cuenta antes de publicar una oferta.");

            if (dto.MonedaOrigenId == dto.MonedaDestinoId)
                throw new InvalidOperationException("La moneda de origen y destino deben ser diferentes.");

            if (dto.TasaCambio <= 0)
                throw new InvalidOperationException("La tasa de cambio debe ser mayor a 0.");

            if (dto.MontoMinimo <= 0)
                throw new InvalidOperationException("El monto mínimo debe ser mayor a 0.");

            if (dto.MontoMaximo <= dto.MontoMinimo)
                throw new InvalidOperationException("El monto máximo debe ser mayor que el monto mínimo.");

            if (dto.MontoDisponible.HasValue && dto.MontoDisponible < dto.MontoMinimo)
                throw new InvalidOperationException("El monto disponible no puede ser menor que el monto mínimo.");

            var metodos = PrepararMetodos(dto);

            if (metodos.Count == 0)
                throw new InvalidOperationException("Debes seleccionar al menos un método de pago.");

            var incompleto = metodos.FirstOrDefault(m =>
                string.IsNullOrWhiteSpace(m.Alias) ||
                string.IsNullOrWhiteSpace(m.DatosRecepcion) ||
                string.IsNullOrWhiteSpace(m.Instrucciones));

            if (incompleto != null)
                throw new InvalidOperationException("Completa alias, datos de recepción e instrucciones para cada método de pago.");

            var e = new Oferta
            {
                UsuarioId = dto.UsuarioId,
                MonedaOrigenId = dto.MonedaOrigenId,
                MonedaDestinoId = dto.MonedaDestinoId,
                TipoOperacion = string.IsNullOrWhiteSpace(dto.TipoOperacion) ? "Venta" : dto.TipoOperacion,
                TasaCambio = dto.TasaCambio,
                MontoMinimo = dto.MontoMinimo,
                MontoMaximo = dto.MontoMaximo,
                MontoDisponible = dto.MontoDisponible,
                Estado = string.IsNullOrWhiteSpace(dto.Estado) ? "Activa" : dto.Estado,
                FechaCreacion = dto.FechaCreacion ?? DateTime.Now,
                OfertaMetodoPagos = metodos.Select(m => new OfertaMetodoPago
                {
                    MetodoPagoId = m.MetodoPagoId,
                    Alias = m.Alias,
                    DatosRecepcion = m.DatosRecepcion,
                    Instrucciones = m.Instrucciones,
                    ResumenPublico = m.ResumenPublico
                }).ToList()
            };

            var creado = await _repo.CreateAsync(e);
            return MapToDto(creado);
        }

        public async Task<bool> UpdateAsync(int id, OfertaDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;

            e.UsuarioId = dto.UsuarioId;
            e.MonedaOrigenId = dto.MonedaOrigenId;
            e.MonedaDestinoId = dto.MonedaDestinoId;
            e.TipoOperacion = dto.TipoOperacion;
            e.TasaCambio = dto.TasaCambio;
            e.MontoMinimo = dto.MontoMinimo;
            e.MontoMaximo = dto.MontoMaximo;
            e.MontoDisponible = dto.MontoDisponible;
            e.Estado = dto.Estado;
            e.FechaCreacion = dto.FechaCreacion;

            await _repo.UpdateAsync(e);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        private static bool PuedeOperar(Usuario usuario)
        {
            if (usuario.Rol == "Administrador")
                return true;

            return usuario.EstadoVerificacion == "Verificado";
        }

        private static List<OfertaMetodoPagoDto> PrepararMetodos(OfertaDto dto)
        {
            if (dto.MetodosPago != null && dto.MetodosPago.Count > 0)
            {
                return dto.MetodosPago
                    .Where(m => m.MetodoPagoId > 0)
                    .GroupBy(m => m.MetodoPagoId)
                    .Select(g => g.First())
                    .ToList();
            }

            return dto.MetodoPagoIds
                .Where(id => id > 0)
                .Distinct()
                .Select(id => new OfertaMetodoPagoDto
                {
                    MetodoPagoId = id
                })
                .ToList();
        }

        private static OfertaDto MapToDto(Oferta e)
        {
            return new OfertaDto
            {
                Id = e.Id,
                UsuarioId = e.UsuarioId,
                NombreVendedor = e.Usuario?.NombreCompleto,
                MonedaOrigenId = e.MonedaOrigenId,
                MonedaDestinoId = e.MonedaDestinoId,
                MonedaOrigenNombre = e.MonedaOrigen?.Nombre ?? string.Empty,
                MonedaDestinoNombre = e.MonedaDestino?.Nombre ?? string.Empty,
                TipoOperacion = e.TipoOperacion,
                TasaCambio = e.TasaCambio,
                MontoMinimo = e.MontoMinimo,
                MontoMaximo = e.MontoMaximo,
                MontoDisponible = e.MontoDisponible,
                Estado = e.Estado,
                FechaCreacion = e.FechaCreacion,
                MetodoPagoIds = e.OfertaMetodoPagos.Select(m => m.MetodoPagoId).ToList(),
                MetodosPago = e.OfertaMetodoPagos.Select(MapMetodoPago).ToList()
            };
        }

        private static OfertaMetodoPagoDto MapMetodoPago(OfertaMetodoPago e)
        {
            return new OfertaMetodoPagoDto
            {
                Id = e.Id,
                OfertaId = e.OfertaId,
                MetodoPagoId = e.MetodoPagoId,
                MetodoPagoNombre = e.MetodoPago?.Nombre ?? string.Empty,
                Alias = e.Alias,
                DatosRecepcion = e.DatosRecepcion,
                Instrucciones = e.Instrucciones,
                ResumenPublico = e.ResumenPublico
            };
        }
    }
}