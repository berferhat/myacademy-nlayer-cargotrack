using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.EmployeeDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Business.Services.Employees
{
    public class EmployeeService(IEmployeeRepository _employeeRepository) : IEmployeeService
    {
        public async Task CreateAsync(CreateEmployeeDto createEmployeeDto)
        {
            var employee = createEmployeeDto.Adapt<Employee>();
            await _employeeRepository.CreateAsync(employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee is null)
                throw new ValidationException("Çalışan bulunamadı.");

            await _employeeRepository.DeleteAsync(employee);
        }

        public async Task<List<ResultEmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Adapt<List<ResultEmployeeDto>>();
        }

        public async Task<UpdateEmployeeDto> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee is null)
                throw new ValidationException("Çalışan bulunamadı.");

            return employee.Adapt<UpdateEmployeeDto>();
        }

        public async Task UpdateAsync(UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = updateEmployeeDto.Adapt<Employee>();
            await _employeeRepository.UpdateAsync(employee);
        }
    }
}
