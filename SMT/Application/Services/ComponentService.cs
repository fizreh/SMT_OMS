using SMT.Application.DTOs;
using SMT.Application.Interfaces;
using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Application.Services
{
    public class ComponentService
    {
        private readonly IComponentRepository _componentRepository;

        public ComponentService(IComponentRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }

        public async Task<Component> CreateComponentAsync(Component component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            await _componentRepository.AddAsync(component);
            return component;
        }

        public async Task<Component> GetComponentByIdAsync(Guid id)
        {
            return await _componentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Component>> GetAllComponentsAsync()
        {
            return await _componentRepository.GetAllAsync();
        }

        public async Task<bool> UpdateComponentAsync(ComponentDto component)
        {
            try
            {
                var existingComponent = await _componentRepository.GetByIdAsync(component.Id);
                if(existingComponent == null)
                {
                    return false;
                }
               existingComponent.Update(component.Name,component.Description);

                await _componentRepository.UpdateAsync(existingComponent);
                return true;

            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public async Task<bool> DeleteComponentAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return false;
            }
            await _componentRepository.DeleteAsync(id);
            return true;
        }

       
    }
}