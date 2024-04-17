namespace TripsS.AutoMapper;

using global::AutoMapper;
using Trips.Models;
using TripsS.ViewModel;

public class TripAutoMapper : Profile
{
   public TripAutoMapper()
    {
        CreateMap<TripViewModel, Trip>()
             .ForMember(x => x.From, opt => opt.MapFrom(src => src.From.ToUpperInvariant()))
             .ForMember(x => x.To, opt => opt.MapFrom(src => src.To.ToUpperInvariant()))
             .ForMember(x => x.StartTrip, opt => opt.MapFrom(src => src.StartTrip.ToUniversalTime()))
             .ForMember(x => x.EndTrip, opt => opt.MapFrom(src => src.EndTrip.ToUniversalTime()))
             .ReverseMap();
        CreateMap<ClientViewModel, Client>()
            .ForMember(ClientViewModel => ClientViewModel.FirstName, opt => opt.MapFrom(src => src.FirstName.ToUpperInvariant()))
            .ForMember(ClientViewModel => ClientViewModel.LastName, opt => opt.MapFrom(src => src.LastName.ToUpperInvariant()))
            .ReverseMap();
        CreateMap<ReservationViewModel, Reservation>()
            .ForMember(ReservationViewModel => ReservationViewModel.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate.ToUniversalTime()))
            .ReverseMap();
    }
}
