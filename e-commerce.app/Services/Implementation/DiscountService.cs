using AutoMapper;
using e_commerce.app.Dto;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using e_commerce.core.Enum;
using e_commerce.core.Exceptions;          // ← ضيف ده

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
        // ✅ كوبون مش موجود أو منتهي
        var discount = await _repo.GetActiveDiscountByCodeAsync(code);
        if (discount == null)
            throw new BusinessRuleException($"Discount code '{code}' is invalid or has expired.");

        // ✅ الأوردر أقل من الحد الأدنى
        if (orderTotal < discount.MinOrderAmount)
            throw new BusinessRuleException(
                $"Minimum order amount for this code is {discount.MinOrderAmount:C}. Your total is {orderTotal:C}.");

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
            throw new NotFoundException("Discount", id);

        return _mapper.Map<DiscountDto>(discount);
    }

    public async Task AddAsync(CreateDiscountDto dto)
    {
        // ✅ Validate dates
        if (dto.EndDate <= dto.StartDate)
            throw new ValidationException("EndDate", "End date must be after start date.");

        if (dto.EndDate < DateTime.UtcNow)
            throw new ValidationException("EndDate", "End date cannot be in the past.");

        if (dto.Value <= 0)
            throw new ValidationException("Value", "Discount value must be greater than zero.");

        // ✅ كوبون موجود قبل كده
        if (await _repo.ExistsByCodeAsync(dto.Code))
            throw new ConflictException($"Discount code '{dto.Code}' already exists.");

        var discount = _mapper.Map<Discount>(dto);
        discount.IsActive = true;

        await _repo.AddAsync(discount);
    }

    public async Task UpdateAsync(UpdateDiscountDto dto)
    {
        var existing = await _repo.GetByIdAsync(dto.Id);

        if (existing == null)
            throw new NotFoundException("Discount", dto.Id);

        // ✅ لو الكود اتغير، نتأكد مش موجود عند حد تاني
        if (dto.Code != existing.Code && await _repo.ExistsByCodeAsync(dto.Code))
            throw new ConflictException($"Discount code '{dto.Code}' already exists.");

        // ✅ Validate dates
        if (dto.EndDate <= dto.StartDate)
            throw new ValidationException("EndDate", "End date must be after start date.");

        if (dto.Value <= 0)
            throw new ValidationException("Value", "Discount value must be greater than zero.");

        existing.Code = dto.Code;
        existing.DiscountType = dto.DiscountType;
        existing.Value = dto.Value;
        existing.StartDate = dto.StartDate;
        existing.EndDate = dto.EndDate;
        existing.MinOrderAmount = dto.MinOrderAmount;

        await _repo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        // ✅ تأكد إن الـ discount موجود قبل الحذف
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException("Discount", id);

        await _repo.DeleteAsync(id);
    }
}