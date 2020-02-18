using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestApp.Core.Guard;
using TestApp.Domain;
using TestApp.Domain.File;
using TestApp.Services.Time;

namespace TestApp.Services
{
    public sealed class FileService : ServiceBase, IFileService
    {
        private readonly IFileRepository _repository;
        private readonly IRepository<FieldEntity> _fieldsRepository;
        private readonly IFileProviderFactory _fileProvider;
        private readonly IMapper _mapper;
        private readonly ITime _time;

        public FileService(
            IFileRepository repository,
            IFileProviderFactory fileProvider,
            IRepository<FieldEntity> fieldRepository,
            IMapper mapper,
            ITime time)
        {
            _repository = repository;
            _fieldsRepository = fieldRepository;
            _fileProvider = fileProvider;
            _time = time;
            _mapper = mapper;
        }

        public async Task<FileDetailsVm> Get(long id)
        {
            Throw.IfNegativeOrZero(id, nameof(id));           
            
            var dbEntity = await _repository.Find(id);
            if (dbEntity == null)
            {
                return null;
            }

            var dbFields = await _fieldsRepository
                .Entities
                .Where(f => f.FileId == id).ToListAsync();            

            var vm = _mapper.Map<FileDetailsVm>(dbEntity);
            vm.Fields = _mapper.Map<List<FieldVm>>(dbFields);

            return vm;
        }

        public async Task<long> Upload(FileVm vm)
        {
            Throw.IfNull(vm, nameof(vm));

            var fileMetadata = _fileProvider.GetMetadata(vm.Content);

            var entity = _mapper.Map<FileEntity>(vm);
            entity.CreatedOn = _time.Now;
            entity.Type = fileMetadata.Type;

            Func<Task<long>> func = new Func<Task<long>>(async () =>
            {
                _repository.Add(entity);
                await _repository.SaveChanges();

                foreach (FieldVm f in fileMetadata.Fields)
                {
                    var fieldEntity = _mapper.Map<FieldEntity>(f);
                    fieldEntity.FileId = entity.Id;
                    _fieldsRepository.Add(fieldEntity);
                }

                await _fieldsRepository.SaveChanges();

                return entity.Id;
            });

            return await ExecuteInTransaction(func);
        }        
    }
}
