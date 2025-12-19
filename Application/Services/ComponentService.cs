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

        public async Task<Component> CreateComponentAsync(string name, string description)
        {
            var component = new Component(name, description);
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

        public async Task UpdateComponentAsync(Component component)
        {
            await _componentRepository.UpdateAsync(component);
        }

        public async Task DeleteComponentAsync(Guid id)
        {
            await _componentRepository.DeleteAsync(id);
        }

        // Simulate download to production line
        public async Task<string> DownloadComponentAsync(Guid id)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null) throw new Exception("Component not found");

            // Return serialized JSON string of component
            var componentJson = System.Text.Json.JsonSerializer.Serialize(component);
            return componentJson;
        }
    }
}