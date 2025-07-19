using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ShiftRequest;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO.ShiftRequest;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.Mappers;
using System;

namespace SWP391_SE1914_ManageHospital.Models.Services.Impl
{
    public class ShiftRequestService : IShiftRequestService
    {
        private readonly ApplicationDBContext _context;
        private readonly IShiftRequestMapper _mapper;

        public ShiftRequestService(ApplicationDBContext context, IShiftRequestMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShiftRequestResponseDTO> CreateAsync(ShiftRequestRequestDTO dto)
        {
            // Kiểm tra ca trực có tồn tại không
            var shift = await _context.Doctor_Shifts.FindAsync(dto.ShiftId);
            if (shift == null)
                throw new ArgumentException("Ca trực không tồn tại.");

            // Kiểm tra bác sĩ có trùng ca trực không
            var isDuplicate = await _context.Shift_Requests
                .AnyAsync(r => r.DoctorId == dto.DoctorId && r.ShiftId == dto.ShiftId && r.Status == ShiftRequestStatus.Pending);
            if (isDuplicate)
                throw new InvalidOperationException("Bạn đã gửi yêu cầu cho ca trực này và đang chờ duyệt.");

            var entity = _mapper.ToEntity(dto);
            _context.Shift_Requests.Add(entity);
            await _context.SaveChangesAsync();

            // Load navigation property nếu cần
            await _context.Entry(entity).Reference(e => e.Doctor).LoadAsync();
            await _context.Entry(entity).Reference(e => e.Doctor_Shift).LoadAsync();

            return _mapper.ToResponseDTO(entity);
        }

        public async Task<IEnumerable<ShiftRequestResponseDTO>> GetByDoctorAsync(int doctorId)
        {
            var requests = await _context.Shift_Requests
                .Include(r => r.Doctor)
                .Include(r => r.Doctor_Shift)
                .Where(r => r.DoctorId == doctorId)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return requests.Select(_mapper.ToResponseDTO);
        }

        public async Task<IEnumerable<ShiftRequestResponseDTO>> GetAllAsync()
        {
            var requests = await _context.Shift_Requests
                .Include(r => r.Doctor)
                .Include(r => r.Doctor_Shift)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return requests.Select(_mapper.ToResponseDTO);
        }

        public async Task<bool> ApproveAsync(int requestId)
        {
            var request = await _context.Shift_Requests.FindAsync(requestId);
            if (request == null)
                throw new ArgumentException("Yêu cầu không tồn tại.");

            if (request.Status != ShiftRequestStatus.Pending)
                throw new InvalidOperationException("Yêu cầu đã được xử lý.");

            request.Status = ShiftRequestStatus.Approved;
            request.ApprovedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(int requestId, string? reason = null)
        {
            var request = await _context.Shift_Requests.FindAsync(requestId);
            if (request == null)
                throw new ArgumentException("Yêu cầu không tồn tại.");

            if (request.Status != ShiftRequestStatus.Pending)
                throw new InvalidOperationException("Yêu cầu đã được xử lý.");

            request.Status = ShiftRequestStatus.Rejected;
            request.ApprovedDate = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(reason))
                request.Reason += $"\n[Lý do từ chối]: {reason}";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}