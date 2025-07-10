using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ImportMedicineEX;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Medicine;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Ultility;
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

        public Task<string> AskUserForConfirmation(string message)
        {
            return Task.FromResult(message);
        }
        private async Task<string> GenerateUniqueMedicineImportDetailCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateMedicineImportDetailCode();
                isExist = await _context.MedicineImportDetails.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<bool> ImportMedicinesAsync(MedicineImportRequest request, bool continueImport)
        {
            
            using var transaction = await _context.Database.BeginTransactionAsync(); 

            try
            {
                foreach (var detail in request.Details)
                {
                    var existingBatch = await _context.MedicineImportDetails
                        .FirstOrDefaultAsync(m => m.MedicineId == detail.MedicineId &&
                                                   m.BatchNumber == detail.BatchNumber &&
                                                   m.SupplierId == request.SupplierId);  

                    if (existingBatch != null)
                    {
                        throw new Exception($"Lỗi: Số lô {detail.BatchNumber} của thuốc {detail.MedicineName} đã tồn tại trong lần nhập trước của nhà cung cấp này. Vui lòng kiểm tra lại số lô.");
                    }
                }


                var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == request.SupplierId);

                if (supplier == null)
                {
                    return false; 
                }
                await _context.SaveChangesAsync();

                

                foreach (var detail in request.Details)
                {

                    var category = await _context.MedicineCategories.FirstOrDefaultAsync(u => u.Name.Trim().ToLower() == detail.CategoryName.Trim().ToLower());

                    if (category == null)
                    {
                        var confirmMessage = "Phát hiện danh mục thuốc không tồn tại, bạn có muốn tạo mới danh mục thuốc với dữ liệu mặc định hay không?";
                        string message = await AskUserForConfirmation(confirmMessage);

                        if (!continueImport || message == null || !message.Contains("không tồn tại"))
                        {
                            return false;
                        }
                        category = new MedicineCategory
                        {
                            ImageUrl = "Chưa có ảnh",
                            Description = "Chưa thêm",
                            Status = MedicineCategoryStatus.Active,
                            Code = "Unknown",
                            Name = detail.CategoryName,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            CreateBy = "system",
                            UpdateBy = "system"
                        };
                        _context.MedicineCategories.Add(category);
                        await _context.SaveChangesAsync();

                    }

                    var import = _mapper.MapToImportEntity(request, supplier.Id);
                    _context.MedicineImports.Add(import);
                    await _context.SaveChangesAsync();

                    var unit = await _context.Units.FirstOrDefaultAsync(u => u.Name == detail.UnitName);
                    if (unit == null)
                    {
                        var confirmMessage = $"Phát hiện đơn vị {detail.UnitName} không tồn tại, bạn có muốn tạo mới đơn vị này hay không?";
                        string message = await AskUserForConfirmation(confirmMessage);

                        if (!continueImport || message == null || !message.Contains("không tồn tại"))
                        {
                            return false;
                        }

                        unit = new Unit
                        {
                            Name = detail.UnitName,
                            Status = 1
                        };
                        _context.Units.Add(unit);
                        await _context.SaveChangesAsync(); 
                    }

                    var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.Code == detail.MedicineCode);
                    if (medicine == null)
                    {
                        var confirmMessage = $"Phát hiện thuốc {detail.MedicineName} không tồn tại, bạn có muốn tạo mới thuốc này hay không?";
                        string message = await AskUserForConfirmation(confirmMessage);

                        if (!continueImport || message == null || !message.Contains("không tồn tại"))
                        {
                            return false;
                        }


                        medicine = new Medicine
                        {
                            ImageUrl = "Chưa có ảnh",
                            Name = detail.MedicineName,
                            Code = detail.MedicineCode,
                            UnitId = unit.Id, 
                            UnitPrice = detail.UnitPrice,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            CreateBy = "system",
                            UpdateBy = "system",
                            Status = MedicineStatus.Active,
                            Description = "Chưa thêm",
                            Dosage = detail.Dosage,
                            Prescribed =detail.Prescribed,
                            MedicineCategoryId = category.Id,
                        };
                        _context.Medicines.Add(medicine);
                        await _context.SaveChangesAsync();

                        var medicineDetail = new MedicineDetail
                        {
                            MedicineId = medicine.Id,
                            Ingredients = detail.Ingredients,
                            ExpiryDate = detail.ExpiryDate,
                            Manufacturer =detail.ManufactureDate,
                            Warning = "Chưa thêm",
                            StorageInstructions = detail.StorageInstructions,
                            Status = 1,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            CreateBy = "system",
                            UpdateBy = "system",
                            Description = "Chưa thêm"
                        };
                        _context.MedicineDetails.Add(medicineDetail);
                    }

                    


                    var importDetail = _mapper.MapToImportDetailEntity(detail, medicine.Id, import.Id);
                    importDetail.UnitId = unit.Id;
                    importDetail.SupplierId = supplier.Id;
                    importDetail.Code = await GenerateUniqueMedicineImportDetailCodeAsync();
                    _context.MedicineImportDetails.Add(importDetail);
                    await _context.SaveChangesAsync();

                    var inventory = _mapper.MapToInventoryEntity(detail, medicine.Id, importDetail.Id);
                    _context.Medicine_Inventories.Add(inventory);
                }



                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); 

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Lỗi khi import:");
                Console.WriteLine($"Message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");
                }
                return false;
            }
        }

    }
}