using AutoMapper;
using e_commerce.app.Dto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepo _repo;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // 🔥 أهم ميثود (اللي هتستخدم في Order)
    public async Task<DiscountDto> ApplyDiscountAsync(string code, decimal orderTotal)
    {
        var discount = await _repo.GetActiveDiscountByCodeAsync(code);

        if (discount == null)
            throw new Exception("Invalid or expired discount code");

        // 🔥 Business Logic
        if (orderTotal < discount.MinOrderAmount)
            throw new Exception("Minimum order amount not reached");

        return _mapper.Map<DiscountDto>(discount);
    }

    // 📋 Get All
    public async Task<IReadOnlyList<DiscountDto>> GetAllAsync()
    {
        var discounts = await _repo.GetAllAsync();
        return _mapper.Map<IReadOnlyList<DiscountDto>>(discounts);
    }

    // 🔍 Get By Id
    public async Task<DiscountDto> GetByIdAsync(int id)
    {
        var discount = await _repo.GetByIdAsync(id);

        if (discount == null)
            throw new Exception("Discount not found");

        return _mapper.Map<DiscountDto>(discount);
    }

    // ➕ Add
    public async Task AddAsync(DiscountDto dto)
    {
        var discount = _mapper.Map<Discount>(dto);
        discount.IsActive = true;

        await _repo.AddAsync(discount);
    }

    // ✏️ Update
    public async Task UpdateAsync(DiscountDto dto)
    {
        var existing = await _repo.GetByIdAsync(dto.Id);

        if (existing == null)
            throw new Exception("Discount not found");

        existing.Code = dto.Code;
        existing.DiscountType = dto.DiscountType;
        existing.Value = dto.Value;
        existing.StartDate = dto.StartDate;
        existing.EndDate = dto.EndDate;
        existing.MinOrderAmount = dto.MinOrderAmount;

        await _repo.UpdateAsync(existing);
    }

    // ❌ Delete (Soft Delete)
    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
    }
}