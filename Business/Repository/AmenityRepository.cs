using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using DataAcesss.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class AmenityRepository : IAmenitiyRepository
    {
        private readonly AplicationDBContext db;
        private readonly IMapper map;

        public AmenityRepository(AplicationDBContext context, IMapper mapper)
        {
            db = context;
            map = mapper;
        }

        public async Task<HotelAmenityDTO> CreateHotelAmenity(HotelAmenityDTO hotelAmenity)
        {
            var amenity = map.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenity);
            amenity.CreatedBy = "";
            amenity.CreatedDate = DateTime.UtcNow;
            var addedHotelAmenity = await db.HotelAmenities.AddAsync(amenity);
            await db.SaveChangesAsync();
            return map.Map<HotelAmenity, HotelAmenityDTO>(addedHotelAmenity.Entity);
        }

        public async Task<HotelAmenityDTO> UpdateHotelAmenity(int amenityId, HotelAmenityDTO hotelAmenity)
        {
            var amenityDetails = await db.HotelAmenities.FindAsync(amenityId);
            var amenity = map.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenity, amenityDetails);
            amenity.UpdatedBy = "";
            amenity.UpdatedDate = DateTime.UtcNow;
            var updatedamenity = db.HotelAmenities.Update(amenity);
            await db.SaveChangesAsync();
            return map.Map<HotelAmenity, HotelAmenityDTO>(updatedamenity.Entity);
        }

        public async Task<int> DeleteHotelAmenity(int amenityId, string userId)
        {
            var amenityDetails = await db.HotelAmenities.FindAsync(amenityId);
            if (amenityDetails != null)
            {
                db.HotelAmenities.Remove(amenityDetails);
                return await db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelAmenityDTO>> GetAllHotelAmenity()
        {
            return map.Map<IEnumerable<HotelAmenity>, IEnumerable<HotelAmenityDTO>>(await db.HotelAmenities.ToListAsync());
        }

        public async Task<HotelAmenityDTO> GetHotelAmenity(int amenityId)
        {
            var amenityData = await db.HotelAmenities.FirstOrDefaultAsync(x => x.Id == amenityId);

            if (amenityData == null)
            {
                return null;
            }
            return map.Map<HotelAmenity, HotelAmenityDTO>(amenityData);
        }

        public async Task<HotelAmenityDTO> IsSameNameAmenityAlreadyExists(string name)
        {
            try
            {
                var amenityDetails =
                    await db.HotelAmenities.FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == name.ToLower().Trim()
                    );
                return map.Map<HotelAmenity, HotelAmenityDTO>(amenityDetails);
            }
            catch (Exception ex)
            {

            }
            return new HotelAmenityDTO();
        }
    }
}