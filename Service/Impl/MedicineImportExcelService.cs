using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ImportMedicineEX;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using static SWP391_SE1914_ManageHospital.Ultility.Status;

namespace SWP391_SE1914_ManageHospital.Service.Impl
{
    public class MedicineImportExcelService : IMedicineImportExcelService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMedicineImportExcelMapper _mapper;

        public MedicineImportExcelService(ApplicationDBContext context, IMedicineImportExcelMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ImportMedicinesAsync(MedicineImportRequest request)
        {

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == request.SupplierName);

            if (supplier == null)
            {
                // ❗ Nếu không có thì tạo mới
                supplier = new Supplier
                {
                    Name = request.SupplierName,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateBy = "system",
                    UpdateBy = "system"
                };
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();
            }

            var import = _mapper.MapToImportEntity(request, supplier.Id);
            _context.MedicineImports.Add(import);
            await _context.SaveChangesAsync();
            foreach (var detail in request.Details)
            {
                var unit = await _context.Units.FirstOrDefaultAsync(u => u.Name == detail.UnitName);

                if (unit == null)
                {
                    unit = new Unit
                    {
                        Name = detail.UnitName,
                        Status = 1
                    };
                    _context.Units.Add(unit);
                    await _context.SaveChangesAsync();
                }

                var category = await _context.MedicineCategories.FirstOrDefaultAsync(c => c.Name == "Không xác định");
                if (category == null)
                {
                    category = new MedicineCategory
                    {
                        Name = "Không xác định",
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        CreateBy = "system",
                        UpdateBy = "system"
                    };
                    _context.MedicineCategories.Add(category);
                    await _context.SaveChangesAsync();
                }
                var medicine = await _context.Medicines
                                    .FirstOrDefaultAsync(m => m.Code == detail.MedicineCode);

                if (medicine == null)
                {
                    // ❗ Tạo mới nếu chưa có
                    medicine = new Medicine
                    {
                        Name = detail.MedicineName,
                        Code = detail.MedicineCode,
                        UnitId = unit.Id,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        CreateBy = "system",
                        UpdateBy = "system",
                        Status = MedicineStatus.Active,
                        Description = "",
                        Dosage = "",
                        Prescribed = 0,
                        MedicineCategoryId = category.Id,
                    };
                    _context.Medicines.Add(medicine);
                    await _context.SaveChangesAsync();

                    var medicineDetail = new MedicineDetail
                    {
                        MedicineId = medicine.Id,
                        Ingredients = "",
                        ExpiryDate = detail.ExpiryDate,
                        Manufacturer = "",
                        Warning = "",
                        StorageInstructions = "",
                        Status = 1,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        CreateBy = "system",
                        UpdateBy = "system",
                        Description = ""
                    };
                    _context.MedicineDetails.Add(medicineDetail);
                    await _context.SaveChangesAsync();
                }

                var importDetail = _mapper.MapToImportDetailEntity(detail, medicine.Id, import.Id);
                _context.MedicineImportDetails.Add(importDetail);
                await _context.SaveChangesAsync();

                var inventory = _mapper.MapToInventoryEntity(detail, medicine.Id, importDetail.Id);
                _context.Medicine_Inventories.Add(inventory);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}