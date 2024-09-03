using AutoMapper;
using Hospital.Models;
using Hospital.Models.Dto.BuildingDto;
using Hospital.Models.Dto.ConsultCategoryDto;
using Hospital.Models.Dto.Department;
using Hospital.Models.Dto.DesignationDto;
using Hospital.Models.Dto.FloorDto;
using Hospital.Models.Dto.MedicineDTo;
using Hospital.Models.Dto.TestCategoryDto;
using Hospital.Models.Dto.TreatmentDTo;
using Hospital.Models.Dto.VendorDto;

namespace Hospital.APIs
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Building, BuildingCreateDto>().ReverseMap();
                config.CreateMap<Building, BuildingCreateDto>().ReverseMap();

                config.CreateMap<ConsultCategory, ConsultCategoryCreateDto>().ReverseMap();
                config.CreateMap<ConsultCategory, ConsultCategoryUpdateDto > ().ReverseMap();

                config.CreateMap<Medicine, MedicineCreateDto>().ReverseMap();
                config.CreateMap<Medicine, MedicineUpdateDto>().ReverseMap();

                config.CreateMap<Department, DepartmentCreateDto>().ReverseMap();
                config.CreateMap<Department, DepartmentUpdateDto>().ReverseMap();

                config.CreateMap<Treatment, TreatmentCreateDto>().ReverseMap();
                config.CreateMap<Treatment, TreatmentUpdateDto>().ReverseMap();

                config.CreateMap<Designation, DesignationCreateDto>().ReverseMap();
                config.CreateMap<Designation, DesignationUpdateDto>().ReverseMap();

                config.CreateMap<Floor, FloorCreateDto>().ReverseMap();
                config.CreateMap<Floor, FloorUpdateDto>().ReverseMap();

                config.CreateMap<TestCategory, TestCategoryCreateDto>().ReverseMap();
                config.CreateMap<TestCategory, TestCategoryUpdateDto>().ReverseMap();

                config.CreateMap<Vendor, VendorCreateDto>().ReverseMap();
                config.CreateMap<Vendor, VendorUpdateDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
