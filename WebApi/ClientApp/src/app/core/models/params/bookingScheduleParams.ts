export interface BookingScheduleParams {
  departureStationId: number;
  arrivalStationId: number;
  departureDate: string;
  returnDate: string | null;
  roundTrip: boolean;
}
