namespace SWP391_SE1914_ManageHospital.Mapper
{
    public interface INurseMapper
    {
        NurseDTO MapToDto(Nurse nurse);
        Nurse MapToEntity(NurseDTO nurseDto);
        IEnumerable<NurseDTO> MapToDtoList(IEnumerable<Nurse> nurses);
    }
}