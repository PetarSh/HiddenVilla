using Business.Repository.IRepository;
using DataAccess.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly AplicationDBContext db;
        private readonly IMapper map;

        public HotelRoomRepository(AplicationDBContext dbContext, IMapper mapper)
        {
            db = dbContext;
            map = mapper;
        }

        public async Task<HotelRoomDTO> CreateHotelRoom (HotelRoomDTO hotelRoomDTO)
        {
            HotelRoom hotelRoom = map.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO);
            hotelRoom.CreatedDate = DateTime.Now;
            hotelRoom.CreatedBy = "";
            var room = await db.HotelRooms.AddAsync(hotelRoom);
            await db.SaveChangesAsync();
            return map.Map<HotelRoom, HotelRoomDTO>(room.Entity);
        }

        public async Task<int> DeleteHotelRoom(int roomId)
        {
            var roomdetails = await db.HotelRooms.FindAsync(roomId);
            if(roomdetails !=null)
            {
                var allimages = await db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();

                db.HotelRoomImages.RemoveRange(allimages);
                db.HotelRooms.Remove(roomdetails);
                return await db.SaveChangesAsync();

            }
            return 0;
        }

        public async Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms(string checkInDateStr, string checkOutDatestr)
        {
            try
            {
                IEnumerable<HotelRoomDTO> hotelRoomDTOs = map.Map<IEnumerable<HotelRoom>,IEnumerable<HotelRoomDTO>>
                    ( db.HotelRooms.Include(x => x.HotelRoomImages));


                return hotelRoomDTOs;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public async Task<HotelRoomDTO> GetHotelRoom(int roomId, string checkInDateStr, string checkOutDatestr)
        {
            try
            {
                HotelRoomDTO hotelRoom = map.Map<HotelRoom, HotelRoomDTO>
                    (await db.HotelRooms.Include(x=>x.HotelRoomImages).FirstOrDefaultAsync(x => x.Id == roomId));

                return hotelRoom;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Task<bool> IsRoomBooked(int RoomId, string checkInDate, string checkOutDate)
        {
            throw new NotImplementedException();
        }

        public async Task<HotelRoomDTO> IsRoomUnique(string name,int roomId=0)
        {
            try
            {
                if(roomId==0)
                { 
                    HotelRoomDTO hotelRoom = map.Map<HotelRoom, HotelRoomDTO>
                    (await db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()));

                return hotelRoom;
                }
                else
                {
                    HotelRoomDTO hotelRoom = map.Map<HotelRoom, HotelRoomDTO>
                   (await db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()
                   && x.Id!=roomId));

                    return hotelRoom;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO hotelRoomDTO)
        {
            try
            {
                if (roomId == hotelRoomDTO.Id)
                {
                    //valid
                    HotelRoom roomdetails =await db.HotelRooms.FindAsync(roomId);
                    HotelRoom room = map.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO, roomdetails);
                    room.UpdatedBy = "";
                    room.UpdatedDate = DateTime.Now;

                    var updatedRoom= db.HotelRooms.Update(room);
                    await db.SaveChangesAsync();

                    return map.Map<HotelRoom, HotelRoomDTO>(updatedRoom.Entity);

                }
                else
                {
                    //invalid
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
