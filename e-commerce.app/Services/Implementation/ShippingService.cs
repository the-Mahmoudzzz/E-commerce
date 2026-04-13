using AutoMapper;
using e_commerce.app.Dto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;

public class ShippingService : IShippingService
{
    private readonly IShippingZoneRepo _repo;
    private readonly IMapper _mapper;

    public ShippingService(IShippingZoneRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // 🔍 Get One
    public async Task<ShippingZoneDto> GetZoneAsync(int id)
    {
        var zone = await _repo.GetByIdAsync(id);

        if (zone == null)
            throw new Exception("Shipping zone not found");

        return _mapper.Map<ShippingZoneDto>(zone);
    }

    // 📋 Get All
    public async Task<IReadOnlyList<ShippingZoneDto>> GetAllZonesAsync()
    {
        var zones = await _repo.GetAllAsync();
        return _mapper.Map<IReadOnlyList<ShippingZoneDto>>(zones);
    }

    // ➕ Add
    public async Task AddZoneAsync(ShippingZoneDto zoneDto)
    {
        var zone = _mapper.Map<ShippingZone>(zoneDto);

        zone.IsActive = true;

        await _repo.AddAsync(zone);
    }

    // ✏️ Update
    public async Task UpdateZoneAsync(ShippingZoneDto zoneDto)
    {
        var existingZone = await _repo.GetByIdAsync(zoneDto.Id);

        if (existingZone == null)
            throw new Exception("Shipping zone not found");

        // update values
        existingZone.CityName = zoneDto.CityName;
        existingZone.ShipingCost = zoneDto.ShippingCost;
        existingZone.EstimatedDays = zoneDto.EstimatedDays;

        await _repo.UpdateAsync(existingZone);
    }

    // ❌ Delete (Soft Delete)
    public async Task DeleteZoneAsync(int id)
    {
        var zone = await _repo.GetByIdAsync(id);

        if (zone == null)
            throw new Exception("Shipping zone not found");

        await _repo.DeleteAsync(id);
    }
}