using AutoMapper;
using e_commerce.app.Dto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepo _repo;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Discount> ApplyDiscountAsync(string code, decimal orderTotal)
    {
        var discount = await _repo.GetActiveDiscountByCodeAsync(code);

        if (discount == null)
            throw new Exception("Invalid or expired discount code");

        if (orderTotal < discount.MinOrderAmount)
            throw new Exception("Minimum order amount not reached");

        return discount;
    }

    public async Task<IReadOnlyList<DiscountDto>> GetAllAsync()
    {
        var discounts = await _repo.GetAllAsync();
        return _mapper.Map<IReadOnlyList<DiscountDto>>(discounts);
    }

    public async Task<DiscountDto> GetByIdAsync(int id)
    {
        var discount = await _repo.GetByIdAsync(id);

        if (discount == null)
            throw new Exception("Discount not found");

        return _mapper.Map<DiscountDto>(discount);
    }

    public async Task AddAsync(CreateDiscountDto dto)
    {
        if (await _repo.ExistsByCodeAsync(dto.Code))
            throw new Exception("Discount code already exists");
        var discount = _mapper.Map<Discount>(dto);
        discount.IsActive = true;

        await _repo.AddAsync(discount);
    }

    public async Task UpdateAsync(UpdateDiscountDto dto)
    {
        var existing = await _repo.GetByIdAsync(dto.Id);

        if (existing == null)
        {

            throw new Exception("Discount not found");
        }
        existing.Code = dto.Code;
        existing.DiscountType = dto.DiscountType;
        existing.Value = dto.Value;
        existing.StartDate = dto.StartDate;
        existing.EndDate = dto.EndDate;
        existing.MinOrderAmount = dto.MinOrderAmount;
        if (await _repo.ExistsByCodeAsync(dto.Code) && existing.Code != dto.Code)
            throw new Exception("Discount code already exists");
        await _repo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
    }

}