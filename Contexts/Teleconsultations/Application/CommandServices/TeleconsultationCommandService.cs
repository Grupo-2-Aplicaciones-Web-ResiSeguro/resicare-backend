
using learning_center_webapi.Contexts.Shared.Domain.Repositories;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Infraestructure;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Teleconsultations.Application.CommandServices;

public class TeleconsultationCommandService
{
    private readonly ITeleconsultationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TeleconsultationCommandService(ITeleconsultationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Teleconsultation> Handle(CreateTeleconsultationCommand command)
    {
        var teleconsultation = new Teleconsultation(
            command.Service,
            command.Date,
            command.Time,
            command.Description,
            command.UserId
        );

        await _repository.AddAsync(teleconsultation);
        await _unitOfWork.CompleteAsync();
        return teleconsultation;
    }

    public async Task<Teleconsultation> Handle(UpdateTeleconsultationCommand command)
    {
        var teleconsultation = await _repository.FindByIdAsync(command.Id);
        
        if (teleconsultation == null)
            throw new Exception($"Teleconsultation with id {command.Id} not found");

        if (!string.IsNullOrEmpty(command.Service))
            teleconsultation.Service = command.Service;
        
        if (!string.IsNullOrEmpty(command.Date))
            teleconsultation.Date = command.Date;
        
        if (!string.IsNullOrEmpty(command.Time))
            teleconsultation.Time = command.Time;
        
        if (!string.IsNullOrEmpty(command.Description))
            teleconsultation.Description = command.Description;

        _repository.Update(teleconsultation);
        await _unitOfWork.CompleteAsync();
        return teleconsultation;
    }

    public async Task<bool> Handle(DeleteTeleconsultationCommand command)
    {
        var teleconsultation = await _repository.FindByIdAsync(command.Id);
        
        if (teleconsultation == null)
            return false;

        _repository.Remove(teleconsultation);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}