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
    public class HotelImagesRepository : IHotelImagesRepository
    {
        private readonly AplicationDBContext db;
        private readonly IMapper map;
        public HotelImagesRepository(AplicationDBContext dbcontext, IMapper mapper)
        {
            map = mapper;
            db = dbcontext;
        }
        public async Task<int> CreateHotelRoomImage(HotelRoomImageDTO imageDTO)
        {
            var image = map.Map<HotelRoomImageDTO, HotelRoomImage>(imageDTO);
            await db.HotelRoomImages.AddAsync(image);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelImageByImageUrl(string imageUrl)
        {
            var allImages = await db.HotelRoomImages.FirstOrDefaultAsync
                                (x => x.RoomImageUrl.ToLower() == imageUrl.ToLower());
            if (allImages == null)
            {
                return 0;
            }
            db.HotelRoomImages.Remove(allImages);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelRoomImageByImageId(int imageId)
        {
            var image = await db.HotelRoomImages.FindAsync(imageId);
            db.HotelRoomImages.Remove(image);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelRoomImageByRoomId(int roomId)
        {
            var imageList = await db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();
            db.HotelRoomImages.RemoveRange(imageList);
            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<HotelRoomImageDTO>> GetHotelRoomImages(int roomId)
        {
            return map.Map<IEnumerable<HotelRoomImage>, IEnumerable<HotelRoomImageDTO>>(
            await db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync());
        }
    }
}
