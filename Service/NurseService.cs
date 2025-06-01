using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;
using SWP391_SE1914_ManageHospital.Ultility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_SE1914_ManageHospital.Service
{
    public class NurseService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public NurseService(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NurseDTO>> GetAllNurses()
        {
            var nurses = await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Department)
                .Include(n => n.Nurse_Appointments)
                .ToListAsync();
            return _mapper.Map<List<NurseDTO>>(nurses);
        }

        public async Task<NurseDTO> GetNurseById(int id)
        {
            var nurse = await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Department)
                .Include(n => n.Nurse_Appointments)
                .FirstOrDefaultAsync(n => n.Id == id);
            return nurse == null ? null : _mapper.Map<NurseDTO>(nurse);
        }

        public async Task<NurseDTO> CreateNurse(NurseDTO nurseDto)
        {
            if (await _context.Nurses.AnyAsync(n => n.Code == nurseDto.Code))
                throw new ArgumentException("Nurse code already exists.");

            if (!await _context.Users.AnyAsync(u => u.Id == nurseDto.UserId))
                throw new ArgumentException("User not found.");

            if (!await _context.Departments.AnyAsync(d => d.Id == nurseDto.DepartmentId))
                throw new ArgumentException("Department not found.");

            var nurse = _mapper.Map<Nurse>(nurseDto);
            nurse.CreateDate = DateTime.Now;
            _context.Nurses.Add(nurse);
            await _context.SaveChangesAsync();
            return _mapper.Map<NurseDTO>(nurse);
        }

        public async Task<NurseDTO> UpdateNurse(int id, NurseDTO nurseDto)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return null;

            if (await _context.Nurses.AnyAsync(n => n.Code == nurseDto.Code && n.Id != id))
                throw new ArgumentException("Nurse code already exists.");

            if (!await _context.Users.AnyAsync(u => u.Id == nurseDto.UserId))
                throw new ArgumentException("User not found.");

            if (!await _context.Departments.AnyAsync(d => d.Id == nurseDto.DepartmentId))
                throw new ArgumentException("Department not found.");

            _mapper.Map(nurseDto, nurse);
            nurse.UpdateDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return _mapper.Map<NurseDTO>(nurse);
        }

        public async Task<bool> DeleteNurse(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return false;

            _context.Nurses.Remove(nurse);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Nurse_AppointmentDTO> CreateNurseAppointment(Nurse_AppointmentDTO dto)
        {
            if (!await _context.Nurses.AnyAsync(n => n.Id == dto.NurseId))
                throw new ArgumentException("Nurse not found.");

            if (!await _context.Appointments.AnyAsync(a => a.Id == dto.AppointmentId))
                throw new ArgumentException("Appointment not found.");

            var appointment = _mapper.Map<Nurse_Appointment>(dto);
            _context.NurseAppointments.Add(appointment);
            await _context.SaveChangesAsync();
            return _mapper.Map<Nurse_AppointmentDTO>(appointment);
        }

        public async Task<Nurse_AppointmentDTO> GetNurseAppointmentById(int id)
        {
            var appointment = await _context.NurseAppointments
                .Include(na => na.Nurse)
                .Include(na => na.Appointment)
                .FirstOrDefaultAsync(na => na.Id == id);
            return appointment == null ? null : _mapper.Map<Nurse_AppointmentDTO>(appointment);
        }
    }
}