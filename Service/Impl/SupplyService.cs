using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Supply;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Services.Interface;

namespace SWP391_SE1914_ManageHospital.Services.Implement
{
    public class SupplyService : ISupplyService
    {
        private readonly List<SupplyResponseDTO> _mockSupplies = new();

        public async Task<List<SupplyResponseDTO>> GetAllAsync()
        {
            return await Task.FromResult(_mockSupplies);
        }

        public async Task<SupplyResponseDTO?> GetByIdAsync(int id)
        {
            var item = _mockSupplies.FirstOrDefault(s => s.Id == id);
            return await Task.FromResult(item);
        }

        public async Task<SupplyResponseDTO> CreateAsync(SupplyCreate dto)
        {
            var newId = _mockSupplies.Count + 1;
            var newSupply = new SupplyResponseDTO
            {
                Id = newId,
                Name = dto.Name,
                Code = dto.Code,
                Status = dto.Status,
                Description = dto.Description,
                Unit = dto.Unit,
                AppointmentId = dto.AppointmentId,
                CreateDate = dto.CreateDate,
                CreateBy = dto.CreateBy
            };
            _mockSupplies.Add(newSupply);
            return await Task.FromResult(newSupply);
        }

        public async Task<bool> UpdateAsync(SupplyUpdate dto)
        {
            var supply = _mockSupplies.FirstOrDefault(s => s.Id == dto.Id);
            if (supply == null) return false;

            supply.Name = dto.Name;
            supply.Code = dto.Code;
            supply.Status = dto.Status;
            supply.Description = dto.Description;
            supply.Unit = dto.Unit;
            supply.AppointmentId = dto.AppointmentId;
            supply.UpdateDate = dto.UpdateDate;
            supply.UpdateBy = dto.UpdateBy;

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supply = _mockSupplies.FirstOrDefault(s => s.Id == id);
            if (supply == null) return false;

            _mockSupplies.Remove(supply);
            return await Task.FromResult(true);
        }

        public async Task<List<SupplyResponseDTO>> SearchAsync(SupplySearch searchDto)
        {
            var query = _mockSupplies.AsQueryable();

            if (!string.IsNullOrEmpty(searchDto.Name))
                query = query.Where(s => s.Name.Contains(searchDto.Name));

            if (!string.IsNullOrEmpty(searchDto.Code))
                query = query.Where(s => s.Code.Contains(searchDto.Code));

            if (searchDto.Status != null)
                query = query.Where(s => s.Status == searchDto.Status);

            if (searchDto.FromCreateDate.HasValue)
                query = query.Where(s => s.CreateDate >= searchDto.FromCreateDate.Value);

            if (searchDto.ToCreateDate.HasValue)
                query = query.Where(s => s.CreateDate <= searchDto.ToCreateDate.Value);

            return await Task.FromResult(query.ToList());
        }
    }
}
