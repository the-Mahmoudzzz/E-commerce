using AutoMapper;
using e_commerce.app.Dto.ZondeDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Exceptions;          // ← ضيف ده

public class ShippingService : IShippingService
{
    private readonly IShippingZoneRepo _repo;
    private readonly IMapper _mapper;

    public ShippingService(IShippingZoneRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ShippingZoneDto> GetZoneAsync(int id)
    {
        var zone = await _repo.GetByIdAsync(id);
        if (zone == null)
            throw new NotFoundException("ShippingZone", id);

        return _mapper.Map<ShippingZoneDto>(zone);
    }

    public async Task<IReadOnlyList<ShippingZoneDto>> GetAllZonesAsync()
    {
        var zones = await _repo.GetAllAsync();
        return _mapper.Map<IReadOnlyList<ShippingZoneDto>>(zones);
    }

    public async Task AddZoneAsync(ShippingZoneDto dto)
    {
        // ✅ Validate shipping cost
        if (dto.ShippingCost < 0)
            throw new ValidationException("ShippingCost", "Shipping cost cannot be negative.");

        if (dto.EstimatedDays <= 0)
            throw new ValidationException("EstimatedDays", "Estimated days must be greater than zero.");

        // ✅ نفس المدينة موجودة قبل كده
        bool exists = await _repo.AnyAsync(z => z.CityName == dto.CityName);
        if (!exists)
            throw new ConflictException($"A shipping zone for '{dto.CityName}' already exists.");

        var zone = _mapper.Map<ShippingZone>(dto);
        zone.IsActive = true;

        await _repo.AddAsync(zone);
    }

    public async Task UpdateZoneAsync(int id, UpdateZoneDto dto)
    {
        var zone = await _repo.GetByIdAsync(id);
        if (zone == null)
            throw new NotFoundException("ShippingZone", id);

        // ✅ Validate values لو موجودين
        if (dto.ShippingCost.HasValue && dto.ShippingCost.Value < 0)
            throw new ValidationException("ShippingCost", "Shipping cost cannot be negative.");

        if (dto.EstimatedDays.HasValue && dto.EstimatedDays.Value <= 0)
            throw new ValidationException("EstimatedDays", "Estimated days must be greater than zero.");

        if (!string.IsNullOrEmpty(dto.CityName))
            zone.CityName = dto.CityName;

        if (dto.ShippingCost.HasValue)
            zone.ShipingCost = dto.ShippingCost.Value;

        if (dto.EstimatedDays.HasValue)
            zone.EstimatedDays = dto.EstimatedDays.Value;

        await _repo.UpdateAsync(zone);
    }

    public async Task DeleteZoneAsync(int id)
    {
        var zone = await _repo.GetByIdAsync(id);
        if (zone == null)
            throw new NotFoundException("ShippingZone", id);

        await _repo.DeleteAsync(id);
    }
}