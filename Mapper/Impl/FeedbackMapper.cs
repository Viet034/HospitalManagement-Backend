using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.Feedback;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Mapper.Impl;

public class FeedbackMapper : IFeedbackMapper
{
    public Feedback CreateToEntity(FeedbackCreate create)
    {
        Feedback feedback = new Feedback();
        feedback.Content = create.Content;
        return feedback;
    }

    public Feedback DeleteToEntity(FeedbackDelete delete)
    {
        Feedback feedback = new Feedback();
        feedback.Id = delete.Id;
        feedback.Content = delete.Content;
        feedback.DoctorId = delete.DoctorId;
        feedback.AppointmentId = delete.AppointmentId;
        return feedback;
    }

    public FeedbackResponseDTO EntityToResponse(Feedback feedback)
    {
        return new FeedbackResponseDTO
        {
            Id = feedback.Id,
            Content = feedback.Content,
            CreateDate = feedback.CreateDate,
            DoctorId = feedback.DoctorId,
            DoctorName = feedback.Doctor?.Name,
            PatientId = feedback.PatientId,
            PatientName = feedback.Patient?.Name
        };
    }


    public IEnumerable<FeedbackResponseDTO> ListEntityToResponse(IEnumerable<Feedback> entities)
    {
        return entities.Select(x => EntityToResponse(x)).ToList();
    }
}
